using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TiendaMinorista.Model.TiendaMinorista.Model;
using TiendaMinorista.View;

namespace TiendaMinorista
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            FormProducto producto = new FormProducto();
            producto.ShowDialog();
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            FormCliente cliente = new FormCliente();
            cliente.ShowDialog();
        }

        private void btnProveedor_Click(object sender, EventArgs e)
        {
            FormProveedor proveedor = new FormProveedor();
            proveedor.ShowDialog();
        }

        private void btnVenta_Click(object sender, EventArgs e)
        {
            FormRegistrarVenta registrar = new FormRegistrarVenta();
            registrar.ShowDialog();
        }

        private void btnReporteVentas_Click(object sender, EventArgs e)
        {
            FormReporteVentas formReporteVentas = new FormReporteVentas();
            formReporteVentas.ShowDialog();
        }
    }
}
