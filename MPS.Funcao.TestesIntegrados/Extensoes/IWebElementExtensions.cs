using Estudo.Framework.VSSelenium.Evidencias;
using Estudo.Framework.VSSelenium.WebDriverFactory;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Estudo.Framework.VSSelenium.Utils;

namespace Estudo.Framework.VSSelenium.Extensoes
{
    public static class IWebElementExtensions
    {
        private static IWebDriver GetDriver(IWebElement webElement = null)
        {
            IWebDriver driver = (webElement as IWrapsDriver)?.WrappedDriver ?? WebDriverFactoryVS.GetDriver();
            return driver;
        }

        /// <summary>
        /// retorna o pai do elemento
        /// </summary>
        /// <param name="webElement"></param>
        /// <returns></returns>
        public static IWebElement GetParent(this IWebElement webElement)
        {
            return webElement.FindElement(By.XPath(".."));
        }

        /// <summary>
        /// retorna o pai do elemento
        /// </summary>
        /// <param name="webElement"></param>
        /// <returns></returns>
        public static IWebElement GetParentJS(this IWebElement webElement)
        {
            return (IWebElement)((IJavaScriptExecutor)GetDriver(webElement)).ExecuteScript(
                                   "return arguments[0].parentNode;", webElement);
        }

        /// <summary>
        /// Retorna da descendencia do elemento, filhos, netos, etc 
        /// </summary>
        /// <param name="webElement"></param>
        /// <returns></returns>
        public static IList<IWebElement> GetChildrens(this IWebElement webElement)
        {
            return webElement.FindElements(By.XPath("descendant::*")).ToList();
        }

        /// <summary>
        /// Retorna apenas filhos diretos do elemento
        /// </summary>
        /// <param name="webElement"></param>
        /// <returns></returns>
        public static IList<IWebElement> GetChildren(this IWebElement webElement)
        {
            return webElement.FindElements(By.XPath("*")).ToList();
        }

        public static IList<IWebElement> GetSiblings(this IWebElement webElement)
        {
            List<IWebElement> lista = new List<IWebElement>();
            lista.AddRange(webElement.FindElements(By.XPath("preceding-sibling::*")));
            lista.AddRange(webElement.FindElements(By.XPath("following-sibling::*")));
            return lista;
        }

        /// <summary>
        /// Retorna apenas filhos diretos do elemento
        /// </summary>
        /// <param name="webElement"></param>
        /// <returns></returns>
        public static IWebElement TopParent(this IWebElement webElement)
        {
            return webElement.FindElement(By.XPath("//*"));
        }

        public static IWebElement SetFocus(this IWebElement webElement)
        {
            ((IJavaScriptExecutor)GetDriver(webElement)).ExecuteScript("arguments[0].focus()", webElement);
            return webElement;
        }


        public static IWebElement ClickJS(this IWebElement webElement)
        {
            ((IJavaScriptExecutor)GetDriver(webElement)).ExecuteScript("arguments[0].click()", webElement);
            return webElement;
        }

        public static IWebElement HighLight(this IWebElement webElement)
        {
            SetHighLight(webElement);
            Thread.Sleep(TimeSpan.FromMilliseconds(1000));
            UnsetHighLight(webElement);
            return webElement;
        }

        public static IWebElement HighLight(this IWebElement webElement, IWebDriver webDriver)
        {
            SetHighLight(webElement, webDriver);
            Thread.Sleep(1000);
            UnsetHighLight(webElement);
            return webElement;
        }

        public static void SetHighLight(this IWebElement webElement, IWebDriver webDriver, string color = "blue")
        {
            var x = webElement.Location.X;
            var y = webElement.Location.Y;
            var widht = webElement.Size.Width;
            var height = webElement.Size.Height;

            ((IJavaScriptExecutor)webDriver).ExecuteScript("$('body').append(\"" +
                "<div id='HighLight-TesteAutomatizado' " +
                "style='border: 5px solid " + color + "; " +
                "width:" + (widht + 16) + "px; " +
                "height:" + (height + 16) + "px; " +
                "top:" + (y - 8) + "px; " +
                "left:" + (x - 8) + "px; " +
                "z-index:999999; " +
                "position:absolute !important; '>" +
                "</div>\");");
        }

        public static void SetHighLight(this IWebElement webElement, string color = "blue")
        {
            IWebDriver driver = (webElement as IWrapsDriver)?.WrappedDriver ?? WebDriverFactoryVS.GetDriver();
            SetHighLight(webElement, driver, color);
        }

        public static void UnsetHighLight(this IWebElement webElement)
        {
            ((IJavaScriptExecutor)GetDriver(webElement)).ExecuteScript("$('#HighLight-TesteAutomatizado').remove()");
        }

        public static void ClickCustom(this IWebElement webElement)
        {
            string Validacao = "Validação do click";
            if (!(webElement.Enabled && webElement.Displayed))
                Ensure.Fail(Validacao, "Elemento disponivel e clicar com sucesso", "O elemento não esta disponivel para ser clicado");
            else
                Validacao = "Validação do click no Elemento: \n" +
                    "Tag: " + webElement.TagName + 
                    " Id: " + webElement.GetAttribute("id")
                    ;
            try
            {
                WaitUtils.WaitUntilActionCuston(()=> webElement.Click());
                WaitUtils.WaitUntilActionCuston(() => webElement.SetHighLight());
                Ensure.Pass(Validacao, "Clicar com sucesso", "Clique realizado com sucesso");
                WaitUtils.WaitUntilActionCuston(() => webElement.UnsetHighLight());
            }
            catch (Exception e )
            {
                Ensure.Fail(Validacao, "Clicar com sucesso", "Não foi possivel clicar no botão Erro: "+ e.ToString());
            }

        }
    }
}
