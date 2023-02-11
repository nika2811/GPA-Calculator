using GPA_Calculator.Models;
using Xunit;

namespace GPA_Calculator.UnitTests;

public class GPA_CalculatorTests
{
    [Fact]
    public async Task Calculate_ValidStudentId_ReturnsGPA()
    {
        // Arrange
        var grades = new List<Grade>
        {
            new() { SubjectId = 1, Score = 95, StudentId = 1 },
            new() { SubjectId = 2, Score = 80, StudentId = 1 },
            new() { SubjectId = 3, Score = 75, StudentId = 1 }
        };

        var subjects = new List<Subject>
        {
            new() { Id = 1, Credits = 4 },
            new() { Id = 2, Credits = 3 },
            new() { Id = 3, Credits = 2 }
        };


        // Act
        var gpa = await Calculate(grades, subjects);

        // Assert
        Assert.Equal(3.22, gpa, 2);
    }


    public Task<double> Calculate(List<Grade> grades, List<Subject> subjects)
    {
        if (grades == null || grades.Count == 0) throw new KeyNotFoundException("Grades not found");
        var totalScore = 0.0;
        var totalCredits = 0;
        foreach (var grade in grades)
        {
            var subject = subjects.FirstOrDefault(s => s.Id == grade.SubjectId);
            var score = ConvertToWentworthScale(grade.Score);
            if (subject != null)
            {
                totalScore += score * subject.Credits;
                totalCredits += subject.Credits;
            }
        }

        return Task.FromResult(totalScore / totalCredits);
    }

    private static double ConvertToWentworthScale(int score)
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