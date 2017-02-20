using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SharpVoxel
{
    public class MeshData
    {
        public List<Vector3> vertices = new List<Vector3>();
        public List<int> triangles = new List<int>();
        public List<Vector2> uv = new List<Vector2>();

        public int startingIndex = 0;

        public void AddVertex(Vector3 vert)
        {
            vertices.Add(vert);
        }

        public void AddTriangle(int tri)
        {
            triangles.Add(startingIndex + tri);
        }

        public void AddQuadTriangles()
        {
            AddTriangle(vertices.Count - 4);
            AddTriangle(vertices.Count - 3);
            AddTriangle(vertices.Count - 2);
            AddTriangle(vertices.Count - 4);
            AddTriangle(vertices.Count - 2);
            AddTriangle(vertices.Count - 1);
        }
    }
}
