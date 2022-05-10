import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {BingoSubmissionEvent} from '../hubs/player/events/bingo-submission.event';

@Injectable({
  providedIn: 'root'
})
export class LeaderboardApi {
  constructor(private _http: HttpClient) {
  }

  /**
   * Passes bingo board data to all connected players
   * enabling them to approve or reject the proposed bingo
   * @param submission
   */
  public async submitBingoForLeaderboard(submission: BingoSubmissionEvent): Promise<void> {
    return await this._http.post<null>('Leaderboard/BingoSubmission', submission).toPromise();
  }

  /**
   * Send a cancel notice to all connected players. Notifies them a user no longer wants their board voted on.
   * @param hubConnID
   */
  public async cancelBingoSubmission(hubConnID: string): Promise<void> {
    return await this._http.post<null>(`Leaderboard/CancelSubmission?hubConnID=${hubConnID}`, null).toPromise();
  }

  /**
   * Trigger an approved event to be sent to the user who requested their bingo board be voted on
   * @param requestorHubConnID
   */
  public async approveBingoSubmission(requestorHubConnID: string): Promise<void> {
    return await this._http.post<null>(`Leaderboard/ApproveSubmission?requestorHubConnID=${requestorHubConnID}`, null).toPromise();
  }

  /**
   * Trigger a rejected event to be sent to the user who requested their bingo board be voted on
   * @param requestorHubConnID
   */
  public async rejectBingoSubmission(requestorHubConnID: string): Promise<void> {
    return await this._http.post<null>(`Leaderboard/RejectSubmission?requestorHubConnID=${requestorHubConnID}`, null).toPromise();
  }

  /**
   * Update a user's leaderboard standing
   */
  public async updateLeaderboard(boardID: number): Promise<void> {
    return await this._http.put<null>(`Leaderboard/UpdatePosition?boardID=${boardID}`, null).toPromise();
  }
}
