using YPermitin.FIASToolSet.DistributionBrowser.Extensions;

namespace YPermitin.FIASToolSet.DistributionBrowser.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void ToAbsoluteUri_Basic_Test()
        {
            string uriAsString = "https://www.google.com";
            Uri correctUri = new Uri(uriAsString);

            Uri parsedUri = uriAsString.ToAbsoluteUri();

            Assert.Equal(correctUri, parsedUri);
        }

        [Fact]
        public void ToAbsoluteUri_Null_Test()
        {
            string uriAsString = null;
            Uri correctUri = null;

            Uri parsedUri = uriAsString.ToAbsoluteUri();

            Assert.Equal(correctUri, parsedUri);
        }

        [Fact]
        public void ToAbsoluteUri_Empty_Test()
        {
            string uriAsString = string.Empty;
            Uri correctUri = null;

            Uri parsedUri = uriAsString.ToAbsoluteUri();

            Assert.Equal(correctUri, parsedUri);
        }
    }
}
