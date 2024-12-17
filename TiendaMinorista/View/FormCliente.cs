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
    public partial class FormCliente : Form
    {
        public FormCliente()
        {
            InitializeComponent();
            CargarClientes();
        }

        private void CargarClientes()
        {
            var clientes = ClienteController.Instance.ObtenerClientes().Select(c => new {
                c.Id,
                c.Nombre,
                c.Direccion,
                c.Contacto,
            }).ToList();

            dgvClientes.DataSource = clientes;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                var nombre = txtNombre.Text.Trim();
                var direccion = txtDireccion.Text.Trim();
                var contacto = txtContacto.Text.Trim();

                if (string.IsNullOrEmpty(nombre))
                {
                    MessageBox.Show("El campo 'Nombre' no puede estar vacio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(direccion))
                {
                    MessageBox.Show("El campo 'Direccion' no puede estar vacio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(contacto))
                {
                    MessageBox.Show("El campo 'Contacto' no puede estar vacio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!long.TryParse(contacto, out _))
                {
                    MessageBox.Show("El campo 'Contacto' debe contener solo numeros.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ClienteController.Instance.AgregarCliente(nombre, direccion, contacto);

                MessageBox.Show("Cliente agregado correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarClientes();

                txtNombre.Clear();
                txtDireccion.Clear();
                txtContacto.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrio un error al agregar el cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvClientes.CurrentRow == null)
                {
                    MessageBox.Show("Debe seleccionar un cliente para modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var clienteId = Convert.ToInt32(dgvClientes.CurrentRow.Cells["Id"].Value);

                var nombre = txtNombre.Text.Trim();
                var direccion = txtDireccion.Text.Trim();
                var contacto = txtContacto.Text.Trim();

                if (string.IsNullOrEmpty(nombre))
                {
                    MessageBox.Show("El campo 'Nombre' no puede estar vacio.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(direccion))
                {
                    MessageBox.Show("El campo 'Dirección' no puede estar vacio.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(contacto))
                {
                    MessageBox.Show("El campo 'Contacto' no puede estar vacio.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!long.TryParse(contacto, out _))
                {
                    MessageBox.Show("El campo 'Contacto' debe contener solo numeros.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ClienteController.Instance.ModificarCliente(clienteId, nombre, direccion, contacto);

                MessageBox.Show("Cliente modificado correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarClientes();
            }
            catch (FormatException)
            {
                MessageBox.Show("Error al convertir los datos. Verifique la informacion ingresada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrio un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvClientes.CurrentRow == null)
                {
                    MessageBox.Show("Debe seleccionar un cliente para eliminar.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var clienteId = Convert.ToInt32(dgvClientes.CurrentRow.Cells["Id"].Value);

                var confirmacion = MessageBox.Show("Seguro desea eliminar este cliente",
                                                  "Confirmar Eliminacion",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

                if (confirmacion == DialogResult.No)
                {
                    return; 
                }

                ClienteController.Instance.EliminarCliente(clienteId);

                MessageBox.Show("Cliente eliminado correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarClientes();
            }
            catch (FormatException)
            {
                MessageBox.Show("Error al obtener el ID del cliente. Verifique la seleccion.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrio un error al eliminar el cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var clienteId = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells["Id"].Value);

                var cliente = ClienteController.Instance.ObtenerClientes().FirstOrDefault(c => c.Id == clienteId);

                if (cliente != null)
                {
                    txtNombre.Text = cliente.Nombre;
                    txtDireccion.Text = cliente.Direccion;
                    txtContacto.Text = cliente.Contacto;
                }
            }
        }
    }
}
