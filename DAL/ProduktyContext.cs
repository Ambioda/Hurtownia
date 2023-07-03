using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Hurtownia.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Hurtownia.DAL
{
    public class ProduktyContext : DbContext
    {
        public ProduktyContext() : base("ProduktyContext")
        {

        }
        static ProduktyContext()
        {
            Database.SetInitializer<ProduktyContext>(new ProduktyInitializer());
        }
        public DbSet<Produkt> Produkty { get; set; }
        public DbSet<Kategoria> Kategorie { get; set; }
        public DbSet<Zamowienie> Zamowienia { get; set; }
        public DbSet<PozycjaZamowienia> PozycjeZamowienia { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}