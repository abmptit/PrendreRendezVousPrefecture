namespace PrendreRendezVousPrefecture
{
    using System.Threading;
    using OpenQA.Selenium;
    using Helpers;

    class Program
    {
        static void Main(string[] args)
        {
            bool rendezVousPris = false;
            //int maxAttempt = 1000000000;
            int attempt = 1;
            while (!rendezVousPris && true/*attempt < maxAttempt*/)
            {
                rendezVousPris = PrendMoiUnRendezVous();
                if (rendezVousPris)
                {
                    string msg = "http://www.hauts-de-seine.gouv.fr/booking/create/4485/0";
                    MailHelper.SendMail("Rendez vous dispoibles", msg);
                }
                if (attempt % 1000 == 0)
                {
                    string msg = "http://www.hauts-de-seine.gouv.fr/booking/create/4485/0";
                    msg += $"\nApres {attempt} essais";
                    MailHelper.SendMail("Pas de Rendez vous disponibles", msg);
                }
                Thread.Sleep(2000);
                attempt++;
            }
        }
        private static bool PrendMoiUnRendezVous()
        {
            string bookingUrl = "http://www.hauts-de-seine.gouv.fr/booking/create/4485/0";

            using (var webDriver = WebDriverHelper.CreateSession())
            {
                webDriver.ResizeWindow(SeleniumConfig.BrowserSize);
                webDriver.Navigate().GoToUrl(bookingUrl);
                IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
                js.ExecuteScript("javascript:accepter()");
                Thread.Sleep(2000);
                var acceptCheck = webDriver.FindElement(By.Id("condition"));
                acceptCheck.Click();
                Thread.Sleep(500);
                var createNewBookingButton = webDriver.FindElement(By.XPath("//input[@name='nextButton']"));
                createNewBookingButton.Click();
                Thread.Sleep(500);
                var formCreate = webDriver.FindElement(By.Id("FormBookingCreate"));
                var text = formCreate.Text;
                return (!text.Contains("no more free time"));                
            }
        }
    }

}
