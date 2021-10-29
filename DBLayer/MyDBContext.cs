using Microsoft.EntityFrameworkCore;
using Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer
{
    public class MyDBContext: DbContext, IDBContext
    {
        //DbSet<> список событий(по идеи валуе обжект, так как с равным полями одно и то же, но с индексацией)
        // список источников (ентити, )
        // список полей для источников(ентити)
        public DbSet<Event> Events { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<SourceFields> SourceFields { get; set; }

        public MyDBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Source>().
                HasOne(x => x.Fields).
                WithOne(f=>f.Source).
                HasForeignKey<SourceFields>(x=>x.SourceId);

            modelBuilder.Entity<Source>().
                HasMany(s => s.Events).
                WithOne(e => e.Source);
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
            await Events.AddAsync(ev);
        }

        public async Task<List<Source>> GetSourcesAsync()
        {
            return await Sources.Include(x=>x.Fields).Include(x=>x.Events).ToListAsync();
        }

    }
}
