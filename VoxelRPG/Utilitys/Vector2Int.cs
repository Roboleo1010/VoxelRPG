using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelRPG.Utilitys
{
    public struct Vector2Int
    {
        public int X { get; set; }
        public int Z { get; set; }

        public Vector2Int(int x, int z)
        {
            X = x;
            Z = z;
        }

        public override string ToString()
        {
            return X + "_" + Z;
        }
    }
}
