using GPA_Calculator.Models;

namespace GPA_Calculator.Services;

public class SubjectScores
{
    public Dictionary<int, double> GetSubjectScores(List<Grade> grades, List<Subject> subjects)
    {
        var subjectScores = new Dictionary<int, double>();
        foreach (var subject in subjects)
        {
            var totalScore = 0;
            var count = 0;
            foreach (var grade in grades)
                if (grade.SubjectId == subject.Id)
                {
                    totalScore += grade.Score;
                    count++;
                }

            if (count > 0)
            {
                var averageScore = totalScore / count;
                subjectScores[subject.Id] = averageScore;
            }
        }

        return subjectScores;
    }
}