using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using DotNetSelenium.Utilities;
using Newtonsoft.Json.Linq;

namespace DotNetSelenium.PageObjects
{
    public class LoginPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        private By usernameInput = By.Id("username_id");
        private By passwordInput = By.Id("password");
        private By loginButton = By.Id("login");
        private By admin = By.XPath("//li[@class=\"dropdown dropdown-user\"]");
        private By logOut = By.XPath("//a[text() = ' Log Out ']");

         //Store login data
        private string username;
        private string password;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        public void PerformLogin()
        {
            JObject testData = TestDataReader.LoadJson("LoginData.json");
            username = testData["ValidLogin"]["Username"]?.ToString();
            password = testData["ValidLogin"]["Password"]?.ToString();

            Console.WriteLine($"Username: {username}, Password: {password}");
            try
            {
                driver.FindElement(usernameInput).SendKeys(username);
                driver.FindElement(passwordInput).SendKeys(password);
                driver.FindElement(loginButton).Click();

                wait.Until(ExpectedConditions.ElementIsVisible(admin));
                Console.WriteLine("Login successful");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error during login: " + e.Message);
                throw;
            }
        }


    }
}
