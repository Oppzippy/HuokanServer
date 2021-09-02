using HuokanServer.Models.Repository.UserRepository;

namespace HuokanServer.Models.Accessors
{
	public interface IUserAccessor
	{
		BackedUser User { get; }
	}
}
