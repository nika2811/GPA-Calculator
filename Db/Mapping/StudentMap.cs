using GPA_Calculator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPA_Calculator.Db.Mapping;

public class StudentMap : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(x => x.Id);


        builder.HasMany(t => t.Grades).WithOne(t => t.Student).HasForeignKey(t => t.StudentId);
    }
}