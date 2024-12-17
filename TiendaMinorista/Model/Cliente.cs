using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaMinorista.Model
{
    public class Cliente
    {
        public int Id { get; set; } 
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }

        public ICollection<Factura> Facturas { get; set; } = new List<Factura>();
    }

}
