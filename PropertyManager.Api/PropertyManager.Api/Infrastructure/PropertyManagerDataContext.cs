using Microsoft.AspNet.Identity.EntityFramework;
using PropertyManager.Api.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

// This is my data base context!

namespace PropertyManager.Api.Infrastructure
{
    // Change DbContext to IdentityDbContext so that it can inherit from Identity

    public class PropertyManagerDataContext : IdentityDbContext<PropertyManagerUser>
    {
        public PropertyManagerDataContext() : base("PropertyManager")
        {
        }

        // SQL Tables
        public IDbSet<Address> Addresses { get; set; }
        public IDbSet<Lease> Leases { get; set; }
        public IDbSet<Property> Properties { get; set; }
        public IDbSet<Tenant> Tenants { get; set; }
        public IDbSet<WorkOrder> WorkOrders { get; set; }

        // Model our relationships
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Check: Code First Fluent API

            // Define Address and its many relationships (Properties & Tenants)
            modelBuilder.Entity<Address>()
                        .HasMany(a => a.Properties)
                        .WithRequired(p => p.Address)
                        .HasForeignKey(p => p.AddressId);

            modelBuilder.Entity<Address>()
                        .HasMany(a => a.Tenants)
                        .WithRequired(t => t.Address)
                        .HasForeignKey(t => t.AddressId)
                        .WillCascadeOnDelete(false);

            // Define Lease and its many relationships 
            // There are no many relationships here, so we move on

            // Define Property and its relationships
            modelBuilder.Entity<Property>()
                        .HasMany(p => p.Leases)
                        .WithRequired(l => l.Property)
                        .HasForeignKey(l => l.PropertyId);

            modelBuilder.Entity<Property>()
                        .HasMany(p => p.WorkOrders)
                        .WithRequired(wo => wo.Property)
                        .HasForeignKey(wo => wo.PropertyId);

            // Define Tenant and its many relationships
            modelBuilder.Entity<Tenant>()
                        .HasMany(t => t.Leases)
                        .WithRequired(l => l.Tenant)
                        .HasForeignKey(l => l.TenantId);

            modelBuilder.Entity<Tenant>()
                        .HasMany(t => t.WorkOrders)
                        .WithOptional(wo => wo.Tenant)
                        .HasForeignKey(wo => wo.TenantId);

            modelBuilder.Entity<PropertyManagerUser>()
                        .HasMany(u => u.Properties)
                        .WithRequired(p => p.User)
                        .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<PropertyManagerUser>()
                        .HasMany(u => u.Tenants)
                        .WithRequired(t => t.User)
                        .HasForeignKey(t => t.UserId)
                        .WillCascadeOnDelete(false);


            // Define WorkOrder and its many relationships
            // There are no many relationships here, only one-to relationships.

            // This will set up all the relationships for ASP Net Identity
            base.OnModelCreating(modelBuilder);
         
        }
    }
}