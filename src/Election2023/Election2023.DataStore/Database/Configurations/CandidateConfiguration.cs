using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Election2023.DataStore.Database.Configurations;

public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.HasOne(p => p.Party)
            .WithMany()
            .HasPrincipalKey(x => x.Abbrv)
            .HasForeignKey(p => p.PartyAbbrv)
            .IsRequired();

        builder.HasIndex(n => new {n.Firstname, n.Surname});
    }
}