namespace FurudType.Core;

public class MetricsCalculator
{
    public double CalculateAccurancy(int currentIndex, int totalKeyPressed)
    {
        double accurancy = ((double)currentIndex / totalKeyPressed) * 100;
        return Math.Round(accurancy, 2);
    }

    public int CalculateCRM(int currentIndex, TimeSpan elapsedTime)
    {
        if (elapsedTime.Seconds == 0 || currentIndex == 0)
        {
            return 0;
        }

        double result = (double)currentIndex / elapsedTime.Seconds * 60;

        return (int)Math.Round(result);
    }
}
