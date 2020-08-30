using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.Funcao.TestesIntegrados.ScreenShot;
using System.Threading;

namespace MPS.Funcao.TestesIntegrados.Wrapper
{
    public static class IWebElementCustom
    {
        public static void ClearCustom(this IWebElement elemento)
        {
            elemento.Clear();
            new ScreenshotCustom().ScreenShot();
        }

        public static void ClickCustom(this IWebElement elemento)
        {
            new ScreenshotCustom().ScreenShot();
            elemento.Click();
        }

        public static Boolean obj;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elemento">Objeto IWebElement que será colocado o foco</param>
        /// <param name="webDriver">WebDriver que contém o contexto</param>
        public static void ClickCustom(this IWebElement elemento, IWebDriver webDriver)
        {
            obj = false;
            //IWebElement element = null;

            //for (int x = 1; x <= tentativas; x++)
            //{
            try
            {
                //obj = webDriver.FindElement(byObject).Displayed;
                if (obj == true)
                {
                    //elemento = webDriver.FindElement(byObject);
                    IJavaScriptExecutor js; // Javascript 
                    js = (IJavaScriptExecutor)webDriver;
                    Thread.Sleep(500);
                    js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", elemento, "color: red; border: 4px solid red;");
                    Thread.Sleep(500);
                    //break;
                }
            }
            catch
            {
                //tempo de esperar do loop
                System.Threading.Thread.Sleep(500);
            }

            //}

            new ScreenshotCustom().ScreenShot();
            elemento.Click();
        }

        public static void SendKeysCustom(this IWebElement elemento, string text)
        {
            elemento.SendKeys(text);
            new ScreenshotCustom().ScreenShot();
        }

        public static void SubmitCustom(this IWebElement elemento)
        {
            new ScreenshotCustom().ScreenShot();
            elemento.Submit();
        }
    }
}
