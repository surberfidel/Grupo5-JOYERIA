using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Entidades;
using Negocio;

namespace Presentacion
{
    public partial class FormVentas : Form
    {
        NegocioProducto negocioProducto = new NegocioProducto();
        ClienteNegocio negocioCliente = new ClienteNegocio();
        VentaNegocio negocioVenta = new VentaNegocio();
        List<DetalleVenta> pedido = new List<DetalleVenta>();

        public FormVentas()
        {
            InitializeComponent();
        }

        private void FormVentas_Load(object sender, EventArgs e)
        {
            cboProducto.DataSource = negocioProducto.ListarProductos();
            cboProducto.DisplayMember = "Nombre";

            cboCliente.DataSource = negocioCliente.ListarClientes();
            cboCliente.DisplayMember = "Nombre";

            dgvPedido.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ActualizarPedido();
        }

        private void btnAgregarVentas_Click(object sender, EventArgs e)
        {
            if (cboProducto.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un producto.");
                return;
            }

            int cantidad;
            if (!int.TryParse(txtCantidadVentas.Text, out cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingresá una cantidad válida.");
                return;
            }

            Producto producto = (Producto)cboProducto.SelectedItem;

            if (cantidad > producto.Stock)
            {
                MessageBox.Show("No hay stock suficiente.");
                return;
            }

            DetalleVenta detalle = new DetalleVenta();
            detalle.Producto = producto;
            detalle.Cantidad = cantidad;
            detalle.PrecioUnitario = producto.PrecioUnitario;

            pedido.Add(detalle);
            ActualizarPedido();
            txtCantidadVentas.Clear();
        }

        private void btnEliminarVentas_Click(object sender, EventArgs e)
        {
            if (dgvPedido.CurrentRow != null)
            {
                DetalleVenta detalle = (DetalleVenta)dgvPedido.CurrentRow.DataBoundItem;
                pedido.Remove(detalle);
                ActualizarPedido();
            }
        }

        private void btnLimpiarVentas_Click(object sender, EventArgs e)
        {
            pedido.Clear();
            ActualizarPedido();
        }

        private void btnFinalizarVenta_Click(object sender, EventArgs e)
        {
            if (cboCliente.SelectedItem == null)
            {
                MessageBox.Show("Primero cargá y seleccioná un cliente.");
                return;
            }

            if (pedido.Count == 0)
            {
                MessageBox.Show("El pedido está vacío.");
                return;
            }

            Venta venta = new Venta();
            venta.Cliente = (Cliente)cboCliente.SelectedItem;
            venta.Detalles = new List<DetalleVenta>(pedido);
            venta.Total = CalcularTotal();

            negocioVenta.AltaVenta(venta);

            foreach (DetalleVenta detalle in pedido)
            {
                detalle.Producto.Stock = detalle.Producto.Stock - detalle.Cantidad;
            }

            MessageBox.Show("Venta N° " + venta.IdVenta + " registrada. Total: $" + venta.Total.ToString("N0"));

            pedido.Clear();
            ActualizarPedido();

            cboProducto.DataSource = null;
            cboProducto.DataSource = negocioProducto.ListarProductos();
            cboProducto.DisplayMember = "Nombre";
        }

        private decimal CalcularTotal()
        {
            decimal total = 0;
            foreach (DetalleVenta detalle in pedido)
            {
                total = total + detalle.Subtotal;
            }
            return total;
        }

        private void ActualizarPedido()
        {
            dgvPedido.DataSource = null;
            dgvPedido.DataSource = pedido;
            lblTotal.Text = "Total: $" + CalcularTotal().ToString("N0");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }

        private void cboProducto_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
