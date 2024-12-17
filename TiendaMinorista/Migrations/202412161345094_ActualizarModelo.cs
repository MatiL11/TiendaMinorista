namespace TiendaMinorista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizarModelo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Productoes", "Proveedor_Id", "dbo.Proveedors");
            DropIndex("dbo.Productoes", new[] { "Proveedor_Id" });
            RenameColumn(table: "dbo.Productoes", name: "Proveedor_Id", newName: "ProveedorId");
            AlterColumn("dbo.Productoes", "ProveedorId", c => c.Int(nullable: true, defaultValue: 0));
            CreateIndex("dbo.Productoes", "ProveedorId");
            AddForeignKey("dbo.Productoes", "ProveedorId", "dbo.Proveedors", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Productoes", "ProveedorId", "dbo.Proveedors");
            DropIndex("dbo.Productoes", new[] { "ProveedorId" });
            AlterColumn("dbo.Productoes", "ProveedorId", c => c.Int());
            RenameColumn(table: "dbo.Productoes", name: "ProveedorId", newName: "Proveedor_Id");
            CreateIndex("dbo.Productoes", "Proveedor_Id");
            AddForeignKey("dbo.Productoes", "Proveedor_Id", "dbo.Proveedors", "Id");
        }
    }
}
