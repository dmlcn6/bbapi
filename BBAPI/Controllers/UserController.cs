using System;
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
				return Ok("User is not here");
			}
			return Ok(user);
		}

		/// <summary>
		/// Creates the user
		/// </summary>
		/// <returns>The user</returns>
		/// <param name="email">Email</param>

		[HttpPost]
		public IHttpActionResult PostUser(string email, [FromBody]string data)
		{
			//if not create user hash and set
			if (data == null)
			{
				var resp = "Data is null. Please send formatted data: ";
				var resp2 = "{name:name}"; 
				string emptyResponse = resp + resp2;
				return Ok(emptyResponse);
			}

			//create key
			var key = "user:" + email;



			//parse email and body data
			var name = data;
			var password = data;

			//check if email is taken in db
			//create hash for new user
			//store hash in Redis
			//send to RedisDB
			var response = RedisDB.createUserHash(key, name, email, password);

			//send error code
			if (response == -1)
			{
				return Ok("email is empty");
			}
			else if (response == -2)
			{
				return Ok("email is not vaild format");
			}
			else if (response == -3)
			{
				return Ok("email is already registered");
			}
			else if (response == -4)
			{
				return Ok("some try catch error");
			}
			else
			{
				return Ok("key sent is empty");
			}

			//store relation "hash" in SQLite
		}

		[HttpPut]
		public IHttpActionResult PutUser(string email, [FromBody]string data)
		{
			return Ok("you put" + data);
		}
	}
}
