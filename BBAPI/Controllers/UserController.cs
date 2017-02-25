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
			//create key for cache
			var key = "user:" + email;

			//search for user hash w key in cache
			return Ok(RedisDB.getUserData(key));
		}

		/// <summary>
		/// Creates the user
		/// </summary>
		/// <returns>The user</returns>
		/// <param name="email">Email</param>

		[HttpPost]
		public IHttpActionResult PostUser(string email, [FromBody]string data)
		{
			//!!![FromBody]!!!
			//sets post body = data


			//check if body is empty, white space or null
			// or appropriate JSON fields are not in post body
			if (String.IsNullOrWhiteSpace(data) || String.Equals("{}",data) || !data.Contains("name:") || !data.Contains("password:"))
			{
				var resp = "Data is null. Please send formatted data: ";
				var resp2 = "\"{name:name, password:pw}\""; 
				string emptyResponse = resp + resp2;
				return Ok(emptyResponse);
			}

			//before any logic, make sure email is formatted and unique
			var emailVerfiyResponse = RedisDB.emailVerify(email);

			if (emailVerfiyResponse != 1)
			{
				//send error code
				switch (emailVerfiyResponse)
				{
					case -1:
						return Ok("email is empty");

					case -2:
						return Ok("email is not vaild format");

					case -3:
						return Ok("email is already registered");

					case -4:
						return Ok("some try catch error");
				}
			}

			//email is now verified as avail in cache
			//create key
			var key = "user:" + email;

			//parse email and body data
			string[] delimiterChars = {"name:", "password:", "{", "}"};
			string[] postParams = data.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);

			//if name or password fields are empty
			if (String.IsNullOrWhiteSpace(postParams[0]) || String.IsNullOrWhiteSpace(postParams[1])) 
			{ 
				return Ok("name and password fields are required");
			}

			//create hash for new user
			//store hash in Redis
			//send to RedisDB
			RedisDB.createUserHash(key, postParams[0], email, postParams[1]);

			var returnString = "user:" + postParams[0] + "pss:" + postParams[1];

			//user registered 200 OK HTTP response
			return Ok(returnString);

			//store relation "hash" in SQLite
		}

		[HttpPut]
		public IHttpActionResult PutUser(string email, [FromBody]string data)
		{
			return Ok("you put" + data);
		}
	}
}
