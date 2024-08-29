using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoTemplate.Domain.Models;
using System;

namespace ProjetoTemplate.Repository.ModelsConfiguration
{
    public class ProfileConfig : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasMany(e => e.ProfileAccess)
                    .WithOne(e => e.Profile)
                    .HasForeignKey(e => e.ProfileId)
                    .HasPrincipalKey(e => e.Id);

            builder.HasData(new Profile
            {
                Id = 1,
                Description = "Administrador",
                AccessAllModules = true,
                CreateDate = Convert.ToDateTime("2024-01-04 00:00:01"),
                Status = true,
                IsAdmin = true,
            });
        }
    }
}
