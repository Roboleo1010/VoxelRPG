using OpenTK;

namespace VoxelRPG.Utilitys
{
    public struct Vector3Int
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Vector3Int(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3Int(Vector3 v)
        {
            X = (int)v.X;
            Y = (int)v.Y;
            Z = (int)v.Z;
        }

        public Vector3Int(string s)
        {
            s.Split('_');

            X = s[0];
            Y = s[1];
            Z = s[2];
        }

        public override string ToString()
        {
            return string.Format("{0}_{1}_{2}", X, Y, Z);
        }
    }
}
