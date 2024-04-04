using System.Collections;
using UnityEngine;
using System;

namespace Assets.GridMap
{
    public abstract class GridGenerator<TGridObject>
    {
        IChunkManager<TGridObject> chunkManager;
        public void SetChunkManager(IChunkManager<TGridObject> chunkManager)
        {
            this.chunkManager = chunkManager;
        }
        public abstract TGridObject GenerateCell(OnNewGridObjectGenerationArgs args);
        public abstract void GenerateChunk( OnNewChunkGenerationArgs args);
    }
}