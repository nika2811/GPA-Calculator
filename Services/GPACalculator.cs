using GPA_Calculator.Db;
using Microsoft.EntityFrameworkCore;

namespace GPA_Calculator.Services;

public class GpaCalculator
{
    private readonly StudentGradesContext _context;

    public GpaCalculator(StudentGradesContext context)
    {
        _context = context;
    }

    public async Task<double> Calculate(int studentId)
    {
        // Load the grades for the specified student
        var grades = await _context.Grades.Where(g => g.StudentId == studentId).ToListAsync();
        if (grades.Count == 0) throw new KeyNotFoundException("Student not found");
        // Convert the grades to the Wentworth scale
        var totalScore = 0.0;
        var totalCredits = 0;
        foreach (var grade in grades)
        {
            var subject = await _context.Subjects.FindAsync(grade.SubjectId);
            var score = ConvertToWentworthScale(grade.Score);
            if (subject != null)
            {
                totalScore += score * subject.Credits;
                totalCredits += subject.Credits;
            }
        }

        // Return the GPA
        return totalScore / totalCredits;
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