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
    public partial class FormProveedor : Form
    {
        public FormProveedor()
        {
            InitializeComponent();
            CargarProveedores();
        }

        private void CargarProveedores()
        {
            try
            {
                var proveedores = ProveedorController.Instance.ObtenerProveedores().ToList();
                var proveedoresConProductos = proveedores.Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.Direccion,
                    p.Contacto,
                    Productos = string.Join(", ", p.Productos.Select(prod => prod.Nombre)) 
                }).ToList();

                dgvProveedores.DataSource = proveedoresConProductos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los proveedores: {ex.Message}");
            }
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El nombre del proveedor es obligatorio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDireccion.Text))
                {
                    MessageBox.Show("La direccion del proveedor es obligatoria.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtContacto.Text))
                {
                    MessageBox.Show("El contacto del proveedor es obligatorio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var nombre = txtNombre.Text;
                var direccion = txtDireccion.Text;
                var contacto = txtContacto.Text;

                ProveedorController.Instance.AgregarProveedor(nombre, direccion, contacto);

                CargarProveedores();

                MessageBox.Show("Proveedor agregado correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrio un error al agregar el proveedor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El nombre del proveedor es obligatorio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDireccion.Text))
                {
                    MessageBox.Show("La direccion del proveedor es obligatoria.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtContacto.Text))
                {
                    MessageBox.Show("El contacto del proveedor es obligatorio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (dgvProveedores.CurrentRow == null)
                {
                    MessageBox.Show("Debe seleccionar un proveedor para modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var proveedorId = Convert.ToInt32(dgvProveedores.CurrentRow.Cells["Id"].Value);

                var nombre = txtNombre.Text;
                var direccion = txtDireccion.Text;
                var contacto = txtContacto.Text;

                ProveedorController.Instance.ModificarProveedor(proveedorId, nombre, direccion, contacto);

                CargarProveedores();

                MessageBox.Show("Proveedor modificado correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrio un error al modificar el proveedor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProveedores.CurrentRow == null)
                {
                    MessageBox.Show("Debe seleccionar un proveedor para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirmacion = MessageBox.Show("Seguro de que desea eliminar este proveedor", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.No)
                {
                    return;
                }

                var proveedorId = Convert.ToInt32(dgvProveedores.CurrentRow.Cells["Id"].Value);

                ProveedorController.Instance.EliminarProveedor(proveedorId);

                CargarProveedores();

                MessageBox.Show("Proveedor eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al eliminar el proveedor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProveedores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var proveedorId = Convert.ToInt32(dgvProveedores.Rows[e.RowIndex].Cells["Id"].Value);

                var proveedor = ProveedorController.Instance.ObtenerProveedores().FirstOrDefault(p => p.Id == proveedorId);

                if (proveedor != null)
                {
                    txtNombre.Text = proveedor.Nombre;
                    txtDireccion.Text = proveedor.Direccion;
                    txtContacto.Text = proveedor.Contacto;
                }
            }
        }
    }
}
