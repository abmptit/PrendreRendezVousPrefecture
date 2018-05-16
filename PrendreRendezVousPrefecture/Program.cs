namespace PrendreRendezVousPrefecture
{
    using System.Threading;
    using OpenQA.Selenium;
    using Helpers;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                PrendMoiUnRendezVous();
            }
        }
        private static void PrendMoiUnRendezVous()
        {
            string bookingUrl = "http://www.hauts-de-seine.gouv.fr/booking/create/4485/0";

            using (var webDriver = WebDriverHelper.CreateSession())
            {
                webDriver.ResizeWindow(SeleniumConfig.BrowserSize);
                webDriver.Navigate().GoToUrl(bookingUrl);
                Thread.Sleep(500);
                IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
                js.ExecuteScript("javascript:accepter()");
                Thread.Sleep(2000);
                int attempt = 1;
                while (true)
                {
                    try
                    {
                        var acceptCheck = webDriver.FindElement(By.Id("condition"));
                        acceptCheck.Click();
                        Thread.Sleep(500);
                        var createNewBookingButton = webDriver.FindElement(By.XPath("//input[@name='nextButton']"));
                        createNewBookingButton.Click();
                        Thread.Sleep(500);
                        var formCreate = webDriver.FindElement(By.Id("FormBookingCreate"));
                        var text = formCreate.Text;
                        var result = (!text.Contains("no more free time"));
                        if (result == true)
                        {
                            string msg = "http://www.hauts-de-seine.gouv.fr/booking/create/4485/0";
                            while (true)
                            {
                                string screenshotFileName = $"ss_ok_{attempt}.png";
                                Screenshot ss = ((ITakesScreenshot)webDriver).GetScreenshot();
                                ss.SaveAsFile(screenshotFileName);
                                MailHelper.SendMail("Rendez vous disponibles", msg, new string[] { screenshotFileName }, new string[] { "benmiledaymen@gmail.com", "rekik.yousra@gmail.com" });
                                Thread.Sleep(5000);
                            }
                        }
                        else
                        {
                            if (attempt % 500 == 1)
                            {
                                string screenshotFileName = $"ss_retry_{attempt}.png";
                                Screenshot ss = ((ITakesScreenshot)webDriver).GetScreenshot();
                                ss.SaveAsFile(screenshotFileName);
                                string msg = "http://www.hauts-de-seine.gouv.fr/booking/create/4485/0";
                                msg += $"\nApres {attempt} essais";
                                MailHelper.SendMail("Pas de Rendez vous disponibles", msg, new string[] { screenshotFileName }, new string[] { "benmiledaymen@gmail.com" });
                            }

                            var finishButton = webDriver.FindElement(By.XPath("//input[@value='Finish']"));
                            finishButton.Click();
                            Thread.Sleep(500);
                        }
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            string screenshotFileName = $"ss_error_{attempt}.png";
                            Screenshot ss = ((ITakesScreenshot)webDriver).GetScreenshot();
                            ss.SaveAsFile(screenshotFileName);
                            Console.WriteLine($"Une erreur s'est produite au {attempt} essai {DateTime.Now.ToString()}");
                            Console.WriteLine(ex.Message);
                            string msg = $"Une erreur s'est produite au {attempt} essai";
                            msg += ex.Message;
                            MailHelper.SendMail("Erreur scheduler", msg, new string[] { screenshotFileName }, new string[] { "benmiledaymen@gmail.com" });
                            return;
                        }
                        catch
                        {
                            return;
                        }
                       
                    }
                    finally
                    {
                        Console.WriteLine($"{attempt} - essai {DateTime.Now.ToString()}");
                        attempt++;
                        Thread.Sleep(40000);
                    }
                }
            }
        }
    }

}
