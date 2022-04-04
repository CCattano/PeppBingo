using Pepp.Web.Apps.Bingo.Data.Repos.User;

namespace Pepp.Web.Apps.Bingo.Data.Schemas
{
    /// <summary>
    /// Interface containing repos to access the tables associated with the Users schema
    /// </summary>
    public interface IUserSchema
    {
        /// <inheritdoc cref="IUserRepo"/>
        IUserRepo UserRepo { get; }
    }

    /// <inheritdoc cref="IUserSchema"/>
    public class UserSchema : BaseSchema, IUserSchema
    {
        private IUserRepo _userRepo;

        public UserSchema(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public IUserRepo UserRepo { get => _userRepo ??= new UserRepo(base.DataSvc); }
    }
}
