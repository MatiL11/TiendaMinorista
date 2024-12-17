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
    public class ProveedorController
    {
        private static ProveedorController _instance;
        private readonly TiendaContext _context;

        private ProveedorController()
        {
            _context = new TiendaContext();
        }

        public static ProveedorController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProveedorController(); 
                }
                return _instance;
            }
        }

        public void AgregarProveedor(string nombre, string direccion, string contacto)
        {
            var proveedor = new Proveedor
            {
                Nombre = nombre,
                Direccion = direccion,
                Contacto = contacto
            };

            _context.Proveedores.Add(proveedor);
            _context.SaveChanges();
        }


        public void ModificarProveedor(int id, string nombre, string direccion, string contacto)
        {
            var proveedor = _context.Proveedores.FirstOrDefault(p => p.Id == id);
            if (proveedor != null)
            {
                proveedor.Nombre = nombre;
                proveedor.Direccion = direccion;
                proveedor.Contacto = contacto;

                _context.SaveChanges();
            }
        }

        public void EliminarProveedor(int id)
        {
            var proveedor = _context.Proveedores.FirstOrDefault(p => p.Id == id);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
                _context.SaveChanges();
            }
        }

        public IQueryable<Proveedor> ObtenerProveedores()
        {
            return _context.Proveedores.Include(p => p.Productos);
        }
    }
}
