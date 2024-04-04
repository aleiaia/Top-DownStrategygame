using UnityEngine;
using Assets.GridMap.Utils;


namespace Assets.GridMap
{
    public class MapCellGridVisuals : GridChunkVisuals<MapCell>
    {
        public Vector2 GetCellUVs(MapCell cell)
        {
            float h = cell.heightNormalised;
            return new Vector2(h, h);
        }
        public override void CreateCellMesh(Vector3[] vertices, Vector2[] uv, int[] triangles, int index, float cellSize, int x, int y)
        {
            Vector3 baseSize = new Vector3(1, 1) * cellSize;
            Vector2 celluv = GetCellUVs(gridChunk.GetCell(x, y));
            MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, gridChunk.GetWorldPosition(x, y), 0f, baseSize, celluv, Vector2.zero);
        }
    }
}