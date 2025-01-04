using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTest;

public class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=0932";
        var filmDatabaseService = new FilmDatabaseService(connectionString);

        IWebDriver driver = new ChromeDriver();

        try
        {
            driver.Navigate().GoToUrl("http://asilmedia.org/films/tarjima_kinolar/page/5/");
            driver.Manage().Window.Maximize();

            bool hasNextPage = true;

            while (hasNextPage)
            {
                var articles = driver.FindElements(By.XPath("//*[@id='dle-content']/article"));
                List<string> filmUrls = new List<string>();

                foreach (var al in articles)
                {
                    string url = al.FindElement(By.TagName("a")).GetAttribute("href");
                    if (!string.IsNullOrEmpty(url))
                    {
                        filmUrls.Add(url);
                    }
                }

                foreach (var url in filmUrls)
                {
                    driver.Navigate().GoToUrl(url);
                    System.Threading.Thread.Sleep(2000);

                    var film = new Film
                    {
                        Title = GetElementText(driver, "//h1"),
                        ImageUrl = GetElementAttribute(driver, "//article//img", "src"),
                        Description = GetElementText(driver, "//*[@id=\"dle-content\"]/article/div/div[2]/div[1]/div[1]/div[1]"),
                        Genres = GetElementText(driver, "//*[@id=\"dle-content\"]/article/div/div[2]/div[1]/div[1]/div[2]/div[1]/span[2]"),
                        Year = GetElementText(driver, "//*[@id=\"dle-content\"]/article/div/div[2]/div[1]/div[2]/div/div[1]/span[2]/a"),
                        Country = GetElementText(driver, "//*[@id=\"dle-content\"]/article/div/div[2]/div[1]/div[2]/div/div[2]/span[2]/a"),
                        ImdbRating = GetElementText(driver, "//*[@id=\"dle-content\"]/article/div/div[2]/div[2]/div/div[3]/div[3]/span[2]"),
                        KinopoiskRating = GetElementText(driver, "//span[contains(text(), 'Кинопоиск')]/following-sibling::span"),
                        Director = GetElementText(driver, "//*[@id=\"dle-content\"]/article/div/div[2]/div[1]/div[1]/div[2]/div[2]/span[2]/a"),
                        Actors = GetElementText(driver, "//*[@id=\"dle-content\"]/article/div/div[2]/div[1]/div[1]/div[2]/div[3]"),
                        DownloadLink = GetElementAttribute(driver, "//a[contains(text(), 'Скачать')]", "href")
                    };

                    filmDatabaseService.AddFilm(film);

                    Console.WriteLine($"Film '{film.Title}' bazaga qo'shildi.");
                    Console.WriteLine("-----------------------------------------");
                }

                driver.Navigate().Back();

                var nextButton = driver.FindElements(By.XPath("//a[contains(@class, 'next')]"));
                if (nextButton.Count > 0)
                {
                    nextButton[0].Click();
                    System.Threading.Thread.Sleep(2000); 
                }
                else
                {
                    hasNextPage = false;
                }
            }
        }
        finally
        {
            driver.Quit();
        }
    }

    static string GetElementText(IWebDriver driver, string xpath)
    {
        try
        {
            return driver.FindElement(By.XPath(xpath)).Text;
        }
        catch
        {
            return "Mavjud emas";
        }
    }

    static string GetElementAttribute(IWebDriver driver, string xpath, string attribute)
    {
        try
        {
            return driver.FindElement(By.XPath(xpath)).GetAttribute(attribute);
        }
        catch
        {
            return "Mavjud emas";
        }
    }
}

