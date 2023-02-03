namespace GPA_Calculator.Services;

public class GpaCalculator
{
    public double ConvertToWentworthScale(int score)
    {
        if (score >= 90)
            return 4;
        if (score >= 80)
            return 3;
        if (score >= 70)
            return 2;
        if (score >= 60)
            return 1;
        if (score >= 51)
            return 0.5;
        return 0;
    }
}