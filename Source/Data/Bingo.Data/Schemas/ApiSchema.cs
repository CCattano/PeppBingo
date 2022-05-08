using Pepp.Web.Apps.Bingo.Data.Repos.API;

namespace Pepp.Web.Apps.Bingo.Data.Schemas
{
    /// <summary>
    /// Interface containing repos to access the tables associated with the API schema
    /// </summary>
    public interface IApiSchema
    {
        /// <inheritdoc cref="ISecretRepo"/>
        ISecretRepo SecretRepo { get; }
    }

    /// <inheritdoc cref="IApiSchema"/>
    internal class ApiSchema : BaseSchema, IApiSchema
    {
        private ISecretRepo _secretRepo;

        public ApiSchema(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public ISecretRepo SecretRepo => _secretRepo ??= new SecretRepo(base.DataSvc);
    }
}
