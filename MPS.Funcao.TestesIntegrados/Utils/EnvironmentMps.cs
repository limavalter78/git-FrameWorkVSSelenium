using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.Funcao.TestesIntegrados.Suite;

namespace MPS.Funcao.TestesIntegrados.Utils
{
 
    public class EnvironmentMps : SuiteBase
    {
        public static string Ambiente;


        public static string RetornarUrl()
        {

            /////Infomar A URL para execução dos testes -  ( definido no coluna Url_Test planilha de massa de dados ) //////////////////////

          
            Ambiente = "Teste";
            return Util.GetTestContext().DataRow["Url_Test"].ToString();

        }

    
        ///// Parametriza o navegador para início dos testes serão executados
        internal static string RetornarBrowser()
        {


            //Infomar na massa de dados o browser para execução dos testes -  (ie, chrome, firefox ou edge )
            
            return Util.GetTestContext().DataRow["Browser"].ToString();

            
        }


        ////////////////////  THIAGO - IMPLEMENTAÇÃO /////////////////////////////////////////////////////

        /// <summary>
        /// Retorna a Url alvo de teste 
        /// </summary>
        /// <returns>String url</returns>
        public static string RetornarURL1() => System.Configuration.ConfigurationManager.AppSettings["Ambiente"].ToString();

        /// <summary>
        /// Retorna o navegador onde os testes devem ser executados
        /// </summary>
        /// <returns>string Navegador</returns>
        public static string RetornarBrowser1() => System.Configuration.ConfigurationManager.AppSettings["Browser"].ToString();

        public static string RetornarEquipe() => System.Configuration.ConfigurationManager.AppSettings["Equipe"].ToString();

        public static string RetornarProjeto() => System.Configuration.ConfigurationManager.AppSettings["Projeto"].ToString();

        public static string RetornarPerfil() => System.Configuration.ConfigurationManager.AppSettings["Perfil"].ToString();

        public static string DiretorioEvidencia() => System.Configuration.ConfigurationManager.AppSettings["DirEvidencia"].ToString();






    }
}
