using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using Xunit;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace PokeAPI.UnitTests
{
    public class PokemonUITests : IDisposable
    {
        private IWebDriver driver;
        private const string BaseUrl = "https://localhost:7283";

        public PokemonUITests()
        {
            driver = new EdgeDriver();
        }

        [Fact]
        public void PokemonListUITest()
        {
            var countCards = 16;
            driver.Navigate().GoToUrl(BaseUrl);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            var cards = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("card"))).ToList();
            Assert.True(cards.Count == countCards);
        }

        [Fact]
        public void PokemonInfoUITest()
        {
            var pokeId = 12;
            driver.Navigate().GoToUrl($"{BaseUrl}/pokemon/{pokeId}");
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            var card = wait.Until(ExpectedConditions.ElementExists(By.ClassName("card")));
            Assert.NotNull(card);
        }

        [Fact]
        public void PokemonSearchUITest()
        {
            var inputString = "pika";
            driver.Navigate().GoToUrl(BaseUrl);
            var searchBox = driver.FindElement(By.ClassName("search__input"));
            searchBox.SendKeys(inputString);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.ElementExists(By.ClassName("card")));

            var searchButton = driver.FindElement(By.ClassName("search__btn"));
            searchButton.Click();

            var cards = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("card-title")));
            Assert.True(cards.Count > 0);
            Assert.True(cards.Where(card => !card.Text.StartsWith(inputString)).Count() == 0);
        }

        [Fact]
        public void PokemonFightUITest()
        {
            var myPokeId = 2;
            var enemyPokeId = 212;
            driver.Navigate().GoToUrl($"{BaseUrl}/pokemon/fight?myPokeId={myPokeId}&enemyPokeId={enemyPokeId}");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            var cards = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("card"))).ToList();
            Assert.True(cards.Count == 2);

            var attackButton = wait.Until(ExpectedConditions.ElementExists(By.ClassName("attack__btn")));
            Assert.NotNull(attackButton);
            attackButton.Click();

            var history = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("fight__history-item"))).ToList();
            Assert.True(history.Count > 0);
        }

        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}
