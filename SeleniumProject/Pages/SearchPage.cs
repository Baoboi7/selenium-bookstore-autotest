using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumProject.Utilities;
using System.Collections.ObjectModel;
using System.Threading;

namespace SeleniumProject.Pages
{
    public class SearchPage
    {
        private readonly IWebDriver _driver;
        private readonly WaitHelpers _wait;
        private readonly string _url = $"{ConfigReader.BaseUrl}/books";

        public SearchPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WaitHelpers(driver);
        }

        private By HeaderSearchInput => By.XPath("/html/body/header/nav/div[2]/div[1]//input");
        private By LeftSearchInput => By.XPath("/html/body/main/div/aside/section[1]/input");
        private By CategoryDropdown => By.XPath("/html/body/main/div/aside/section[2]/select");
        private By SortDropdown => By.XPath("/html/body/main/div/section/div[1]/div[2]/select");
        private By PriceSliderTrack => By.XPath("/html/body/main/div/aside/section[3]/div");
        private By PriceSliderThumb => By.XPath("/html/body/main/div/aside/section[3]/div/input");
        private By BookTitleList => By.XPath("//h3[contains(@class,'line-clamp-2')]");

        private void Pause(int ms = 1500)
        {
            Thread.Sleep(ms);
        }

        public void Open()
        {
            _driver.Navigate().GoToUrl(_url);
            Pause(2000);
        }

        public void SearchByHeader(string keyword)
        {
            var input = _wait.WaitForElementVisible(HeaderSearchInput);
            input.Clear();
            Pause(1000);
            input.SendKeys(keyword);
            Pause(1000);
            input.SendKeys(Keys.Enter);
            Pause(2000);
        }

        public void SearchByLeft(string keyword)
        {
            var input = _wait.WaitForElementVisible(LeftSearchInput);
            input.Clear();
            Pause(1000);
            input.SendKeys(keyword);
            Pause(1000);
            input.SendKeys(Keys.Enter);
            Pause(2000);
        }

        public void SelectCategory(string categoryText)
        {
            var dropdown = new SelectElement(_wait.WaitForElementVisible(CategoryDropdown));
            dropdown.SelectByText(categoryText);
            Pause(2000);
        }

        public void SelectSort(string sortText)
        {
            var dropdown = new SelectElement(_wait.WaitForElementVisible(SortDropdown));

            if (sortText.Trim().Equals("Giá tăng dần", StringComparison.OrdinalIgnoreCase))
                dropdown.SelectByIndex(1);
            else if (sortText.Trim().Equals("Giá giảm dần", StringComparison.OrdinalIgnoreCase))
                dropdown.SelectByIndex(2);
            else
                dropdown.SelectByIndex(0);

            Pause(2000);
        }

        public void MovePriceSliderToValue(int targetPrice)
        {
            var sliderInput = _wait.WaitForElementVisible(PriceSliderThumb);

            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript(@"
        const slider = arguments[0];
        const value = arguments[1];

        slider.value = value;
        slider.dispatchEvent(new Event('input', { bubbles: true }));
        slider.dispatchEvent(new Event('change', { bubbles: true }));
    ", sliderInput, targetPrice);

            Pause(2000);
        }

        public List<string> GetAllBookTitles()
        {
            ReadOnlyCollection<IWebElement> elements = _driver.FindElements(BookTitleList);

            return elements
                .Where(e => !string.IsNullOrWhiteSpace(e.Text))
                .Select(e => e.Text.Trim())
                .ToList();
        }
    }
}