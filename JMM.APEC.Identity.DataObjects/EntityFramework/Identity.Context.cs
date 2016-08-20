﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ApecIdentityEntities : DbContext
    {
        public ApecIdentityEntities()
            : base("name=ApecIdentityEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<tblIdentity_UserSystemRoles> tblIdentity_UserSystemRoles { get; set; }
        public virtual DbSet<tblIdentity_SystemRoles> tblIdentity_SystemRoles { get; set; }
        public virtual DbSet<tblIdentity_UserGateways> tblIdentity_UserGateways { get; set; }
        public virtual DbSet<tblIdentity_Clients> tblIdentity_Clients { get; set; }
        public virtual DbSet<tblIdentity_RefreshTokens> tblIdentity_RefreshTokens { get; set; }
        public virtual DbSet<tblIdentity_Gateways> tblIdentity_Gateways { get; set; }
        public virtual DbSet<tblIdentity_Portals> tblIdentity_Portals { get; set; }
        public virtual DbSet<tblIdentity_UserPortals> tblIdentity_UserPortals { get; set; }
        public virtual DbSet<tblIdentity_UserProfiles> tblIdentity_UserProfiles { get; set; }
        public virtual DbSet<tblIdentity_Permissions> tblIdentity_Permissions { get; set; }
        public virtual DbSet<tblIdentity_Statuses> tblIdentity_Statuses { get; set; }
        public virtual DbSet<tblIdentity_PasswordPolicies> tblIdentity_PasswordPolicies { get; set; }
        public virtual DbSet<tblIdentity_GatewaySystemRoles> tblIdentity_GatewaySystemRoles { get; set; }
        public virtual DbSet<tblIdentity_SystemRolePermissions> tblIdentity_SystemRolePermissions { get; set; }
        public virtual DbSet<tblIdentity_UserAssets> tblIdentity_UserAssets { get; set; }
    }
}
