using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SmallCommitsWorkshop;
using SmallCommitsWorkshop.Models;
using SmallCommitsWorkshopTests.Extensions;

namespace SmallCommitsWorkshopTests.Controllers {
	[TestFixture]
	internal sealed class UsersControllerTests {
		private WebApplicationFactory<Startup> m_factory;
		private HttpClient m_client;
		private IServiceScope m_scope;
		private UsersContext m_usersContext;

		[SetUp]
		public void SetUp() {
			m_factory = new WebApplicationFactory<Startup>();
			m_client = m_factory.CreateClient();
			m_scope = m_factory.Server?.Host.Services.CreateScope();
			m_usersContext = m_scope.ServiceProvider.GetService<UsersContext>();
		}

		[TearDown]
		public void TearDown() {
			m_scope.Dispose();
			m_client.Dispose();
			m_factory.Dispose();
		}

		[Test]
		public async Task GetAll_ReturnsUsers() {
			User[] users = new User[] {
				new User() { Id = 42, UserName = "JaneSmith", Buzzword = "ProbablyALady" },
				new User() { Id = 156, UserName = "JoeSmith", Buzzword = "ProbablyADude" },
			};

			foreach( User user in users ) {
				m_usersContext.Users.Add( user );
			}
			await m_usersContext.SaveChangesAsync();

			using( HttpResponseMessage response = await m_client.GetAsync( "/api/users" ) ) {
				CollectionAssert.AreEquivalent(
					users.ToDictionary(
						user => user.Id,
						user => new Dictionary<string, object>( 2 ) {
							{ "id", user.Id },
							{ "userName", user.UserName },
							{ "buzzword", user.Buzzword }
						}
					),
					await response.Content.ReadAsJsonAsync<IDictionary<long, IDictionary<string, object>>>()
				);
			}
		}
	}
}
