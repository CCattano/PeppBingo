using Pepp.Web.Apps.Bingo.Data.Repos.API;

namespace Pepp.Web.Apps.Bingo.Data.Schemas
{
    /// <summary>
    /// Interface containing repos to access the tables associated with the API schema
    /// </summary>
    public interface IApiSchema
    {
        /// <inheritdoc cref="ISecretValueDetailDescRepo"/>
        ISecretValueDetailDescRepo SecretValueDetailDescRepo { get; }
    }

    /// <inheritdoc cref="IApiSchema"/>
    internal class ApiSchema : IApiSchema
    {
        private readonly BaseDataService _dataSvc;
        private ISecretValueDetailDescRepo _secretValueDetailDescRepo;

        public ApiSchema(BaseDataService dataSvc) => _dataSvc = dataSvc;

        public ISecretValueDetailDescRepo SecretValueDetailDescRepo { get => _secretValueDetailDescRepo ??= new SecretValueDetailDescRepo(_dataSvc); }
    }
}
