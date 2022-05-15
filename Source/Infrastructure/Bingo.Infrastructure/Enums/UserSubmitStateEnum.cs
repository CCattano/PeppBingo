namespace Pepp.Web.Apps.Bingo.Infrastructure.Enums
{
    public enum UserSubmissionStatus {
        /// User is able to submit a bingo for leaderboard advancement
        CanSubmitBingo = 0,
        /// User has already submitted a bingo for leaderboard advancement
        AlreadySubmitted = 1,
        /// User cannot submit a bingo for leaderboard advancement
        CannotSubmitBingo = 2
    }
}