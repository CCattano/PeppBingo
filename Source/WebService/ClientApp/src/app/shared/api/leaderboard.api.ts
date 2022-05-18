import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {BingoSubmissionEvent} from '../hubs/player/events/bingo-submission.event';
import {LeaderboardDto} from '../dtos/leaderboard.dto';
import {LeaderboardPosDto} from '../dtos/leaderboard-pos.dto';

@Injectable({
  providedIn: 'root'
})
export class LeaderboardApi {
  constructor(private _http: HttpClient) {
  }

  /**
   * Get an array of all the leaderboards that currently exist
   */
  public async getAllLeaderboards(): Promise<LeaderboardDto[]> {
    return this._http.get<LeaderboardDto[]>('Leaderboard/All').toPromise();
  }

  /**
   * Get an array of all the leaderboards positions for a given leaderboard
   */
  public async getAllLeaderboardPositions(leaderboardID: number): Promise<LeaderboardPosDto[]> {
    return this._http.get<LeaderboardPosDto[]>(`Leaderboard/Positions?leaderboardID=${leaderboardID}`).toPromise();
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
