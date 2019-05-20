using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelRPG.Utilitys
{
    public struct Vector3Int
    {
        public int X { get; set; }

        public int Z { get; set; }
        public int Y { get; set; }

        public Vector3Int(int x, int z, int y)
        {
            X = x;
            Z = z;
            Y = y;
        }

        public override string ToString()
        {
            return X + "_" + Z + "_" + Y;
        }
    }
}
