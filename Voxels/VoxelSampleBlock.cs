using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SharpVoxel
{
    namespace Voxels
    {
        public class SampleBlock : Voxel
        {
            public bool GetFaceSolidity(Direction d)
            {
                return true;
            }

            public VoxelShape GetShape()
            {
                return VoxelShape.BLOCK;
            }
        }
    }
   
}