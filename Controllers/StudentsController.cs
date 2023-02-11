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
    private readonly SubjectScores _subjectScores;

    public StudentsController(StudentGradesContext context, GpaCalculator calculator, SubjectScores subjectScores)
    {
        _context = context;
        _calculator = calculator;
        _subjectScores = subjectScores;
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
        var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
        if (student == null) return NotFound("Student not found");

        student.GPA = await _calculator.Calculate(studentId);
        _context.Students.Update(student);
        await _context.SaveChangesAsync();

        return Ok(student.GPA);
    }


    [Route("/top10")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetTop10()
    {
        var top10 = await _context.Students
            .OrderByDescending(s => s.GPA)
            .Take(10)
            .ToListAsync();

        return top10;
    }

    [Route("api/subjects/top3")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Subject>>> GetTop3Subjects()
    {
        var grades = await _context.Grades.ToListAsync();
        var subjects = await _context.Subjects.ToListAsync();

        var gradeCalculator = new GradeCalculator();
        var top3Subjects = gradeCalculator.GetTop3Subjects(grades, subjects);

        if (top3Subjects == null || top3Subjects.Count == 0) return NotFound();

        return top3Subjects;
    }


    [Route("api/subjects/bottom3")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Subject>>> GetBottom3Subjects()
    {
        var grades = await _context.Grades.ToListAsync();
        var subjects = await _context.Subjects.ToListAsync();

        var subjectScores = _subjectScores.GetSubjectScores(grades, subjects);

        var bottom3Subjects = subjectScores
            .OrderBy(x => x.Value)
            .Take(3)
            .Select(x => subjects.FirstOrDefault(s => s.Id == x.Key))
            .ToList();

        return Ok(bottom3Subjects);
    }
}