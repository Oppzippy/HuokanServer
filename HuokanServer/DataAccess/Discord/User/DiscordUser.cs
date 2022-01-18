using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

namespace HuokanServer.DataAccess.Discord.User
{
	/// <summary>Provides a high level interface for obtaining information about a discord user.
	/// Ensure Authenticate is called before doing anything.</summary>
	public class DiscordUser : IDiscordUser
	{
		private readonly IDiscordUserAuthenticationHandler _authenticationHandler;
		private readonly string _token;
		private DiscordRestClient _discord;

		public DiscordUser(IDiscordUserAuthenticationHandler authenticationHandler, string token)
		{
			_authenticationHandler = authenticationHandler;
			_token = token;
		}


		public async Task<ulong> GetId()
		{
			await Authenticate();
			return _discord.CurrentUser.Id;
		}

		public async Task<List<ulong>> GetGuildIds()
		{
			await Authenticate();
			IEnumerable<DiscordGuild> guilds = await _discord.GetCurrentUserGuildsAsync();
			return guilds.Select(guild => guild.Id).ToList();
		}

		private async Task Authenticate()
		{
			if (_discord == null)
			{
				var configuration = new DiscordConfiguration()
				{
					Token = _token,
					TokenType = TokenType.Bearer,
				};
				var discord = new DiscordRestClient(configuration);
				try
				{
					await discord.InitializeAsync();
				}
				catch (UnauthorizedException)
				{
					string token = await _authenticationHandler.ForceRefreshToken();
					configuration.Token = token;
					discord = new DiscordRestClient(configuration);
					await discord.InitializeAsync();
				}
				_discord = discord;
			}
		}
	}

}
