using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// to hash and salt pword: 
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace BBAPI.Controllers
{
    class RedisDB 
    {
        private static string windowsVMCache = string.Format("{0}:{1}", "localhost",6379);

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(windowsVMCache);
        });
        
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        private static IDatabase cache = Connection.GetDatabase();

        public static void createUserHash(string key, string name, string email, string password)
        {
     		//no need to check email here, check in controller
			SHA512 sha512Hash = SHA512.Create();
			string salt = Guid.NewGuid().ToString();
			string saltedPassword = password + salt;
			saltedPassword = GetSha512Hash(sha512Hash, saltedPassword);

			cache.HashSet(key, new HashEntry[] { new HashEntry("name", name), new HashEntry("email", email), new HashEntry("password", saltedPassword) });

        }
        public static void updateUserHash(string key, string updatedField, string newValue)
        {
            cache.HashSet(key, new HashEntry[] {new HashEntry(updatedField, newValue)});
        }
        
        public static string getUserData(string key)
        {
            var data = cache.HashGetAll(key);
			return data.ToString();
        }
		/*
        public static bool StoreData( string key, string value)
        {
            return cache.StringSet(key, value);
        }

        private static void DeleteData(IDatabase cache, string key)
        {
            cache.KeyDelete(key);
        }
        */

        //close connection needed

        //check email validation
        public static int emailVerify(string email)
        {
			var key = "user:" + email;
			 
            //check if fields are empty
            if(String.IsNullOrEmpty(key))
            {
                //send error message
                return -1;
            }

			try
			{
				var mail = new MailAddress(email);

				if (mail.Host.Contains("."))
				{
					//check if unique emailaddress
					if (cache.KeyExists(key))
					{
						//email taken
						//send error code for email taken
						return -3;
					}
					else
					{
						return 1;
					}
				}
				else
				{
					return -2;
				}
			}
			catch
			{
				return -4;
			}
            
        }

        //Compute a hash using the Sha512 algorithm
        private static string GetSha512Hash(SHA512 sha512Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
