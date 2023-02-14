using GPA_Calculator.Models;
using Xunit;

namespace GPA_Calculator.UnitTests;

public class GPA_CalculatorTests
{
    private readonly UnitTestFunctions _unitTestFunctions;

    public GPA_CalculatorTests(UnitTestFunctions unitTestFunctions)
    {
        _unitTestFunctions = unitTestFunctions;
    }

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
        var gpa = await _unitTestFunctions.Calculate(grades, subjects);

        // Assert
        Assert.Equal(3.22, gpa, 2);
    }
}