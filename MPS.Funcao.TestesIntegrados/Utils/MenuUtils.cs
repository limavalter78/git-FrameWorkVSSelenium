using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Internal;
using MPS.Funcao.TestesIntegrados.Extensoes;
using MPS.Funcao.TestesIntegrados.WebDriverFactory;
using System.Threading;
using MPS.Funcao.TestesIntegrados.Evidencias;

namespace MPS.Funcao.TestesIntegrados.Utils
{
    public class MenuUtils
    {
        private IWebElement element;
        private IWebDriver driver;
        //private Actions actions;

        public MenuUtils(IWebElement element)
        {
            this.element = element;
            this.driver = WebDriverFactoryMps.GetDriver();
        }

        public void NavegaMenu(String caminho)
        {
            String[] caminhoPao;
            caminhoPao = caminho.Split(';');
            for (int i = 0; caminhoPao.Length > i; i++)
            {
                String menuAlvo = caminhoPao[i];
                var menus = element.FindElements(By.TagName("li"));

                foreach (var menu in menus)
                {
                    var teste = menu.GetAttribute("innerText").ToString().Replace("\r\n", "*").Split('*');
                    var strteste = teste.Where(x => !String.IsNullOrWhiteSpace(x)).ToArray().FirstOrDefault()?.Trim()?.ToLower()??"";

                    if (strteste.Equals(menuAlvo.ToLower()) )
                    {
                        var item = menu.FindElement(By.TagName("A"));
                        Console.WriteLine(item.Text);
                        item.SetHighLight();
                        Ensure.Pass("Valida navegação do menu","item de menu deve existir", "Menu foi encontrado");
                        item.UnsetHighLight();
                        item.SetFocus().Click();
                        element = menu;
                        break;
                    }
                }
            }
            Thread.Sleep(500);
            Ensure.Pass("Apresenta resultado da Navegação", "", "");
        }
    }
}
