using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto2
{
    public class Camera
    {
        private float angX = 0, angY = 0;
        private float zoom = 1;
        private Point lastMouse;
        private int offsetX = 0, offsetY = 0;

        public void MouseDown(object sender, MouseEventArgs e)
        {
            lastMouse = e.Location;
        }

        public void MouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                angY += (e.X - lastMouse.X) * 0.01f;
                angX += (e.Y - lastMouse.Y) * 0.01f;
                lastMouse = e.Location;
            }
        }

        public void Zoom(int delta)
        {
            zoom += delta > 0 ? 0.1f : -0.1f;
            zoom = Math.Max(0.1f, zoom);
        }

        public void Mover(Keys key)
        {
            switch (key)
            {
                case Keys.W: offsetY -= 10; break;
                case Keys.S: offsetY += 10; break;
                case Keys.A: offsetX -= 10; break;
                case Keys.D: offsetX += 10; break;
            }
        }

        public void Dibujar(Figure3D figura, Graphics g, Size size)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            PointF[] puntos2D = new PointF[figura.Vertices.Count];
            for (int i = 0; i < figura.Vertices.Count; i++)
            {
                Vector3D v = figura.Vertices[i];

                // Rotación
                v = v.RotarX(angX).RotarY(angY);

                // Proyección
                float f = zoom * 300 / (300 + v.Z);
                float x = v.X * f + size.Width / 2 + offsetX;
                float y = -v.Y * f + size.Height / 2 + offsetY;

                puntos2D[i] = new PointF(x, y);
            }

            using (Pen pen = new Pen(Color.White))
            {
                foreach (var arista in figura.Aristas)
                {
                    g.DrawLine(pen, puntos2D[arista.Item1], puntos2D[arista.Item2]);
                }
            }
        }
    }
}
