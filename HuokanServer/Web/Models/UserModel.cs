using System;
using System.ComponentModel.DataAnnotations;
using HuokanServer.DataAccess.Repository.UserRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models;

public record UserModel
{
	[Required]
	[BindRequired]
	public Guid Id { get; init; }

	[Required]
	[BindRequired]
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