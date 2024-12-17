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
using TiendaMinorista.Model.TiendaMinorista.Model;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;

namespace TiendaMinorista.View
{
    public partial class FormProducto : Form
    {
        public FormProducto()
        {
            InitializeComponent();
            CargarCategorias();
            CargarProductos();
            CargarProveedores();
        }

        private void CargarProductos()
        {
            var productos = ProductoController.Instance.ObtenerProductos().Select(p => new{
                p.Id,
                p.Nombre,
                p.Descripcion,
                p.Precio,
                p.Stock,
                Categoria = p.Categoria.Nombre ,
                Proveedor = p.Proveedor.Nombre
            }).ToList();

            dgvProductos.DataSource = productos;
        }

        private void CargarCategorias()
        {
            using (var context = new TiendaContext())
            {
                var categorias = context.Categorias.ToList();

                cmbCategoria.DataSource = categorias;
                cmbCategoria.DisplayMember = "Nombre";
                cmbCategoria.ValueMember = "Id";
            }
        }

        private void CargarProveedores()
        {
            var proveedores = ProveedorController.Instance.ObtenerProveedores().ToList();
            cmbProveedores.DataSource = proveedores;
            cmbProveedores.DisplayMember = "Nombre";
            cmbProveedores.ValueMember = "Id";
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            cmbCategoria.SelectedIndex = -1;
            cmbProveedores.SelectedIndex = -1;
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El campo 'Nombre' es obligatorio.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    MessageBox.Show("El campo 'Descripción' es obligatorio.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPrecio.Text))
                {
                    MessageBox.Show("El campo 'Precio' es obligatorio.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtStock.Text))
                {
                    MessageBox.Show("El campo 'Stock' es obligatorio.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
                {
                    MessageBox.Show("El campo 'Precio' debe ser un numero positivo valido.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
                {
                    MessageBox.Show("El campo 'Stock' debe ser un numero entero no negativo.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbCategoria.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar una categoria.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbProveedores.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un proveedor.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var nombre = txtNombre.Text;
                var descripcion = txtDescripcion.Text;
                var categoriaId = (int)cmbCategoria.SelectedValue;
                var proveedorId = (int)cmbProveedores.SelectedValue;

                ProductoController.Instance.CrearProducto(nombre, descripcion, precio, stock, categoriaId, proveedorId);

                MessageBox.Show("Producto creado exitosamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarProductos();

                LimpiarCampos();
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Error en el formato de los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrio un error al crear el producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.CurrentRow == null)
                {
                    MessageBox.Show("Debe seleccionar un producto para eliminar.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmacion = MessageBox.Show("Seguro de que desea eliminar este producto", "Confirmar Eliminacion",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion == DialogResult.No)
                {
                    return; 
                }

                if (!int.TryParse(dgvProductos.CurrentRow.Cells["Id"].Value.ToString(), out int productoId))
                {
                    MessageBox.Show("El identificador del producto no es valido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ProductoController.Instance.EliminarProducto(productoId);

                CargarProductos();
                MessageBox.Show("Producto eliminado exitosamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrio un error al intentar eliminar el producto: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El nombre del producto es obligatorio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    MessageBox.Show("La descripcion del producto es obligatoria.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
                {
                    MessageBox.Show("El precio debe ser un valor numerico mayor a 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
                {
                    MessageBox.Show("El stock debe ser un valor numerico mayor o igual a 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cmbCategoria.SelectedValue == null || cmbProveedores.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar una categoria y un proveedor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var productoId = Convert.ToInt32(dgvProductos.CurrentRow.Cells["Id"].Value);

                var nombre = txtNombre.Text;
                var descripcion = txtDescripcion.Text;
                var categoriaId = (int)cmbCategoria.SelectedValue;
                var proveedorId = (int)cmbProveedores.SelectedValue;

                ProductoController.Instance.ModificarProducto(productoId, nombre, descripcion, precio, stock, categoriaId, proveedorId);

                CargarProductos();

                MessageBox.Show("Producto modificado correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrio un error al modificar el producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var productoId = Convert.ToInt32(dgvProductos.Rows[e.RowIndex].Cells["Id"].Value);

                var producto = ProductoController.Instance.ObtenerProductos().FirstOrDefault(p => p.Id == productoId);

                if (producto != null)
                {
                    txtNombre.Text = producto.Nombre;
                    txtDescripcion.Text = producto.Descripcion;
                    txtPrecio.Text = producto.Precio.ToString();
                    txtStock.Text = producto.Stock.ToString();

                    cmbCategoria.SelectedValue = producto.CategoriaId;
                    if (producto.ProveedorId == null)
                    {
                        cmbProveedores.SelectedValue = 0; 
                    }
                    else
                    {
                        cmbProveedores.SelectedValue = producto.ProveedorId; 
                    }
                }
            }
        }
    }
}
