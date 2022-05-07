import {Component, EventEmitter, Input, Output, TemplateRef, ViewChild} from '@angular/core';
import {faUserCircle, IconDefinition} from '@fortawesome/free-solid-svg-icons';
import {NgbModal, NgbModalRef} from '@ng-bootstrap/ng-bootstrap';
import {UserDto} from '../../../shared/dtos/user.dto';
import {BingoSubmissionEvent} from '../../../shared/hubs/player/events/bingo-submission.event';
import {PlayerHub} from '../../../shared/hubs/player/player.hub';
import {GameTileVM} from '../../../shared/viewmodels/game-tile.viewmodel';
import {LeaderboardApi} from '../../../shared/api/leaderboard.api';

enum SubmissionStepEnum {
  ConfirmSubmit = 0,
  AwaitVotes = 1
}

@Component({
  selector: 'app-leaderboard-submission-flow',
  templateUrl: './leaderboard-submission-flow.component.html',
  styleUrls: ['./leaderboard-submission-flow.component.scss']
})
export class LeaderboardSubmissionFlowComponent {
  @ViewChild('leaderboardSubmissionModal', {static: true})
  private readonly leaderboardSubmissionModalRef: TemplateRef<any>;

  @Input()
  public board: GameTileVM[][];

  @Output()
  public workflowEnd: EventEmitter<void> = new EventEmitter<void>();

  /**
   * Fontawesome icons used in the template
   */
  public readonly icons: { [icon: string]: IconDefinition; } = {
    'faUserCircle': faUserCircle,
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
  public _approvers: UserDto[] = [];

  /**
   * Array representing the quantity of users who have rejected this bingo
   * Users who reject a bingo are kept anonymous,
   * so the array is a arbitrary type of boolean array
   * We only need something to iterate over in
   * the template and display a font awesome icon for
   */
  public _rejectors: boolean[] = [];

  private _modalInstance: NgbModalRef;

  constructor(private _modalService: NgbModal,
              private _leaderboardApi: LeaderboardApi,
              private _playerHub: PlayerHub) {
  }

  /**
   * Open the leaderboard submission workflow modal
   */
  public openSubmissionFlowModal(): void {
    if (!this._modalInstance || this._modalInstance.closed) {
      this._modalInstance =
        this._modalService.open(this.leaderboardSubmissionModalRef, {
          animation: true,
          backdrop: 'static',
          centered: true,
          keyboard: false,
          size: 'lg'
        });
    }
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
    await this._leaderboardApi.submitBingoForLeaderboard(submission);
    this._currentSubmissionStep = SubmissionStepEnum.AwaitVotes;
  }

  /**
   * Event handler for when the modal should be closed at
   * any point during the leaderboard submission workflow
   */
  public _onClose(): void {
    this._modalInstance.close();
    this._resetModalState();
    this.workflowEnd.emit();
  }

  private _resetModalState(): void {
    this._currentSubmissionStep = SubmissionStepEnum.ConfirmSubmit;
    this._approvers = [];
    this._rejectors = [];
  }
}
