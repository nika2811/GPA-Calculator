using GPA_Calculator.Db;
using GPA_Calculator.Models;
using GPA_Calculator.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GPA_Calculator.UnitTests;

public class StudentsControllerTests
{
    private readonly GpaCalculator _calculator;
    private readonly StudentGradesContext _context;

    public StudentsControllerTests(GpaCalculator calculator)
    {
        _calculator = calculator;
        var options = new DbContextOptionsBuilder<StudentGradesContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new StudentGradesContext(options);
    }

    [Fact]
    public async void TestGetStudentGPA_SubjectsAreSavedSuccessfully()
    {
        // Arrange
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
    public async Task TestGetStudentGPA_CalculateGPA()
    {
        // Arrange
        var student = new Student { Id = 1, Name = "John Doe" };
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();

        var subject1 = new Subject { Id = 1, Name = "Math", Credits = 3 };
        await _context.Subjects.AddAsync(subject1);
        var subject2 = new Subject { Id = 2, Name = "English", Credits = 3 };
        await _context.Subjects.AddAsync(subject2);

        var grade1 = new Grade { Id = 1, StudentId = 1, SubjectId = 1, Score = 85 };
        await _context.Grades.AddAsync(grade1);
        var grade2 = new Grade { Id = 2, StudentId = 1, SubjectId = 2, Score = 75 };
        await _context.Grades.AddAsync(grade2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _calculator.Calculate(1);

        // Assert
        Assert.IsType<OkResult>(result);
    }


    [Fact]
    public async void TestGetStudentGPA_InvalidStudentId_ReturnsNotFound()
    {
        // Arrange
        var studentId = 99;

        // Act
        var result = await _calculator.Calculate(studentId);

        // Assert
        Assert.IsType<NotFound>(result);
    }
}