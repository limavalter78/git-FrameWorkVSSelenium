using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPS.Funcao.TestesIntegrados.Evidencias
{
    public static class Ensure
    {
        public static void Fail(string tituloValidacao, string valorEsperado, string valorObtido)
        {
            Evidencia.GerarEvidencia(tituloValidacao, valorEsperado, valorObtido, false);
            Assert.Fail(tituloValidacao + "\n" + "Era esperado: " + valorEsperado + "\n" + "O Valor Obtido foi: " + valorObtido);
        }

        public static void Fail(string tituloValidacao, string valorEsperado, string valorObtido, Exception e)
        {
            Evidencia.GerarEvidencia(tituloValidacao, valorEsperado, valorObtido, false);
            Assert.Fail(tituloValidacao + "\n" + "Era esperado: " + valorEsperado + "\n" + "O Valor Obtido foi: " + valorObtido +
                "******** Exception ********\n" +
                "Mensage: " + e.Message +
                "InnerException: " + e.InnerException +
                "StackTrace: " + e.StackTrace);
        }

        public static void GreaterThan(string tituloValidacao, int valorEsperado, int valorObtido)
        {
            Evidencia.GerarEvidencia(tituloValidacao, "Valor deve ser maior que:" + valorEsperado, valorObtido.ToString(), valorEsperado < valorObtido);
            Assert.IsTrue(valorEsperado < valorObtido, "O valor esperado (" + valorEsperado + ") não é maior que o obtido(" + valorObtido + ")");
        }

        public static void LessThan(string tituloValidacao, int valorEsperado, int valorObtido)
        {
            Evidencia.GerarEvidencia(tituloValidacao, "Valor deve ser menor que:" + valorEsperado, valorObtido.ToString(), valorEsperado > valorObtido);
            Assert.IsTrue(valorEsperado > valorObtido, "O valor esperado (" + valorEsperado + ") não é maior que o obtido(" + valorObtido + ")");
        }

        public static void GreaterOrEqualThan(string tituloValidacao, int valorEsperado, int valorObtido)
        {
            Evidencia.GerarEvidencia(tituloValidacao, "Valor deve ser maior que:" + valorEsperado, valorObtido.ToString(), valorEsperado <= valorObtido);
            Assert.IsTrue(valorEsperado <= valorObtido, "O valor esperado (" + valorEsperado + ") não é maior que o obtido(" + valorObtido + ")");
        }

        public static void LessOrEqualThan(string tituloValidacao, int valorEsperado, int valorObtido)
        {
            Evidencia.GerarEvidencia(tituloValidacao, "Valor deve ser menor que:" + valorEsperado, valorObtido.ToString(), valorEsperado >= valorObtido);
            Assert.IsTrue(valorEsperado >= valorObtido, "O valor esperado (" + valorEsperado + ") não é maior que o obtido(" + valorObtido + ")");
        }

        public static void Pass(string tituloValidacao, string valorEsperado, string valorObtido)
        {
            Evidencia.GerarEvidencia(tituloValidacao, valorEsperado, valorObtido, true);
        }

        public static void AreEquals(object expected, object actual, string message)
        {
            Evidencia.GerarEvidencia(message, expected.ToString(), actual.ToString(), expected.Equals(actual));
            Assert.AreEqual(expected, actual, message);
        }

        public static void AreNotEquals(object expected, object actual, string message)
        {
            Evidencia.GerarEvidencia(message, "Diferente de: " + expected.ToString(), actual.ToString(), !expected.Equals(actual));
            Assert.AreNotEqual(expected, actual, message);
        }

        public static void IsTrue(bool isTrue, string message)
        {
            Evidencia.GerarEvidencia(message, "True", isTrue.ToString(), isTrue == true);
            Assert.IsTrue(isTrue, message);
        }

        public static void IsFalse(bool isFalse, string message)
        {
            Evidencia.GerarEvidencia(message, "false", isFalse.ToString(), isFalse == false);
            Assert.IsFalse(isFalse, message);
        }

        public static void StringContains(string value, string substring, string message)
        {
            Evidencia.GerarEvidencia(message, "Contém: " + substring, value, value.Contains(substring));
            StringAssert.Contains(value, substring, message);
        }

        public static void CollectionAreEqual(ICollection collectionExpected, ICollection collectionActual, string message)
        {
            var colecaoA = string.Join(" ;", (collectionExpected as List<object>).Select(x => x.ToString()));
            var colecaoB = string.Join(" ;", (collectionActual as List<object>).Select(x => x.ToString()));

            Evidencia.GerarEvidencia(message, "[" + colecaoA + "]", "[" + colecaoB + "]", collectionExpected.Equals(collectionActual));
            CollectionAssert.AreEqual(collectionExpected, collectionActual);
        }

        public static void CollectionAreNotEqual(ICollection<string> collectionExpected, ICollection<string> collectionActual, string message)
        {
            var colecaoA = string.Join(" ;", collectionExpected);
            var colecaoB = string.Join(" ;", collectionActual);

            Evidencia.GerarEvidencia(message, "A coleção abaixo deve ser diferente da coleção obtida: \n" + "[" + colecaoA + "]", "[" + colecaoB + "]", !collectionExpected.Equals(collectionActual));
            CollectionAssert.AreNotEqual(collectionExpected.ToList(), collectionActual.ToList());
        }

        public static void CollectionContains(ICollection collectionExpected, object element, string message)
        {
            var colecaoA = string.Join(" ;", (collectionExpected as List<object>).Select(x => x.ToString()));

            Evidencia.GerarEvidencia(message,"A coleção deve conter o elemento: "+ element.ToString(), "[" + colecaoA + "]",  (collectionExpected as List<object>).Contains(element.ToString()));
            CollectionAssert.Contains(collectionExpected, element);
        }
        //TODO: TO BE CONTINUED... 
    }
}
