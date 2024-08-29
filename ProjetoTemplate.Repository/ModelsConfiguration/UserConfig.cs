using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoTemplate.Domain.Models;
using System;

namespace ProjetoTemplate.Repository.ModelsConfiguration
{
    public class UsersConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.Name)
                .HasColumnType("varchar(150)");

            builder.Property(p => p.Email)
                .HasColumnType("varchar(150)");

            builder.Property(p => p.Login)
                .HasColumnType("varchar(150)");

            builder.Property(p => p.Password)
            .HasColumnType("varchar(150)");

            builder.HasOne(a => a.Profile).WithMany().HasForeignKey(fk => fk.ProfileId).OnDelete(DeleteBehavior.Restrict);

            builder.HasData(new User
            {
                Id = 1,
                Name = "Administrador",
                LastName = "do Sistema",
                Email = "admin",
                Password = "ejnKgi6XKgefY7xO5+eGavzl5l/v8Zeb", //Desenvolvimento@Tria
                Login = "admin",
                ProfileId = 1,
                CreateDate = Convert.ToDateTime("2024-01-04 00:00:01"),
                Status = true,
                FirstAccess = false
            });
        }
    }
}
