using UnityEngine;
using Assets.GridMap.Utils;


namespace Assets.GridMap
{
    public class MapCellGridHeightVisuals : GridChunkVisuals<MapCell>
    {
        public override void CreateCellMesh(Vector3[] vertices, Vector2[] uv, int[] triangles, int index, float cellSize, int x, int y)
        {
            Vector3 baseSize = new Vector3(1, 1) * cellSize;
            MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, gridChunk.GetWorldPosition(x, y), 0f, baseSize, Vector2.zero, Vector2.zero);
        }
    }

}
