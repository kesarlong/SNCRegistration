﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

//namespace SNCRegistration.ViewModels
//{
//    using System;
//    using System.Data.Entity;
//    using System.Data.Entity.Infrastructure;
    
//    public partial class SNCRegistrationEntities : DbContext
//    {
//        public SNCRegistrationEntities()
//            : base("name=SNCRegistrationEntities")
//        {
//        }
    
//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            throw new UnintentionalCodeFirstException();
//        }
    
//        public virtual DbSet<FamilyMember> FamilyMembers { get; set; }
//        public virtual DbSet<Guardian> Guardians { get; set; }
//        public virtual DbSet<LeadContact> LeadContacts { get; set; }
//        public virtual DbSet<Participant> Participants { get; set; }
//        public virtual DbSet<Volunteer> Volunteers { get; set; }
//        public virtual DbSet<database_firewall_rules> database_firewall_rules { get; set; }
//        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
//        public virtual DbSet<Admin> Admins { get; set; }
//        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
//        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
//        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
//        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
//        public virtual DbSet<Attendance> Attendances { get; set; }
//        public virtual DbSet<User> Users { get; set; }
//    }
//}