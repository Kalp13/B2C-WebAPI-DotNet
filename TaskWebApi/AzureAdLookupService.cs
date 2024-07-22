namespace TaskWebApi
{
    public class AzureAdLookupService : IAzureAdLookupService
	{
		public AzureAdLookupService()
		{
			
		}

		public async Task<AzureADB2CUserQueryResponse> Lookup(string email)
		{
			return new AzureADB2CUserQueryResponse()
			{
				odatametadata = "test metadata",
				value = new List<User>()
				{
					new User()
					{
						accountEnabled = true,
						city = "Centurion",
						country = "ZA",
						displayName = "Rudolph Kalp",
						objectId = Guid.NewGuid().ToString()
					},
				}.ToArray(),
			};
		}
	}
}
