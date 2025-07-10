using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2
{
    public class Figure3D
    {
        public List<Vector3D> Vertices = new List<Vector3D>();
        public List<(int, int)> Aristas = new List<(int, int)>();
    }

    public class Vector3D
    {
        public float X, Y, Z;
        public Vector3D(float x, float y, float z) => (X, Y, Z) = (x, y, z);

        public Vector3D RotarX(float ang) =>
            new Vector3D(X, (float)(Y * Math.Cos(ang) - Z * Math.Sin(ang)), (float)(Y * Math.Sin(ang) + Z * Math.Cos(ang)));

        public Vector3D RotarY(float ang) =>
            new Vector3D((float)(X * Math.Cos(ang) + Z * Math.Sin(ang)), Y, (float)(-X * Math.Sin(ang) + Z * Math.Cos(ang)));
    }
}
