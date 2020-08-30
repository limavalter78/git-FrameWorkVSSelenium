using MPS.Funcao.TestesIntegrados.WebDriverFactory;
using System;

namespace MPS.Funcao.TestesIntegrados.Evidencias
{
    public class InitEvidenceAttribute : Attribute
    {
        public InitEvidenceAttribute()
        {
            //IWebDriver driver = Activator.CreateInstance(type: webDriver) as IWebDriver;
            Evidencia.Iniciar(WebDriverFactoryMps.GetDriver());
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
