using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpVoxel;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public partial class Chunk : MonoBehaviour {
    /* Parameters */
    public int chunkSize = 16; // This is not const. It is adjustable based on the world.
    public float scaleFactor = 1;
    public bool collidable = true;
    public VoxelPosition position;
    // Voxel Data
    private VoxelData[] voxels;
    private int voxShiftY;
    private int voxShiftZ;

    // Mesh stuff
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private MeshRenderer meshRenderer;
    private bool needsUpdate = false;
    
    [HideInInspector]
    public Map map;
    private SharpVoxel.Renderers.IRenderer chunkRenderer;

    public SharpVoxel.Renderers.RenderType type = SharpVoxel.Renderers.RenderType.GREEDY;
    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        voxels = new VoxelData[chunkSize * chunkSize * chunkSize];
        voxShiftZ = chunkSize / 2;
        voxShiftY = voxShiftZ / 2;
        switch (type)
        {
            case SharpVoxel.Renderers.RenderType.STUPID:
                chunkRenderer = new SharpVoxel.Renderers.StupidRenderer();
                break;
            case SharpVoxel.Renderers.RenderType.CULLED:
                chunkRenderer = new SharpVoxel.Renderers.CulledRenderer();
                break;
            case SharpVoxel.Renderers.RenderType.MARCHCUBES:
                chunkRenderer = new SharpVoxel.Renderers.MarchingCubesRenderer();
                break;
            case SharpVoxel.Renderers.RenderType.GREEDY:
                chunkRenderer = new SharpVoxel.Renderers.GreedyRenderer();
                break;
        }
    }

    void Start()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {   
                for (int z = 0; z < chunkSize; z++)
                {
                    this[x, y, z] = new VoxelData(1);
                }
            }
        }


        for (int x = 2; x < chunkSize - 2; x++)
        {
            for (int y = 2; y < chunkSize - 2; y++)
            {
                for (int z = 2; z < chunkSize - 2; z++)
                {
                    this[x, y, z] = new VoxelData(0);
                }
            }
        }

        for (int x = 6; x < chunkSize - 6; x++)
        {
            for (int y = chunkSize - 2; y < chunkSize; y++)
            {
                for (int z = 6; z < chunkSize - 6; z++)
                {
                    this[x, y, z] = new VoxelData(0);
                }
            }
        }
        this[8, 15, 8] = new VoxelData(2);
        this[8, 14, 8] = new VoxelData(2);
        this[8, 13, 8] = new VoxelData(2);
        this[8, 12, 8] = new VoxelData(2);
        Render();
    }
    
    void Update()
    {
        if(needsUpdate)
        {
            Render();
        }
    }

    public bool InRange(int index) { return !(index < 0 || index >= chunkSize); }

    void Render()
    {
        needsUpdate = false;
        chunkRenderer.Reset();
        meshFilter.mesh.Clear();

        // Calculate data
        ChunkRenderData r = new ChunkRenderData(this);
        for(int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    r.Add(this[x, y, z], x, y, z);
                }
            }
        }

        // Start rendering the mesh
        List<Vector3> verts = new List<Vector3>();
        List<MeshData> genMesh = new List<MeshData>();
        int i = 0;


		List<int> voxelOrder = new List<int> ();

        foreach (RenderQueue q in r.queues)
        {
			voxelOrder.Add(q.id);
            MeshData m = chunkRenderer.Render(this, r.queues[i++]);
            verts.AddRange(m.vertices);
            genMesh.Add(m);


        }
        meshFilter.mesh.subMeshCount = i;
        meshFilter.mesh.vertices = verts.ToArray();
        i = 0;

        // We have to have all verts so we can assign tris
        foreach (MeshData m in genMesh)
        {
            meshFilter.mesh.SetTriangles(m.triangles, i++);
        }

        meshFilter.mesh.RecalculateNormals();
        if(collidable)
        {
            meshCollider.sharedMesh = meshFilter.mesh;
        }
        else
        {
            meshCollider.sharedMesh = null;
        }
		i = 0;
		// Now, lets set the materials

		Material[] matlist = new Material[r.queues.Count];

		foreach (int vox in voxelOrder) {
			matlist[i++] = map.mapMaterials.getFromBlockId(vox);
		}

		meshRenderer.materials = matlist;


    }

}
