namespace Pepp.Web.Apps.Bingo.Data.Schemas
{
    public abstract class BaseSchema
    {
        protected readonly BaseDataService DataSvc;

        protected BaseSchema(BaseDataService dataSvc)
        {
            DataSvc = dataSvc;
        }
    }
}
