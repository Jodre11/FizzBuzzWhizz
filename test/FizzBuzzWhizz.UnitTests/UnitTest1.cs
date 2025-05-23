using Xunit;

namespace FizzBuzzWhizz.UnitTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var subject = new Class1();

        // Act
        var result = subject.Add(1, 1);

        // Assert
        Assert.Equal(2, result);
    }
}