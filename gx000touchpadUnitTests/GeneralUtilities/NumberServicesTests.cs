using System.Globalization;
using GeneralUtilities;

namespace gx000touchpadUnitTests.GeneralUtilities;

[TestFixture]
public class NumberServicesTests
{
    [SetUp]
    public void SetUp()
    {
    }
    
    [Test]
    [TestCase("1", "1", "nl-NL")]
    [TestCase("1.358,22", "1358,22","nl-NL")]
    [TestCase("1.369.258,789", "1369258,789", "nl-NL")]
    [TestCase("1.123.456.987", "1123456987", "nl-NL")]
    [TestCase("-1", "-1", "nl-NL")]
    [TestCase("-1.123,55", "-1123,55", "nl-NL")]
    [TestCase("1,358.22", "1358.22","en-US")]
    [TestCase("1,369,258.789", "1369258.789", "en_US")]
    [TestCase("-1,123.55", "-1123.55", "en_US")]
    public void UnformatNumber_PassStringWithNumber_ResultIsStringWithValidNumberWithoutFormatting(
        string input, string expectedResult, string cultureName)
    {
        var cultureInfo = new CultureInfo(cultureName);
        CultureInfo.CurrentCulture = cultureInfo;
        
        var result = NumberServices.UnformatNumber(input);
        
        Assert.That(result, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public void UnformatNumber_PassStringNoNumber_ThrowFormatException()
    {
        var test = "tes,567,test-";
        
        Assert.That(() => NumberServices.UnformatNumber(test), Throws.InstanceOf<FormatException>());
    }
    
    [Test]
    public void UnformatNumber_PassNull_ThrowArgumentNullException()
    {
        Assert.That(() => NumberServices.UnformatNumber(null), Throws.ArgumentNullException);
    }
    
    [Test]
    [TestCase("")]
    [TestCase(" ")]
    public void UnformatNumber_PassEmptyString_ThrowArgumentException(string test)
    {
        Assert.That(() => NumberServices.UnformatNumber(test), Throws.ArgumentException);
    }
}