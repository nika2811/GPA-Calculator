using System.Collections.Generic;
using System.Threading.Tasks;
using GPA_Calculator.Controllers;
using GPA_Calculator.Db;
using GPA_Calculator.Models;
using GPA_Calculator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GPA_Calculator.UnitTests
{
    public class StudentControllerTestV2
    {
        [Fact]
        public async Task RegisterStudent_AddsStudentToContext_ReturnsOkResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StudentGradesContext>()
                .UseInMemoryDatabase("RegisterStudent_AddsStudentToContext_ReturnsOkResult")
                .Options;

            var context = new StudentGradesContext(options);
            var calculator = new GpaCalculator();
            var controller = new StudentsController(context, calculator);

            var student = new Student
            {
                Name = "John",
                Surname = "Doe",
                PersonalNumber = "1234567890",
                Course = "Computer Science"
            };

            // Act
            var result = await controller.RegisterStudent(student);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(student, okResult.Value);
            Assert.Equal(1, await context.Students.CountAsync());
        }

        [Fact]
        public async Task RegisterSubject_AddsSubjectToContext_ReturnsOkResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StudentGradesContext>()
                .UseInMemoryDatabase("RegisterSubject_AddsSubjectToContext_ReturnsOkResult")
                .Options;

            var context = new StudentGradesContext(options);
            var calculator = new GpaCalculator();
            var controller = new StudentsController(context, calculator);

            var subject = new Subject
            {
                Name = "Math",
                Credits = 3
            };

            // Act
            var result = await controller.RegisterSubject(subject);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(subject, okResult.Value);
            Assert.Equal(1, await context.Subjects.CountAsync());
        }

        [Fact]
        public async Task PostGrade_AddsGradeToContext_ReturnsOkResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StudentGradesContext>()
                .UseInMemoryDatabase("PostGrade_AddsGradeToContext_ReturnsOkResult")
                .Options;

            var context = new StudentGradesContext(options);
            var calculator = new GpaCalculator();
            var controller = new StudentsController(context, calculator);

            var student = new Student
            {
                Name = "John",
                Surname = "Doe",
                PersonalNumber = "1234567890",
                Course = "Computer Science"
            };
            context.Students.Add(student);
            await context.SaveChangesAsync();

            var subject = new Subject
            {
                Name = "Math",
                Credits = 3
            };

            // Act
            var result = await controller.RegisterSubject(subject);

            // Assert
            var createdResult = Assert.IsType<OkObjectResult>(result);
            var createdSubject = Assert.IsType<Subject>(createdResult.Value);
            Assert.Equal(subject, createdSubject);

            using (var assertContext = new StudentGradesContext(options))
            {
                var subjectsInDb = await assertContext.Subjects.ToListAsync();
                Assert.Single(subjectsInDb);
                Assert.Equal(subject, subjectsInDb[0]);
            }
        }

        [Fact]
        public async Task PostGrade_AddsGradeToDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StudentGradesContext>()
                .UseInMemoryDatabase(databaseName: "AddGradeDb")
                .Options;
            var context = new StudentGradesContext(options);
            var calculator = new GpaCalculator();
            var controller = new StudentsController(context, calculator);

            var student = new Student { Id = 1, Name = "John", Surname = "Doe" };
            var subject = new Subject { Id = 1, Name = "Math", Credits = 3 };
            var grade = new Grade { StudentId = 1, SubjectId = 1, Score = 90 };

            context.Students.Add(student);
            context.Subjects.Add(subject);
            await context.SaveChangesAsync();

// Act
            var result = await controller.PostGrade(student.Id, grade);

// Assert
            var savedGrade =
                context.Grades.FirstOrDefault(g => g.StudentId == student.Id && g.SubjectId == subject.Id);
            Assert.NotNull(savedGrade);
            Assert.Equal(grade.Score, savedGrade.Score);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
        
