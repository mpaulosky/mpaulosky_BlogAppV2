// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     UserCreator.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : BlogServiceApp
// Project Name :  BlogService.Library
// =============================================

namespace BlogApp.Data.FakerCreators;

/// <summary>
///   FakeUser class
/// </summary>
public static class UserCreator
{
	/// <summary>
	///   Gets a new user.
	/// </summary>
	/// <param name="keepId">bool whether to keep the generated Id</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>User</returns>
	public static ApplicationUser GetNewUser(bool keepId = false, bool useNewSeed = false)
	{
		ApplicationUser user = GenerateFake(useNewSeed).Generate();

		if (keepId)
		{
			return user;
		}

		user.Id = string.Empty;
		user.IsArchived = false;

		return user;
	}

	/// <summary>
	///   Gets a list of users.
	/// </summary>
	/// <param name="numberOfUsers">The number of users.</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A List of Users</returns>
	public static List<ApplicationUser> GetUsers(int numberOfUsers, bool useNewSeed = false)
	{
		List<ApplicationUser> users = GenerateFake(useNewSeed).Generate(numberOfUsers);

		return users;
	}

	/// <summary>
	///   GenerateFake method
	/// </summary>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>Fake User</returns>
	private static Faker<ApplicationUser> GenerateFake(bool useNewSeed = false)
	{
		var seed = 0;
		if (useNewSeed)
		{
			seed = Random.Shared.Next(10, int.MaxValue);
		}

		return new Faker<ApplicationUser>()
			.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
			.RuleFor(x => x.DisplayName, (f) => f.Internet.UserName())
			.RuleFor(x => x.Email, (f, u) => f.Internet.Email(u.DisplayName))
			.RuleFor(f => f.IsArchived, f => f.Random.Bool())
			.UseSeed(seed);
	}
}