using System.Data;
using NUnit.Framework;
using SeleniumProject.Base;
using SeleniumProject.Pages;
using SeleniumProject.Utilities;

namespace SeleniumProject.Tests
{

    [TestFixture]
    public class CartTests : BaseTest
    {
        private static string ExcelFilePath =>
            Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "CartData.xlsx");

        public static IEnumerable<TestCaseData> CartTestData()
        {
            DataTable table = ExcelReader.GetSheet(ExcelFilePath, "CartData");

            foreach (DataRow row in table.Rows)
            {
                if (row[5].ToString()?.Trim().ToUpper() != "Y")
                    continue;
                string testCaseId = row["TestCaseID"]?.ToString()?.Trim() ?? "";

                yield return new TestCaseData(
     row[0]?.ToString(), // TestCaseID
     row[1]?.ToString(), // ProductUrl
     row[2]?.ToString(), // Quantity
     row[3]?.ToString(), // Action
     row[4]?.ToString()  // ExpectedMessage
 ).SetName(testCaseId);
            }
        }

        [Test, TestCaseSource(nameof(CartTestData))]
        public void Cart_DataDriven(
            string testCaseId,
            string productUrl,
            string quantity,
            string action,
            string expectedMessage)
        {
            var cartPage = new CartPage(driver);

            driver.Navigate().GoToUrl(productUrl);

            int qty = int.Parse(quantity);

            if (qty > 1)
                cartPage.ClickPlusOnDetailPage(qty - 1);

            cartPage.ClickAddToCart();
            cartPage.ClickCartIcon();

            switch (action.Trim().ToLower())
            {
                case "add":
                    Assert.That(cartPage.GetCartItemCount(), Is.GreaterThan(0));
                    break;

                case "remove":
                    cartPage.RemoveItemFromCart();
                    Assert.That(cartPage.IsEmptyCartMessageDisplayed() || cartPage.IsCartEmpty(), Is.True);
                    break;

                case "increase_in_cart":
                    string subtotalBefore = cartPage.GetSubtotalText();
                    cartPage.IncreaseQuantityInCart(1);
                    string subtotalAfter = cartPage.GetSubtotalText();

                    Assert.That(subtotalAfter, Is.Not.EqualTo(subtotalBefore));
                    break;

                case "subtotal":
                    Assert.That(cartPage.GetSubtotalText(), Is.Not.Empty);
                    break;

                default:
                    Assert.Fail($"Action không hợp lệ: {action}");
                    break;
            }
        }

        [SetUp]
        public void CartSetUp()
        {
            var loginPage = new LoginPage(driver);
            loginPage.Open();
            loginPage.Login(ConfigReader.TestEmail, ConfigReader.TestPassword);
        }
    }
}