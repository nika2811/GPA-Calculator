using GPA_Calculator.Models;

namespace GPA_Calculator.Services;

public class GradeCalculator
{
    public List<Subject> GetTop3Subjects(List<Grade> grades, List<Subject> subjects)
    {
        var subjectGrades = new Dictionary<Subject, int>();
        var subjectCounts = new Dictionary<Subject, int>();

        foreach (var grade in grades)
        {
            var subject = subjects.Find(s => s.Id == grade.SubjectId);

            if (!subjectGrades.ContainsKey(subject))
            {
                subjectGrades[subject] = 0;
                subjectCounts[subject] = 0;
            }

            subjectGrades[subject] += grade.Score;
            subjectCounts[subject]++;
        }

        var top3Subjects = subjectGrades
            .OrderByDescending(s => (double)s.Value / subjectCounts[s.Key])
            .Take(3)
            .Select(s => s.Key)
            .ToList();

        return top3Subjects;
    }
}