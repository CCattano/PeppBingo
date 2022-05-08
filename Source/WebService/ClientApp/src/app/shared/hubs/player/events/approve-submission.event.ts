export class ApproveSubmissionEvent
{
  /*
   * The internal UserID of the user who responded to the bingo
   */
  public userID: number;
  /*
   * The display name of the user who responded to the bingo
   */
  public displayName: string;
  /*
   * The profile image uri of the use who responded to the bingo
   */
  public profileImageUri: string;
}
