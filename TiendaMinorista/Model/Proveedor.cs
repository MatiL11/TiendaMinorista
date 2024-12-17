using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaMinorista.Model
{
    public class Proveedor
    {
        public int Id { get; set; } 
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }

        // Relación con Productos
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }

}
