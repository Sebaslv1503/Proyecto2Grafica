﻿using Proyecto2;
using System;
using System.Linq;

namespace Proyecto2
{
    public static class PrimitiveFactory
    {
        public static Figure3D Crear(string nombre)
        {
            switch (nombre)
            {
                case "Cubo": return CrearCubo();
                case "Esfera": return CrearEsfera();
                case "Cilindro": return CrearCilindro();
                case "Cono": return CrearCono();
                case "Dona": return CrearDona();
                default: return null;
            }
        }


        public static Figure3D CrearCubo()
        {
            var f = new Figure3D();
            float s = 50;
            var v = f.Vertices;

            v.Add(new Vector3D(-s, -s, -s)); // 0
            v.Add(new Vector3D(s, -s, -s));  // 1
            v.Add(new Vector3D(s, s, -s));   // 2
            v.Add(new Vector3D(-s, s, -s));  // 3
            v.Add(new Vector3D(-s, -s, s));  // 4
            v.Add(new Vector3D(s, -s, s));   // 5
            v.Add(new Vector3D(s, s, s));    // 6
            v.Add(new Vector3D(-s, s, s));   // 7

            int[,] edges = {
                {0,1},{1,2},{2,3},{3,0},
                {4,5},{5,6},{6,7},{7,4},
                {0,4},{1,5},{2,6},{3,7}
            };

            foreach (var i in Enumerable.Range(0, edges.GetLength(0)))
                f.Aristas.Add((edges[i, 0], edges[i, 1]));

            return f;
        }

        public static Figure3D CrearEsfera()
        {
            var f = new Figure3D();
            int stacks = 10, slices = 20;
            float radius = 50;

            for (int i = 0; i <= stacks; i++)
            {
                float phi = (float)(Math.PI * i / stacks);
                for (int j = 0; j <= slices; j++)
                {
                    float theta = (float)(2 * Math.PI * j / slices);
                    float x = radius * (float)Math.Sin(phi) * (float)Math.Cos(theta);
                    float y = radius * (float)Math.Cos(phi);
                    float z = radius * (float)Math.Sin(phi) * (float)Math.Sin(theta);
                    f.Vertices.Add(new Vector3D(x, y, z));
                }
            }

            for (int i = 0; i < stacks; i++)
            {
                for (int j = 0; j < slices; j++)
                {
                    int idx = i * (slices + 1) + j;
                    f.Aristas.Add((idx, idx + 1));
                    f.Aristas.Add((idx, idx + slices + 1));
                }
            }

            return f;
        }

        public static Figure3D CrearCilindro()
        {
            var f = new Figure3D();
            int slices = 30;
            float radius = 40;
            float height = 100;

            for (int i = 0; i < slices; i++)
            {
                float angle = (float)(2 * Math.PI * i / slices);
                float x = radius * (float)Math.Cos(angle);
                float z = radius * (float)Math.Sin(angle);
                f.Vertices.Add(new Vector3D(x, -height / 2, z)); // base
                f.Vertices.Add(new Vector3D(x, height / 2, z));  // top
            }

            for (int i = 0; i < slices; i++)
            {
                int b1 = i * 2;
                int b2 = (i * 2 + 2) % (slices * 2);
                int t1 = b1 + 1;
                int t2 = b2 + 1;

                f.Aristas.Add((b1, b2));
                f.Aristas.Add((t1, t2));
                f.Aristas.Add((b1, t1));
            }

            return f;
        }

        public static Figure3D CrearCono()
        {
            var f = new Figure3D();
            int slices = 30;
            float radius = 40;
            float height = 100;
            Vector3D apex = new Vector3D(0, height / 2, 0);
            int centerIdx = 0;

            for (int i = 0; i < slices; i++)
            {
                float angle = (float)(2 * Math.PI * i / slices);
                float x = radius * (float)Math.Cos(angle);
                float z = radius * (float)Math.Sin(angle);
                f.Vertices.Add(new Vector3D(x, -height / 2, z));
            }

            f.Vertices.Add(apex); // punta

            for (int i = 0; i < slices; i++)
            {
                int next = (i + 1) % slices;
                f.Aristas.Add((i, next));               // base
                f.Aristas.Add((i, f.Vertices.Count - 1)); // lateral
            }

            return f;
        }
        public static Figure3D CrearDona()
        {
            /*
             Fórmulas paramétricas para un toroide:
             x = (R + r * cos(φ)) * cos(θ)
             y = (R + r * cos(φ)) * sin(θ)
             z = r * sin(φ)
            */

            var f = new Figure3D();

            int segments = 30; // (ángulo horizontal θ numero de divisiones)
            int rings = 20;    // (ángulo  vertical φ numero de divisiones)

            float R = 60f;     
            float r = 20f;     

            // Generar los vértices de la dona
            for (int i = 0; i <= segments; i++) // Giro alrededor del eje central (θ)
            {
                float theta = (float)(2 * Math.PI * i / segments);

                for (int j = 0; j <= rings; j++) // Giro del círculo interior (φ)
                {
                    float phi = (float)(2 * Math.PI * j / rings); 

                    float x = (R + r * (float)Math.Cos(phi)) * (float)Math.Cos(theta);
                    float y = (R + r * (float)Math.Cos(phi)) * (float)Math.Sin(theta);
                    float z = r * (float)Math.Sin(phi);

                    f.Vertices.Add(new Vector3D(x, y, z));
                }
            }

            // Conexión de vértices con aristas 
            for (int i = 0; i < segments; i++) // filas
            {
                for (int j = 0; j < rings; j++) // columnas
                {
                    // Índice actual en la matriz de vértices
                    int current = i * (rings + 1) + j;

                    // Vértice a la derecha (dentro del mismo anillo)
                    int right = current + 1;

                    // Vértice en la siguiente fila (otro ángulo θ)
                    int below = current + (rings + 1);

                    // Conectamos el punto actual con el siguiente en horizontal y vertical
                    f.Aristas.Add((current, right));  // Línea horizontal
                    f.Aristas.Add((current, below)); // Línea vertical
                }
            }

            return f;
        }


    }
}
