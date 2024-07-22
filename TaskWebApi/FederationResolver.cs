namespace TaskWebApi
{
    public class FederationResolver : IFederationResolver
	{
		private readonly IConfiguration _configuration;

		public FederationResolver(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public Task<AuthenticationIdentity?> Resolve(string domain)
		{
			var identityProviderResult = this._configuration.GetValue<string>($"SingleSignOn:Federation:Domain:Provider:{domain}");
			var forceResult = this._configuration.GetValue<bool>($"SingleSignOn:Federation:Domain:Force:{domain}");
			return Task.FromResult<AuthenticationIdentity?>(new AuthenticationIdentity()
			{
				identityProvider = identityProviderResult,
				force = forceResult
			});
		}
	}
}
