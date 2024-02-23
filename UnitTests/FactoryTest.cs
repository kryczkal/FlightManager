using NUnit.Framework;
using Classes;

namespace Factory.Tests
{
    public class FactoryTests
    {
        [Test]
        public void Create_ValidType_ReturnsInstance()
        {
            // Arrange
            var factory = new BaseFactory();
            var expectedInstance = new Passenger();

            // Act
            var instance = factory.Create("P");

            // Assert
            Assert.That(instance, Is.InstanceOf<Passenger>());
        }


        [Test]
        public void Create_InvalidType_ThrowsArgumentException()
        {
            // Arrange
            var factory = new BaseFactory();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => factory.Create("InvalidType"));
        }
    }
}