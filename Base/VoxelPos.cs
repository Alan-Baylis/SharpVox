namespace SharpVoxel
{
    [System.Serializable]
    public struct VoxelPosition
    {
        public int x, y, z;

        public VoxelPosition(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override bool Equals(object obj)
        {
            if (GetHashCode() == obj.GetHashCode())
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 47;
                hash = hash * 227 + x.GetHashCode();
                hash = hash * 227 + y.GetHashCode();
                hash = hash * 227 + z.GetHashCode();
                return hash;
            }
        }
    }
}