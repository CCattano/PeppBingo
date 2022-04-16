using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Facades;
using Pepp.Web.Apps.Bingo.Managers;
using System.Net;
using System.Threading.Tasks;
using WebException = Pepp.Web.Apps.Bingo.Infrastructure.Exceptions.WebException;

namespace Pepp.Web.Apps.Bingo.Adapters
{
    public interface IUserAdapter
    {
        /// <summary>
        /// Fetches a user by the JSON web token provided
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<UserBE> GetUser(string jwt);
    }

    /// <inheritdoc cref="IUserAdapter"/>
    public class UserAdapter : IUserAdapter
    {
        private readonly IUserFacade _facade;

        public UserAdapter(IUserFacade facade) => _facade = facade;

        public async Task<UserBE> GetUser(string jwt)
        {
            int userID = TokenManager.GetUserIDFromToken(jwt);
            UserBE user = await _facade.GetUser(userID);
            if (user == null)
                throw new WebException(HttpStatusCode.NotFound, "User not found for ID provided");
            return user;
        }
    }
}
