namespace FurudType.Core.Tests;

public class MetricsCalculatorTests
{
    [Theory]
    [InlineData(10, 10, 100)]
    [InlineData(10, 12, 83.33)]
    public void CalculateAccurancyTest(int currentIndex, int totalKeyPressed, float expected)
    {
        // Arrange
        MetricsCalculator calculator = new MetricsCalculator();

        // Act
        double accurancy = calculator.CalculateAccurancy(currentIndex, totalKeyPressed);

        // Assert
        Assert.Equal(expected, accurancy, precision: 2);
    }

    [Theory]
    [InlineData(120, 40, 180)]
    [InlineData(10, 30, 20)]
    [InlineData(200, 50, 240)]
    [InlineData(200, 0, 0)]
    [InlineData(1, 1, 60)]
    [InlineData(0, 1, 0)]
    public void CalculateSuccessCPMTest(int currentIndex, int seconds, int expected)
    {
        // Act
        MetricsCalculator calculator = new MetricsCalculator();
        TimeSpan elapsedTime = TimeSpan.FromSeconds(seconds);


        // Act
        int result = calculator.CalculateCRM(currentIndex, elapsedTime);

        // Arrange
        Assert.Equal(expected, result);
    }
}
