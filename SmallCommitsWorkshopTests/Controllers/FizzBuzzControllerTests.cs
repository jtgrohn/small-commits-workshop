using Moq;
using NUnit.Framework;
using SmallCommitsWorkshop.Controllers;
using SmallCommitsWorkshop.Services;

namespace SmallCommitsWorkshopTests.Controllers {
	[TestFixture]
	internal sealed class FizzBuzzControllerTests {

		[Test]
		public void Get_CallsCalculate() {
			const int number = 5;
			const string calculatedValue = "Buzz";
			Mock<IFizzBuzzService> fizzBuzzService = new Mock<IFizzBuzzService>( MockBehavior.Strict );
			fizzBuzzService
				.Setup( x => x.Calculate( number, "Fizz", "Buzz" ) )
				.Returns( calculatedValue );

			using( FizzBuzzController sut = new FizzBuzzController( fizzBuzzService: fizzBuzzService.Object ) ) {

				Assert.AreEqual(
					expected: calculatedValue,
					actual: sut.Get( number: number )
				);
			}
		}
	}
}
