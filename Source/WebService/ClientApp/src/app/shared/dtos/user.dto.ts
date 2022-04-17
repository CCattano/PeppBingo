export class UserDto {
  /**
   * Internal User ID to represent a User
   */
  public userID: number;
  /**
   * Twitch-provided display name for a User
   */
  public displayName: string;
  /**
   * Uri to fetch a User's Twitch profile image
   */
  public profileImageUri: string;
  /**
   * The user is an application Administrator
   */
  public isAdmin: boolean;
}
