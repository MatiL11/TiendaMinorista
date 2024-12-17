using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TiendaMinorista.Controller;

namespace TiendaMinorista.View
{
    public partial class FormReporteVentas : Form
    {
        public FormReporteVentas()
        {
            InitializeComponent();
            CargarClientes();
        }

        private void GenerarColumnasReportes()
        {
            dgvFacturas.Columns.Add("NumeroFactura", "Número de Factura");
            dgvFacturas.Columns.Add("Fecha", "Fecha");
            dgvFacturas.Columns.Add("Cliente", "Cliente");
            dgvFacturas.Columns.Add("Total", "Total");
        }

        private void GenerarColumnasMasVendidos()
        {
            dgvFacturas.Columns.Add("Producto", "Producto");
            dgvFacturas.Columns.Add("CantidadVendida", "Cantidad Vendida");
        }

        private void CargarClientes()
        {
            try
            {

                var clientes = ClienteController.Instance.ObtenerClientes().ToList();

                if (clientes.Count == 0)
                {
                    MessageBox.Show("No hay clientes registrados. Registre un cliente antes de continuar.");
                    return;
                }

                cmbClientes.DataSource = clientes;       
                cmbClientes.DisplayMember = "Nombre";    
                cmbClientes.ValueMember = "Id";         
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los clientes: {ex.Message}");
            }
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                GenerarColumnasReportes();

                DateTime fechaInicio = dtpFechaInicio.Value.Date;
                DateTime fechaFin = dtpFechaFin.Value.Date;

                if (fechaInicio > fechaFin)
                {
                    MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha de fin.");
                    return;
                }

                var facturas = FacturaController.Instance.ObtenerFacturasPorRangoDeFechas(fechaInicio, fechaFin);

                dgvFacturas.Rows.Clear();

                foreach (var factura in facturas)
                {
                    dgvFacturas.Rows.Add(factura.Numero, factura.Fecha, factura.Cliente.Nombre, factura.Total.ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el listado: {ex.Message}");
            }
        }

        private void btnTotalVendido_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbClientes.SelectedValue == null || !int.TryParse(cmbClientes.SelectedValue.ToString(), out int clienteId))
                {
                    MessageBox.Show("Seleccione un cliente válido.");
                    return;
                }

                DateTime fechaInicio = dtpFechaInicio.Value.Date;
                DateTime fechaFin = dtpFechaFin.Value.Date;

                if (fechaInicio > fechaFin)
                {
                    MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha de fin.");
                    return;
                }

                var totalVendido = FacturaController.Instance.ObtenerTotalVendidoPorCliente(clienteId, fechaInicio, fechaFin);

                MessageBox.Show("Total vendido: " + totalVendido.ToString("C"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el reporte: {ex.Message}");
            }
        }

        private void btnProductosMasVendidos_Click(object sender, EventArgs e)
        {
            try
            {
                GenerarColumnasMasVendidos();
                var productosMasVendidos = FacturaController.Instance.ObtenerProductosMasVendidos();

                dgvFacturas.Rows.Clear();

                foreach (var producto in productosMasVendidos)
                {
                    dgvFacturas.Rows.Add(producto.NombreProducto, producto.CantidadVendida);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el reporte: {ex.Message}");
            }
        }
    }
}
