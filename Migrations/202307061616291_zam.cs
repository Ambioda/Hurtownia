namespace Hurtownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Zamowienie", "UserId", c => c.String());
            AddColumn("dbo.Zamowienie", "Adres", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Zamowienie", "Ulica");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Zamowienie", "Ulica", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Zamowienie", "Adres");
            DropColumn("dbo.Zamowienie", "UserId");
        }
    }
}
