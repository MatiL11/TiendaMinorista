using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaMinorista.Model;
using TiendaMinorista.Model.TiendaMinorista.Model;
using System.Data.Entity;

namespace TiendaMinorista.Controller
{

    public class ProductoController
    {
        private static ProductoController _instance;
        private readonly TiendaContext _context;

        public ProductoController()
        {
            _context = new TiendaContext();
        }

        public static ProductoController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProductoController();
                }
                return _instance;
            }
        }

        public void CrearProducto(string nombre, string descripcion, decimal precio, int stock, int categoriaId, int proveedorId)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.Id == categoriaId);
            var proveedor = _context.Proveedores.FirstOrDefault(p => p.Id == proveedorId);
            if (proveedor != null)
            {
                var producto = new Producto
                {
                    Nombre = nombre,
                    Descripcion = descripcion,
                    Precio = precio,
                    Stock = stock,
                    CategoriaId = categoria.Id,
                    ProveedorId = proveedor.Id
                };

                categoria.Productos.Add(producto);
                proveedor.Productos.Add(producto);

                _context.Productos.Add(producto);
                _context.SaveChanges();
            }
        }

        public void ModificarProducto(int id, string nombre, string descripcion, decimal precio, int stock, int categoriaId, int proveedorId)
        {
            var producto = _context.Productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                producto.Nombre = nombre;
                producto.Descripcion = descripcion;
                producto.Precio = precio;
                producto.Stock = stock;
                producto.CategoriaId = categoriaId;
                producto.ProveedorId= proveedorId;

                _context.SaveChanges();
            }
        }

        public void EliminarProducto(int id)
        {
            var producto = _context.Productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                _context.SaveChanges();
            }
        }

        public IQueryable<Producto> ObtenerProductos()
        {
            return _context.Productos.Include(p => p.Categoria);
        }
    }

}
