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

    [Route("students")]
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

        if (!grades.Any()) return NotFound("No grades found for the specified student");

        return grades;
    }

    [Route("/students/{studentId}/gpa")]
    [HttpGet]
    public async Task<ActionResult<double>> GetStudentGpa(int studentId)
    {
        var gpa = await _calculator.Calculate(studentId);
        return Ok(gpa);
    }
}