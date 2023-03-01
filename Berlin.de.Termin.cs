using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Xml.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace mySelenium
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--disable-blink-features=AutomationControlled");

            IWebDriver driver = new ChromeDriver(@"C:\Users\Galina\Downloads\chromedriver_win32", options);

            // 1 page
            driver.Navigate().GoToUrl("https://otv.verwalt-berlin.de/ams/TerminBuchen");        //site url
            ElementClickIfExist(driver, By.LinkText("EN"));                                     //en/de language
            ElementClickIfExist(driver, By.LinkText("Book Appointment"));                       //book appointment

            //2 page
            ElementClickIfExist(driver, By.Id("xi-cb-1"));                                      //agree checkbox
            ElementClickIfExist(driver, By.Id("applicationForm:managedForm:proceed"));          //next

            //3 page
            ElementChooseIfExist(driver, By.Id("xi-sel-400"), "160");                           //country
            ElementChooseIfExist(driver, By.Id("xi-sel-422"), "1");                             //number of persons
            ElementChooseIfExist(driver, By.Id("xi-sel-427"), "2");                             //alone?

            ElementClickIfExist(driver, By.CssSelector("label[for=SERVICEWAHL_EN3160-0-1]"));               //applay for resident visa
            ElementClickIfExist(driver, By.CssSelector("label[for=SERVICEWAHL_EN_160-0-1-1]"));             //economic activity
            ElementClickIfExist(driver, By.CssSelector("label[for=SERVICEWAHL_EN160-0-1-1-305304]"));       //18a

            // repeat
            do
            {
                ElementClickIfExist(driver, By.Id("applicationForm:managedForm:proceed"));      //next
                Thread.Sleep(10000);
            } while (
                !ElementExists(driver, By.Id("xi-fs-4")));                                    //choose time box
            
        }

        public static bool ElementExists(IWebDriver driver, By condition)
        {
            bool IsElementExist = false;
            IsElementExist = driver.FindElements(condition).Any();
            return IsElementExist;
        }

        public static void ElementClickIfExist(IWebDriver driver, By condition)
        {
            var done = false;
            do
            {
                try
                {
                    driver.FindElement(condition).Click();
                    done = true;
                    Thread.Sleep(250);
                }
                catch { }
            } while (done != true);
        }

        public static void ElementChooseIfExist(IWebDriver driver, By condition, string Value)
        {
            var done = false;
            do
            {
                try
                {
                    ElementClickIfExist(driver, condition);
                    SelectElement selElements = new(driver.FindElement(condition));
                    selElements.SelectByValue(Value);
                    done = true;
                    Thread.Sleep(250);
                }
                catch { }
            } while (done != true);
        }
    }
}