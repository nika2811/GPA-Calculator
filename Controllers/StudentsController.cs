using GPA_Calculator.Db;
using GPA_Calculator.Models;
using GPA_Calculator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GPA_Calculator.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly GpaCalculator _calculator;
    private readonly StudentGradesContext _context;

    public StudentsController(StudentGradesContext context, GpaCalculator calculator)
    {
        _context = context;
        _calculator = calculator;
    }

    [Route("/students")]
    [HttpPost]
    public async Task<ActionResult<Student>> RegisterStudent(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return Ok(student);
    }

    [Route("/subjects")]
    [HttpPost]
    public async Task<ActionResult<Subject>> RegisterSubject(Subject subject)
    {
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
        return Ok(subject);
    }

    [HttpPost("{studentId}/grades")]
    public async Task<ActionResult<Grade>> PostGrade(int studentId, Grade grade)
    {
        grade.StudentId = studentId;
        _context.Grades.Add(grade);
        await _context.SaveChangesAsync();

        return Ok(grade);
    }

    [HttpGet("{studentId}/grades")]
    public async Task<ActionResult<IEnumerable<Grade>>> GetGrades(int studentId)
    {
        var grades = await _context.Grades
            .Where(g => g.StudentId == studentId)
            .Include(g => g.Subject)
            .ToListAsync();

        return grades;
    }

    // [Route("/students/{studentId}/gpa")]
    // [HttpGet]
    // public async Task<ActionResult<double>> GetStudentGPA(int studentId)
    // {
    //     var grades = await _context.Grades
    //         .Where(g => g.StudentId == studentId)
    //         .Include(g => g.Subject)
    //         .ToListAsync();
    //
    //     double totalScore = 0;
    //     var totalCredits = 0;
    //     foreach (var grade in grades)
    //     {
    //         totalScore += _calculator.ConvertToWentworthScale(grade.Score) * grade.Subject.Credits;
    //         totalCredits += grade.Subject.Credits;
    //     }
    //
    //
    //     // var student = _context.Students
    //     //     .Include(s => s.Grades)
    //     //     .SingleOrDefault(s => s.Id == studentId);
    //     //
    //     // double totalScore = 0;
    //     // int totalCredits = 0;
    //     // for (var i = 0; i < student.Grades.Count; i++)
    //     // {
    //     //     var grade = student.Grades[i];
    //     //     totalScore += _calculator.ConvertToWentworthScale(grade.Score) * grade.Subject.Credits;
    //     //     totalCredits += grade.Subject.Credits;
    //     // }
    //
    //     var GPA = totalScore / totalCredits;
    //     return Ok(GPA);
    // }


    //         Not Async

    // [Route("/students/{studentId}/gpa")]
    // [HttpGet]
    // public ActionResult<double> GetStudentGPA(int studentId)
    // {
    //     var student = _context.Students.Find(studentId);
    //     if (student == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     double totalPoints = 0;
    //     int totalCredits = 0;
    //
    //     var gpaCalculator = new GpaCalculator();
    //     foreach (var grade in student.Grades)
    //     {
    //         var subject = _context.Subjects.Find(grade.SubjectId);
    //         if (subject == null)
    //         {
    //             return NotFound();
    //         }
    //
    //         var score = grade.Score;
    //         var points = gpaCalculator.ConvertToWentworthScale(score) * subject.Credits;
    //
    //         totalPoints += points;
    //         totalCredits += subject.Credits;
    //     }
    //
    //     var gpa = totalPoints / totalCredits;
    //
    //     return Ok(gpa);
    // }

    [Route("/students/{studentId}/gpa")]
    [HttpGet]
    public async Task<double> GetStudentGPA(int studentId)
    {
        // Load the grades for the specified student
        var grades = await _context.Grades.Where(g => g.StudentId == studentId).ToListAsync();

        // Convert the grades to the Wentworth scale
        var totalScore = 0.0;
        var totalCredits = 0;
        foreach (var grade in grades)
        {
            var subject = await _context.Subjects.FindAsync(grade.SubjectId);
            var score = _calculator.ConvertToWentworthScale(grade.Score);
            totalScore += score * subject.Credits;
            totalCredits += subject.Credits;
        }

        // Return the GPA
        return totalScore / totalCredits;
    }
}