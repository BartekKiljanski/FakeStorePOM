using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace FakestorePageObjects
{
    public class MainPage : BasePage
    {


        public DismissNoticeSection dismissNoticeSection;

        public MainPage(IWebDriver driver) :base(driver)
        {
           dismissNoticeSection  = new DismissNoticeSection(driver);
        }

        public MainPage GoTo()
        {
            driver.Navigate().GoToUrl(baseUrl);
            return this;
        }
    }
}