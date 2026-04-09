using OpenQA.Selenium;
using SeleniumProject.Utilities;
using System.Collections.ObjectModel;
using System.Threading;

namespace SeleniumProject.Pages
{
    public class AdminPage
    {
        private readonly IWebDriver _driver;
        private readonly WaitHelpers _wait;

        public AdminPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WaitHelpers(driver);
        }

        // User Management
        private By UserMenu => By.XPath("/html/body/div[2]/aside/nav/a[4]");
        private By UserSearchInput => By.XPath("/html/body/div[2]/main/div/div[1]/div[1]/div[2]/input");
        private By UserSearchButton => By.XPath("/html/body/div[2]/main/div/div[1]/div[1]/div[2]/button");
        private By UserList => By.XPath("/html/body/div[2]/main/div/div[2]/div[1]/table");

        // Product Management
        private By ProductMenu => By.XPath("/html/body/div[2]/aside/nav/a[2]");
        private By ProductList => By.XPath("/html/body/div[2]/main/div/section[2]/table");
        private By EditButton => By.XPath("/html/body/div[2]/main/div/section[2]/table/tbody/tr[1]/td[7]/div/button[1]");
        private By ProductNameInput => By.XPath("//*[@id='product-form']/div[2]/input[1]");
        private By ProductPriceInput => By.XPath("//*[@id='product-form']/div[2]/input[2]");
        private By SaveButton => By.XPath("//*[@id='product-form']/div[3]/button[1]");
        private By DeleteButton => By.XPath("/html/body/div[2]/main/div/section[2]/table/tbody/tr[2]/td[7]/div/button[2]");
        private By SuccessMessage => By.XPath("//*[@id='product-form']/div[2]");

        private void Pause(int ms = 1500)
        {
            Thread.Sleep(ms);
        }

        public void OpenUserManagement()
        {
            _wait.WaitForElementClickable(UserMenu).Click();
            Pause(2000);
        }

        public void SearchUser(string keyword)
        {
            var input = _wait.WaitForElementVisible(UserSearchInput);
            input.Clear();
            input.SendKeys(keyword);
            Pause(1000);
            _wait.WaitForElementClickable(UserSearchButton).Click();
            Pause(2000);
        }

        public bool IsUserListDisplayed()
        {
            return _driver.FindElements(UserList).Count > 0;
        }

        public void OpenProductManagement()
        {
            _wait.WaitForElementClickable(ProductMenu).Click();
            Pause(2000);
        }

        public bool IsProductListDisplayed()
        {
            return _driver.FindElements(ProductList).Count > 0;
        }

        public void ClickEditProduct()
        {
            _wait.WaitForElementClickable(EditButton).Click();
            Pause(2000);
        }

        public void UpdateProductName(string newName)
        {
            var input = _wait.WaitForElementVisible(ProductNameInput);
            input.Clear();
            input.SendKeys(newName);
            Pause(1000);
        }

        public void UpdateProductPrice(string newPrice)
        {
            var input = _wait.WaitForElementVisible(ProductPriceInput);
            input.Clear();
            input.SendKeys(newPrice);
            Pause(1000);
        }

        public void ClickSave()
        {
            _wait.WaitForElementClickable(SaveButton).Click();
            Pause(2000);
        }

        public void ClickDeleteProduct()
        {
            _wait.WaitForElementClickable(DeleteButton).Click();
            Pause(2000);
        }

        public string GetSuccessMessageText()
        {
            return _wait.WaitForElementVisible(SuccessMessage).Text.Trim();
        }
    }
}