using NUnit.Framework;
using SmallCommitsWorkshop.Services;

namespace SmallCommitsWorkshopTests.Services {
	[TestFixture]
	internal sealed class FizzBuzzServiceTests {
		private readonly IFizzBuzzService m_fizzBuzzService = new FizzBuzzService();

		[TestCase( 1 )]
		[TestCase( 2 )]
		[TestCase( 13 )]
		[TestCase( 9998 )]
		public void Calculate_IsNotDivisibleBy3Or5( int number ) {
			Assert.AreEqual(
				expected: number.ToString(),
				actual: m_fizzBuzzService.Calculate( number: number )
			);
		}

		[TestCase( 3 )]
		[TestCase( 123 )]
		[TestCase( 999999 )]
		public void Calculate_IsOnlyDivisibleBy3_ReturnsFizz( int number ) {
			Assert.AreEqual(
				expected: "Fizz",
				actual: m_fizzBuzzService.Calculate( number: number )
			);
		}

		[TestCase( 5 )]
		[TestCase( 125 )]
		[TestCase( 55555 )]
		public void Calculate_IsOnlyDivisibleBy5_ReturnsBuzz( int number ) {
			Assert.AreEqual(
				expected: "Buzz",
				actual: m_fizzBuzzService.Calculate( number: number )
			);
		}

		[TestCase( 0 )]
		[TestCase( 15 )]
		[TestCase( 135 )]
		public void Calculate_IsDivisibleBy3And5_ReturnsFizzBuzz( int number ) {
			Assert.AreEqual(
				expected: "FizzBuzz",
				actual: m_fizzBuzzService.Calculate( number: number )
			);
		}

		[TestCase( 15, "Buzz", "Fizz", "BuzzFizz" )]
		[TestCase( 3, "Tim", "Tam", "Tim" )]
		[TestCase( 5, "Apple", "Orange", "Orange" )]
		public void Calculate_OverridingDefaultFizzAndBuzzValues_ReturnsNonDefaultValues( int value, string fizz, string buzz, string expected ) {
			Assert.AreEqual(
				expected: expected,
				actual: m_fizzBuzzService.Calculate( number: 15, fizz: fizz, buzz: buzz )
			);
		}

	}
}
