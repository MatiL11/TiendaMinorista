using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaMinorista.Model;
using TiendaMinorista.Model.TiendaMinorista.Model;

namespace TiendaMinorista.Controller
{
    public class FacturaController
    {
        private static FacturaController _instance;
        private readonly TiendaContext _context;

        private FacturaController()
        {
            _context = new TiendaContext(); 
        }

        public static FacturaController Instance
        {
            get 
            { 
                if (_instance == null)
                {
                    _instance = new FacturaController();
                }
                return _instance;
            }
        }

        public void RegistrarVenta(List<DetalleFactura> detalles, int clienteId)
        {

            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == clienteId);
            if (cliente == null)
            {
                throw new Exception("Cliente no encontrado.");
            }

            var factura = new Factura
            {
                Numero = "FAC-" + Guid.NewGuid().ToString().Substring(0, 6), 
                Fecha = DateTime.Now,
                ClienteId = clienteId,
                Total = detalles.Sum(d => d.Subtotal),
                Detalles = detalles 
            };

            foreach (var detalle in detalles)
            {
                detalle.Factura = factura;
            }

            _context.Facturas.Add(factura);

            foreach (var detalle in detalles)
            {
                var producto = _context.Productos.FirstOrDefault(p => p.Id == detalle.ProductoId);
                if (producto != null)
                {
                    producto.AjustarStock(-detalle.Cantidad); 
                }
            }

            _context.SaveChanges();
        }

        public List<Factura> ObtenerFacturasPorRangoDeFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            return _context.Facturas
                .Where(f => f.Fecha >= fechaInicio && f.Fecha <= fechaFin)
                .Include(f => f.Cliente) 
                .ToList();
        }

        public decimal ObtenerTotalVendidoPorCliente(int clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            return _context.Facturas
                .Where(f => f.ClienteId == clienteId && f.Fecha >= fechaInicio && f.Fecha <= fechaFin)
                .Sum(f => (decimal?)f.Total) ?? 0; 
        }

        public List<(string NombreProducto, int CantidadVendida)> ObtenerProductosMasVendidos()
        {
            var productosMasVendidos = _context.DetallesFactura
                .GroupBy(df => df.ProductoId) 
                .Select(g => new
                {
                    ProductoId = g.Key, 
                    CantidadVendida = g.Sum(df => df.Cantidad) 
                })
                .Join( 
                    _context.Productos, 
                    df => df.ProductoId, 
                    p => p.Id, 
                    (df, p) => new { p.Nombre, df.CantidadVendida } 
                )
                .ToList(); 
            return productosMasVendidos
                .Select(r => (r.Nombre, r.CantidadVendida)) 
                .ToList(); 
        }
    }
}
