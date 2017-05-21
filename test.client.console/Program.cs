using System;
using System.Net.Http;

using IdentityModel.Client;

namespace test.client.console
{
	class Program
	{
		static void Main(string[] args)
		{
			TokenResponse clientToken = GetClientToken();

			CallApi(clientToken);

			TokenResponse userToken = GetUserToken();

			CallApi(userToken);

			Console.ReadLine();
		}

		static TokenResponse GetUserToken()
		{
			TokenClient client = new TokenClient(
				 "http://localhost:30080/connect/token",
				 "carbon",
				 "21B5F798-BE55-42BC-8AA8-0025B903DC3B");

			return client.RequestResourceOwnerPasswordAsync("dgago", "talleres", "api1").Result;
		}

		static TokenResponse GetClientToken()
		{
			TokenClient client = new TokenClient(
				 "http://localhost:30080/connect/token",
				 "silicon",
				 "F621F470-9731-4A25-80EF-67A6F7C5F4B8");

			return client.RequestClientCredentialsAsync("api1").Result;
		}

		static void CallApi(TokenResponse token)
		{
			HttpClient client = new HttpClient();
			client.SetBearerToken(token.AccessToken);

			try
			{
				Console.WriteLine(client.GetStringAsync("http://localhost:31080/test").Result);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			try
			{
				Console.WriteLine(client.GetStringAsync("http://localhost:31080/settings").Result);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

	}
}
