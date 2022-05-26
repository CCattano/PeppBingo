import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  Renderer2,
  TemplateRef,
  ViewChild
} from '@angular/core';
import {faUserCircle, faExternalLinkAlt, IconDefinition} from '@fortawesome/free-solid-svg-icons';
import {NgbModal, NgbModalRef} from '@ng-bootstrap/ng-bootstrap';
import {BingoSubmissionEvent} from '../../../shared/hubs/player/events/bingo-submission.event';
import {PlayerHub} from '../../../shared/hubs/player/player.hub';
import {GameTileVM} from '../../../shared/viewmodels/game-tile.viewmodel';
import {LeaderboardApi} from '../../../shared/api/leaderboard.api';
import {ApproveSubmissionEvent} from '../../../shared/hubs/player/events/approve-submission.event';
// @ts-ignore
import * as confetti from 'canvas-confetti';
import {Observable, Subject, Subscription} from 'rxjs';
import {delay, map, tap} from 'rxjs/operators';
import {UserApi} from '../../../shared/api/user.api';
import {UserSubmissionStatus} from '../../../shared/enums/user-submission-state.enum';

enum SubmissionStepEnum {
  ConfirmSubmit = 0,
  CannotSubmit = 1,
  AlreadySubmitted = 2,
  AwaitVotes = 3,
  VerifiedBingo = 4,
}

@Component({
  selector: 'app-leaderboard-submission-flow',
  templateUrl: './leaderboard-submission-flow.component.html',
  styleUrls: ['./leaderboard-submission-flow.component.scss']
})
export class LeaderboardSubmissionFlowComponent implements OnInit, OnDestroy {
  @ViewChild('leaderboardSubmissionModal', {static: true})
  private readonly leaderboardSubmissionModalRef: TemplateRef<any>;

  /**
   * The ID of the board the user has gotten a bingo for
   */
  @Input()
  public boardID: number;

  /**
   * The state of the user's board
   */
  @Input()
  public board: GameTileVM[][];

  /**
   * Event emitted when the modal is closed.
   *
   * Emits true if the bingo was accepted by the community
   *
   * Emits false if the user canceled their submission request
   */
  @Output()
  public readonly workflowEnd: EventEmitter<UserSubmissionStatus> = new EventEmitter<UserSubmissionStatus>();

  /**
   * Event emitter signaling to the parent that the user has
   * requested a screenshot of the board they are submitting
   */
  @Output()
  public readonly screenshotRequest: EventEmitter<void> = new EventEmitter<void>();

  /**
   * Fontawesome icons used in the template
   */
  public readonly icons: { [icon: string]: IconDefinition; } = {
    'faUserCircle': faUserCircle,
    'faExternalLinkAlt': faExternalLinkAlt
  };

  /**
   * Instance ref of the SubmissionStepEnum for use in the template
   */
  public readonly submissionStepEnum: typeof SubmissionStepEnum = SubmissionStepEnum;

  /**
   * The current step the submission flow is working through
   */
  public _currentSubmissionStep: SubmissionStepEnum = SubmissionStepEnum.ConfirmSubmit;

  /**
   * Users who have confirmed this bingo
   */
  public _approvers: ApproveSubmissionEvent[] = [];

  /**
   * Array representing the quantity of users who have rejected this bingo
   *
   * Users who reject a bingo are kept anonymous,
   * so the array is an arbitrary type of null array
   *
   * We only need something to iterate over in
   * the template and display a font awesome icon for
   */
  public _rejectors: null[] = [];

  private _modalInstance: NgbModalRef;

  private _userSubmissionStatus: UserSubmissionStatus;

  private _confettiEventSource: Subject<void> = new Subject<void>();
  private _confettiSub: Subscription;

  constructor(private _modalService: NgbModal,
              private _leaderboardApi: LeaderboardApi,
              private _userApi: UserApi,
              private _playerHub: PlayerHub,
              private _renderer2: Renderer2) {
  }

  /**
   * @inheritDoc
   */
  public ngOnInit(): void {
    this._confettiSub = this._initConfettiPipeline().subscribe();
  }

  /**
   * @inheritDoc
   */
  public ngOnDestroy(): void {
    this._confettiSub?.unsubscribe();
    this._confettiSub = null;
    this._playerHub.unregisterSubmissionResponseHandlers();
  }

  /**
   * Open the leaderboard submission workflow modal
   */
  public openSubmissionFlowModal(userSubmissionStatus: UserSubmissionStatus): void {
    this._userSubmissionStatus = userSubmissionStatus;
    if (userSubmissionStatus === UserSubmissionStatus.CannotSubmitBingo)
      this._currentSubmissionStep = SubmissionStepEnum.CannotSubmit;
    else if (userSubmissionStatus === UserSubmissionStatus.AlreadySubmitted)
      this._currentSubmissionStep = SubmissionStepEnum.AlreadySubmitted

    this._modalInstance =
      this._modalService.open(this.leaderboardSubmissionModalRef, {
        animation: true,
        backdrop: 'static',
        centered: true,
        keyboard: false,
        size: 'lg'
      });
  }

