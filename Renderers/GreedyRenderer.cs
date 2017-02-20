using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SharpVoxel
{
    namespace Renderers
    {
        public class GreedyRenderer : IRenderer
        {
            int vertIndex = 0;
            public void Reset()
            {
                vertIndex = 0;
            }


            bool voxelAt(Chunk context, RenderQueue q, int x, int y, int z)
            {
                int chunkSz = context.chunkSize;

                if (x < 0 || y < 0 || z < 0) { return false; }
                if (x > chunkSz || y > chunkSz || z > chunkSz) { return false; }
                return q.voxels[x + chunkSz * (y + chunkSz * z)];
            }

            // We're going to implement our own greedy mesh renderer
            public MeshData Render(Chunk context, RenderQueue r)
            {
                int dimension = 0; // The current dimension (x = 0, y = 1, z = 2)
                int chunkSz = context.chunkSize;

                MeshData mesh = new MeshData();
                mesh.startingIndex = vertIndex;
                BitArray mask = new BitArray((chunkSz + 1) * (chunkSz + 1));

                for (dimension = 0; dimension < 3; dimension++) // We sweep over all 3 dimensions
                {
                    int[] x = new int[3];
                    int[] q = new int[3];
                    q[dimension] = 1;

                    // The other dimensions. We modulo 3 because we're not aiming to have 4D hyper meshes
                    int u = (dimension + 1) % 3;
                    int v = (dimension + 2) % 3;

                    // We do this twice, to cover backfaces. This can be optimized.
                    for (bool backFace = false, b = true; b != backFace; backFace = true, b = !b)
                    {
                        // Dimension = -1 to get extremes of the cube. It will only render half the outside faces otherwise!
                        for (x[dimension] = -1; x[dimension] < chunkSz;)
                        {
                            int n = 0;
                            // Generate the mask
                            for (x[v] = 0; x[v] < chunkSz; ++x[v])
                            {
                                for (x[u] = 0; x[u] < chunkSz; ++x[u])
                                {
                                    bool vox = (0 <= x[dimension] ? voxelAt(context, r, x[0], x[1], x[2]) : false) !=
                                        (x[dimension] < chunkSz - 1 ? voxelAt(context, r, x[0] + q[0], x[1] + q[1], x[2] + q[2]) : false);

                                    mask[n++] = vox;
                                }
                            }
                            n = 0;
                            x[dimension]++;

                            /*
                             * Generating a mesh looks somewhat like this (this is in mask-space by the way)
                             * 1: Compute width
                             * 2: Compute maximum height
                             * 3: Generate Quad.
                             * 4: Go to 1.
                             * This means quads are generated in this order:
                             * 123
                             * 456
                             * 789
                             * 
                             * EG:
                             * 111
                             * 2 3
                             * 444
                             */

                            // Scan over all blocks in the mask plane
                            for(int j = 0; j < chunkSz; j++)
                            {
                                for(int i = 0; i < chunkSz;)
                                {
                                    // If there's a voxel at this position...
                                    if(mask[n])
                                    {
                                        int width = 0;
                                        int height = 0;

                                        // Find the width
                                        for (width = 1; i + width < chunkSz && mask[n + width]; ++width);

                                        // Find the height
                                        for (height = 1; j + height < chunkSz; ++height)
                                        {
                                            bool done = false;
                                            for (int k = 0; k < width; ++k)
                                            {
                                                if (!mask[n + k + height * chunkSz])
                                                {
                                                    done = true;
                                                    break;
                                                }
                                            }
                                            if (done) break;
                                        }

                                        //We now have the dimensions of the quad we're generating. (width * height)
                                        //Let's actually create the quad
                                        x[u] = i; x[v] = j;
                                        int[] du = new int[3];
                                        int[] dv = new int[3];
                                        du[u] = width;
                                        dv[v] = height;

                                        // Render

                                        Vector3 v1 = new Vector3(x[0], x[1], x[2]);
                                        Vector3 v2 = new Vector3(x[0] + du[0], x[1] + du[1], x[2] + du[2]);
                                        Vector3 v3 = new Vector3(x[0] + du[0] + dv[0], x[1] + du[1] + dv[1], x[2] + du[2] + dv[2]);
                                        Vector3 v4 = new Vector3(x[0] + dv[0], x[1] + dv[1], x[2] + dv[2]);
                                        
                                        if(!backFace)
                                        {
                                            mesh.AddVertex(v1);
                                            mesh.AddVertex(v2);
                                            mesh.AddVertex(v3);
                                            mesh.AddVertex(v4);
                                            mesh.AddQuadTriangles();
                                        }
                                        else
                                        {
                                            // We specify them in reverse order to get a quad in the correct orientation
                                            mesh.AddVertex(v4);
                                            mesh.AddVertex(v3);
                                            mesh.AddVertex(v2);
                                            mesh.AddVertex(v1);
                                            mesh.AddQuadTriangles();
                                        }

                                        // Okay, now, since this quad is rendered, we don't want to include it again. So we remove it from the mask
                                        for (int l = 0; l < height; ++l)
                                        {
                                            for (int k = 0; k < width; ++k)
                                            {
                                                mask[n + k + l * chunkSz] = false;
                                            }
                                        }
                                        i += width; n += width; // And we skip ahead.
                                    }
                                    else
                                    {
                                        // If no voxel, we keep searching.
                                        ++i; ++n;
                                    }
                                }
                            }
                        }
                    }
                }
                vertIndex = mesh.vertices.Count;
                return mesh;
            }

        }
    }
}