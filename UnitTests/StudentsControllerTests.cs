using GPA_Calculator.Controllers;
using GPA_Calculator.Db;
using GPA_Calculator.Models;
using GPA_Calculator.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace GPA_Calculator.UnitTests;

public class StudentsControllerTests
{
    private readonly StudentGradesContext _context;
    private readonly StudentsController _controller;
    private readonly ITestOutputHelper _testOutputHelper;

    public StudentsControllerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var options = new DbContextOptionsBuilder<StudentGradesContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var calculator = new GpaCalculator();
        _context = new StudentGradesContext(options);
        _controller = new StudentsController(_context, calculator);
    }

    /*[Fact]
    public async void TestGetStudentGPA_ValidInput_ReturnsCorrectGPA()
    {
        // Arrange
        const int studentId = 1;

        var subject1 = new Subject { Id = 1, Name = "Math", Credits = 3 };
        var subject2 = new Subject { Id = 2, Name = "History", Credits = 4 };
        var subject3 = new Subject { Id = 3, Name = "Physics", Credits = 5 };

        _context.Subjects.Add(subject1);
        _context.Subjects.Add(subject2);
        _context.Subjects.Add(subject3);
        await _context.SaveChangesAsync();

        var grade1 = new Grade { Id = 1, StudentId = studentId, SubjectId = subject1.Id, Score = 85 };
        var grade2 = new Grade { Id = 2, StudentId = studentId, SubjectId = subject2.Id, Score = 80 };
        var grade3 = new Grade { Id = 3, StudentId = studentId, SubjectId = subject3.Id, Score = 90 };

        _context.Grades.Add(grade1);
        _context.Grades.Add(grade2);
        _context.Grades.Add(grade3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetStudentGPA(studentId);

        // Assert
        Assert.IsType<double>(result.Value);
        Assert.Equal(86.67, result.Value, 2);
    }*/

    [Fact]
    public async void TestGetStudentGPA_SubjectsAreSavedSuccessfully()
    {
// Arrange
        const int studentId = 1;

        var subject1 = new Subject { Id = 1, Name = "Math", Credits = 3 };
        var subject2 = new Subject { Id = 2, Name = "History", Credits = 4 };
        var subject3 = new Subject { Id = 3, Name = "Physics", Credits = 5 };

// Act
        _context.Subjects.Add(subject1);
        _context.Subjects.Add(subject2);
        _context.Subjects.Add(subject3);
        await _context.SaveChangesAsync();

// Assert
        Assert.Equal(3, _context.Subjects.Count());
    }

    [Fact]
    public async void TestGetStudentGPA_GradesAreSavedSuccessfully()
    {
// Arrange
        const int studentId = 1;

        var subject1 = new Subject { Id = 1, Name = "Math", Credits = 3 };
        var subject2 = new Subject { Id = 2, Name = "History", Credits = 4 };
        var subject3 = new Subject { Id = 3, Name = "Physics", Credits = 5 };

        _context.Subjects.Add(subject1);
        _context.Subjects.Add(subject2);
        _context.Subjects.Add(subject3);
        await _context.SaveChangesAsync();

        var grade1 = new Grade { Id = 1, StudentId = studentId, SubjectId = subject1.Id, Score = 85 };
        var grade2 = new Grade { Id = 2, StudentId = studentId, SubjectId = subject2.Id, Score = 80 };
        var grade3 = new Grade { Id = 3, StudentId = studentId, SubjectId = subject3.Id, Score = 90 };

// Act
        _context.Grades.Add(grade1);
        _context.Grades.Add(grade2);
        _context.Grades.Add(grade3);
        await _context.SaveChangesAsync();

// Assert
        var result = await _controller.GetGrades(1);
        var returnedGrades = result.Value.ToList();

        // Assert
        Assert.Equal(3, returnedGrades.Count);
        Assert.Equal(1, returnedGrades[0].Id);
        Assert.Equal(2, returnedGrades[1].Id);

        Assert.Equal(3, _context.Grades.Count());
    }


    [Fact]
    public async void TestGetStudentGPA_CalculateGPA()
    {
// Arrange
        const int studentId = 1;
// Act
        var result = await _controller.GetStudentGPA(studentId);

// Assert
        Assert.IsType<double>(result);
        Assert.Equal(86.67, result, 2);
    }


    [Fact]
    public async void TestGetStudentGPA_InvalidStudentId_ReturnsNotFound()
    {
        // Arrange
        var studentId = 99;

        // Act
        var result = await _controller.GetStudentGPA(studentId);

        // Assert
        Assert.IsType<NotFound>(result);
    }
}