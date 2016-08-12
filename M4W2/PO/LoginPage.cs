using System;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace M4W2
{
    public class LoginPage
    {
        public IWebDriver _driver;

        [FindsBy(How = How.Id, Using = "Email")] public IWebElement accoutTextField;
        [FindsBy(How = How.Id, Using = "Passwd")] public IWebElement passwordTextField;
        [FindsBy(How = How.Id, Using = "next")] public IWebElement nextButton;
        [FindsBy(How = How.Id, Using = "signIn")] public IWebElement signInButton;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public MailPage Login(string user, string password)
        {
            IJavaScriptExecutor js = _driver as IJavaScriptExecutor;

            accoutTextField.Clear();
            accoutTextField.SendKeys(user);

            new Actions(_driver).Click(nextButton).Build().Perform();

            new WebDriverWait(_driver, TimeSpan.FromSeconds(20)).Until(ExpectedConditions.ElementIsVisible(By.Id("Passwd")));


            js.ExecuteScript("document.getElementById('Passwd').style.backgroundColor = 'red'");

            passwordTextField.Clear();
            passwordTextField.SendKeys(password);

            js.ExecuteScript("document.getElementById('signIn').removeAttribute('class')");
            js.ExecuteScript("document.getElementById('signIn').style.backgroundColor = 'green'");

            new Actions(_driver).Click(signInButton).Build().Perform();

            return new MailPage(_driver);
        }
    }
}