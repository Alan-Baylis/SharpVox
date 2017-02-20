using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SharpVoxel
{
    namespace Renderers
    {
        /// <summary>
        /// The most simple renderer possible. No optimizations
        /// </summary>
        public class CulledRenderer : IRenderer
        {
            int vertIndex = 0;
            int triIndex = 0;
            public void Reset()
            {
                vertIndex = 0;
                triIndex = 0;
            }

            public MeshData Render(Chunk context, RenderQueue r)
            {
                Mesh m = new Mesh();
                MeshData meshData = new MeshData();
                meshData.startingIndex = vertIndex;

                for (int x = 0; x < context.chunkSize; x++)
                {
                    for (int y = 0; y < context.chunkSize; y++)
                    {
                        for (int z = 0; z < context.chunkSize; z++)
                        {
                            if(r.voxels[x + context.chunkSize * (y + context.chunkSize * z)] == false)
                            {
                                continue;
                            }
                            switch (context[x, y, z].shape)
                            {
                                case VoxelShape.BLOCK:
                                    RenderCube(ref meshData, ref context, context[x, y, z], x, y, z);
                                    break;
                                default:
                                    break;
                            }
                            
                        }
                    }
                }
                vertIndex += meshData.vertices.Count;
                triIndex += meshData.triangles.Count;
                return meshData;
            }

            void RenderCube(ref MeshData meshData, ref Chunk context, VoxelData vox, int x, int y, int z)
            {
                float lx = x * context.scaleFactor;
                float ly = y * context.scaleFactor;
                float lz = z * context.scaleFactor;
                float adjustAmount = context.scaleFactor / 2;

                if (!context[x, y + 1, z].isSolid(Direction.DOWN))
                {
                    //Top
                    meshData.vertices.Add(new Vector3(lx - adjustAmount, ly + adjustAmount, lz + adjustAmount));
                    meshData.vertices.Add(new Vector3(lx + adjustAmount, ly + adjustAmount, lz + adjustAmount));
                    meshData.vertices.Add(new Vector3(lx + adjustAmount, ly + adjustAmount, lz - adjustAmount));
                    meshData.vertices.Add(new Vector3(lx - adjustAmount, ly + adjustAmount, lz - adjustAmount));
                    meshData.AddQuadTriangles();
                }
                if (!context[x, y - 1, z].isSolid(Direction.UP))
                {
                    //Bottom
                    meshData.vertices.Add(new Vector3(lx - adjustAmount, ly - adjustAmount, lz - adjustAmount));
                    meshData.vertices.Add(new Vector3(lx + adjustAmount, ly - adjustAmount, lz - adjustAmount));
                    meshData.vertices.Add(new Vector3(lx + adjustAmount, ly - adjustAmount, lz + adjustAmount));
                    meshData.vertices.Add(new Vector3(lx - adjustAmount, ly - adjustAmount, lz + adjustAmount));
                    meshData.AddQuadTriangles();
                }
                if (!context[x, y, z + 1].isSolid(Direction.SOUTH))
                {
                    //N
                    meshData.vertices.Add(new Vector3(lx + adjustAmount, ly - adjustAmount, lz + adjustAmount));
                    meshData.vertices.Add(new Vector3(lx + adjustAmount, ly + adjustAmount, lz + adjustAmount));
                    meshData.vertices.Add(new Vector3(lx - adjustAmount, ly + adjustAmount, lz + adjustAmount));
                    meshData.vertices.Add(new Vector3(lx - adjustAmount, ly - adjustAmount, lz + adjustAmount));
                    meshData.AddQuadTriangles();
                }
                if (!context[x + 1, y, z].isSolid(Direction.WEST))
                {
                    //E
                    meshData.vertices.Add(new Vector3(lx + adjustAmount, ly - adjustAmount, lz - adjustAmount));
                    meshData.vertices.Add(new Vector3(lx + adjustAmount, ly + adjustAmount, lz - adjustAmount));
                    meshData.vertices.Add(new Vector3(lx + adjustAmount, ly + adjustAmount, lz + adjustAmount));
                    meshData.vertices.Add(new Vector3(lx + adjustAmount, ly - adjustAmount, lz + adjustAmount));
                    meshData.AddQuadTriangles();
                }
                if (!context[x, y, z - 1].isSolid(Direction.NORTH))
                {
                    //S
                    meshData.vertices.Add(new Vector3(lx - adjustAmount, ly - adjustAmount, lz - adjustAmount));
                    meshData.vertices.Add(new Vector3(lx - adjustAmount, ly + adjustAmount, lz - adjustAmount));
                    meshData.vertices.Add(new Vector3(lx + adjustAmount, ly + adjustAmount, lz - adjustAmount));
                    meshData.vertices.Add(new Vector3(lx + adjustAmount, ly - adjustAmount, lz - adjustAmount));
                    meshData.AddQuadTriangles();
                }
                if (!context[x - 1, y, z].isSolid(Direction.EAST))
                {
                    //W
                    meshData.vertices.Add(new Vector3(lx - adjustAmount, ly - adjustAmount, lz + adjustAmount));
                    meshData.vertices.Add(new Vector3(lx - adjustAmount, ly + adjustAmount, lz + adjustAmount));
                    meshData.vertices.Add(new Vector3(lx - adjustAmount, ly + adjustAmount, lz - adjustAmount));
                    meshData.vertices.Add(new Vector3(lx - adjustAmount, ly - adjustAmount, lz - adjustAmount));
                    meshData.AddQuadTriangles();
                }
            }
        }
    }
}
