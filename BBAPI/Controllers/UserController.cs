﻿using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using BBAPI.Models;

namespace BBAPI.Controllers
{
	public class UserController : ApiController
	{
		User[] users = new User[]{
			new User {Email = "dlopez@me.com", Name = "me", Gender = "male", Age = 22},
			new User {Email = "d@me.com", Name = "you", Gender = "male", Age = 57},
			new User {Email = "dl@me.com", Name = "us", Gender = "male", Age = 100}
		};

		/// <summary>
		/// Gets all users.
		/// </summary>
		/// <returns>The all users.</returns>

		[HttpGet]
		public IEnumerable<User> GetAllUsers()
		{
			return users;
		}

		/// <summary>
		/// Gets the user.
		/// </summary>
		/// <returns>The user.</returns>
		/// <param name="email">Email.</param>

		[HttpGet]
		public IHttpActionResult GetUser(string email)
		{
			var user = users.FirstOrDefault((u) => u.Email == email);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user);
		}

		/// <summary>
		/// Creates the user.
		/// </summary>
		/// <returns>The user.</returns>
		/// <param name="email">Email.</param>

		[HttpPost]
		public IHttpActionResult CreateUser(string email, string data)
		{
			//check if email is correct format

			//check if email is taken in db

			//if not create user hash and set
			if (data != null)
			{
				var Newstring = data.ToString();
				data.
				return Ok(Newstring);
			}
			else return Ok("data empty");
		}
	}
}
