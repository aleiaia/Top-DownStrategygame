using UnityEngine;
using Assets.GridMap.Utils;

namespace Assets.GridMap
{
    public class MapCellGridVisuals3D : GridChunkVisuals<MapCell>
    {
        public override void CreateCellMesh(Vector3[] vertices, Vector2[] uv, int[] triangles, int index, float cellSize, int x, int y)
        {
            Vector3 baseSize = new Vector3(1, 1) * cellSize;
            Vector2 celluv = new Vector2(0f, 0f);
            float height = gridChunk.GetCell(x, y).height;
            MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, gridChunk.GetWorldPosition(x, y), height, baseSize, celluv, Vector2.zero);
        }
    }
}
