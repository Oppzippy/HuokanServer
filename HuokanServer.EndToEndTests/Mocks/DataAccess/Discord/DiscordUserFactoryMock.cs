using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Discord;

namespace HuokanServer.EndToEndTests.Mocks.DataAccess.Discord
{
	public class DiscordUserFactoryMock : IDiscordUserFactory
	{
		public Task<IDiscordUser> Create(Guid userId)
		{
			return Task.FromResult<IDiscordUser>(null);
		}

		public Task<IDiscordUser> Create(string token)
		{
			return Task.FromResult<IDiscordUser>(null);
		}
	}
}
