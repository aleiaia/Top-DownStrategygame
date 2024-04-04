using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.GridMap
{

    public class GridChunk<TGridObject> : IGridChunk<TGridObject>
    {
        public Vector2Int chunkSizes { get; private set; }
        public float cellSize { get; private set; }
        private Vector3 originPosition;
        //private TGridObject[,] gridArray;
        private TGridObject[] gridArray;
        private Vector3[] cellPosition;
        private Dictionary<int, List<int>> connectionMap;

        public GridChunk(int width, int height, float cellSize, Vector3 originPosition)
        {
            chunkSizes = new Vector2Int(width, height);
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new TGridObject[chunkSizes.x * chunkSizes.y];
        }
        public GridChunk(Vector2Int chunkSizes, float cellSize, Vector3 originPosition)
        {
            this.chunkSizes = chunkSizes;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new TGridObject[chunkSizes.x * chunkSizes.y];
        }

        public void generateChunk()
        {
            for (int x = 0; x < chunkSizes.x; x++)
            {
                for (int y = 0; y < chunkSizes.y; y++)
                {
                    gridArray[x * chunkSizes.y + y] = createObject(new OnNewGridObjectGenerationArgs(new Vector2Int(x, y)));
                }
            }
        }

        #region set methods
        public void SetCell(int x, int y, TGridObject value)
        {
            if (x >= 0 && y >= 0 && x < chunkSizes.x && y < chunkSizes.y)
            {
                gridArray[x * chunkSizes.y + y] = value;
                if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedArgs { cellRelativeCoords = new Vector2Int(x, y) });
            }
        }
        public void SetCell(Vector3 worldPosition, TGridObject value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetCell(x, y, value);
        }
        public void SetGenerator(Func<OnNewGridObjectGenerationArgs, TGridObject> createObject)
        {
            this.createObject = createObject;
        }
        #endregion

        #region get methods
        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize + originPosition;
        }
        public void GetXY(Vector3 worldPosistion, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosistion.x - originPosition.x) / cellSize);
            y = Mathf.FloorToInt((worldPosistion.y - originPosition.y) / cellSize);
        }
        public TGridObject GetCell(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < chunkSizes.x && y < chunkSizes.y)
            {
                return gridArray[x * chunkSizes.y + y];
            }
            else
            {
                return default;
            }
        }
        public TGridObject GetCell(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetCell(x, y);
        }
        public Vector2Int GetChunkSizes()
        {
            return chunkSizes;
        }
        public float GetCellSize()
        {
            return cellSize;
        }
        public List<Vector2Int> GetNeighborhood(int x, int y)
        {
            return GetNeighborhood(new Vector2Int(x, y));
        }

        public List<Vector2Int> GetNeighborhood(Vector2Int coords)
        {
            List<Vector2Int> neigborhood = new List<Vector2Int>();
            foreach (var dir in neighborOffset)
            {
                neigborhood.Add(dir.Value + coords);
            }
            return neigborhood;
        }
        #endregion

        public void SubscribeToOnGridValueChanged(EventHandler<OnGridValueChangedArgs> func)
        {
            OnGridValueChanged += func;
        }
        public event EventHandler<OnGridValueChangedArgs> OnGridValueChanged;

        public Func<OnNewGridObjectGenerationArgs, TGridObject> createObject;

        private enum Direction
        {
            Up, Down, Left, Right
        }

        private Dictionary<Direction, Vector2Int> neighborOffset = new Dictionary<Direction, Vector2Int> {
            {  Direction.Up , new Vector2Int(0,1) },
            {  Direction.Left, new Vector2Int(1,0) },
            {  Direction.Right, new Vector2Int(-1,0) },
            {  Direction.Down , new Vector2Int(0,-1) }
        };
    }



    public class OnNewGridObjectGenerationArgs
    {
        //public Vector3 worldPosition;
        public Vector2Int cellRelativeCoords;
        public Vector2Int chunkCoords;
        public Vector2Int chunkSizes;
        public Func<List<Vector2Int>> getNeighborhood;
        public Vector2Int cellAbsoluteCoords
        {
            get
            {
                if (chunkCoords != null && chunkSizes != null)
                {
                    Vector2Int chunkOrigin = chunkCoords;
                    chunkOrigin.Scale(chunkSizes);
                    return cellRelativeCoords + chunkOrigin;
                }
                return default;
            }
        }

        public OnNewGridObjectGenerationArgs(Vector2Int cellRelativeCoords)
        {
            this.cellRelativeCoords = cellRelativeCoords;
        }

        public void SetChunkCoords(Vector2Int chunkCoords, Vector2Int chunkSizes)
        {
            this.chunkCoords = chunkCoords;
            this.chunkSizes = chunkSizes;
        }


    }
}