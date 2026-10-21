using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Negocio;
using Entidades;

namespace Presentacion
{
    public partial class FormSistema : Form
    {
        NegocioProducto ngp1 = new NegocioProducto();

        public FormSistema()
        {
            InitializeComponent();

            dgvProductos.DataSource = ngp1.ListarProductos();
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvProductos.SelectionChanged += dgvProductos_SelectionChanged;

            AgregarDiseñoMysticCharm();
        }

        private void AgregarDiseñoMysticCharm()
        {
            this.Text = "Joyería Mystic Charm";
            this.BackColor = Color.FromArgb(250, 245, 239);

            Label titulo = new Label();
            titulo.Text = "MYSTIC CHARM";
            titulo.Font = new Font("Georgia", 24);
            titulo.ForeColor = Color.FromArgb(90, 61, 43);
            titulo.AutoSize = true;
            titulo.Location = new Point(620, 205);
            this.Controls.Add(titulo);

            Label frase = new Label();
            frase.Text = "Brillá tu propia historia";
            frase.Font = new Font("Georgia", 12, FontStyle.Italic);
            frase.ForeColor = Color.FromArgb(140, 90, 70);
            frase.AutoSize = true;
            frase.Location = new Point(650, 245);
            this.Controls.Add(frase);

            Button btnClientes = new Button();
            btnClientes.Text = "Clientes";
            btnClientes.Location = new Point(590, 421);
            btnClientes.Size = new Size(150, 38);
            btnClientes.Click += btnClientes_Click;
            this.Controls.Add(btnClientes);

            Label lblFoto = new Label();
            lblFoto.Text = "Fotografía del producto";
            lblFoto.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblFoto.ForeColor = Color.FromArgb(90, 61, 43);
            lblFoto.AutoSize = true;
            lblFoto.Location = new Point(590, 268);
            this.Controls.Add(lblFoto);

            PictureBox picProducto = new PictureBox();
            picProducto.Name = "picProducto";
            picProducto.Location = new Point(590, 292);
            picProducto.Size = new Size(310, 112);
            picProducto.SizeMode = PictureBoxSizeMode.Zoom;
            picProducto.BackColor = Color.White;
            picProducto.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(picProducto);

            TextBox txtBuscarProducto = new TextBox(); txtBuscarProducto.Name="txtBuscarProducto"; txtBuscarProducto.SetBounds(180, 421, 180, 28); this.Controls.Add(txtBuscarProducto);
            Button btnBuscar = new Button(); btnBuscar.Text="Buscar"; btnBuscar.SetBounds(365,421,80,28); btnBuscar.Click += (s,e) => { dgvProductos.DataSource=null; dgvProductos.DataSource=ngp1.BuscarPorNombre(txtBuscarProducto.Text); }; this.Controls.Add(btnBuscar);
            Button btnStock = new Button(); btnStock.Text="Stock bajo"; btnStock.SetBounds(450,421,100,28); btnStock.Click += (s,e) => { dgvProductos.DataSource=null; dgvProductos.DataSource=ngp1.ProductosStockBajo(2); }; this.Controls.Add(btnStock);
            Button btnHistorial = new Button(); btnHistorial.Text="Ventas"; btnHistorial.SetBounds(750,421,80,38); btnHistorial.Click += (s,e) => new FormHistorialVentas().ShowDialog(); this.Controls.Add(btnHistorial);
            Button btnPrecio = new Button(); btnPrecio.Text="Ordenar precio"; btnPrecio.SetBounds(180,455,110,28); btnPrecio.Click += (s,e) => { dgvProductos.DataSource=null; dgvProductos.DataSource=ngp1.OrdenarPorPrecio(); }; this.Controls.Add(btnPrecio);
            Button btnTodos = new Button(); btnTodos.Text="Ver todos"; btnTodos.SetBounds(295,455,90,28); btnTodos.Click += (s,e) => { dgvProductos.DataSource=null; dgvProductos.DataSource=ngp1.ListarProductos(); }; this.Controls.Add(btnTodos);
            Button btnValor = new Button(); btnValor.Text="Valor inventario"; btnValor.SetBounds(390,455,120,28); btnValor.Click += (s,e) => MessageBox.Show("Valor del inventario: $" + ngp1.ValorInventario().ToString("N0")); this.Controls.Add(btnValor);
            Button btnFiltrarCat = new Button(); btnFiltrarCat.Text="Filtrar categoría"; btnFiltrarCat.SetBounds(20,455,150,28); btnFiltrarCat.Click += (s,e) => { dgvProductos.DataSource=null; dgvProductos.DataSource=ngp1.FiltrarPorCategoria(cboCategoria.Text); }; this.Controls.Add(btnFiltrarCat);
            Button btnFiltrarMarca = new Button(); btnFiltrarMarca.Text="Filtrar marca"; btnFiltrarMarca.SetBounds(20,490,150,28); btnFiltrarMarca.Click += (s,e) => { dgvProductos.DataSource=null; dgvProductos.DataSource=ngp1.FiltrarPorMarca(cboMarca.Text); }; this.Controls.Add(btnFiltrarMarca);

            btnAgregar.BackColor = Color.FromArgb(231, 210, 195);
            btnModificar.BackColor = Color.FromArgb(231, 210, 195);
            btnEliminar.BackColor = Color.FromArgb(230, 180, 170);
            btnCrearPedido.BackColor = Color.FromArgb(205, 169, 111);
        }

        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow == null || dgvProductos.CurrentRow.DataBoundItem == null)
                return;

