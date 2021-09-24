using System;
using System.Security.Principal;

namespace HuokanServer.Web.Authentication
{
	public class ApiKeyPrincipal : IIdentity
	{
		public string AuthenticationType => throw new NotImplementedException();

		public bool IsAuthenticated => throw new NotImplementedException();

		public string Name => throw new NotImplementedException();
	}
}
