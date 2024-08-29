using Microsoft.EntityFrameworkCore;
using ProjetoTemplate.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ProjetoTemplate.Repository
{
    public class ProjetoTemplateDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProjetoTemplateDbContext(
            DbContextOptions options,
            IHttpContextAccessor httpContextAccessor
        ) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        #region DBSets

        // Add DbSet for each entity
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ChangePassword> ChangePassword { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<ProfileAccess> ProfileAccess { get; set; }
        public virtual DbSet<LayoutColumns> LayoutColumns { get; set; }

        #endregion DBSets

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var propertie in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties())
                                                                           .Where(p => p.ClrType == typeof(decimal)))
                propertie.SetColumnType("DECIMAL(18, 2)");

            foreach (var propertie in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties())
                                                                          .Where(p => p.ClrType == typeof(string)))
                propertie.SetColumnType("VARCHAR(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjetoTemplateDbContext).Assembly);

            foreach (var relacao in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relacao.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            SetAuditableDate();
            SetAuditableUser();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditableDate();
            SetAuditableUser();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetAuditableDate()
        {
            var auditableDateEntries = ChangeTracker.Entries().Where(entry => entry.Entity is BaseModel);

            foreach (var entry in auditableDateEntries.Select(entry => entry.Property(nameof(BaseModel.CreateDate))))
            {
                if (entry.EntityEntry.State == EntityState.Added)
                {
                    entry.CurrentValue = DateTime.UtcNow;
                }

                if (entry.EntityEntry.State == EntityState.Modified)
                {
                    entry.IsModified = false;
                }
            }

            foreach (var entry in auditableDateEntries.Select(entry => entry.Property(nameof(BaseModel.LastUpdateDate))))
            {
                if (entry.EntityEntry.State == EntityState.Modified)
                {
                    entry.CurrentValue = DateTime.UtcNow;
                }
            }
        }

        private void SetAuditableUser()
        {
            var auditableDateEntries = ChangeTracker.Entries().Where(entry => entry.Entity is BaseModel);
            var userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? "System";


            foreach (var entry in auditableDateEntries.Select(entry => entry.Property(nameof(BaseModel.CreatedBy))))
            {
                if (entry.EntityEntry.State == EntityState.Added)
                {
                    entry.CurrentValue = userEmail;
                }

                if (entry.EntityEntry.State == EntityState.Modified)
                {
                    entry.IsModified = false;
                }
            }

            foreach (var entry in auditableDateEntries.Select(entry => entry.Property(nameof(BaseModel.LastUpdateBy))))
            {
                if (entry.EntityEntry.State == EntityState.Modified)
                {
                    entry.CurrentValue = userEmail;
                }
            }
        }
    }
}
