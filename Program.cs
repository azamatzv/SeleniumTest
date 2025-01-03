using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTest;

public class Program
{
    private int CurPage = 1;
    static void Main(string[] args)
    {
        IWebDriver driver = new ChromeDriver();

        try
        {
            driver.Navigate().GoToUrl("http://asilmedia.org/films/tarjima_kinolar/");
            driver.Manage().Window.Maximize();

            var pageCon = driver.FindElement(By.XPath("//*[@id=\"bottom-nav\"]/div[2]"));
            var pageConUrls = pageCon.FindElements(By.TagName("a"));
            List<string> pageUrls = new List<string>();

            foreach (var it in pageConUrls)
            {
                string url = it.GetAttribute("href");
                if (!string.IsNullOrEmpty(url))
                {
                    pageUrls.Add(url);
                }
            }



            var articls = driver.FindElements(By.XPath("//*[@id='dle-content']/article"));
            List<string> filmUrls = new List<string>();

            foreach (var al in articls)
            {
                string url = al.FindElement(By.TagName("a")).GetAttribute("href");
                if (!string.IsNullOrEmpty(url))
                {
                    filmUrls.Add(url);
                }
            }

            foreach (var url in filmUrls)
            {
                //Fulm Haqidagi malumotlar
                ////*[@id="dle-content"]/article/div/div[2]/div[1]/div[1]/div[1]/p

                //Fulmning janri
                ////*[@id="dle-content"]/article/div/div[2]/div[1]/div[1]/div[2]/div[1]/span[2]


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