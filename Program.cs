using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTest;

public class Program
{
    static void Main(string[] args)
    {
        IWebDriver driver = new ChromeDriver();

        try
        {
            driver.Navigate().GoToUrl("http://asilmedia.org/films/tarjima_kinolar/");
            driver.Manage().Window.Maximize();

            var filmLinks = driver.FindElements(By.XPath("//*[@id='dle-content']/article//a"));
            List<string> filmUrls = new List<string>();

            foreach (var link in filmLinks)
            {
                string url = link.GetAttribute("href");
                if (!string.IsNullOrEmpty(url))
                {
                    filmUrls.Add(url);
                }
            }

            foreach (var url in filmUrls)
            {
                driver.Navigate().GoToUrl(url);

                Thread.Sleep(5000);

                driver.Navigate().Back();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Xatolik yuz berdi: " + ex.Message);
        }
    }
}