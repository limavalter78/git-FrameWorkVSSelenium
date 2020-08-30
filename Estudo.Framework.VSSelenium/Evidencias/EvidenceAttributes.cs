
using Estudo.Framework.VSSelenium.WebDriverFactory;
using System;

namespace Estudo.Framework.VSSelenium.Evidencias
{
    public class InitEvidenceAttribute : Attribute
    {
        public InitEvidenceAttribute()
        {
            //IWebDriver driver = Activator.CreateInstance(type: webDriver) as IWebDriver;
            Evidencia.Iniciar(WebDriverFactoryVS.GetDriver());
        }
    }

    public class ParamsEvidenceAttribute : Attribute
    {
        public ParamsEvidenceAttribute(string nomeCenarioTeste, string idTFS, string nomeArquivo, TipoEvidencias tipoEvidencia = TipoEvidencias.PADRÃO_SEMPRE, FormatoEvidencia formato = FormatoEvidencia.HTML)
        {
            Evidencia.ParametrizaEvidencia(nomeCenarioTeste, idTFS, nomeArquivo, tipoEvidencia, formato);
        }
    }


    public class FinalizeEvidenceAttribute : Attribute
    {
        public FinalizeEvidenceAttribute()
        {
            Evidencia.Finalizar();
        }
    }

}
