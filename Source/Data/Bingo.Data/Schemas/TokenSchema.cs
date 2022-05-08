using Pepp.Web.Apps.Bingo.Data.Repos.Token;

namespace Pepp.Web.Apps.Bingo.Data.Schemas
{
    /// <summary>
    /// Interface containing repos to access the tables associated with the API schema
    /// </summary>
    public interface ITokenSchema
    {
        /// <inheritdoc cref="ISecretRepo"/>
        ISecretRepo SecretRepo { get; }
    }

    /// <inheritdoc cref="ITokenSchema"/>
    class TokenSchema : BaseSchema, ITokenSchema
    {
        private ISecretRepo _secretRepo;

        public TokenSchema(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public ISecretRepo SecretRepo => _secretRepo ??= new SecretRepo(base.DataSvc);
    }
}
