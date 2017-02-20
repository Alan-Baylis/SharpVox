using UnityEngine;

namespace SharpVoxel
{
    public enum VoxelShape {
        NONE,
        OTHER,
        BLOCK,
        WEDGE
    }

    [System.Flags]
    public enum Direction
    {
        UP    = 0,
        DOWN  = 1,
        NORTH = 2,
        SOUTH = 4,
        WEST  = 8,
        EAST  = 16
    }

    public struct VoxelData {
        public short id;
        public byte data;

        public VoxelShape shape
        {
            get
            {
                return VoxelManager.GetVoxelFromId(id).GetShape();
            }
        }

        public bool isSolid(Direction face) {
            return VoxelManager.GetVoxelFromId(id).GetFaceSolidity(face);
        }

        public VoxelData(short id)
        {
            this.id = id;
            data = 0;
        }

    }

    public interface Voxel {
        bool GetFaceSolidity(Direction d);
        VoxelShape GetShape();
    }
}
