namespace TaskWebApi
{
	public class AuthenticationIdentity
	{
		public string identityProvider { get; set; }

		public bool force { get; set; } = false;
	}
}
