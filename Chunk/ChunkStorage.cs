using SharpVoxel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Chunk : MonoBehaviour
{
    void Clear()
    {
        VoxelData air = new VoxelData(0);
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    this[x, y, z] = air;
                }
            }
        }
    }

    public void ForceUpdate()
    {
        needsUpdate = true;
    }

    public void MoveIntoPlace()
    {
        transform.position = new Vector3(position.x / scaleFactor, position.y / scaleFactor, position.z / scaleFactor);
    }

    private int GetVoxelIndex(int x, int y, int z)
    {
        return x | y << voxShiftY | z << voxShiftZ;
    }

    private VoxelPosition GetVoxelPosition(uint index)
    {
        uint blockX = index & 0xF;
        uint blockY = (index >> voxShiftY) & 0xF;
        uint blockZ = (index >> voxShiftZ) & 0xF;
        return new VoxelPosition((int)blockX, (int)blockY, (int)blockZ);
    }


    // Getter / Setter
    public VoxelData this[int lx, int ly, int lz]
    {
        get
        {
            if (InRange(lx) && InRange(ly) && InRange(lz)) return voxels[GetVoxelIndex(lx,ly,lz)];
            return map.GetVoxel(position.x + lx, position.y + ly, position.z + lz);
        }
        set
        {
            if (InRange(lx) && InRange(ly) && InRange(lz))
            {
                voxels[GetVoxelIndex(lx, ly, lz)] = value;
                needsUpdate = true; // Invalidate state
            }
            else
            {
                map.SetVoxel(position.x + lx, position.y + ly, position.z + lz, value);
            }
        }
    }

}
