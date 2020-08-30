using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MPS.Funcao.TestesIntegrados.WebDriverFactory
{
    public static class WebDriverFactoryMps
    {
        #region :: Declarações 

        private static InternetExplorerOptions OptionsIE
        {
            get
            {
                var options = new InternetExplorerOptions()
                {
          
                    IntroduceInstabilityByIgnoringProtectedModeSettings = true
                    ,
                    IgnoreZoomLevel = true
                    ,
                    EnsureCleanSession = true


                };

                return options;
            }

        }

        private static ChromeOptions OptionsChrome
        {
            get
            {
                var options = new ChromeOptions()
                {
                    AcceptInsecureCertificates = true,
                };
                //options.AddArguments(/*"headless",*/ "window-size=1920,1080");
                //options.AddArguments("--incognito");
                return options;
            }
        }


        private static EdgeOptions OptionsEdge
        {
            get
            {
                var options = new EdgeOptions()
                {
                    AcceptInsecureCertificates = true,
                };
                return options;
            }
        }

        private static IWebDriver webDriver { get; set; }

        #endregion

        #region :: Metodos 
        public static void AbrirNavegador(string url)
        
        {
            //try
            //{
            //    //Limpa eventuais processos que fica preso na memória, causando erro inesperado ao rodar os teste na VM, esta deixando apenas 1 para não parar a aplicação.
            //    var p = Process.GetProcessesByName("IEDriverServer");
            //    int count = p.Count();
            //    foreach (var item in p)
            //    {
            //        item.Kill();
            //    }
            //}
            //catch (Exception)
            //{

            //}

            var driver = GetDriver();

            driver.Navigate().GoToUrl(url);
            driver.Manage().Window.Maximize();
        }

        public static IWebDriver GetDriver()
        {
            if (webDriver != null)
                return webDriver;

            switch (Utils.EnvironmentMps.RetornarBrowser().ToLower())
            {
           
                case "chrome":
                    webDriver = new ChromeDriver(OptionsChrome);
                    break;
                case "ie":
                    webDriver = new InternetExplorerDriver(OptionsIE);
                    break;
                case "firefox":
                    webDriver = new FirefoxDriver();
                    break;
                case "edge":
                    webDriver = new EdgeDriver(OptionsEdge);
                    break;
            }
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return webDriver;
        }

        public static void Close()
        {
            webDriver.Close();
            webDriver = null;
        }


        #endregion
    }
}
