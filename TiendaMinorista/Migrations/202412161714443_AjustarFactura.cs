namespace TiendaMinorista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjustarFactura : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Facturas", "Numero", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Facturas", "Numero");
        }
    }
}
