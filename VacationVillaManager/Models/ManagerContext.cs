﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VacationVillaManager.Migrations;

namespace VacationVillaManager.Models
{
    public class ManagerContext : DbContext
    {
        public ManagerContext()
            : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Special> Specials { get; set; }
        public DbSet<Cost> Costs { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<HouseManager> HouseManagers { get; set; }
        public DbSet<GuestReview> GuestReviews { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ManagerContext, Configuration>());
        }

    }
}