using System;

namespace HuokanServer.Models.Repository
{
	public class DuplicateItemException : Exception
	{
		public DuplicateItemException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
