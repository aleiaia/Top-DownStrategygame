using UnityEngine;
using Assets.GridMap.Utils;

namespace Assets.GridMap
{
    public abstract class GridChunkVisuals<TGridObject> : MonoBehaviour
    {

        public IGridChunk<TGridObject> gridChunk;

        private Mesh mesh;
        private bool updateMesh;
        public abstract void CreateCellMesh(Vector3[] vertices, Vector2[] uv, int[] triangles, int index, float cellSize, int x, int y);

        public void Inizialise()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        public void Awake()
        {
            Inizialise();
        }

        public void SetGrid(IGridChunk<TGridObject> grid)
        {
            this.gridChunk = grid;
            UpdateGridVisual();

            grid.SubscribeToOnGridValueChanged(Grid_OnGridValueChanged);
        }

        private void Grid_OnGridValueChanged(object sender, System.EventArgs e)
        {
            updateMesh = true;
        }

        private void LateUpdate()
        {
            if (updateMesh)
            {
                updateMesh = false;
                UpdateGridVisual();
            }
        }
        public void UpdateGridVisual()
        {
            Vector2Int chunkSizes = gridChunk.GetChunkSizes();
            int cellNum = chunkSizes.x * chunkSizes.y;
            MeshUtils.CreateEmptyMeshArrays(cellNum , out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

            float cellSize = gridChunk.GetCellSize();

            for (int x = 0; x < chunkSizes.x; x++)
            {
                for (int y = 0; y < chunkSizes.y; y++)
                {
                    int index = x * chunkSizes.y + y;
                    CreateCellMesh(vertices, uv, triangles, index, cellSize, x, y);
                }
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
        }
    }
}
