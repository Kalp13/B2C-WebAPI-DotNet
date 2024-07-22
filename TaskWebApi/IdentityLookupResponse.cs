using System.Collections.Generic;

namespace TaskWebApi
{
	public class IdentityLookupResponse
	{
		public bool Exists { get; set; }
		public List<string> IdentityProviders { get; set; }
		public bool IsOnboarded { get; set; }
		public bool Force { get; set; }
	}
}
