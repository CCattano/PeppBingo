import {Component, Input, TemplateRef, ViewChild} from '@angular/core';
import {BingoSubmissionEvent} from '../../../shared/hubs/player/events/bingo-submission.event';
import {NgbModal, NgbModalRef} from '@ng-bootstrap/ng-bootstrap';
import {GameTileVM} from '../../../shared/viewmodels/game-tile.viewmodel';
import {LeaderboardApi} from '../../../shared/api/leaderboard.api';

@Component({
  selector: 'app-leaderboard-vote-flow',
  templateUrl: './leaderboard-vote-flow.component.html',
  styleUrls: ['./leaderboard-vote-flow.component.scss']
})
export class LeaderboardVoteFlowComponent {
  @ViewChild('voteFlowModal', {static: true})
  private readonly _voteModalTemplateRef: TemplateRef<any>;

  /**
   * All the current bingo confirmation requests
   */
  @Input()
  public requests: BingoSubmissionEvent[];

  /**
   * The bingo submission metadata being voted on in this component
   */
  public _submission: BingoSubmissionEvent;

  /**
   * The board being voted on currently
   */
  public _board: GameTileVM[][];

  private _activeModalRef: NgbModalRef;

  constructor(private _ngbModal: NgbModal,
              private _leaderboardApi: LeaderboardApi) {
  }

  /**
   * Opens the Bingo Submission Voting Modal
   */
  public openModal(): void {
    this._activeModalRef =
      this._ngbModal.open(this._voteModalTemplateRef, {
        animation: true,
        backdrop: 'static',
        centered: true,
        keyboard: false,
        size: 'lg',
        windowClass: 'd-flex flex-row justify-content-evenly',
        modalDialogClass: 'my-0 w-100'
      });
  }

  /**
   * Open the voting modal for the submission metadata provided
   * @param submission
   */
  public _onBoardClick(submission: BingoSubmissionEvent): void {
    this._submission = submission;
    this._board =
      new Array(5)
        .fill(0)
        .map((_, i) =>
          [...this._submission.boardTiles.filter(tile => tile.row === i)
            .sort((a, b) => a.column > b.column ? 1 : -1)
            .map(tile => ({
              text: tile.text,
              isSelected: tile.isSelected
            } as GameTileVM))])
  }

  /**
   * Close the modal that is currently open
   */
  public _onCloseModal(): void {
    this._activeModalRef.close();
    this._activeModalRef = null;
    this._board = null;
  }

  /**
   * Event handler for when a board is approved
   */
  public async _onApproveClick(): Promise<void> {
    await this._leaderboardApi.approveBingoSubmission(this._submission.submitterConnectionID);
    this._removeProcessedSubmission();
  }

  /**
   * Event handler for when a board is rejected
   */
  public async _onRejectClick(): Promise<void> {
    await this._leaderboardApi.rejectBingoSubmission(this._submission.submitterConnectionID);
    this._removeProcessedSubmission();
  }

  private _removeProcessedSubmission(): void {
    const submissionIndex: number = this.requests.indexOf(this._submission);
    this.requests.splice(submissionIndex, 1);
    this._submission = null;
    this._board = null;
    if (this.requests.length === 0)
      this._onCloseModal();
  }
}
