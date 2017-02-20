using System.Collections.Generic;

namespace SharpVoxel
{
    public static class VoxelManager {

        private static Dictionary<short,Voxel> types;
        private static bool isInit = false;
        public static void EnsureInit()
        {
            if(isInit) { return; }

            types = new Dictionary<short, Voxel>();
            types.Add(0, new Voxels.Air());
            types.Add(1, new Voxels.SampleBlock());
            types.Add(2, new Voxels.SampleBlock());
            isInit = true;
        }

        public static Voxel GetVoxelFromId(short id)
        {
            EnsureInit();
            return types[id];
        }
    }
}
