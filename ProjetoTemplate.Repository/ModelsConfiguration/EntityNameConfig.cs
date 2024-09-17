using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoTemplate.Domain.Models;
using System;

namespace ProjetoTemplate.Repository.ModelsConfiguration
{
    public class EntityNameConfig : IEntityTypeConfiguration<EntityName>
    {
        public void Configure(EntityTypeBuilder<EntityName> builder)
        {
            
        }
    }
}
