using System;

namespace HuokanServer.DataAccess.Repository;

public class DuplicateItemException : Exception
{
	public DuplicateItemException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
