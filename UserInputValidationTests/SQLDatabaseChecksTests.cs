using NUnit.Framework;
using NSubstitute;
using FluentAssertions;


namespace UserInputValidationTests
{
    [TestFixture]
    public class SQLDatabaseChecks
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void AreDateTimesLessThanSQLDatabaseMinimumBeingMarkedTrue()
        {
            // arrange
            bool result = true;
            DateTime DateToCheck = new(1752, 12, 12, 23, 59, 59);

            // act
            bool a = UserInputValidation.SQLDatabaseChecks.IsLowerEqualThanSQLDatabaseMinimum(DateToCheck);

            // assert
            a.Should().Be(result);
        }

        [Test]
        public void AreDateTimesEqualToSQLDatabaseMinimumBeingMarkedTrue()
        {
            // arrange
            bool result = true;
            DateTime DateToCheck = new(1753, 01, 01, 00, 00, 00);

            // act
            bool a = UserInputValidation.SQLDatabaseChecks.IsLowerEqualThanSQLDatabaseMinimum(DateToCheck);

            // assert
            a.Should().Be(result);
        }

        [Test]
        public void AreDateTimesGreaterThanSQLDatabaseMinimumBeingMarkedFalse()
        {
            // arrange
            bool result = false;
            DateTime DateToCheck = new(1753, 01, 01, 00, 00, 01);

            // act
            bool a = UserInputValidation.SQLDatabaseChecks.IsLowerEqualThanSQLDatabaseMinimum(DateToCheck);

            // assert
            a.Should().Be(result);
        }
    }
}
