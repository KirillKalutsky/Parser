﻿using Microsoft.EntityFrameworkCore;
using Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class MyDBContext: DbContext, IDBContext
    {
        //DbSet<> список событий(по идеи валуе обжект, так как с равным полями одно и то же, но с индексацией)
        // список источников (ентити, )
        // список полей для источников(ентити)
        public DbSet<Event> Events { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<SourceFields> SourceFields { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public MyDBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Source>()
                .HasOne(x => x.Fields)
                .WithOne(f => f.Source)
                .HasForeignKey<SourceFields>(x => x.SourceId);

            modelBuilder.Entity<Source>()
                .HasMany(s => s.Events)
                .WithOne(e => e.Source);

            modelBuilder.Entity<District>()
                .HasMany(d => d.Addresses)
                .WithOne(a => a.District);

            modelBuilder.Entity<Event>()
               .HasOne(ev => ev.District)
               .WithMany(d => d.Events);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=redZone;Username=postgres;Password=abrakadabra77");
        }

        public async Task AddSourceAsync(Source source)
        {
            await Sources.AddAsync(source);
        }
        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await Events.ToListAsync();
        }

        public async Task AddEventAsync(Event ev)
        {
            Debug.Print("Add Event");
            if(await Events.FindAsync(ev.Link)==null)
            {
                await Events.AddAsync(ev);
            }
            Debug.Print("Succes Add Event");
        }

        public async Task<List<Source>> GetSourcesAsync()
        {
            return await Sources.Include(source=>source.Fields).Include(source =>source.Events).ToListAsync();
        }

        public async Task<List<Event>> GetLastEventsByTimeAsync(DateTime minDateTime, DateTime maxDateTime)
        {
            return await Events
                .Where(ev => ev.DateOfDownload >= minDateTime && ev.DateOfDownload <= maxDateTime)
                .ToListAsync();
        }

        public async Task<List<Event>> GetEventsByPeriodTimeAsync(DateTime minDateTime)
        {
            return await Events
                .Where(ev => ev.DateOfDownload >= minDateTime)
                .ToListAsync();
        }

    }
}
