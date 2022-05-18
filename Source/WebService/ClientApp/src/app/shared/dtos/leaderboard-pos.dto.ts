export class LeaderboardPosDto {
  /**
   * The ID of the Leaderboard this position is assoc. w/
   */
  public LeaderboardID: number;

  /**
   * Twitch-provided display name for a User
   */
  public DisplayName: string;

  /**
   * Uri to fetch a User's Twitch profile image
   */
  public ProfileImageUri: string;

  /**
   * The number of bingos this user has for this bingo board
   */
  public BingoQty: number;
}
