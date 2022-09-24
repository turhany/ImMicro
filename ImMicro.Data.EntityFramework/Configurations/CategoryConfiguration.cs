using ImMicro.Model.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImMicro.Data.EntityFramework.Configurations;

public class CategoryConfiguration: IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(nameof(Category));

        builder.HasKey(o => o.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}