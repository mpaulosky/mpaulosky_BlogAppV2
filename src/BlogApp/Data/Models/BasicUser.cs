﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     BasicUser.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : BlogServiceApp
// Project Name :  BlogService.Library
// =============================================

namespace BlogApp.Data.Models;

/// <summary>
///   BasicUser class
/// </summary>
[Serializable]
public class BasicUser
{
	
	/// <summary>
	///   Initializes a new instance of the <see cref="BasicUser" /> class.
	/// </summary>
	public BasicUser()
	{
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BasicUser" /> class.
	/// </summary>
	/// <param name="user">The user.</param>
	public BasicUser(ApplicationUser user)
	{
		Id = user.Id;
		DisplayName = user.DisplayName;
		EmailAddress = user.Email!;

	}

	/// <summary>
	///   Gets the identifier.
	/// </summary>
	/// <value>
	///   The identifier.
	/// </value>
	public string Id { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the display name.
	/// </summary>
	/// <value>
	///   The display name.
	/// </value>
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the email address.
	/// </summary>
	/// <value>
	///   The email address.
	/// </value>
	public string EmailAddress { get; set; } = string.Empty;
	
}