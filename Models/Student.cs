namespace GPA_Calculator.Models;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PersonalNumber { get; set; }
    public string Course { get; set; }
    public List<Grade> Grades { get; set; }
}