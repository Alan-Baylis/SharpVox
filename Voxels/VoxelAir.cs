using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SharpVoxel
{
    namespace Voxels
    {
        class Air : Voxel
        {
            public bool GetFaceSolidity(Direction d)
            {
                return false;
            }

            public VoxelShape GetShape()
            {
                return VoxelShape.NONE;
            }
        }
    }
}