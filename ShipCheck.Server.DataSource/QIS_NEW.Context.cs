﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShipCheck.Server.DataSource
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QIS_NEWEntities : DbContext
    {
        public QIS_NEWEntities()
            : base("name=QIS_NEWEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<T_II_PROD_TIME> T_II_PROD_TIME { get; set; }
        public virtual DbSet<T_II_START_NEW> T_II_START_NEW { get; set; }
        public virtual DbSet<T_USER> T_USER { get; set; }
        public virtual DbSet<T_II_HANG_SILK_DETAIL> T_II_HANG_SILK_DETAIL { get; set; }
        public virtual DbSet<T_II_HANG_SILK_MASTER> T_II_HANG_SILK_MASTER { get; set; }
        public virtual DbSet<T_III_PROD_SILK_LEN_TIME> T_III_PROD_SILK_LEN_TIME { get; set; }
        public virtual DbSet<T_III_MACHINE_CHECK> T_III_MACHINE_CHECK { get; set; }
        public virtual DbSet<T_III_MACHINE_EXCEPTION> T_III_MACHINE_EXCEPTION { get; set; }
        public virtual DbSet<Version> Version { get; set; }
    }
}