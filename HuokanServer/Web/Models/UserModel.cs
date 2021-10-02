using System;
using HuokanServer.DataAccess.Repository.UserRepository;

namespace HuokanServer.Web.Models
{
	public class UserModel
	{
		public Guid Id { get; init; }
		public ulong DiscordUserId { get; init; }

		public static UserModel From(BackedUser user)
		{
			return new UserModel()
			{
				Id = user.Id,
				DiscordUserId = user.DiscordUserId,
			};
		}
	}
}
