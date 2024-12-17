using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace TiendaMinorista.Model
{

    namespace TiendaMinorista.Model
    {
        public class TiendaContext : DbContext
        {

            public DbSet<Producto> Productos { get; set; }
            public DbSet<Categoria> Categorias { get; set; }
            public DbSet<Proveedor> Proveedores { get; set; }
            public DbSet<Cliente> Clientes { get; set; }
            public DbSet<Factura> Facturas { get; set; }
            public DbSet<DetalleFactura> DetallesFactura { get; set; }

            public TiendaContext() : base("name=TiendaContext")
            {
                this.Configuration.AutoDetectChangesEnabled = true;

                Database.SetInitializer(new CreateDatabaseIfNotExists<TiendaContext>());
            }


            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
            }
        }
    }


}
