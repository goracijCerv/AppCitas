namespace AppcitasUniTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            int x = 2;
            //Act
            int y = 1 * x;
            //Asert
            Assert.Equal(2, y);
        }
    }
}
//las tres partes que se deben tener o se llaman son arrage podemos decir que se establecen las variables, act
// seria lo que se va hacer con esas variables, asert seria la evaluacion de los resultados