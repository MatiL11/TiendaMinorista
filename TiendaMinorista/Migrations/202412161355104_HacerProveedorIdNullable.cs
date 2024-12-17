namespace TiendaMinorista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HacerProveedorIdNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Productoes", "ProveedorId", "dbo.Proveedors");
            DropIndex("dbo.Productoes", new[] { "ProveedorId" });
            AlterColumn("dbo.Productoes", "ProveedorId", c => c.Int());
            CreateIndex("dbo.Productoes", "ProveedorId");
            AddForeignKey("dbo.Productoes", "ProveedorId", "dbo.Proveedors", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Productoes", "ProveedorId", "dbo.Proveedors");
            DropIndex("dbo.Productoes", new[] { "ProveedorId" });
            AlterColumn("dbo.Productoes", "ProveedorId", c => c.Int(nullable: false, defaultValue: 0));
            CreateIndex("dbo.Productoes", "ProveedorId");
            AddForeignKey("dbo.Productoes", "ProveedorId", "dbo.Proveedors", "Id", cascadeDelete: true);
        }
    }
}
