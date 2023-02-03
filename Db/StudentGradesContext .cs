using GPA_Calculator.Models;
using Microsoft.EntityFrameworkCore;

namespace GPA_Calculator.Db;

public class StudentGradesContext : DbContext
{
    public StudentGradesContext(DbContextOptions<StudentGradesContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Grade> Grades { get; set; }
}