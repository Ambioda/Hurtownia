namespace Hurtownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Kategoria",
                c => new
                    {
                        KategoriaId = c.Int(nullable: false, identity: true),
                        NazwaKategorii = c.String(nullable: false, maxLength: 100),
                        OpisKategorii = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.KategoriaId);
            
            CreateTable(
                "dbo.Produkt",
                c => new
                    {
                        ProduktId = c.Int(nullable: false, identity: true),
                        KategoriaId = c.Int(nullable: false),
                        NazwaProduktu = c.String(nullable: false, maxLength: 100),
                        DataDodania = c.DateTime(nullable: false),
                        NazwaPlikuObrazka = c.String(maxLength: 100),
                        OpisProduktu = c.String(),
                        CenaProduktu = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Bestseller = c.Boolean(nullable: false),
                        Ukryty = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProduktId)
                .ForeignKey("dbo.Kategoria", t => t.KategoriaId, cascadeDelete: true)
                .Index(t => t.KategoriaId);
            
            CreateTable(
                "dbo.PozycjaZamowienia",
                c => new
                    {
                        PozycjaZamowieniaId = c.Int(nullable: false, identity: true),
                        ZamowienieId = c.Int(nullable: false),
                        ProduktId = c.Int(nullable: false),
                        Ilosc = c.Int(nullable: false),
                        CenaZakupu = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PozycjaZamowieniaId)
                .ForeignKey("dbo.Produkt", t => t.ProduktId, cascadeDelete: true)
                .ForeignKey("dbo.Zamowienie", t => t.ZamowienieId, cascadeDelete: true)
                .Index(t => t.ZamowienieId)
                .Index(t => t.ProduktId);
            
            CreateTable(
                "dbo.Zamowienie",
                c => new
                    {
                        ZamowienieId = c.Int(nullable: false, identity: true),
                        Imie = c.String(nullable: false, maxLength: 20),
                        Nazwisko = c.String(nullable: false, maxLength: 30),
                        Ulica = c.String(nullable: false, maxLength: 50),
                        Miasto = c.String(nullable: false, maxLength: 50),
                        KodPocztowy = c.String(nullable: false, maxLength: 6),
                        Telefon = c.String(nullable: false, maxLength: 11),
                        Email = c.String(nullable: false, maxLength: 50),
                        Komentarz = c.String(),
                        DataDodania = c.DateTime(nullable: false),
                        StanZamowienia = c.Int(nullable: false),
                        WartoscZamowienia = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ZamowienieId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PozycjaZamowienia", "ZamowienieId", "dbo.Zamowienie");
            DropForeignKey("dbo.PozycjaZamowienia", "ProduktId", "dbo.Produkt");
            DropForeignKey("dbo.Produkt", "KategoriaId", "dbo.Kategoria");
            DropIndex("dbo.PozycjaZamowienia", new[] { "ProduktId" });
            DropIndex("dbo.PozycjaZamowienia", new[] { "ZamowienieId" });
            DropIndex("dbo.Produkt", new[] { "KategoriaId" });
            DropTable("dbo.Zamowienie");
            DropTable("dbo.PozycjaZamowienia");
            DropTable("dbo.Produkt");
            DropTable("dbo.Kategoria");
        }
    }
}
