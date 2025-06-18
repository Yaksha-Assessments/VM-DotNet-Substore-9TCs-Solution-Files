using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System;
using System.IO;

public class CommonEvents
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private readonly IJavaScriptExecutor _jsExecutor;

    public CommonEvents(IWebDriver driver, int timeoutInSeconds = 10)
    {
        _driver = driver; // ✅ Now we're assigning the constructor parameter to the private field
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
        _jsExecutor = (IJavaScriptExecutor)_driver;
    }
// ✅ Version 1: Accepts By locator
    public bool IsDisplayed(By locator)
    {
        try
        {
            return _driver.FindElement(locator).Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    // ✅ Version 2: Accepts IWebElement
    public bool IsDisplayed(IWebElement element)
    {
        try
        {
            return element.Displayed;
        }
        catch (StaleElementReferenceException)
        {
            return false;
        }
    }

    public void WaitTillElementVisible(IWebElement element, int timeoutInSeconds)
{
    try
    {
        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
        wait.Until(driver => element.Displayed);
    }
    catch (WebDriverTimeoutException ex)
    {
        throw new Exception("Element was not visible after waiting for " + timeoutInSeconds + " seconds.", ex);
    }
}


    public void Click(By locator)
    {
        _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator)).Click();
    }

    public void Click(IWebElement element)
{
    try
    {
        WaitTillElementVisible(element, 10); // Optional: Wait to ensure the element is visible
        element.Click();
    }
    catch (Exception ex)
    {
        throw new Exception("Failed to click on the element: " + ex.Message, ex);
    }
}

    public void JsClick(By locator)
    {
        var element = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));
        _jsExecutor.ExecuteScript("arguments[0].click();", element);
    }

    public string GetTitle()
    {
        return _driver.Title;
    }

    public void HighlightElement(By locator)
    {
        try
        {
            IWebElement element = _driver.FindElement(locator);
            string originalStyle = element.GetAttribute("style");

            _jsExecutor.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);",
                element, "border: 2px solid red; background-color: yellow;");

            // Optional: revert style after a short delay
            System.Threading.Thread.Sleep(500); // half a second pause
            _jsExecutor.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);",
                element, originalStyle);
        }
        catch (Exception e)
        {
            Console.WriteLine("Highlight failed: " + e.Message);
        }
    }
    public void WaitForElementToBeVisible(By locator, int timeoutInSeconds = 10)
    {
        _wait.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
        _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
    }

    public void WaitForPageToLoad()
    {
        _wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
    }

    /// <summary>
    /// Finds a web element using a given locator with an explicit wait.
    /// </summary>
    /// <param name="by">The locator of the element.</param>
    /// <returns>The found IWebElement.</returns>
    public IWebElement FindElement(By by)
    {
        try
        {
            return _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(by));
        }
        catch (WebDriverTimeoutException)
        {
            throw new NoSuchElementException($"Element with locator '{by}' not found within timeout.");
        }
    }

    /// <summary>
    /// Scrolls to the specified element using JavaScript.
    /// </summary>
    /// <param name="element">The web element to scroll into view.</param>
    public void ScrollIntoView(IWebElement element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element));

        IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
        js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
    }

    public void PerformAltN()
    {
        Actions actions = new Actions(_driver);
        actions.KeyDown(Keys.Alt)
               .SendKeys("n")
               .KeyUp(Keys.Alt)
               .Perform();
    }

    public string GetText(By by)
    {
        return _driver.FindElement(by).Text;
    }

    /// <summary>
    /// Highlights an element on the page using JavaScript.
    /// </summary>
    public void HighlightElement(IWebElement element)
    {
        try
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("arguments[0].style.border='3px solid red'", element);
        }
        catch (Exception)
        {
            // Fails silently if highlight is not essential
        }
    }
    /// <summary>
    /// Gets the current page URL.
    /// </summary>
    public string GetCurrentUrl()
    {
        return _driver.Url;
    }

    /// <summary>
    /// Sends text to the specified element.
    /// </summary>
    public void SendKeys(By locator, string text)
    {
        IWebElement element = FindElement(locator);
        element.Clear();
        element.SendKeys(text);
    }
    
    public void SendKeys(IWebElement element, string value)
{
    element.SendKeys(value);
}

    public string GetAttribute(By locator, string attributeName)
    {
        try
        {
            IWebElement element = FindElement(locator);
            return element.GetAttribute(attributeName);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get attribute '{attributeName}' from element: {e.Message}");
            throw;
        }
    }

public IList<IWebElement> GetWebElements(By locator)
    {
        return _driver.FindElements(locator);
    }

    public void Highlight(IWebElement element)
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
        js.ExecuteScript("arguments[0].style.border='3px solid red'", element);
    }

    public void WaitForUrlContains(string partialUrl, int timeoutInMilliseconds)
    {
        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(timeoutInMilliseconds));
        bool urlContains = wait.Until(drv => drv.Url.Contains(partialUrl));

        if (!urlContains)
        {
            throw new WebDriverTimeoutException($"Timed out waiting for URL to contain: {partialUrl}");
        }
    }

public void TakeScreenshot(string fileName)
    {
        Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string path = Path.Combine(Directory.GetCurrentDirectory(), $"{fileName}_{timestamp}.png");

        screenshot.SaveAsFile(path);
    }

}
