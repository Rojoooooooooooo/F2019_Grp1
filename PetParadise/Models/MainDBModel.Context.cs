﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PetParadise.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MainDBEntities : DbContext
    {
        public MainDBEntities()
            : base("name=MainDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<account_credential> account_credential { get; set; }
        public virtual DbSet<clinic_contact> clinic_contact { get; set; }
        public virtual DbSet<clinic_profile> clinic_profile { get; set; }
        public virtual DbSet<login_sessions> login_sessions { get; set; }
        public virtual DbSet<owner_contact> owner_contact { get; set; }
        public virtual DbSet<owner_profile> owner_profile { get; set; }
        public virtual DbSet<owner_address> owner_address { get; set; }
        public virtual DbSet<clinic_address> clinic_address { get; set; }
        public virtual DbSet<pet_profile> pet_profile { get; set; }
    }
}