  /**
   * Event handler for the {@link SubmissionStepEnum.ConfirmSubmit} step
   * in which a user decides they do want to submit their bingo board to
   * increase their leaderboard standing
   */
  public async _onSubmitBingoRequest(): Promise<void> {
    const submission: BingoSubmissionEvent = {
      submitterConnectionID: this._playerHub.connectionID,
      userID: undefined, // Provided by server
      boardTiles: this.board.flatMap((row, ri) => row.map((col, ci) => ({
        row: ri,
        column: ci,
        isSelected: col.isSelected,
        text: col.text
      })))
    }
    await this._registerSubmissionResponseEventHandlers();
    await this._leaderboardApi.submitBingoForLeaderboard(submission);
    this._currentSubmissionStep = SubmissionStepEnum.AwaitVotes;
  }

  /**
   * Event handler for when a user declines to
   * submit their bingo for leaderboard advancement
   */
  public _onDeclineToSubmit(): void {
    this._modalInstance.close();
    this.workflowEnd.emit(UserSubmissionStatus.CanSubmitBingo);
    this._resetModalState();
  }

  /**
   * Event handler for the screenshot button in the template
   */
  public _onScreenshotRequest(): void {
    this.screenshotRequest.emit();
  }

  /**
   * Event handler for when the modal should be closed at
   * any point during the leaderboard submission workflow
   */
  public async _onCancelSubmission(): Promise<void> {
    this._playerHub.unregisterSubmissionResponseHandlers();
    await this._leaderboardApi.cancelBingoSubmission(this._playerHub.connectionID);
    this._modalInstance.close();
    this.workflowEnd.emit(UserSubmissionStatus.CanSubmitBingo);
    this._resetModalState();
  }

  /**
   * Event handler for when a user closes the submission workflow modal after they
   * have successfully submitted a bingo and increased their leaderboard position
   */
  public _onCloseAfterVerified(): void {
    this._modalInstance.close();
    this.workflowEnd.emit(UserSubmissionStatus.AlreadySubmitted);
    this._resetModalState();
  }

  /**
   * Event handler for when a user has closed the modal after being notified
   * that they cannot submit their bingo for leaderboard standing improvement
   */
  public _onCloseAfterUnableToSubmit(): void {
    this._modalInstance.close();
    this.workflowEnd.emit(this._userSubmissionStatus);
    this._resetModalState();
  }

  private _resetModalState(): void {
    this._currentSubmissionStep = SubmissionStepEnum.ConfirmSubmit;
    this._userSubmissionStatus = UserSubmissionStatus.CanSubmitBingo;
    this._approvers = [];
    this._rejectors = [];
  }

  private async _registerSubmissionResponseEventHandlers(): Promise<void> {
    await this._playerHub.connect();
    this._playerHub.registerApproveSubmissionHandler(this._onApproveSubmission);
    this._playerHub.registerRejectSubmissionHandler(this._onRejectSubmission);
  }

  private _onApproveSubmission = (evtData: ApproveSubmissionEvent): void => {
    this._approvers.push(evtData);
    if (this._approvers.length === 2) {
      this._playerHub.unregisterSubmissionResponseHandlers();
      this._leaderboardApi.updateLeaderboard(this.boardID).finally(() => {
        this._userApi.markUserAsBingoSubmitted().finally(() => {
          this._currentSubmissionStep = SubmissionStepEnum.VerifiedBingo;
          this._confettiEventSource.next();
        });
      });
    }
  }

  private _onRejectSubmission = (): void => {
    this._rejectors.push(null);
  }

  private _initConfettiPipeline(): Observable<void> {
    return this._confettiEventSource.asObservable().pipe(
      // Waiting for animate.css animations to complete
      delay(1800),
      map(() => {
        const canvasEl: HTMLCanvasElement = this._renderer2.createElement('canvas');

        const modalEl: HTMLDivElement =
          document.getElementsByTagName('ngb-modal-window')?.item(0).firstChild.firstChild as HTMLDivElement;

        this._renderer2.appendChild(modalEl, canvasEl);

        let confettiCannon = confetti.create(canvasEl, {
          resize: true // will fit all containing-element sizes
        });
        const count: number = 400;
        const defaults: any = {
          origin: {
            y: 1
          }
        };
        [
          {
            particleRatio: 0.25,
            opts: {
              spread: 26,
              startVelocity: 55,
            }
          },
          {
            particleRatio: 0.2,
            opts: {
              spread: 60,
            }
          },
          {
            particleRatio: 0.35,
            opts: {
              spread: 100,
              decay: 0.91,
              scalar: 0.8
            }
          },
          {
            particleRatio: 0.1,
            opts: {
              spread: 120,
              startVelocity: 25,
              decay: 0.92,
              scalar: 1.2
            }
          },
          {
            particleRatio: 0.1,
            opts: {
              spread: 120,
              startVelocity: 45,
            }
          }
        ].forEach(config => {
          const confettiConf: any = {
            ...defaults,
            ...config.opts,
            particleCount: Math.floor(count * config.particleRatio)
          };
          confettiCannon(confettiConf);
        });
        confettiCannon = null;
        return canvasEl;
      }),
      delay(4000),
      tap(canvas => canvas?.parentElement?.removeChild(canvas)),
      map(() => null)
    )
  }
}
