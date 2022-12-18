using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakestorePageObjects;
using OpenQA.Selenium.Firefox;
using Helpers;

namespace SeleniumTests
{
    public class BaseTest
    {

        private bool isRemote = true;
        private Uri remoteAddress = new Uri("http://localhost:4444/wd/hub");
        protected string browser = "chrome";

        protected IWebDriver driver;

       

        [SetUp]
        public void Setup()
        {
            
            DriverFactory factory = new DriverFactory();

            driver = factory.Create(browser, isRemote, remoteAddress);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            MainPage mainPage = new MainPage(driver);
            mainPage.GoTo().dismissNoticeSection.DismissNotice();
            
            
        }
        [TearDown]
        public void QuitDriver()
        {
            driver.Quit();
        }

        

        //local Firefox new FirefoxDriver

        // local Chrome new ChromeDriver

    }
    
}
