using Xunit;
using MainHub.Internal.PeopleAndCulture.Common;

namespace MainHub.Internal.PeopleAndCulture
{
    public class ValidatorTest
    {
        [Theory]
        [InlineData("+351-123-456-789")]  // Valid phone number
        [InlineData("+123-456-789")]      // Invalid phone number
        public void VerifyPhoneNumber_ValidAndInvalidNumbers_ReturnsExpected(string phoneNumber)
        {
            bool isValid = Validator.VerifyPhoneNumber(phoneNumber);

            if (phoneNumber.StartsWith("+351"))
            {
                Assert.True(isValid);
            }
            else
            {
                Assert.False(isValid);
            }
        }

        [Theory]
        [InlineData("test@example.com")]   // Valid email
        [InlineData("test.example.com")]    // Invalid email
        public void VerifyEmail_ValidAndInvalidEmails_ReturnsExpected(string email)
        {
            bool isValid = Validator.VerifyEmail(email);

            if (email == "test@example.com")
            {
                Assert.True(isValid);
            }
            else
            {
                Assert.False(isValid);
            }
        }

        [Theory]
        [InlineData("123456789")]   // Valid tax number
        [InlineData("12345")]       // Invalid tax number
        public void VerifyTaxNumber_ValidAndInvalidTaxNumbers_ReturnsExpected(string tax)
        {
            bool isValid = Validator.VerifyTaxNumber(tax);

            if (tax == "123456789")
            {
                Assert.True(isValid);
            }
            else
            {
                Assert.False(isValid);
            }
        }
    }
}
