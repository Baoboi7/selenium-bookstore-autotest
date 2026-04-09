using System.Data;
using NUnit.Framework;
using SeleniumProject.Base;
using SeleniumProject.Pages;
using SeleniumProject.Utilities;

namespace SeleniumProject.Tests
{
    [TestFixture]
    public class AuthTests : BaseTest
    {
        private static string ExcelFilePath =>
            Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "AuthData.xlsx");

        public static IEnumerable<TestCaseData> RegisterTestData()
        {
            DataTable table = ExcelReader.GetSheet(ExcelFilePath, "RegisterData");

            foreach (DataRow row in table.Rows)
            {
                if (row["Run"].ToString()?.Trim().ToUpper() != "Y")
                    continue;

                yield return new TestCaseData(
                    row["TestCaseID"].ToString(),
                    row["FullName"].ToString(),
                    row["Email"].ToString(),
                    row["Phone"].ToString(),
                    row["Password"].ToString(),
                    row["ConfirmPassword"].ToString(),
                    row["AcceptTerms"].ToString(),
                    row["ExpectedMessage"].ToString()
                ).SetName(row["TestCaseID"].ToString());
            }
        }

        public static IEnumerable<TestCaseData> LoginTestData()
        {
            DataTable table = ExcelReader.GetSheet(ExcelFilePath, "LoginData");
            foreach (DataColumn col in table.Columns)
            {
                TestContext.Progress.WriteLine($"COL=[{col.ColumnName}]");
            }

            foreach (DataRow row in table.Rows)
            {
                if (row["Run"].ToString()?.Trim().ToUpper() != "Y")
                    continue;

                yield return new TestCaseData(
                    row["TestCaseID"].ToString(),
                    row["EmailOrPhone"].ToString(),
                    row["Password"].ToString(),
                    row["ExpectedMessage"].ToString()
                ).SetName(row["TestCaseID"].ToString());
            }
        }

        [Test, TestCaseSource(nameof(RegisterTestData))]
        public void Register_DataDriven(
            string testCaseId,
            string fullName,
            string email,
            string phone,
            string password,
            string confirmPassword,
            string acceptTerms,
            string expectedMessage)
        {
            var registerPage = new RegisterPage(driver);
            registerPage.Open();

            if (email.Equals("AUTO_GENERATE", StringComparison.OrdinalIgnoreCase))
            {
                email = $"autotest_{DateTime.Now:yyyyMMddHHmmss}@mail.com";
            }

            bool isAcceptTerms = acceptTerms.Equals("TRUE", StringComparison.OrdinalIgnoreCase);

            registerPage.Register(fullName, email, phone, password, confirmPassword, isAcceptTerms);

            switch (expectedMessage.Trim().ToLower())
            {
                case "success":
                    Assert.That(registerPage.IsRegisterSuccessful("/login"), Is.True,
                        $"{testCaseId} thất bại: đăng ký thành công nhưng không chuyển về /login.");
                    break;

                case "email exists":
                    Assert.That(registerPage.GetErrorMessage(), Does.Contain("tồn tại").IgnoreCase
                        .Or.Contain("email").IgnoreCase);
                    break;

                case "invalid email":
                    Assert.That(registerPage.GetErrorMessage(), Does.Contain("email").IgnoreCase
                        .Or.Contain("hợp lệ").IgnoreCase
                        .Or.Contain("không đúng").IgnoreCase);
                    break;

                case "password mismatch":
                    Assert.That(registerPage.GetErrorMessage(), Does.Contain("không khớp").IgnoreCase
                        .Or.Contain("mật khẩu").IgnoreCase
                        .Or.Contain("xác nhận").IgnoreCase);
                    break;

                default:
                    Assert.Fail($"ExpectedMessage không hợp lệ: {expectedMessage}");
                    break;

                case "weak password":
                    Assert.That(registerPage.GetErrorMessage(), Does.Contain("ít nhất 6 ký tự").IgnoreCase
                        .Or.Contain("chữ hoa").IgnoreCase
                        .Or.Contain("ký tự đặc biệt").IgnoreCase);
                    break;

                case "required fields":
                    Assert.That(registerPage.GetErrorMessage(), Does.Contain("vui lòng nhập đầy đủ thông tin").IgnoreCase);
                    break;
            }
        }

        [Test, TestCaseSource(nameof(LoginTestData))]
        public void Login_DataDriven(
            string testCaseId,
            string emailOrPhone,
            string password,
            string expectedMessage)
        {
            var loginPage = new LoginPage(driver);
            loginPage.Open();

            loginPage.Login(emailOrPhone, password);

            switch (expectedMessage.Trim().ToLower())
            {
                case "success":
                    Assert.That(loginPage.IsLoginSuccessful(), Is.True,
                        $"{testCaseId} thất bại: login chưa thành công.");
                    break;

                case "wrong password":
                    Assert.That(loginPage.GetErrorMessage(), Does.Contain("sai").IgnoreCase
                        .Or.Contain("không đúng").IgnoreCase
                        .Or.Contain("mật khẩu").IgnoreCase);
                    break;

                case "required email":
                    Assert.That(loginPage.GetErrorMessage(), Does.Contain("vui lòng nhập đầy đủ thông tin").IgnoreCase);
                    break;

                case "required password":
                    Assert.That(loginPage.GetErrorMessage(), Does.Contain("vui lòng nhập đầy đủ thông tin").IgnoreCase);
                    break;

                default:
                    Assert.Fail($"ExpectedMessage không hợp lệ: {expectedMessage}");
                    break;
            }
        }
    }
}