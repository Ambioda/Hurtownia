using Hurtownia.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Hurtownia.Migrations;
using System.Data.Entity.Migrations;

namespace Hurtownia.DAL
{
    public class ProduktyInitializer : DbMigrationsConfiguration<Hurtownia.DAL.ProduktyContext>
    {


        public static void SeedProduktyData(ProduktyContext context)
        {
            var  kategorie = new List<Kategoria>
            {
                new Kategoria() {KategoriaId=1, NazwaKategorii="Hamulce", OpisKategorii="opis hamulce"},
                new Kategoria() {KategoriaId=2, NazwaKategorii="Koła", OpisKategorii="opis koła"},
                new Kategoria() {KategoriaId=3, NazwaKategorii="Opony", OpisKategorii="opis opony"},
                new Kategoria() {KategoriaId=4, NazwaKategorii="Sprzegła", OpisKategorii="opis sprzęgła"},
                new Kategoria() {KategoriaId=5, NazwaKategorii="Szyby", OpisKategorii="opis szyby"},
                new Kategoria() {KategoriaId=6, NazwaKategorii="Wnetrze", OpisKategorii="opis wnetrze"}
            };

                kategorie.ForEach(k => context.Kategorie.AddOrUpdate(k));
            context.SaveChanges();

            var produkty = new List<Produkt>
            {
                new Produkt() {ProduktId=1, NazwaProduktu="Tarcza hamulcowa", KategoriaId=1, CenaProduktu=230, NazwaPlikuObrazka="tarcza.jpg",Bestseller=true, Ukryty=false,OpisProduktu="Tarcza opis",DataDodania=DateTime.Now},
                new Produkt() {ProduktId=2, NazwaProduktu="Stalówka", KategoriaId=2, CenaProduktu=90, NazwaPlikuObrazka="stalowka.jpg",Bestseller=false, Ukryty=false,OpisProduktu="stalowka opis",DataDodania=DateTime.Now},
                new Produkt() {ProduktId=3, NazwaProduktu="Opona Frigo", KategoriaId=3, CenaProduktu=130, NazwaPlikuObrazka="frigo.jpg",Bestseller=true, Ukryty=false,OpisProduktu="Frigo opis",DataDodania=DateTime.Now},
                new Produkt() {ProduktId=4, NazwaProduktu="Dwumas", KategoriaId=4, CenaProduktu=1300, NazwaPlikuObrazka="dwumas.jpg",Bestseller=true, Ukryty=false,OpisProduktu="Dwumas opis",DataDodania=DateTime.Now},
                new Produkt() {ProduktId=5, NazwaProduktu="Szyba Tył", KategoriaId=5, CenaProduktu=450, NazwaPlikuObrazka="szyba.jpg",Bestseller=false, Ukryty=false,OpisProduktu="Szyba opis",DataDodania=DateTime.Now},
                new Produkt() {ProduktId=6, NazwaProduktu="Uchwyt", KategoriaId=6, CenaProduktu=70, NazwaPlikuObrazka="uchwyt.jpg",Bestseller=true, Ukryty=false,OpisProduktu="Uchwyt opis",DataDodania=DateTime.Now}
            };
            produkty.ForEach(k => context.Produkty.AddOrUpdate(k));
            context.SaveChanges();
            }
        }
    }
