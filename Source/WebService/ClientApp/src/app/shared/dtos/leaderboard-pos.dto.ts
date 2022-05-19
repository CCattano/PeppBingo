export class LeaderboardPosDto {
  /**
   * The ID of the Leaderboard this position is assoc. w/
   */
  public leaderboardID: number;

  /**
   * Twitch-provided display name for a User
   */
  public displayName: string;

  /**
   * Uri to fetch a User's Twitch profile image
   */
  public profileImageUri: string;

  /**
   * The number of bingos this user has for this bingo board
   */
  public bingoQty: number;
}
