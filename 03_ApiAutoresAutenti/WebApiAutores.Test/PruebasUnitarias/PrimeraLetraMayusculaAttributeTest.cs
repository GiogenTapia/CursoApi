using _02_ApiAutores.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Test.PruebasUnitarias
{
    [TestClass]
    public class PrimeraLetraMayusculaAttributeTest
    {
        [TestMethod]
        public void PrimeraLetraMinuscula_DevuelveError()
        {
            //Preparaci�n
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            var valor = "gio";
            var valContext = new ValidationContext( new {Nombre = valor});

            //Ejecuci�n
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            //Verificaci�n
            Assert.AreEqual("La primera letra debe ser mayuscula", resultado.ErrorMessage);
        }


        [TestMethod]
        public void ValorNulo_NoDevuelveError()
        {
            //Preparaci�n
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            string valor = null;
            var valContext = new ValidationContext(new { Nombre = valor });

            //Ejecuci�n
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            //Verificaci�n
            Assert.IsNull(resultado);
        }


        [TestMethod]
        public void ValorConPrimeraLetraMayuscula_NoDevuelveError()
        {
            //Preparaci�n
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            string valor = "Giovanni";
            var valContext = new ValidationContext(new { Nombre = valor });

            //Ejecuci�n
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            //Verificaci�n
            Assert.IsNull(resultado);
        }
    }

}