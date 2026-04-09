using System.Data;
using NUnit.Framework;
using SeleniumProject.Base;
using SeleniumProject.Pages;
using SeleniumProject.Utilities;

namespace SeleniumProject.Tests
{
    [TestFixture]
    public class CheckoutTests : BaseTest
    {
        private static string ExcelFilePath =>
            Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "CheckoutData.xlsx");

        [SetUp]
        public void CheckoutSetUp()
        {
            var loginPage = new LoginPage(driver);
            loginPage.Open();
            loginPage.Login(ConfigReader.TestEmail, ConfigReader.TestPassword);
        }

        public static IEnumerable<TestCaseData> CheckoutTestData()
        {
            DataTable table = ExcelReader.GetSheet(ExcelFilePath, "CheckoutData");

            foreach (DataRow row in table.Rows)
            {
                if (row[7].ToString()?.Trim().ToUpper() != "Y")
                    continue;

                string testCaseId = row[0]?.ToString()?.Trim() ?? "";

                yield return new TestCaseData(
                    row[0]?.ToString(), // TestCaseID
                    row[1]?.ToString(), // ProductUrl
                    row[2]?.ToString(), // FullName
                    row[3]?.ToString(), // Phone
                    row[4]?.ToString(), // Email
                    row[5]?.ToString(), // Address
                    row[6]?.ToString()  // ExpectedMessage
                ).SetName(testCaseId);
            }
        }

        [Test, TestCaseSource(nameof(CheckoutTestData))]
        public void Checkout_DataDriven(
            string testCaseId,
            string productUrl,
            string fullName,
            string phone,
            string email,
            string address,
            string expectedMessage)
        {
            var cartPage = new CartPage(driver);
            var checkoutPage = new CheckoutPage(driver);

            driver.Navigate().GoToUrl(productUrl);
            cartPage.ClickAddToCart();
            cartPage.ClickCartIcon();

            checkoutPage.GoToCheckout();
            string totalBeforeOrder = checkoutPage.GetTotalAmountText();

            checkoutPage.FillCheckoutForm(fullName, phone, email, address);
            checkoutPage.ClickPlaceOrder();

            switch (expectedMessage.Trim().ToLower())
            {
                case "success":
                    Assert.That(
                        checkoutPage.IsSuccessPage() ||
                        checkoutPage.GetSuccessHeadingText().Contains("Đặt hàng thành công", StringComparison.OrdinalIgnoreCase),
                        Is.True);
                    break;

                case "invalid_email":
                    Assert.That(checkoutPage.GetAlertTextAndAccept(), Does.Contain("Email không đúng định dạng"));
                    break;

                case "required_address":
                    Assert.That(checkoutPage.GetAlertTextAndAccept(), Does.Contain("địa chỉ").IgnoreCase
                        .Or.Contain("Vui lòng").IgnoreCase);
                    break;

                case "total_show":
                    Assert.That(totalBeforeOrder, Is.Not.Empty);
                    break;

                default:
                    Assert.Fail($"ExpectedMessage không hợp lệ: {expectedMessage}");
                    break;
            }
        }
    }
}