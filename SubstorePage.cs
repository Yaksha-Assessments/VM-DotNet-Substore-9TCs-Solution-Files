using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using OpenQA.Selenium.Interactions;
using System.Net.Sockets;

namespace DotNetSelenium.PageObjects
{
    public class SubstorePage
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private readonly CommonEvents commonEvents;

        public SubstorePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            commonEvents = new CommonEvents(driver);
        }

        // TC-1 Locators
        public By UsernameTextfieldLocator => By.Id("username_id");
        public By PasswordTextboxLocator => By.XPath("//input[@id='password']");
        public By SignInButtonLocator => By.XPath("//button[@id='login']");
        public By DropDownLocator => By.XPath("//a[@href='#/WardSupply']");

        // TC-2 Locators
        public By CounterButtonFourth => By.XPath("//a[@class='report_list']");

        // TC-3 Locators
        public By AnchorTagLocatorInventory => By.XPath("//a[contains(text(),'Inventory')]");
        public By ModuleSignoutLocator => By.XPath("//i[contains(@class,'sign-out')]");
        public By HoverText => By.XPath("//h6[contains(text(),'To change, you can always click here.')]");

        // TC-4 Locators
        public By AnchorTagLocatorPharmacy => By.XPath("//a[contains(text(),'Pharmacy')]");

        // TC-5 Locators
        public By SubModuleLocator => By.XPath("//ul[contains(@class,'nav-tabs')]//li//a");

        // TC-6 Locators
        public By AnchorTagLocatorByText(string anchorTagName)
        {
            return By.XPath($"//a[contains(text(),'{anchorTagName}')]");
        }

        public By AnchorTagLocatorStock => By.XPath("//a[contains(text(),'Stock')]");
        public By AnchorTagLocatorByTextInventoryRequisition => By.XPath("//a[contains(text(),'Inventory Requisition')]");
        public By AnchorTagLocatorConsumption => By.XPath("//a[contains(text(),'Consumption')]");
        public By AnchorTagLocatorReports => By.XPath("//a[contains(text(),'Reports')]");
        public By AnchorTagLocatorPatientConsumption => By.XPath("//a[contains(text(),'Patient Consumption')]");
        public By AnchorTagLocatorReturn => By.XPath("//a[contains(text(),'Return')]");

        // TC-8 Locators
        public By CreateRequisitionButton => By.XPath("//button/span[text()='Create Requisition']");
        public By SearchBarId() => By.Id("quickFilterInput");
        public By StarIconLocator => By.XPath("//i[contains(@class,'icon-favourite')]/..");
        public By ButtonLocatorFirst => By.XPath("//button[contains(text(),'First')]");
        public By ButtonLocatorPrevious => By.XPath("//button[contains(text(),'Previous')]");
        public By ButtonLocatorNext => By.XPath("//button[contains(text(),'Next')]");
        public By ButtonLocatorLast => By.XPath("//button[contains(text(),'Last')]");
        public By ButtonLocatorOK => By.XPath("//button[contains(text(),'OK')]");
        public By RadioButtonLocatorPending => By.XPath("//label[contains(text(),'Pending')]/span");
        public By RadioButtonLocatorComplete => By.XPath("//label[contains(text(),'Complete')]/span");
        public By RadioButtonLocatorCancelled => By.XPath("//label[contains(text(),'Cancelled')]/span");
        public By RadioButtonLocatorWithdrawn => By.XPath("//label[contains(text(),'Withdrawn')]/span");
        public By RadioButtonLocatorAll => By.XPath("//label[contains(text(),'All')]/span");

        // TC-9 Locators
        public By RequestButton => By.CssSelector("input#save_requisition");
        public By TargetInventory => By.XPath("//input[@id='activeInventory']");
        public By ItemName => By.XPath("//input[@id='itemName0']");
        public By RequiredQuantity => By.XPath("//input[@id='qtyip0']");

        public By GetPopUpMessageText(string msgStatus, string messageText)
        {
            return By.XPath($"//p[contains(text(),' {msgStatus} ')]/../p[contains(text(),'{messageText}')]");
        }

        public By PopupCloseButton => By.CssSelector("a.close-btn");
        public By CloseModalLocator => By.CssSelector("a[title='Cancel']");

        /// <summary>
        /// Test Case 1 : Scrolls to the "Substore" tab on the web page, clicks it, and verifies that the navigation was successful by checking the URL.
        /// </summary>
        /// <remarks>
        /// This method ensures that the "Substore" tab is brought into the visible viewport using JavaScript,
        /// then clicks the tab and waits for the browser to navigate to the expected page.
        /// The method highlights the tab with a red border for debugging purposes during test execution.
        /// </remarks>
        /// <returns>
        /// The current URL of the browser after navigation to the "Substore" tab.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the "Substore" tab is not found, the click fails, or the expected URL is not detected within the timeout.
        /// </exception>
        public string ScrollToSubstoreTabAndVerifyUrl()
        {
            try
            {
                // Locate the "Substore" tab
                IWebElement substoreTab = driver.FindElement(By.XPath("//a[@href='#/WardSupply']"));

                // Scroll into view
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", substoreTab);
                js.ExecuteScript("window.scrollBy(0, -50);");

                // Optional: highlight element (for debugging)
                js.ExecuteScript("arguments[0].style.border='3px solid red'", substoreTab);

                // Click the tab
                substoreTab.Click();

                // Wait until URL contains "WardSupply"
                wait.Until(driver => driver.Url.Contains("WardSupply"));

                // Return current URL
                return driver.Url;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to click Substore tab or verify URL.", ex);
            }
        }

        /// <summary>
        /// Test Case 2 : Attempts to click on the fourth counter element (if available) on the page.
        /// </summary>
        /// <remarks>
        /// This method retrieves a list of web elements that match the locator <c>CounterButtonFourth</c>.
        /// If at least one matching element is found, it highlights and clicks the first one in the list.
        /// Useful for conditional UI actions where a specific counter might not always be visible or rendered.
        /// </remarks>
        /// <returns>
        /// Returns <c>true</c> if the operation completes successfully, regardless of whether a matching element was found.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown if an error occurs while locating, highlighting, or clicking the counter element.
        /// </exception>
        
        public bool ClickFourthCounterIfAvailable()
        {
            try
            {
                IList<IWebElement> counterElements = commonEvents.GetWebElements(CounterButtonFourth);
                Console.WriteLine("Elements size >> " + counterElements.Count);

                if (counterElements.Count > 0)
                {
                    commonEvents.Highlight(counterElements[0]);
                    counterElements[0].Click();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to click the fourth counter if available.", ex);
            }
        }

        /// <summary>
        /// Test Case 3 : Verifies that the hover text displayed on the "Sign Out" module matches the expected value.
        /// </summary>
        /// <param name="substoreExpectedData">
        /// A dictionary containing expected UI text values, with the key <c>"moduleSignOutHoverText"</c>
        /// used to validate the actual hover text displayed when hovering over the "Sign Out" module.
        /// </param>
        /// <returns>
        /// Returns <c>true</c> if the actual hover text contains the expected value; throws an exception otherwise.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown if the hover text is not as expected, or if any failure occurs during interaction with the web elements.
        /// </exception>
        public bool VerifyModuleSignoutHoverText(Dictionary<string, string> substoreExpectedData)
        {
            try
            {
                // Click the Inventory section
                commonEvents.Click(AnchorTagLocatorInventory);

                // Hover over the Sign Out module
                IWebElement elementToHover = driver.FindElement(ModuleSignoutLocator);
                Actions actions = new Actions(driver);

                IWebElement hoverTextElement = commonEvents.FindElement(HoverText);
                actions.MoveToElement(hoverTextElement).Perform();

                // Get the hover text
                string actualHoverText = driver.FindElement(HoverText).Text;
                Console.WriteLine("Element text --> " + actualHoverText);

                // Validate the hover text
                if (actualHoverText.Contains(substoreExpectedData["moduleSignOutHoverText"]))
                {
                    return true;
                }
                else
                {
                    throw new Exception("Hover text did not match the expected value.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to verify the hover text on the 'Sign Out' module: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Test Case 4 : Verifies the presence and clickability of the "Inventory" and "Pharmacy" sub-modules on the Substore page.
        /// </summary>
        /// <param name="substoreExpectedData">
        /// A dictionary containing expected values for verification, including the expected Substore page URL under the key <c>"URL"</c>.
        /// </param>
        /// <returns>
        /// Returns <c>true</c> if both the "Inventory" and "Pharmacy" sub-modules are found and clicked successfully; otherwise throws an exception.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when either the "Inventory" or "Pharmacy" sub-module is not found, not clickable, or any step fails during the process.
        /// </exception>
        public bool VerifySubstoreSubModule(Dictionary<string, string> substoreExpectedData)
        {
            try
            {
                Console.WriteLine("Substore Page URL: " + substoreExpectedData["URL"]);

                // Find the Inventory and Pharmacy sub-modules
                IWebElement inventorySubModule = commonEvents.FindElement(AnchorTagLocatorInventory);
                IWebElement pharmacySubModule = commonEvents.FindElement(AnchorTagLocatorPharmacy);

                // Highlight and click on the Inventory sub-module
                commonEvents.Highlight(inventorySubModule);
                commonEvents.Click(AnchorTagLocatorInventory);

                // Highlight and click on the Pharmacy sub-module
                commonEvents.Highlight(pharmacySubModule);
                commonEvents.Click(AnchorTagLocatorPharmacy);

                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to verify substore sub-modules due to: " + e.Message, e);
            }
        }

        /// <summary>
        /// Test Case 5 : Verifies the presence and visibility of sub-modules under the "Inventory" section in the Substore module.
        /// </summary>
        /// <returns>
        /// Returns <c>true</c> if at least one sub-module is found and displayed under the Inventory section; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when an error occurs during the process of clicking the Inventory tab or locating sub-module elements.
        /// </exception>
        /// <remarks>
        /// <para>This method performs the following steps:</para>
        /// <list type="number">
        ///   <item>Clicks on the "Inventory" module tab using the provided locator.</item>
        ///   <item>Retrieves all sub-module elements under the Inventory module.</item>
        ///   <item>Checks each sub-module to see if it is displayed on the page.</item>
        ///   <item>If at least one sub-module is visible, the method returns true; otherwise, false.</item>
        /// </list>
        /// <para>This method is helpful for verifying UI readiness and validating that the Inventory module correctly loads its sub-sections.</para>
        /// </remarks>
        public bool SubModulePresentInventory()
        {
            bool areModulesDisplayed = false;

            try
            {
                // Click on the specified module
                commonEvents.Click(AnchorTagLocatorInventory);

                // Get the list of sub-module elements
                IList<IWebElement> subModuleElements = commonEvents.GetWebElements(SubModuleLocator);
                Console.WriteLine("Sub-module count: " + subModuleElements.Count);

                // Check if the sub-modules are displayed
                if (subModuleElements.Any())
                {
                    foreach (IWebElement subModule in subModuleElements)
                    {
                        bool isDisplayed = commonEvents.IsDisplayed(subModule);
                        Console.WriteLine("Sub-module displayed: " + isDisplayed);
                        areModulesDisplayed = areModulesDisplayed || isDisplayed;
                    }
                }
                else
                {
                    Console.WriteLine("No sub-modules found under the specified module.");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to find elements: " + e.Message, e);
            }

            return areModulesDisplayed;
        }

        /// <summary>
        /// Test Case 6 : Verifies that navigation between all defined sub-modules under the "Inventory" module functions correctly.
        /// </summary>
        /// <returns>
        /// <c>true</c> if all sub-module navigations succeed and corresponding URLs are matched; otherwise, an exception is thrown.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when any sub-module fails to load, or the expected URL does not match during navigation.
        /// </exception>
        /// <remarks>
        /// <para>
        /// This method simulates user navigation through various sub-modules within the Inventory module of the application.
        /// For each sub-module, it:
        /// </para>
        /// <list type="number">
        ///   <item>Clicks on the sub-module tab/link using the defined locator.</item>
        ///   <item>Waits until the page URL contains an expected substring specific to that sub-module.</item>
        /// </list>
        /// <para>
        /// The following sub-modules are validated in sequence:
        /// </para>
        /// <list type="bullet">
        ///   <item>Stock</item>
        ///   <item>Inventory Requisition</item>
        ///   <item>Consumption</item>
        ///   <item>Reports</item>
        ///   <item>Patient Consumption</item>
        ///   <item>Return</item>
        /// </list>
        /// <para>
        /// Finally, it navigates back to the "Stock" sub-module to ensure the application remains functional after multiple transitions.
        /// </para>

        public bool VerifyNavigationBetweenSubmodules()
        {
            try
            {
                // Clicking on the "Inventory" submodule to start navigation.
                commonEvents.Click(AnchorTagLocatorInventory);

                // Navigating to the "Stock" submodule and waiting for the URL to update.
                commonEvents.Click(AnchorTagLocatorStock);
                commonEvents.WaitForUrlContains("Inventory/Stock", 5000);

                // Navigating to the "Inventory Requisition" submodule and waiting for the URL to update.
                commonEvents.Click(AnchorTagLocatorByTextInventoryRequisition);
                commonEvents.WaitForUrlContains("Inventory/InventoryRequisitionList", 5000);

                // Navigating to the "Consumption" submodule and waiting for the URL to update.
                commonEvents.Click(AnchorTagLocatorConsumption);
                commonEvents.WaitForUrlContains("Inventory/Consumption/ConsumptionList", 5000);

                // Navigating to the "Reports" submodule and waiting for the URL to update.
                commonEvents.Click(AnchorTagLocatorReports);
                commonEvents.WaitForUrlContains("Inventory/Reports", 5000);

                // Navigating to the "Patient Consumption" submodule and waiting for the URL to update.
                commonEvents.Click(AnchorTagLocatorPatientConsumption);
                commonEvents.WaitForUrlContains("Inventory/PatientConsumption/PatientConsumptionList", 5000);

                // Navigating to the "Return" submodule and waiting for the URL to update.
                commonEvents.Click(AnchorTagLocatorReturn);
                commonEvents.WaitForUrlContains("Inventory/Return", 5000);

                // Finally, navigating back to the "Stock" submodule.
                commonEvents.Click(AnchorTagLocatorStock);

                // Return true if all navigations are successful.
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Navigation between submodules failed: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Test Case 7 : Captures a screenshot of the current page and saves it with the filename prefix "SubStore".
        /// </summary>
        /// <returns>
        /// <c>true</c> if the screenshot is successfully captured and saved; otherwise, an exception is thrown.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown if there is any failure while attempting to capture or save the screenshot.
        /// </exception>
        /// <remarks>
        /// <para>
        /// This method delegates the screenshot functionality to the <c>TakeScreenshot</c> method of the <c>commonEvents</c> utility.
        /// The screenshot is typically saved in the project's execution directory, with a timestamp appended to the filename.
        /// </para>
        /// <para>
        /// Useful for debugging UI tests, verifying visual states, or recording the outcome of specific actions.
        /// </para>
        /// </remarks>
        
        public bool TakingScreenshotOfTheCurrentPage()
        {
            bool isDisplayed = false;
            try
            {
                commonEvents.TakeScreenshot("SubStore");
                isDisplayed = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error capturing screenshot: " + ex.Message, ex);
            }
            return isDisplayed;
        }

        /// <summary>
        /// Test Case 8 : Verifies the visibility of all critical UI elements on the Inventory Requisition page.
        /// </summary>
        /// <returns>
        /// <c>true</c> if all listed UI components are visible; otherwise, an exception is thrown.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown if any UI element is not found or is not visible on the page.
        /// </exception>
        /// <remarks>
        /// <para>
        /// This method performs a complete UI audit of the Inventory Requisition screen by:
        /// </para>
        /// <list type="number">
        ///   <item><description>Clicking the "Inventory Requisition" sub-module link.</description></item>
        ///   <item><description>Waiting for the target URL to confirm the page has loaded.</description></item>
        ///   <item><description>Locating and highlighting key UI components including buttons, input fields, icons, and radio buttons.</description></item>
        ///   <item><description>Throwing an exception if any of these elements are not visible.</description></item>
        /// </list>
        /// </remarks>
        public bool VerifyInventoryRequisitionUIElements()
        {
            try
            {
                commonEvents.Click(AnchorTagLocatorByText("Inventory Requisition"));
                commonEvents.WaitForUrlContains("Inventory/InventoryRequisitionList", 5000);

                var elements = new List<IWebElement>
                    {
                        commonEvents.FindElement(ButtonLocatorFirst),
                        commonEvents.FindElement(ButtonLocatorPrevious),
                        commonEvents.FindElement(ButtonLocatorNext),
                        commonEvents.FindElement(ButtonLocatorLast),
                        commonEvents.FindElement(ButtonLocatorOK),
                        commonEvents.FindElement(CreateRequisitionButton),
                        commonEvents.FindElement(SearchBarId()),
                        commonEvents.FindElement(StarIconLocator),
                        commonEvents.FindElement(RadioButtonLocatorPending),
                        commonEvents.FindElement(RadioButtonLocatorComplete),
                        commonEvents.FindElement(RadioButtonLocatorCancelled),
                        commonEvents.FindElement(RadioButtonLocatorWithdrawn),
                        commonEvents.FindElement(RadioButtonLocatorAll)
                    };

                foreach (var element in elements)
                {
                    commonEvents.Highlight(element);
                    if (!element.Displayed)
                    {
                        throw new Exception("Visibility check failed for: " + element.Text);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to verify if all fields are displayed!", ex);
            }
        }

        /// <summary>
        /// Test Case 9 : Automates the process of creating a new inventory requisition and verifies the success message.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> containing the success message text after submitting the requisition.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown if any step in the requisition creation process fails, including element interactions or validations.
        /// </exception>
        /// <remarks>
        /// This method simulates a user flow for creating an inventory requisition. It includes:
        /// <list type="number">
        ///   <item><description>Clicking the "Create Requisition" button on the Inventory Requisition page.</description></item>
        ///   <item><description>Waiting for the requisition item entry page to load.</description></item>
        ///   <item><description>Selecting the target inventory ("General-Inventory").</description></item>
        ///   <item><description>Entering the item name ("tissue") and required quantity ("5").</description></item>
        ///   <item><description>Submitting the requisition by clicking the "Request" button.</description></item>
        ///   <item><description>Verifying that the success message is displayed.</description></item>
        ///   <item><description>Closing the success popup and the requisition modal.</description></item>
        /// </list>
        /// Logs and highlights are used to assist in debugging and visual validation during test runs.
        /// </remarks>

        public string VerifyCreateRequisitionButton()
            {
                string successMessage = string.Empty;

            try
            {
                var createReqButton = commonEvents.FindElement(CreateRequisitionButton);
                commonEvents.Highlight(createReqButton);
                commonEvents.Click(createReqButton);

                commonEvents.WaitForUrlContains("Inventory/InventoryRequisitionItem", 5000);

                var requestButton = commonEvents.FindElement(RequestButton);
                commonEvents.WaitTillElementVisible(requestButton, 10000);

                // Fill in target inventory
                var targetInventory = TargetInventory;
                commonEvents.Click(targetInventory);
                commonEvents.SendKeys(targetInventory, "General-Inventory");
                commonEvents.SendKeys(targetInventory, Keys.Tab);
                commonEvents.SendKeys(targetInventory, Keys.Tab);
                Thread.Sleep(3000);

                // Fill in item name
                var itemName = driver.FindElement(ItemName);
                commonEvents.Highlight(itemName);
                commonEvents.SendKeys(itemName, "tissue");
                Thread.Sleep(3000);
                commonEvents.SendKeys(itemName, Keys.Enter);
                
                

                // Fill in required quantity
                var quantityField = driver.FindElement(RequiredQuantity);
                commonEvents.Highlight(quantityField);
                Thread.Sleep(3000);
                commonEvents.SendKeys(quantityField, "1");

                // Submit
                commonEvents.Highlight(requestButton);
                Thread.Sleep(3000);
                commonEvents.Click(requestButton);

                // Verify success message
                var successElement = commonEvents.FindElement(GetPopUpMessageText("success", "Requisition is Generated and Saved"));
                successMessage = successElement.Text;

                // Close pop-up and modal
                commonEvents.Click(PopupCloseButton);
                commonEvents.Click(CloseModalLocator);
                    Console.WriteLine("Click-4 ");

                }
            catch (Exception ex)
            {
                throw new Exception("Failed to create requisition: " + ex.Message, ex);
            }

                return successMessage;
            }

        

    }
}
