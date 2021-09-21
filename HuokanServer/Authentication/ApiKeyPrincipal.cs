using System;
using System.Security.Principal;

namespace HuokanServer.Authentication
{
	public class ApiKeyPrincipal : IIdentity
	{
		public string AuthenticationType => throw new NotImplementedException();

		public bool IsAuthenticated => throw new NotImplementedException();

		public string Name => throw new NotImplementedException();
	}
}
