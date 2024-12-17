using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaMinorista.Model
{
    public class Factura
    {
        public int Id { get; set; } 
        public string Numero { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; } 
        public Cliente Cliente { get; set; }
        public decimal Total { get; set; }


        public ICollection<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();
    }

}
