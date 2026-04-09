using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using SeleniumProject.Base;
using SeleniumProject.Pages;
using SeleniumProject.Utilities;
using System.Data;

namespace SeleniumProject.Tests
{
    [TestFixture]
    public class SearchTests : BaseTest
    {
        private static string ExcelFilePath =>
            Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "SearchData.xlsx");

        public static IEnumerable<TestCaseData> SearchTestData()
        {
            DataTable table = ExcelReader.GetSheet(ExcelFilePath, "SearchData");

            foreach (DataRow row in table.Rows)
            {
                if (row[7].ToString()?.Trim().ToUpper() != "Y")
                    continue;

                string testCaseId = row["TestCaseID"]?.ToString()?.Trim() ?? "";
                TestContext.Progress.WriteLine($"Loaded: {testCaseId}");

                yield return new TestCaseData(
                  row[0]?.ToString(), // TestCaseID
                    row[1]?.ToString(), // SearchType
                   row[2]?.ToString(), // Keyword
                  row[3]?.ToString(), // Category
                  row[4]?.ToString(), // Sort
                  row[5]?.ToString(), // SliderAction
                    row[6]?.ToString()  // ExpectedMessage
                    ).SetName(testCaseId);
            }
        }

            [Test, TestCaseSource(nameof(SearchTestData))]
        public void Search_DataDriven(
            string testCaseId,
            string searchType,
            string keyword,
            string category,
            string sort,
            string sliderAction,
            string expectedMessage)
        {
            var searchPage = new SearchPage(driver);
            searchPage.Open();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                if (searchType.Trim().ToLower() == "left")
                    searchPage.SearchByLeft(keyword);
                else
                    searchPage.SearchByHeader(keyword);
            }

            if (!string.IsNullOrWhiteSpace(category))
                searchPage.SelectCategory(category);

            if (!string.IsNullOrWhiteSpace(sort))
                searchPage.SelectSort(sort);
            if (!string.IsNullOrWhiteSpace(sliderAction) && int.TryParse(sliderAction, out int targetPrice))
                searchPage.MovePriceSliderToValue(targetPrice);
            var titles = searchPage.GetAllBookTitles();

            switch (expectedMessage.Trim().ToLower())
            {
                case "has_result":
                    Assert.That(titles.Count, Is.GreaterThan(0));
                    break;

                case "no_result":
                    Assert.That(titles.Count == 0 || titles.All(t => !t.Contains(keyword, StringComparison.OrdinalIgnoreCase)), Is.True);
                    break;
                case "price_200k":
                    Assert.That(titles.Count, Is.GreaterThan(0));
                    break;

                default:
                    Assert.Fail($"ExpectedMessage không hợp lệ: {expectedMessage}");
                    break;
            }
        }
    }
}