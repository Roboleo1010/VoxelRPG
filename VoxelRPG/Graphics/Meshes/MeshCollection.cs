using OpenTK;
using System.Collections.Generic;

namespace VoxelRPG.Graphics.Meshes
{
    public class MeshCollection : Mesh
    {
        Vector3[] vertices;
        Vector3[] colors;
        int[] indices;

        public MeshCollection(List<Mesh> meshes)
        {
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> col = new List<Vector3>();
            List<int> ind = new List<int>();

            int indiceOffset = 0;

            foreach (Mesh m in meshes)
            {
                verts.AddRange(m.GetVertices());
                col.AddRange(m.GetColors());

                int[] subIndices = m.GetIndices();
                if (subIndices.Length != 0)
                {
                    foreach (int i in subIndices)
                        ind.Add((indiceOffset * 8) + i);
                    indiceOffset++;
                }
            }

            vertices = verts.ToArray();
            colors = col.ToArray();
            indices = ind.ToArray();

            VertexCount = vertices.Length;
            ColorCount = colors.Length;
            IndiceCount = indices.Length;
        }

        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.Identity;
        }

        public override Vector3[] GetColors()
        {
            return colors;
        }

        public override int[] GetIndices(int offset = 0)
        {
            if (offset != 0)
                for (int i = 0; i < indices.Length; i++)
                    indices[i] += offset;

            return indices;
        }

        public override Vector3[] GetVertices()
        {
            return vertices;
        }
    }
}