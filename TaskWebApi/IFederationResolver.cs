namespace TaskWebApi
{
    public interface IFederationResolver
	{
		Task<AuthenticationIdentity?> Resolve(string domain);
	}
}
