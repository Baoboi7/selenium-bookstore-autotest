using System.Data;
using NUnit.Framework;
using SeleniumProject.Base;
using SeleniumProject.Pages;
using SeleniumProject.Utilities;

namespace SeleniumProject.Tests
{
    [TestFixture]
    public class BookDetailTests : BaseTest
    {
        private static string ExcelFilePath =>
            Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "BookDetailData.xlsx");

        public static IEnumerable<TestCaseData> BookDetailTestData()
        {
            DataTable table = ExcelReader.GetSheet(ExcelFilePath, "BookDetailData");

            foreach (DataRow row in table.Rows)
            {
                if (row[4].ToString()?.Trim().ToUpper() != "Y")
                    continue;

                string testCaseId = row[0]?.ToString()?.Trim() ?? "";

                yield return new TestCaseData(
                    row[0]?.ToString(), 
                    row[1]?.ToString(), 
                    row[2]?.ToString(), 
                    row[3]?.ToString()  
                ).SetName(testCaseId);
            }
        }

        [Test, TestCaseSource(nameof(BookDetailTestData))]
        public void BookDetail_DataDriven(
            string testCaseId,
            string productUrl,
            string clickTimes,
            string expectedMessage)
        {
            driver.Navigate().GoToUrl(productUrl);
            var detailPage = new BookDetailPage(driver);

            int times = int.Parse(clickTimes);

            switch (expectedMessage.Trim().ToLower())
            {
                case "quantity_increase":
                    detailPage.ClickPlusQuantity(times);
                    Assert.That(detailPage.GetQuantityValue(), Is.GreaterThan(1));
                    break;

                case "quantity_increase_by_1":
                    int before = detailPage.GetQuantityValue();
                    detailPage.ClickPlusQuantity(times);
                    int after = detailPage.GetQuantityValue();

                    Assert.That(after, Is.EqualTo(before + 1));
                    break;

                case "price_display":
                    Assert.That(detailPage.GetBookPriceText(), Is.Not.Empty);
                    break;

                case "title_display":
                    Assert.That(detailPage.GetBookTitleText(), Is.Not.Empty);
                    break;

                default:
                    Assert.Fail($"ExpectedMessage không hợp lệ: {expectedMessage}");
                    break;
            }
        }
    }
}