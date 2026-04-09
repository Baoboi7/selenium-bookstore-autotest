using System.Data;
using NUnit.Framework;
using SeleniumProject.Base;
using SeleniumProject.Pages;
using SeleniumProject.Utilities;

namespace SeleniumProject.Tests
{
    [TestFixture]
    public class AdminTests : BaseTest
    {
        private static string ExcelFilePath =>
            Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "AdminData.xlsx");

        [SetUp]
        
        public void AdminSetUp()
        {
            var loginPage = new LoginPage(driver);
            loginPage.Open();
            loginPage.Login(ConfigReader.AdminEmail, ConfigReader.AdminPassword);
        }

        public static IEnumerable<TestCaseData> AdminTestData()
        {
            DataTable table = ExcelReader.GetSheet(ExcelFilePath, "AdminData");

            foreach (DataRow row in table.Rows)
            {
                if (row[7].ToString()?.Trim().ToUpper() != "Y")
                    continue;

                string testCaseId = row[0]?.ToString()?.Trim() ?? "";

                yield return new TestCaseData(
                    row[0]?.ToString(), // TestCaseID
                    row[1]?.ToString(), // Module
                    row[2]?.ToString(), // Keyword
                    row[3]?.ToString(), // NewName
                    row[4]?.ToString(), // NewPrice
                    row[5]?.ToString(), // Action
                    row[6]?.ToString()  // ExpectedMessage
                ).SetName(testCaseId);
            }
        }

        [Test, TestCaseSource(nameof(AdminTestData))]
        public void Admin_DataDriven(
            string testCaseId,
            string module,
            string keyword,
            string newName,
            string newPrice,
            string action,
            string expectedMessage)
        {
            var adminPage = new AdminPage(driver);

            switch (action.Trim().ToLower())
            {
                case "open_user":
                    adminPage.OpenUserManagement();
                    Assert.That(adminPage.IsUserListDisplayed(), Is.True);
                    break;

                case "search_user":
                    adminPage.OpenUserManagement();
                    adminPage.SearchUser(keyword);
                    Assert.That(adminPage.IsUserListDisplayed(), Is.True);
                    break;

                case "open_product":
                    adminPage.OpenProductManagement();
                    Assert.That(adminPage.IsProductListDisplayed(), Is.True);
                    break;

                case "edit_name":
                    adminPage.OpenProductManagement();
                    adminPage.ClickEditProduct();
                    adminPage.UpdateProductName(newName);
                    adminPage.ClickSave();
                    Assert.That(adminPage.GetSuccessMessageText(), Is.Not.Empty);
                    break;

                case "edit_price":
                    adminPage.OpenProductManagement();
                    adminPage.ClickEditProduct();
                    adminPage.UpdateProductPrice(newPrice);
                    adminPage.ClickSave();
                    Assert.That(adminPage.GetSuccessMessageText(), Is.Not.Empty);
                    break;

                case "delete_product":
                    adminPage.OpenProductManagement();
                    adminPage.ClickDeleteProduct();
                    Assert.That(adminPage.IsProductListDisplayed(), Is.True);
                    break;

                default:
                    Assert.Fail($"Action không hợp lệ: {action}");
                    break;
            }
        }
    }
}