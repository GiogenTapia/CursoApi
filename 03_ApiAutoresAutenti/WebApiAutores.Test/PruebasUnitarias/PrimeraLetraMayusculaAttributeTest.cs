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
            //Preparación
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            var valor = "gio";
            var valContext = new ValidationContext( new {Nombre = valor});

            //Ejecución
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            //Verificación
            Assert.AreEqual("La primera letra debe ser mayuscula", resultado.ErrorMessage);
        }
    }
}