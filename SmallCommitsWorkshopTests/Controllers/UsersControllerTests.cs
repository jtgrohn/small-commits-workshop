using System.Collections.Generic;
using System.Linq;
using System.Net;
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
		private User[] m_defaultUsers = new User[] {
				new User() { Id = 42, UserName = "JaneSmith", IsActive = true },
				new User() { Id = 156, UserName = "JoeSmith", IsActive = false },
			};

		[SetUp]
		public async Task SetUp() {
			m_factory = new WebApplicationFactory<Startup>();
			m_client = m_factory.CreateClient();
			m_scope = m_factory.Server?.Host.Services.CreateScope();
			m_usersContext = m_scope.ServiceProvider.GetService<UsersContext>();
			await AddUsers( m_defaultUsers );
		}

		[TearDown]
		public void TearDown() {
			m_scope.Dispose();
			m_client.Dispose();
			m_factory.Dispose();
		}

		[Test]
		public async Task GetAll_ReturnsUsers() {

			using( HttpResponseMessage response = await m_client.GetAsync( "/api/users" ) ) {
				CollectionAssert.AreEquivalent(
					m_defaultUsers.ToDictionary(
						user => user.Id,
						user => new Dictionary<string, object>( 2 ) {
							{ "id", user.Id },
							{ "userName", user.UserName },
							{ "isActive", user.IsActive }
						}
					),
					await response.Content.ReadAsJsonAsync<IDictionary<long, IDictionary<string, object>>>()
				);
			}
		}

		[Test]
		public async Task Get_ReturnsUser() {
			User user = m_defaultUsers.First();
			using( HttpResponseMessage response = await m_client.GetAsync( $"/api/users/{user.Id}" ) ) {
				Assert.AreEqual( response.StatusCode, HttpStatusCode.OK );
				CollectionAssert.AreEquivalent(
					new Dictionary<long, IDictionary<string, object>>( 2 ) {
						{
							user.Id,
							new Dictionary<string, object>( 2 ) {
								{ "id", user.Id },
								{ "userName", user.UserName },
								{ "isActive", user.IsActive }
							}
						}
					},
					await response.Content.ReadAsJsonAsync<IDictionary<long, IDictionary<string, object>>>()
				);
			}
		}

		private Task AddUsers( params User[] users ) {
			m_usersContext.Users.AddRange( users );
			return m_usersContext.SaveChangesAsync();
		}
	}
}
