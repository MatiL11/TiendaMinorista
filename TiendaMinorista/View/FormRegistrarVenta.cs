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
using TiendaMinorista.Model;

namespace TiendaMinorista.View
{
    public partial class FormRegistrarVenta : Form
    {
        public FormRegistrarVenta()
        {
            InitializeComponent();
            CargarClientes();
            CargarProductosDisponibles();
            ConfigurarDataGridView();
            dgvDetallesFactura.AllowUserToAddRows = false;
        }

        private void ConfigurarDataGridView()
        {
            dgvDetallesFactura.Columns.Clear();

            dgvDetallesFactura.Columns.Add("Producto", "Producto");
            dgvDetallesFactura.Columns.Add("Cantidad", "Cantidad");
            dgvDetallesFactura.Columns.Add("PrecioUnitario", "Precio Unitario");
            dgvDetallesFactura.Columns.Add("Subtotal", "Subtotal");

            dgvDetallesFactura.Columns["Producto"].Width = 200;
            dgvDetallesFactura.Columns["Cantidad"].Width = 100;
            dgvDetallesFactura.Columns["PrecioUnitario"].Width = 150;
            dgvDetallesFactura.Columns["Subtotal"].Width = 150;
        }

        private void CargarProductosDisponibles()
        {
            try
            {
                var productos = ProductoController.Instance.ObtenerProductos().Select( p => new
                {
                    p.Nombre,
                    p.Id
                }).ToList();

                cmbProductos.DataSource = productos;
                cmbProductos.DisplayMember = "Nombre";
                cmbProductos.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}");
            }
        }

        private void CargarClientes()
        {
            var clientes = ClienteController.Instance.ObtenerClientes().Select(c => new {
                c.Nombre,
                c.Id
            }).ToList();
            cmbClientes.DataSource = clientes;
            cmbClientes.DisplayMember = "Nombre";
            cmbClientes.ValueMember = "Id";
        }

        private void CalcularTotalFactura()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dgvDetallesFactura.Rows)
            {
                if (row.Cells["Subtotal"].Value != null && decimal.TryParse(row.Cells["Subtotal"].Value.ToString(), out decimal subtotal))
                {
                    total += subtotal;
                }
            }

            lblTotalFactura.Text = "Total: " + total.ToString("C");
        }

        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDetallesFactura.Rows.Count == 0)
                {
                    MessageBox.Show("Por favor, agregue productos a la factura.");
                    return;
                }

                var detalles = new List<DetalleFactura>();


                foreach (DataGridViewRow row in dgvDetallesFactura.Rows)
                {
                    if (row.Cells["Producto"].Value == null || string.IsNullOrWhiteSpace(row.Cells["Producto"].Value.ToString()) ||
                        row.Cells["Cantidad"].Value == null || string.IsNullOrWhiteSpace(row.Cells["Cantidad"].Value.ToString()) ||
                        row.Cells["PrecioUnitario"].Value == null || string.IsNullOrWhiteSpace(row.Cells["PrecioUnitario"].Value.ToString()))
                    {
                        MessageBox.Show("Asegúrese de que todas las celdas tengan valores válidos.");
                        return;
                    }

                    if (!int.TryParse(row.Cells["Cantidad"].Value.ToString(), out int cantidad) || cantidad <= 0)
                    {
                        MessageBox.Show("La cantidad debe ser un numero valido y mayor a 0.");
                        return;
                    }

                    if (!decimal.TryParse(row.Cells["PrecioUnitario"].Value.ToString(), out decimal precioUnitario) || precioUnitario <= 0)
                    {
                        MessageBox.Show("El precio unitario debe ser un número valido y mayor a 0.");
                        return;
                    }

                    string productoNombre = row.Cells["Producto"].Value.ToString();

                    var producto = ProductoController.Instance.ObtenerProductos().FirstOrDefault(p => p.Nombre == productoNombre);
                    if (producto == null)
                    {
                        MessageBox.Show($"El producto '{productoNombre}' no existe.");
                        return;
                    }

                    var detalle = new DetalleFactura
                    {
                        ProductoId = producto.Id,
                        Cantidad = cantidad,
                        PrecioUnitario = precioUnitario
                    };

                    detalles.Add(detalle);
                }

                if (cmbClientes.SelectedValue == null || !int.TryParse(cmbClientes.SelectedValue.ToString(), out int clienteId))
                {
                    MessageBox.Show("Seleccione un cliente válido.");
                    return;
                }

                FacturaController.Instance.RegistrarVenta(detalles, clienteId); 

                MessageBox.Show("Venta registrada con éxito.");
                CargarProductosDisponibles(); 
                dgvDetallesFactura.Rows.Clear(); 
                lblTotalFactura.Text = "Total: $0.00"; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (cmbProductos.SelectedValue == null || string.IsNullOrEmpty(txtCantidad.Text) || int.Parse(txtCantidad.Text) <= 0)
            {
                MessageBox.Show("Por favor, seleccione un producto y especifique una cantidad válida.");
                return;
            }

            int productoId = (int)cmbProductos.SelectedValue;
            var producto = ProductoController.Instance.ObtenerProductos().FirstOrDefault(p => p.Id == productoId);

            if (producto == null)
            {
                MessageBox.Show("Producto no encontrado.");
                return;
            }

            int cantidad = int.Parse(txtCantidad.Text);

            if (producto.Stock < cantidad)
            {
                MessageBox.Show($"No hay suficiente stock para el producto: {producto.Nombre}");
                return;
            }

            decimal precioUnitario = producto.Precio;
            decimal subtotal = precioUnitario * cantidad;

            dgvDetallesFactura.Rows.Add(producto.Nombre, cantidad, precioUnitario, subtotal);

            txtCantidad.Clear();

            CalcularTotalFactura();
        }

        private void btnAplicarDescuento_Click(object sender, EventArgs e)
        {
            try
            {
                decimal porcentajeDescuento = 10; 

                if (decimal.TryParse(lblTotalFactura.Text.Replace("Total: $", ""), out decimal totalFactura))
                {
                    decimal totalConDescuento = totalFactura * (1 - porcentajeDescuento / 100);


                    lblTotalFactura.Text = "Total con descuento: " + totalConDescuento.ToString("C");
                }
                else
                {
                    MessageBox.Show("El total de la factura no es valido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrio un error al aplicar el descuento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
