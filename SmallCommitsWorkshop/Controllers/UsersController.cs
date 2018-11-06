using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmallCommitsWorkshop.Models;

namespace SmallCommitsWorkshop.Controllers {
	[Route( "api/[controller]" )]
	public class UsersController : Controller {
		private readonly UsersContext m_usersContext;

		public UsersController( UsersContext usersContext ) {
			m_usersContext = usersContext;

			if( !m_usersContext.Users.Any() ) {
				m_usersContext.Users.Add( new User() { Id = 169, UserName = "D2LSupport", IsActive = true } );
				m_usersContext.Users.Add( new User() { Id = 175, UserName = "user1", IsActive = false } );
				m_usersContext.SaveChanges();
			}
		}

		[HttpGet]
		public async Task<ActionResult<IDictionary<long, User>>> GetAll() {
			IEnumerable<User> users = await m_usersContext.Users.ToListAsync();
			return users.ToDictionary( user => user.Id, user => user );
		}

		[HttpGet( "{userId}" )]
		public async Task<ActionResult<IDictionary<long, User>>> Get( long userId ) {
			User user = await m_usersContext.Users.FindAsync( userId );
			if( user == null ) {
				return NotFound();
			}
			return new Dictionary<long, User>( 1 ) { { user.Id, user } };
		}

		[HttpPost]
		public async Task<ActionResult> CreateOrUpdate( [FromBody] User newUser ) {
			User user = m_usersContext.Users.Find( newUser.Id );
			ActionResult response;
			if( user == null ) {
				m_usersContext.Add( newUser );
				response = Created( newUser.Id.ToString(), newUser );
			} else {
				user.UserName = newUser.UserName;
				m_usersContext.Update( user );
				response = NoContent();
			}
			
			await m_usersContext.SaveChangesAsync();
			return response;
		}
	}
}
