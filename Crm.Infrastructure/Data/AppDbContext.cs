using Crm.Domain.CRM;
using Crm.Domain.Statuses;
using Crm.Infrastructure.Data.Entities;
using Crm.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Crm.Infrastructure.Data;

public sealed class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Domain tables
    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<BusinessContact> BusinessContacts => Set<BusinessContact>();

    public DbSet<StatusType> StatusTypes => Set<StatusType>();
    public DbSet<Status> Statuses => Set<Status>();

    // RBAC tables
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserPermissionOverride> UserPermissionOverrides => Set<UserPermissionOverride>();

    // Record-level assignments
    public DbSet<BusinessAssignment> BusinessAssignments => Set<BusinessAssignment>();
    public DbSet<ContactAssignment> ContactAssignments => Set<ContactAssignment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Business>(b =>
        {
            b.ToTable("Businesses");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasColumnName("BusinessId");
            b.Property(x => x.TenantId).IsRequired();
            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.Property(x => x.Email).HasMaxLength(256);
            b.Property(x => x.Phone).HasMaxLength(50);
            b.Property(x => x.Website).HasMaxLength(256);
            b.HasIndex(x => new { x.TenantId, x.Name });
        });

        builder.Entity<Contact>(c =>
        {
            c.ToTable("Contacts");
            c.HasKey(x => x.Id);
            c.Property(x => x.Id).HasColumnName("ContactId");
            c.Property(x => x.TenantId).IsRequired();
            c.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            c.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            c.Property(x => x.Email).HasMaxLength(256);
            c.Property(x => x.Phone).HasMaxLength(50);
            c.HasIndex(x => new { x.TenantId, x.LastName, x.FirstName });
        });

        builder.Entity<BusinessContact>(bc =>
        {
            bc.ToTable("BusinessContacts");
            bc.HasKey(x => new { x.BusinessId, x.ContactId });

            bc.HasOne(x => x.Business)
                .WithMany()
                .HasForeignKey(x => x.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            bc.HasOne(x => x.Contact)
                .WithMany()
                .HasForeignKey(x => x.ContactId)
                .OnDelete(DeleteBehavior.Cascade);

            bc.HasIndex(x => x.TenantId);
        });

        builder.Entity<StatusType>(st =>
        {
            st.ToTable("StatusTypes");
            st.HasKey(x => x.Id);
            st.Property(x => x.Id).HasColumnName("StatusTypeId");
            st.Property(x => x.TenantId).IsRequired();
            st.Property(x => x.Key).HasMaxLength(100).IsRequired();
            st.Property(x => x.Name).HasMaxLength(200).IsRequired();
            st.HasIndex(x => new { x.TenantId, x.Key }).IsUnique();
        });

        builder.Entity<Status>(s =>
        {
            s.ToTable("Statuses");
            s.HasKey(x => x.Id);
            s.Property(x => x.Id).HasColumnName("StatusId");
            s.Property(x => x.TenantId).IsRequired();
            s.Property(x => x.Key).HasMaxLength(100).IsRequired();
            s.Property(x => x.Name).HasMaxLength(200).IsRequired();

            s.HasOne(x => x.StatusType)
                .WithMany()
                .HasForeignKey(x => x.StatusTypeId);

            s.HasIndex(x => new { x.TenantId, x.StatusTypeId, x.Key }).IsUnique();
        });

        builder.Entity<Permission>(p =>
        {
            p.ToTable("Permissions");
            p.HasKey(x => x.PermissionId);
            p.Property(x => x.Key).HasMaxLength(200).IsRequired();
            p.HasIndex(x => new { x.TenantId, x.Key }).IsUnique();
        });

        builder.Entity<RolePermission>(rp =>
        {
            rp.ToTable("RolePermissions");
            rp.HasKey(x => new { x.RoleId, x.PermissionId });
            rp.HasOne(x => x.Permission)
                .WithMany()
                .HasForeignKey(x => x.PermissionId);
            rp.HasIndex(x => x.TenantId);
        });

        builder.Entity<UserPermissionOverride>(upo =>
        {
            upo.ToTable("UserPermissionOverrides");
            upo.HasKey(x => x.UserPermissionOverrideId);
            upo.HasIndex(x => new { x.TenantId, x.UserId, x.PermissionId }).IsUnique();
            upo.HasOne(x => x.Permission)
                .WithMany()
                .HasForeignKey(x => x.PermissionId);
        });

        builder.Entity<BusinessAssignment>(ba =>
        {
            ba.ToTable("BusinessAssignments");
            ba.HasKey(x => new { x.BusinessId, x.UserId });
            ba.HasIndex(x => x.TenantId);
        });

        builder.Entity<ContactAssignment>(ca =>
        {
            ca.ToTable("ContactAssignments");
            ca.HasKey(x => new { x.ContactId, x.UserId });
            ca.HasIndex(x => x.TenantId);
        });

        builder.Entity<ApplicationUser>(u =>
        {
            u.HasOne(x => x.ManagerUser)
                .WithMany()
                .HasForeignKey(x => x.ManagerUserId)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}