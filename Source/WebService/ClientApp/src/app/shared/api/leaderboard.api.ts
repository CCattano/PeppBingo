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
    return await this._http.post<void>('Leaderboard/BingoSubmission', submission).toPromise();
  }
}
