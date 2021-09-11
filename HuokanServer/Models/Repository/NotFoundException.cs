using System;

namespace HuokanServer.Models.Repository
{
	public class NotFoundException : Exception
	{
		public NotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
