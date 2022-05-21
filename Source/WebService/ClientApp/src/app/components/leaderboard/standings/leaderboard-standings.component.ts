import {Component, OnDestroy, OnInit, TemplateRef, ViewChild} from '@angular/core';
import {LeaderboardApi} from '../../../shared/api/leaderboard.api';
import {LeaderboardDto} from '../../../shared/dtos/leaderboard.dto';
import {LeaderboardPosDto} from '../../../shared/dtos/leaderboard-pos.dto';
import {faTrophy, IconDefinition} from '@fortawesome/free-solid-svg-icons';
import {Observable, Subject, Subscription} from 'rxjs';
import {debounceTime, delay, filter, map, tap} from 'rxjs/operators';
import {UserApi} from '../../../shared/api/user.api';
import {UserDto} from '../../../shared/dtos/user.dto';
import {NgbModal, NgbModalRef} from '@ng-bootstrap/ng-bootstrap';
import {AdminApi} from '../../../shared/api/admin.api';

@Component({
  templateUrl: './leaderboard-standings.component.html',
  styleUrls: ['leaderboard-standings.component.scss']
})
export class LeaderboardStandingsComponent implements OnInit, OnDestroy {
  @ViewChild('confirmReset', {static: true})
  private readonly _confirmResetModal: TemplateRef<any>;

  /**
   * The leaderboards a user can view additional data about
   */
  public _leaderboards: LeaderboardDto[];

  /**
   * The leaderboard a user is currently viewing the details of
   */
  public _currentBoard: LeaderboardDto = null;

  /**
   * The current leaderboard positions for the leaderboard selected
   */
  public _positions: LeaderboardPosDto[];
  /**
   * Bool flag indicating if the currently authenticated user is
   * represented in the leaderboard standings currently on display
   */
  public _userIsOnBoard: boolean = false;

  /**
   * Fontawesome icons used in the template
   */
  public readonly icons: { [icon: string]: IconDefinition; } = {
    'faTrophy': faTrophy,
  };

  public readonly iconColor: string[] = [
    '#FFD700', // Gold
    '#C0C0C0', // Silver
    '#FF5733' // Bronze
  ];

  public _authUser: UserDto;
  private _inputChangeSubscription: Subscription;
  private readonly _inputChangeSource: Subject<string> = new Subject<string>();
  private readonly _inputChange$: Observable<string> = this._inputChangeSource.asObservable();
  private _modalInstance: NgbModalRef;

  constructor(private _leaderboardApi: LeaderboardApi,
              private _adminApi: AdminApi,
              private _userApi: UserApi,
              private _ngbModal: NgbModal) {
  }

  /**
   * @inheritDoc
   */
  public async ngOnInit(): Promise<void> {
    this._authUser = await this._userApi.getUser();
    this._inputChangeSubscription = this._initInputValueChangePipeline().subscribe();
    this._leaderboards = await this._leaderboardApi.getAllLeaderboards();
  }

  /**
   * @inheritdoc
   */
  public ngOnDestroy(): void {
    this._inputChangeSubscription?.unsubscribe();
    this._inputChangeSubscription = null;
  }

  /**
   * Event handler for when a leaderboard is chosen in the template
   * @param board
   */
  public async _onShowBoardStandings(board: LeaderboardDto): Promise<void> {
    if (board === null) {
      this._positions = null;
      this._currentBoard = null;
      return;
    }
    this._currentBoard = board;
    this._positions =
      await this._leaderboardApi.getAllLeaderboardPositions(board.leaderboardID);
    if(!this._positions?.length) return;
    this._positions = this._positions?.sort((a, b) => a.bingoQty > b.bingoQty ? -1 : 1) || [];
    this._userIsOnBoard =
      !!this._positions.find(p => p.displayName.toLowerCase() === this._authUser.displayName.toLowerCase());
  }

  /**
   * Event handler for the reset leaderboard button
   */
  public _onResetLeaderboardClick(): void {
    this._modalInstance = this._ngbModal.open(this._confirmResetModal, {
      animation: true,
      backdrop: 'static',
      centered: true,
      keyboard: false,
      size: 'lg'
    });
  }

  /**
   * Event handler for when a leaderboard should be reset
   */
  public async _onConfirmResetClick(): Promise<void> {
    await this._adminApi.resetLeaderboard(this._currentBoard.leaderboardID);
    this._positions = null;
    this._modalInstance.close();
  }

  /**
   * Event handler for when a leaderboard reset is canceled
   */
  public _onCancelResetClick(): void {
    this._modalInstance.close();
  }

  /**
   * Event handler for the Jump To Me button
   */
  public _onJumpToMeClick(): void {
    this._inputChangeSource.next(this._authUser.displayName);
  }

  /**
   * Event handler for the input changed event
   * Emits the latest value on the input to our
   * user search pipeline
   * @param event
   */
  public _onInput(event: Event): void {
    event.stopPropagation();
    this._inputChangeSource.next((event.target as HTMLInputElement).value);
  }

  /**
   * Blur event handler for the User Search input
   * Clears search results and empties input to
   * prepare for next search to perform
   */
  public _onBlur(event: Event): void {
    (event.target as HTMLInputElement).value = '';
    this._inputChangeSource.next('');
  }

  private _initInputValueChangePipeline(): Observable<void> {
    return this._inputChange$.pipe(
      debounceTime(250),
      filter((searchTerm: string) => !!searchTerm && searchTerm.length > 3),
      map((searchTerm: string) => {
        let regEx: RegExp = new RegExp(`^(${searchTerm})$`, 'i');
        // end-to-end case-insensitive search first
        let pos: LeaderboardPosDto = this._positions.find(p => regEx.test(p.displayName));

        if (pos) return pos;

        regEx = new RegExp(`(${searchTerm})`, 'i');
        // floating case-insensitive search second
        pos = this._positions.find(p => regEx.test(p.displayName));
        return pos;
      }),
      filter((pos: LeaderboardPosDto) => !!pos),
      map((pos: LeaderboardPosDto) => document.getElementById(pos.displayName)),
      tap((targetEl: HTMLElement) => {
        targetEl.scrollIntoView({
          block: 'center',
          inline: 'center',
          behavior: 'smooth'
        } as ScrollIntoViewOptions);
        targetEl.classList.add('highlight');
      }),
      // class transitions the bg in over 1000ms, wait for it to finish
      // Let the style sit for 1000ms
      // This requires a total of a 2000ms delay
      delay(2000),
      // Remove the style
      tap((targetEl: HTMLElement) => targetEl.classList.remove('highlight')),
      map(() => null)
    );
  }
}
