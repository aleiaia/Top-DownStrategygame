using System;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.GridMap
{
    public interface IChunkManager<TGridObject>
    {
        public Dictionary<Vector2Int, IGridChunk<TGridObject>> GetChunks();
        public void Update();
        public TGridObject GetCell(Vector3 worldPosition);
        public void SetGenerator(GridGenerator<TGridObject> generator);
    }
    public interface IGridChunk<TGridObject>
    {

        #region Get Methods
        public void generateChunk();
        public Vector2Int GetChunkSizes();
        public float GetCellSize();
        public TGridObject GetCell(int x, int y);
        public TGridObject GetCell(Vector3 worldPosition);
        public Vector3 GetWorldPosition(int x, int y);
        public List<Vector2Int> GetNeighborhood(int x, int y);

        #endregion

        #region Set Methods
        public void SetGenerator(Func<OnNewGridObjectGenerationArgs, TGridObject> createObject);
        public void SetCell(int x, int y, TGridObject value);
        public void SetCell(Vector3 worldPosition, TGridObject value);
        public void SubscribeToOnGridValueChanged(EventHandler<OnGridValueChangedArgs> func);
        #endregion
    }
    public class OnGridValueChangedArgs : EventArgs
    {
        public Vector2Int cellRelativeCoords;
    }

}