            Producto producto = (Producto)dgvProductos.CurrentRow.DataBoundItem;

            txtNombre.Text = producto.Nombre;
            cboCategoria.Text = producto.Categoria;
            cboMarca.Text = producto.Marca;
            txtPrecio.Text = producto.PrecioUnitario.ToString();
            txtStock.Text = producto.Stock.ToString();

            PictureBox picProducto = this.Controls["picProducto"] as PictureBox;

            if (picProducto != null)
            {
                // Limpiar la imagen anterior y liberar memoria
                if (picProducto.Image != null)
                {
                    picProducto.Image.Dispose();
                    picProducto.Image = null;
                }

                if (!string.IsNullOrWhiteSpace(producto.ImagenUrl))
                {
                    // Construimos la ruta combinando la carpeta base del proyecto/ejecutable con el nombre de la foto
                    // Busca en la carpeta ImagenesProductos dentro del directorio de la solución/ejecutable
                    string rutaBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "ImagenesProductos");
                    string rutaCompleta = Path.GetFullPath(Path.Combine(rutaBase, producto.ImagenUrl));

                    // Si no la encuentra ahí, busca directamente en ImagenesProductos del directorio de trabajo actual
                    if (!File.Exists(rutaCompleta))
                    {
                        rutaCompleta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImagenesProductos", producto.ImagenUrl);
                    }

                    if (File.Exists(rutaCompleta))
                    {
                        try
                        {
                            using (var stream = new FileStream(rutaCompleta, FileMode.Open, FileAccess.Read))
                            {
                                picProducto.Image = Image.FromStream(stream);
                            }
                        }
                        catch
                        {
                            picProducto.Image = null;
                        }
                    }
                    else
                    {
                        picProducto.Image = null;
                    }
                }
                else
                {
                    picProducto.Image = null;
                }
            }
        }
        private void btnCrearPedido_Click(object sender, EventArgs e)
        {
            FormVentas ventanaVentas = new FormVentas();
            ventanaVentas.ShowDialog();
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            FormClientes ventanaClientes = new FormClientes();
            ventanaClientes.ShowDialog();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Producto producto = new Producto();

            producto.Nombre = txtNombre.Text;
            producto.Categoria = cboCategoria.Text;
            producto.Marca = cboMarca.Text;
            producto.PrecioUnitario = decimal.Parse(txtPrecio.Text);
            producto.Stock = int.Parse(txtStock.Text);
            // Asigna la ruta/URL (puedes agregar un TextBox para esto si la interfaz lo requiere)
            producto.ImagenUrl = "";

            ngp1.AltaProducto(producto);

            dgvProductos.DataSource = null;
            dgvProductos.DataSource = ngp1.ListarProductos();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow != null)
            {
                Producto producto = (Producto)dgvProductos.CurrentRow.DataBoundItem;

                producto.Nombre = txtNombre.Text;
                producto.Categoria = cboCategoria.Text;
                producto.Marca = cboMarca.Text;
                producto.PrecioUnitario = decimal.Parse(txtPrecio.Text);
                producto.Stock = int.Parse(txtStock.Text);
                // Mantiene la URL existente del producto al modificar
                // producto.ImagenUrl = producto.ImagenUrl; 

                ngp1.ModificarProducto(producto);

                dgvProductos.DataSource = null;
                dgvProductos.DataSource = ngp1.ListarProductos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow != null)
            {
                Producto producto = (Producto)dgvProductos.CurrentRow.DataBoundItem;
                ngp1.EliminarProducto(producto);

                dgvProductos.DataSource = null;
                dgvProductos.DataSource = ngp1.ListarProductos();
            }
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void txtStock_TextChanged(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void txtNombre_TextChanged(object sender, EventArgs e) { }

        private void FormSistema_Load(object sender, EventArgs e)
        {

        }
    }
}
