using System.Text;

namespace SmallCommitsWorkshop.Services {
	public class FizzBuzzService : IFizzBuzzService {
		string IFizzBuzzService.Calculate( int number, string fizz, string buzz ) {
			StringBuilder response = new StringBuilder();

			if( number % 3 == 0 ) {
				response.Append( fizz );
			}

			if( number % 5 == 0 ) {
				response.Append( buzz );
			}

			return response.Length == 0
				? number.ToString()
				: response.ToString();
		}
	}
}
