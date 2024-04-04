using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.GridMap
{
    public class MapCellGridVisualManager : ChunkVisualManager<MapCell>
    {
        public override GridChunkVisuals<MapCell> CreateVisual(Vector2 ChunkXY, IGridChunk<MapCell> chunk, Transform parentTransform, Material material)
        {
            GameObject obj = new GameObject($"Chunk {ChunkXY.x},{ChunkXY.y} ", typeof(MeshFilter));
            obj.transform.parent = parentTransform;
            MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            MapCellGridVisuals chunkVisual = obj.AddComponent<MapCellGridVisuals>();
            chunkVisual.Inizialise();
            chunkVisual.SetGrid(chunk);
            
            return chunkVisual;
        }
    }
}
