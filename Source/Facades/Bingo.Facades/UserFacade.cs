using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Data;
using Pepp.Web.Apps.Bingo.Data.Entities.User;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.Facades
{
    /// <summary>
    /// Facade for working with the information
    /// we store that is explicitly related to Users
    /// </summary>
    public interface IUserFacade
    {
        /// <summary>
        /// Updates an existing user if they
        /// exist otherwise inserts the user
        /// </summary>
        /// <param name="userBE"></param>
        /// <returns></returns>
        Task<UserBE> PersistUser(UserBE userBE);
        /// <summary>
        /// Fetches a User for the <paramref name="userID"/> provided
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<UserBE> GetUser(int userID);
        /// <summary>
        /// Fetches Users who's <see cref="UserBE.DisplayName"/> contains the <paramref name="displayName"/> provided
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The <paramref name="displayName"/> provided is treated as a substring and any users who's
        ///         <see cref="UserBE.DisplayName"/> contains the <paramref name="displayName"/> will be returned
        ///     </para>
        ///     <para>
        ///         Given a <paramref name="displayName"/> of "sti"
        ///         Then users, "Christian", "Stirling", and "Dustin"
        ///         could be returned as all contain, "sti"
        ///     </para>
        /// </remarks>
        /// <example>
        ///     Given a <paramref name="displayName"/> of "sti"
        ///     Then users, "Christian", "Stirling", and "Dustin"
        ///     could be returned as all contain, "sti"
        /// </example>
        /// <param name="displayName"></param>
        /// <returns></returns>
        Task<List<UserBE>> GetUsers(string displayName);
    }

    /// <inheritdoc cref="IUserFacade"/>
    public class UserFacade : IUserFacade
    {
        #region TEMP
        private const string TestProfileImageUri = @"https://static-cdn.jtvnw.net/jtv_user_pictures/28589ef5-43c3-468d-a839-f5c6f8bb4421-profile_image-300x300.png";
        private static readonly List<UserBE> TestUsers = new()
        {
            new() { DisplayName = "William Holt", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Michael Booth", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "David Mcgrath", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "John Simpson", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Brian Miles", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "James Todd", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Robert King", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Christopher Lee", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Matthew Walters", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Daniel Jacobson", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Mark Carver", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Kevin Figueroa", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Timothy Aguirre", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Eric Gates", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Jeffrey Mullen", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Richard Shah", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Steven Ramos", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Scott Rogers", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Joseph Crane", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Thomas Richard", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Todd Mcconnell", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Jason Reed", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Jonathan Sharpe", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Gary Montgomery", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Kenneth Clark", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Bryan Delgado", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Anthony Vincent", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Stephen Conner", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Paul Andersen", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Shawn Tyler", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Sean Davis", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Patrick Hendrix", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Larry Kelly", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Donald Schmidt", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "George Pope", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Craig Horton", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Andrew Watson", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Ronald Glover", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Peter Hernandez", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Charles Mckenzie", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Keith Fowler", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Edward Hartman", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Gregory Gay", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Douglas Brooks", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Aaron Cannon", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Chad Atkins", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Jeremy Ayers", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Adam Buckley", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Jose Stark", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Troy Moon", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Randy Bryan", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Jeffery Sanders", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Tony Ballard", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Dennis Moss", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Raymond Tate", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Terry Summers", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Johnny Schwartz", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Jerry Wright", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Bobby Dillon", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Carl Pennington", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Leslie Orozco", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Samuel Mata", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Frank Juarez", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Phillip Hines", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Rodney Wilcox", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Russell Crosby", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Bradley Mclaughlin", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Billy Contreras", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Danny Garrett", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Joel Haas", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Roger Oneal", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Jimmy Haines", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Benjamin Avila", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Curtis Elliott", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Marc James", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Jon Dawson", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Shane Goodman", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Brent Pham", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Chris Cross", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Corey Little", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Brett Morton", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Lawrence Mejia", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Derek Rowland", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Juan Boyer", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Derrick Jordan", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Carlos Delacruz", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Marcus Mccoy", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Walter Hodges", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Travis Ray", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Joshua Conway", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Ryan Valencia", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Philip Calderon", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Vincent House", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Erik Beck", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Wayne Neal", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Gerald Blankenship", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Brandon Hawkins", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Darren Ingram", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Alan Stewart", ProfileImageUri = TestProfileImageUri },
            new() { DisplayName = "Lance Richmond", ProfileImageUri = TestProfileImageUri }
        };
        #endregion
        private readonly IMapper _mapper;
        private readonly IBingoDataService _dataSvc;

        public UserFacade(IMapper mapper, IBingoDataService dataSvc)
        {
            _mapper = mapper;
            _dataSvc = dataSvc;
        }

        public async Task<UserBE> GetUser(int userID)
        {
            UserEntity userEntity = await _dataSvc.User.UserRepo.GetUser(userID);
            UserBE userBE = userEntity != null ? _mapper.Map<UserBE>(userEntity) : null;
            return userBE;
        }

        public async Task<List<UserBE>> GetUsers(string displayName)
        {
            //List<UserEntity> userEntities =
            //    await _dataSvc.User.UserRepo.GetUsers(displayName);
            //List<UserBE> userBEs =
            //    userEntities?.Select(user => _mapper.Map<UserBE>(user)).ToList();
            Regex namePattern = new($"({displayName})");
            ConcurrentBag<UserBE> userBag = new();
            ParallelQuery<UserBE> query =
                from user in TestUsers.AsParallel()
                where namePattern.IsMatch(user.DisplayName)
                select user;
            query.ForAll(user => userBag.Add(user));
            List<UserBE> userBEs = userBag.OrderBy(user => user.DisplayName).ToList();
            return await Task.FromResult(userBEs);
        }

        public async Task<UserBE> PersistUser(UserBE userBE)
        {
            // Look for existing user by TwitchUserID
            UserEntity userEntity = await _dataSvc.User.UserRepo.GetUser(userBE.TwitchUserID);
            if (userEntity == null)
            {
                // Token does not exist, make new entity from BE data
                userEntity = _mapper.Map<UserEntity>(userBE);
                // Insert BE data into Db
                await _dataSvc.User.UserRepo.InsertUser(userEntity);
                UserBE newUser = _mapper.Map<UserBE>(userEntity);
                return newUser;
            }
            // Map BE values onto Entity from Db to update entity contents
            userEntity = _mapper.Map(userBE, userEntity);
            // Post latest contents to Db
            await _dataSvc.User.UserRepo.UpdateUser(userEntity);
            UserBE updatedUser = _mapper.Map<UserBE>(userEntity);
            return updatedUser;
        }
    }
}
