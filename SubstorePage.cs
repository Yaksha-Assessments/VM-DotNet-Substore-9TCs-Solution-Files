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

        // TC-1
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
        //TC-2
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

        // TC-3
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

        // TC-4
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

        // TC-5
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

        // TC-6

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
        /// Test Case 7: Capture a screenshot of the current page
        /// </summary>
        /// <returns>true if the screenshot is taken and saved successfully; false otherwise</returns>
        /// <exception cref="Exception">Thrown if any error occurs during the screenshot capture process</exception>
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

        // TC-8
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
        // TC-9

        public string VerifyCreateRequisitionButton()
            {
                string successMessage = string.Empty;

            try
            {
                var createReqButton = commonEvents.FindElement(CreateRequisitionButton);
                commonEvents.Highlight(createReqButton);
                commonEvents.Click(createReqButton);
                Console.WriteLine("Click-1 ");

                commonEvents.WaitForUrlContains("Inventory/InventoryRequisitionItem", 5000);

                var requestButton = commonEvents.FindElement(RequestButton);
                commonEvents.WaitTillElementVisible(requestButton, 10000);

                // Fill in target inventory
                var targetInventory = TargetInventory;
                commonEvents.Click(targetInventory);
                Console.WriteLine("Click-2 ");
                commonEvents.SendKeys(targetInventory, "General-Inventory");
                commonEvents.SendKeys(targetInventory, Keys.Tab);
                commonEvents.SendKeys(targetInventory, Keys.Tab);

                // Fill in item name
                var itemName = driver.FindElement(ItemName);
                commonEvents.Highlight(itemName);
                commonEvents.SendKeys(itemName, "tissue");
                commonEvents.SendKeys(itemName, Keys.Enter);

                // Fill in required quantity
                var quantityField = driver.FindElement(RequiredQuantity);
                commonEvents.Highlight(quantityField);
                commonEvents.SendKeys(quantityField, "5");

                // Submit
                commonEvents.Highlight(requestButton);
                commonEvents.Click(requestButton);
                Console.WriteLine("Click-3 ");

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
