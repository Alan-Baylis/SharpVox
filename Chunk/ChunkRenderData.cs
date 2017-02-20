using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SharpVoxel
{
    public struct RenderQueue
    {
        public VoxelShape shape;
        public short id;
        public BitArray voxels;

    }

    public class ChunkRenderData
    {
        public List<RenderQueue> queues = new List<RenderQueue>();
        public Chunk parent;
        public ChunkRenderData(Chunk p)
        {
            parent = p;
        }
        private RenderQueue getQueue(short id, VoxelShape shape)
        {
            Predicate<RenderQueue> idFinder = (RenderQueue q) => { return q.id == id; };
            bool exists = queues.Exists(idFinder);
            if (exists)
            {
                return queues.Find(idFinder);
            }
            else
            {
                RenderQueue q = new RenderQueue();
                q.id = id;
                q.shape = shape;
                q.voxels = new BitArray(parent.chunkSize * parent.chunkSize * parent.chunkSize);
                queues.Add(q);
                return q;
            }
        }

        public void Add(VoxelData voxel, int x, int y, int z)
        {
            switch (voxel.shape)
            {
                case VoxelShape.BLOCK:
                    RenderQueue r = getQueue(voxel.id, voxel.shape);
                    r.voxels[x + parent.chunkSize * (y + parent.chunkSize * z)] = true;
                    break;
                case VoxelShape.NONE:
                default: // Don't render if not implemented
                    return;
            }
        }
    }
}
