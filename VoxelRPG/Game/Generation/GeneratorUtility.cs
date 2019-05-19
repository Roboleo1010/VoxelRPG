namespace VoxelRPG.Game.Generation
{
    public static class GeneratorUtility
    {
        public static float Map(float newmin, float newmax, float origmin, float origmax, float value)
        {
            return (value - origmin) / (origmax - origmin) * (newmax - newmin) + newmin;
        }

        public static byte EnsureValueInRange(int input, byte min, byte max)
        {
            if (input < min)
                return min;
            else if (input > max)
                return max;
            else
                return (byte)input;
        }
    }
}
