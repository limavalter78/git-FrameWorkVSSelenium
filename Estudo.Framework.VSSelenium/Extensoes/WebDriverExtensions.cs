using Estudo.Framework.VSSelenium.Evidencias;
using OpenQA.Selenium;

namespace TestesIntegrados.Extensoes
{
	public class WebDriverExtensions
	{
		/// <summary>
		/// Navega entre as Abas a partir da URL 
		/// </summary>
		/// <param name="webDriver"></param>
		/// <param name="urlDesejada"></param>
		/// <returns></returns>
		public static IWebDriver NavegaOutraPagina(IWebDriver webDriver, string urlDesejada)
		{
			var paginas = webDriver.WindowHandles;
			for (int i = 0; i <= paginas.Count - 1; i++)
			{
				if (webDriver.SwitchTo().Window(paginas[i]).Url.Contains(urlDesejada))
				{
					Ensure.Pass(webDriver.Url, urlDesejada, "Valida acesso a URL de '" + urlDesejada + "'");
					return webDriver.SwitchTo().Window(paginas[i]);
				}
			}
			Ensure.Fail("Valida Navegação", "Navegar para a url :" + urlDesejada, "foi encontrado nenhuma instancia (única) de Brownser que contenha a url desejada");
			return null;
		}
	}
}
