using GPA_Calculator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPA_Calculator.Db.Mapping;

public class GradeMap : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.HasKey(x => x.Id);


        builder.HasOne(x => x.Subject).WithMany().HasForeignKey(t => t.SubjectId);
    }
}