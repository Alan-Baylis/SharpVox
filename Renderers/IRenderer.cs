using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SharpVoxel
{
    namespace Renderers
    {
        public interface IRenderer
        {
            void Reset();
            MeshData Render(Chunk context, RenderQueue r);
        }

        public enum RenderType
        {
            STUPID,
            CULLED,
            MARCHCUBES,
            GREEDY
        }
    }
}
