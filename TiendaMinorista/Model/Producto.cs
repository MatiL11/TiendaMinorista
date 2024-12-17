using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaMinorista.Model
{
    public class Producto
    {
        public int Id { get; set; } 
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; } 
        public Categoria Categoria { get; set; }
        public int? ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }

        public void AjustarStock(int cantidad)
        {
            Stock += cantidad;
            if (Stock < 0) Stock = 0; 
        }

        public decimal AplicarDescuento(decimal porcentaje)
        {
            if (porcentaje < 0 || porcentaje > 100)
                throw new ArgumentException("El porcentaje de descuento debe estar entre 0 y 100.");

            return Precio * (1 - porcentaje / 100);
        }
    }

}
