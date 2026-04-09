using System.Data;
using NUnit.Framework;
using SeleniumProject.Base;
using SeleniumProject.Pages;
using SeleniumProject.Utilities;

namespace SeleniumProject.Tests
{
    [TestFixture]
    public class ProfileTests : BaseTest
    {
        private static string ExcelFilePath =>
            Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProfileData.xlsx");

        [SetUp]
        public void ProfileSetUp()
        {
            var loginPage = new LoginPage(driver);
            loginPage.Open();
            loginPage.Login(ConfigReader.TestEmail, ConfigReader.TestPassword);
        }

        public static IEnumerable<TestCaseData> ProfileTestData()
        {
            DataTable table = ExcelReader.GetSheet(ExcelFilePath, "ProfileData");

            foreach (DataRow row in table.Rows)
            {
                if (row[5].ToString()?.Trim().ToUpper() != "Y")
                    continue;

                string testCaseId = row[0]?.ToString()?.Trim() ?? "";

                yield return new TestCaseData(
                    row[0]?.ToString(), // TestCaseID
                    row[1]?.ToString(), // FullName
                    row[2]?.ToString(), // Phone
                    row[3]?.ToString(), // Address
                    row[4]?.ToString()  // ExpectedMessage
                ).SetName(testCaseId);
            }
        }

        [Test, TestCaseSource(nameof(ProfileTestData))]
        public void Profile_DataDriven(
            string testCaseId,
            string fullName,
            string phone,
            string address,
            string expectedMessage)
        {
            var profilePage = new ProfilePage(driver);
            profilePage.Open();

            profilePage.UpdateProfile(fullName, phone, address);

            switch (expectedMessage.Trim().ToLower())
            {
                case "success":
                    Assert.That(profilePage.GetMessageText(), Does.Contain("thành công").IgnoreCase
                        .Or.Contain("cập nhật").IgnoreCase);
                    break;

                case "invalid_phone":
                    Assert.That(profilePage.GetMessageText(), Does.Contain("không hợp lệ").IgnoreCase
                        .Or.Contain("số điện thoại").IgnoreCase);
                    break;

                default:
                    Assert.Fail($"ExpectedMessage không hợp lệ: {expectedMessage}");
                    break;
            }
        }
    }
}