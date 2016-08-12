using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using M4W2.Data;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace M4W2
{
    public class MailPage
    {
        private IWebDriver _driver;

        private const string draftRowTemplate = "//div[@class='y6']/span[text()='{0}']/../span[contains(text(),'{1}')]";
        private const string sentRowTemplate = "//tr[contains(@class,'zA')]//span[@email='{0}']/../../..//*[text()='{1}']";

        [FindsBy(How = How.XPath, Using = "//div[@class='z0']/div[@role='button' and @tabindex=0]")] public IWebElement createNewMailButton;
        [FindsBy(How = How.CssSelector, Using = ".Am.Al.editable")] public IWebElement mailBodyTextArea;
        [FindsBy(How = How.CssSelector, Using = ".aoD.hl")] public IWebElement sendToElement;
        [FindsBy(How = How.CssSelector, Using = ".wO.nr.l1 > textarea")] public IWebElement sendToTextField;
        [FindsBy(How = How.CssSelector, Using = ".aoD.hl span[email]")] public IWebElement sendToTextFieldForRead;
        [FindsBy(How = How.CssSelector, Using = "[name='subjectbox']")] public IWebElement subjectTextField;
        [FindsBy(How = How.CssSelector, Using = "[name = 'subject']")] public IWebElement subjectTextFieldForRead;
        [FindsBy(How = How.CssSelector, Using = ".Hm>.Ha")] public IWebElement closeMailDialogButton;
        [FindsBy(How = How.XPath, Using = "//a[contains(@href,'draft')]")] public IWebElement draftLink;
        [FindsBy(How = How.XPath, Using = "//a[contains(@href,'sent')]")] public IWebElement sentLink;
        [FindsBy(How = How.CssSelector, Using = ".n1tfz .gU.Up [role='button']")] public IWebElement sendButton;
        public MailPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public MailPage CreateNewMail(Mail mail)
        {
            createNewMailButton.Click();
            PopulateMailContent(mail);

            return this;
        }

        public void PopulateMailContent(Mail mail)
        {
            new Actions(_driver)
                .MoveToElement(mailBodyTextArea)
                .Click(mailBodyTextArea)
                .SendKeys(mail.body)
                .Click(sendToElement)
                .MoveToElement(sendToTextField)
                .Click(sendToTextField)
                .SendKeys(mail.mailTo)
              //  .SendKeys(Keys.Escape)
                .Build().Perform();

            new Actions(_driver).Click(subjectTextField)
                .SendKeys(subjectTextField, mail.subject)
                .Build()
                .Perform();
        }

        public MailPage VerifyMailPresentInDraftFolder(Mail mail)
        {
            draftLink.Click();
            Assert.AreEqual(true, _driver.FindElement(By.XPath(string.Format(draftRowTemplate, mail.subject, mail.body))).Displayed);
            return this;
        }

        public MailPage VerifyMailContentInDraftFolder(Mail mail)
        {
            draftLink.Click();
            _driver.FindElement(By.XPath(string.Format(draftRowTemplate, mail.subject, mail.body))).Click();
            Assert.AreEqual(mail.body, mailBodyTextArea.Text);
            sendToTextFieldForRead.Click();
            Assert.AreEqual(mail.mailTo, sendToTextFieldForRead.GetAttribute("email"));
            Assert.AreEqual(mail.subject, subjectTextFieldForRead.GetAttribute("value"));

            return this;
        }

        public MailPage VerifyMailNotPresentInDraftFolder(Mail mail)
        {
            draftLink.Click();
            new WebDriverWait(_driver, new TimeSpan(0, 0, 25)).Until(
                ExpectedConditions.InvisibilityOfElementLocated(
                By.XPath(string.Format(draftRowTemplate, mail.subject, mail.body))));

            return this;
        }

        public MailPage VerifyMailPresentInSentFolder(Mail mail)
        {
            sentLink.Click();
            Assert.AreEqual(true, _driver.FindElement(By.XPath(string.Format(sentRowTemplate, mail.mailTo, mail.subject))).Displayed);

            return this;
        }

        public MailPage CloseMailDialog()
        {
            closeMailDialogButton.Click();
            return this;
        }

        public MailPage SendMail()
        {
            sendButton.Click();
            return this;
        }
    }
}
