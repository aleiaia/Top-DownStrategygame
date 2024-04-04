using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GridMap
{
    public abstract class ChunkVisualManager<TGridObject> : MonoBehaviour
    {      
        public Material material;

        private IChunkManager<TGridObject> chunkManager;
        private Dictionary<Vector2Int, GridChunkVisuals<TGridObject>> gridVisual;
        private Transform parentTransform;

        public abstract GridChunkVisuals<TGridObject> CreateVisual(Vector2 ChunkXY, IGridChunk<TGridObject> chunk, Transform parentTransform, Material material);

        public ChunkVisualManager()
        {
            gridVisual = new Dictionary<Vector2Int, GridChunkVisuals<TGridObject>>();   
        }

        public void SetChunkManager(IChunkManager<TGridObject> chunkManager)
        {
            this.chunkManager = chunkManager;
        }

        public void SetMaterial(Material material)
        {
            this.material = material;
        }

        public void SetParentTransform(Transform parentTransform)
        {
            this.parentTransform = parentTransform;
        }

        public void ResetData()
        {
            if (gridVisual != null)
            {
                foreach (var chunkVisual in gridVisual)
                {
                    if (chunkVisual.Value != null && chunkVisual.Value.gameObject != null)
                    {
                        DestroyImmediate(chunkVisual.Value.gameObject);
                    }
                }
            }
            gridVisual = new Dictionary<Vector2Int, GridChunkVisuals<TGridObject>>();
        }

        public void UpdateGridVisuals()
        {
            foreach (var chunk in chunkManager.GetChunks())
            {
                if (!gridVisual.TryGetValue(chunk.Key, out GridChunkVisuals<TGridObject> chunkVisual) || chunkVisual == null)
                {
                    gridVisual.Remove(chunk.Key);
                    gridVisual.Add(chunk.Key, CreateVisual(chunk.Key, chunk.Value, parentTransform, material));
                }
            }
            foreach (var chunkVisual in gridVisual)
            {
                if (!chunkManager.GetChunks().TryGetValue(chunkVisual.Key, out IGridChunk<TGridObject> chunk))
                {
                    DestroyImmediate(chunkVisual.Value.gameObject);
                }
            }
        }


    }
}
