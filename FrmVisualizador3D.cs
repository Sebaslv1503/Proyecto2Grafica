using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto2
{
    public partial class FrmVisualizador3D : Form
    {
        private PictureBox picCanvas;
        private ComboBox cmbFiguras;
        private Figure3D figuraActual;
        private Camera cam;

        public FrmVisualizador3D()
        {
            this.Text = "Visualizador 3D Interactivo";
            this.Size = new Size(800, 600);
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            picCanvas = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black
            };

            cmbFiguras = new ComboBox
            {
                Location = new Point(10, 10),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFiguras.Items.AddRange(new string[] { "Cubo", "Esfera", "Cilindro", "Cono", "Dona" });
            cmbFiguras.SelectedIndex = 0;

            this.Controls.Add(picCanvas);
            this.Controls.Add(cmbFiguras);

            cam = new Camera();
            figuraActual = PrimitiveFactory.Crear("Cubo");

            cmbFiguras.SelectedIndexChanged += (s, e) =>
            {
                figuraActual = PrimitiveFactory.Crear(cmbFiguras.SelectedItem.ToString());
                picCanvas.Invalidate();
            };

            picCanvas.Paint += picCanvas_Paint;
            picCanvas.MouseDown += cam.MouseDown;
            picCanvas.MouseMove += (s, e) => { cam.MouseMove(e); picCanvas.Invalidate(); };
            picCanvas.MouseWheel += (s, e) => { cam.Zoom(e.Delta); picCanvas.Invalidate(); };
            this.KeyDown += (s, e) => { cam.Mover(e.KeyCode); picCanvas.Invalidate(); };
        }

        private void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (figuraActual == null) return;

            cam.Dibujar(figuraActual, e.Graphics, picCanvas.Size);

            using (Font font = new Font("Segoe UI", 14, FontStyle.Bold))
            using (Brush brush = new SolidBrush(Color.White))
            {
                string titulo = $"Figura: {cmbFiguras.SelectedItem}";
                e.Graphics.DrawString(titulo, font, brush, 20, 50);
            }

            using (Font font = new Font("Segoe UI", 10))
            using (Brush brush = new SolidBrush(Color.LightGray))
            {
                string guia = "WASD: Mover cámara\nMouse: Rotar figura\nRueda: Zoom\n← →: Cambiar figura";
                e.Graphics.DrawString(guia, font, brush, 20, 80);
            }
        }
    }
}
