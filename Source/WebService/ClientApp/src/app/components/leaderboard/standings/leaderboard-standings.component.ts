import {Component, OnInit} from '@angular/core';
import {LeaderboardApi} from '../../../shared/api/leaderboard.api';
import {LeaderboardDto} from '../../../shared/dtos/leaderboard.dto';
import {LeaderboardPosDto} from '../../../shared/dtos/leaderboard-pos.dto';

@Component({
  templateUrl: './leaderboard-standings.component.html',
  styleUrls: ['leaderboard-standings.component.scss']
})
export class LeaderboardStandingsComponent implements OnInit {
  /**
   * The leaderboards a user can view additional data about
   */
  public _leaderboards: LeaderboardDto[];

  /**
   * The leaderboard a user is currently viewing the details of
   */
  public _currentBoard: LeaderboardDto = null;

  public _positions: LeaderboardPosDto[];

  constructor(private _leaderboardApi: LeaderboardApi) {
  }

  /**
   * @inheritDoc
   */
  public async ngOnInit(): Promise<void> {
    this._leaderboards = await this._leaderboardApi.getAllLeaderboards();
  }

  /**
   * Event handler for when a leaderboard is chosen in the template
   * @param board
   */
  public async _onShowBoardStandings(board: LeaderboardDto): Promise<void> {
    this._positions =
      await this._leaderboardApi.getAllLeaderboardPositions(board.leaderboardID);
    this._currentBoard = board;
  }
}
