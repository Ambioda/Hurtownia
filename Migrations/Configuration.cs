namespace Hurtownia.Migrations
{
    using DAL;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Hurtownia.DAL.ProduktyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Hurtownia.DAL.ProduktyContext";
        }

        protected override void Seed(Hurtownia.DAL.ProduktyContext context)
        {
            ProduktyInitializer.SeedProduktyData(context);
            ProduktyInitializer.SeedUzytkownicy(context);
        }
    }
}
