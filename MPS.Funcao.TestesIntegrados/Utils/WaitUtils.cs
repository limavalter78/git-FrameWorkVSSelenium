using MPS.Funcao.TestesIntegrados.WebDriverFactory;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.Funcao.TestesIntegrados.Wrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPS.Funcao.TestesIntegrados.Utils
{
    public static class WaitUtils
    {

        public static void WaitUntilElementToBeClickable(IWebElement element)
        {
            IWebDriver driver = WebDriverFactoryMps.GetDriver();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));

            //wait.IgnoreExceptionTypes(typeof(Exception));

            wait.Until(ExpectedConditions.ElementToBeClickable(element));


            //wait.Until(ExpectedConditions.ElementToBeSelected(element));

        }

        public static void WaitUntilInvisibilityOfElementLocated(IWebElement element)
        {
            IWebDriver driver = WebDriverFactoryMps.GetDriver();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            wait.IgnoreExceptionTypes(typeof(Exception));

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id(element.GetAttribute("id"))));

            //wait.Until(ExpectedConditions.ElementToBeSelected(element));

        }




        /// <summary>
        /// Realiza uma tentativa a cada 0,5s e tem como timeout 60s
        /// </summary>
        /// <param name="funcao"></param>
        /// <returns></returns>
        public static bool WaitUntilCuston(Func<bool> funcao, out Exception e)
        {
            e = null;
            for (int i = 0; i < 120; i++)
            {
                try
                {
                    if (funcao())
                    {
                        return true;
                    }
                }
                catch (Exception err)
                {
                    Thread.Sleep(500);
                    e = err;
                }
            }

            return false;
        }


        public static bool WaitUntilfuncCuston(Func<bool> funcao)
        {
            for (int i = 0; i < 120; i++)
            {
                try
                {
                    if (funcao())
                    {
                        return true;
                    }
                }
                catch (Exception err)
                {
                    Thread.Sleep(500);
                }
            }
            return false;
        }

        public static void WaitUntilActionCuston(Action funcao)
        {
            Exception e = null;
            for (int i = 0; i < 120; i++)
            {
                try
                {
                    funcao();
                    return;
                }
                catch (Exception err)
                {
                    Thread.Sleep(500);
                    e = err;
                }
            }

            if (e != null)
            {
                throw new Exception("deu ruim...." +e.Message);
            }
        }

        public static IWebElement WaitUntilCustonElement(IWebElement element, Action<IWebElement> funcao)
        {
            Exception e = null;
            for (int i = 0; i < 120; i++)
            {
                try
                {
                    funcao(element);
                    return element;
                }
                catch (Exception err)
                {
                    Thread.Sleep(500);
                    e = err;
                }
            }

            if (e != null)
                throw e;

            return null;
        }










        public static bool WaitUntilAsAllFunctions(out Exception e, params Func<bool>[] funcaos)
        {
            e = null;
            bool retorno = false;
            foreach (var funcao in funcaos)
            {
                retorno = false;
                for (int i = 0; i < 120; i++)
                {
                    try
                    {
                        if (funcao())
                        {
                            retorno = true;
                            break;
                        }
                    }
                    catch (Exception err)
                    {
                        retorno = false;
                        e = err;
                        Thread.Sleep(500);
                    }
                }

                if (!retorno)
                    return false;
            }
            return retorno;
        }



        public static void WaitUntilAsAllActions(params Action[] funcaos)
        {
            Exception e;
            int f = 0;
            foreach (var funcao in funcaos)
            {
                f++;
                for (int i = 1; i <= 120; i++)
                {
                    try
                    {
                        funcao();
                        break;
                    }
                    catch (Exception err)
                    {
                        Thread.Sleep(500);
                        e = err;
                    }

                    if (i == 120)
                        throw new Exception("funcao " + f + " falhou", e);
                }

            }
        }
    }


}
