using OpenTK;
using System.Collections.Generic;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Graphics.Meshes
{
    public class AdaptiveCube : Mesh
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> colors = new List<Vector3>();
        List<int> indices = new List<int>();

        public AdaptiveCube(Vector3 color, Vector3Int pos, bool hasFrontNeighbour, bool hasBackNeighbour, bool hasLeftNeighbour,
                                                           bool hasRightNeighbour, bool hasTopNeighbour, bool hasBottomNeighbour)
        {
            GetMeshData(color, pos, hasFrontNeighbour, hasBackNeighbour, hasLeftNeighbour,
                        hasRightNeighbour, hasTopNeighbour, hasBottomNeighbour);
        }

        public override Vector3[] GetVertices()
        {
            return vertices.ToArray();
        }

        public override int[] GetIndices(int offset = 0)
        {
            int[] indiceArr = indices.ToArray();

            if (offset != 0)
                for (int i = 0; i < indiceArr.Length; i++)
                    indiceArr[i] += offset;

            return indiceArr;
        }

        public override Vector3[] GetColors()
        {
            return colors.ToArray();
        }

        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.Identity;

            //ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) *
            //              Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
        }

        private void GetMeshData(Vector3 color, Vector3Int pos, bool hasFrontNeighbour, bool hasBackNeighbour, bool hasLeftNeighbour,
                                                                bool hasRightNeighbour, bool hasTopNeighbour, bool hasBottomNeighbour)
        {
            if (hasFrontNeighbour == false || hasBackNeighbour == false || hasLeftNeighbour == false ||
                hasRightNeighbour == false || hasTopNeighbour == false || hasBottomNeighbour == false)
            {
                vertices.Add(new Vector3(0f + pos.X, 0f + pos.Y, 0f + pos.Z));
                vertices.Add(new Vector3(1f + pos.X, 0f + pos.Y, 0f + pos.Z));
                vertices.Add(new Vector3(1f + pos.X, 1f + pos.Y, 0f + pos.Z));
                vertices.Add(new Vector3(0f + pos.X, 1f + pos.Y, 0f + pos.Z));

                vertices.Add(new Vector3(0f + pos.X, 0f + pos.Y, 1f + pos.Z));
                vertices.Add(new Vector3(1f + pos.X, 0f + pos.Y, 1f + pos.Z));
                vertices.Add(new Vector3(1f + pos.X, 1f + pos.Y, 1f + pos.Z));
                vertices.Add(new Vector3(0f + pos.X, 1f + pos.Y, 1f + pos.Z));
            }

            VertexCount = vertices.Count;

            for (int i = 0; i < VertexCount; i++)
                colors.Add(color);

            ColorDataCount = colors.Count;

            if (!hasFrontNeighbour)
                indices.AddRange(new int[] { 0, 7, 3, 0, 4, 7 }); //front
            if (!hasBackNeighbour)
                indices.AddRange(new int[] { 1, 2, 6, 6, 5, 1 }); //back
            if (!hasLeftNeighbour)
                indices.AddRange(new int[] { 0, 2, 1, 0, 3, 2 });  //left
            if (!hasRightNeighbour)
                indices.AddRange(new int[] { 4, 5, 6, 6, 7, 4 }); //right
            if (!hasTopNeighbour)
                indices.AddRange(new int[] { 2, 3, 6, 6, 3, 7 }); //top
            if (!hasBottomNeighbour)
                indices.AddRange(new int[] { 0, 1, 5, 0, 5, 4 }); //bottom

            IndiceCount = indices.Count;
        }
    }
}
