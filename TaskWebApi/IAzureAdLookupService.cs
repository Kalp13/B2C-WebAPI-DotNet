namespace TaskWebApi
{
    public interface IAzureAdLookupService
	{
		Task<AzureADB2CUserQueryResponse> Lookup(string email);
	}
}
