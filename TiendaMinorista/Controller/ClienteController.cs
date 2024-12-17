using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaMinorista.Model;
using TiendaMinorista.Model.TiendaMinorista.Model;

namespace TiendaMinorista.Controller
{
    public class ClienteController
    {
        private static ClienteController _instance;
        private readonly TiendaContext _context;

        private ClienteController()
        {
            _context = new TiendaContext();
        }

        public static ClienteController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ClienteController(); 
                }
                return _instance;
            }
        }

        public void AgregarCliente(string nombre, string direccion, string contacto)
        {
            var cliente = new Cliente
            {
                Nombre = nombre,
                Direccion = direccion,
                Contacto = contacto
            };

            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }



        public void ModificarCliente(int id, string nombre, string direccion, string contacto)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == id);
            if (cliente != null)
            {
                cliente.Nombre = nombre;
                cliente.Direccion = direccion;
                cliente.Contacto = contacto;

                _context.SaveChanges();
            }
        }

        public void EliminarCliente(int id)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                _context.SaveChanges();
            }
        }

        public IQueryable<Cliente> ObtenerClientes()
        {
            return _context.Clientes;
        }
    }
}
