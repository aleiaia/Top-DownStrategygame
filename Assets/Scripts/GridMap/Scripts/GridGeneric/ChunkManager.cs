using System.Collections.Generic;
using UnityEngine;
using Assets.Utils;


namespace Assets.GridMap
{
    public class ChunkManager<TGridObject> : IChunkManager<TGridObject>
    {
        private GridGenerator<TGridObject> generator;

        private Dictionary<Vector2Int, IGridChunk<TGridObject>> gridChunks;
        private Vector2Int chunkSizes;
        private float cellSize;
        private Vector3 originPosition;
        private Camera camera;
        private float chunkWidth { get { return chunkSizes.x * cellSize; } }
        private float chunkHeight { get { return chunkSizes.y * cellSize; } }
        public ChunkManager(Camera camera, int width, int height, float cellSize, Vector3 originPosition)
        {
            this.camera = camera;
            chunkSizes = new Vector2Int(width, height);
            this.cellSize = cellSize;
            this.originPosition = originPosition;
            gridChunks = new Dictionary<Vector2Int, IGridChunk<TGridObject>>();
        }
        public ChunkManager(Camera camera, Vector2Int chunkSizes, float cellSize, Vector3 originPosition)
        {
            this.camera = camera;
            this.chunkSizes = chunkSizes;
            this.cellSize = cellSize;
            this.originPosition = originPosition;
            gridChunks = new Dictionary<Vector2Int, IGridChunk<TGridObject>>();
        }
        public void Update()
        {
            Vector3 Center = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 1f));
            Stack<Vector3> pointsToCheck = new Stack<Vector3>();
            Stack<Vector3> checkedPoints = new Stack<Vector3>();
            pointsToCheck.Push(Center);
            while (pointsToCheck.Count > 0)
            {
                Vector3 point = pointsToCheck.Pop();
                if (!checkedPoints.Contains(point))
                {
                    checkedPoints.Push(point);
                    Vector3[] chunkVertices = GetChunkVertices(point);
                    bool chunkIsVisible = false;
                    for (int i = 0; i < chunkVertices.Length; i++)
                    {
                        chunkIsVisible = chunkIsVisible || UtilsClass.CheckPointOnScreenLoose(chunkVertices[i]);
                    }
                    if (chunkIsVisible)
                    {
                        AddChunk(point);
                        pointsToCheck.Push(point + new Vector3(chunkWidth, 0));
                        pointsToCheck.Push(point + new Vector3(-chunkWidth, 0));
                        pointsToCheck.Push(point + new Vector3(0, chunkHeight));
                        pointsToCheck.Push(point + new Vector3(0, -chunkHeight));
                    }
                }
            }
        }
        private void AddChunk(Vector3 worldPosition)
        {
            Vector2Int chunkCoords = GetChunkCoords(worldPosition);
            generator.GenerateChunk(new OnNewChunkGenerationArgs(chunkCoords, chunkSizes.x, chunkSizes.y));
            if (!gridChunks.TryGetValue(chunkCoords, out IGridChunk<TGridObject> chunk))
            {
                Vector3 chunkOrigin = GetChunkOrigin(chunkCoords);
                Vector2Int chunkPosition = chunkCoords;
                if (generator != null)
                {
                    IGridChunk<TGridObject> currentChunk = new GridChunk<TGridObject>(chunkSizes, cellSize, chunkOrigin);
                    currentChunk.SetGenerator(i =>
                    {
                        i.SetChunkCoords(chunkCoords, chunkSizes);
                        return generator.GenerateCell(i);
                    });
                    currentChunk.generateChunk();
                    gridChunks.Add(chunkPosition, currentChunk);
                }
            }
        }
        #region Get Methods
        public TGridObject GetCell(Vector3 worldPosition)
        {
            Vector2Int chunkCoords = GetChunkCoords(worldPosition);
            if (gridChunks.TryGetValue(chunkCoords, out IGridChunk<TGridObject> chunk))
            {
                return chunk.GetCell(worldPosition);
            }
            return default;
        }
        private Vector3 GetChunkOrigin(Vector2Int point)
        {
            return GetChunkOrigin(point.x, point.y);
        }
        private Vector3 GetChunkOrigin(int x, int y)
        {
            return originPosition + new Vector3(x * chunkWidth, y * chunkHeight);
        }
        private Vector2Int GetChunkCoords(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt((worldPosition.x - originPosition.x) / chunkWidth);
            int y = Mathf.FloorToInt((worldPosition.y - originPosition.y) / chunkHeight);
            return new Vector2Int(x, y);
        }
        private Vector3[] GetChunkVertices(Vector3 worldPosition)
        {
            Vector2Int chunkCoords = GetChunkCoords(worldPosition);
            Vector3[] chunkVertices = new Vector3[4];
            chunkVertices[0] = GetChunkOrigin(chunkCoords);
            chunkVertices[1] = chunkVertices[0] + new Vector3(chunkWidth, 0);
            chunkVertices[2] = chunkVertices[0] + new Vector3(0, chunkHeight);
            chunkVertices[3] = chunkVertices[0] + new Vector3(chunkWidth, chunkHeight);
            return chunkVertices;
        }
        public Dictionary<Vector2Int, IGridChunk<TGridObject>> GetChunks()
        {
            return gridChunks;
        }
        #endregion
        #region Set Methods
        public void SetGenerator(GridGenerator<TGridObject> generator)
        {
            this.generator = generator;
        }
        public void SetCell(Vector3 worldPosition, TGridObject value)
        {
            Vector2Int chunkCoords = GetChunkCoords(worldPosition);
            if (gridChunks.TryGetValue(chunkCoords, out IGridChunk<TGridObject> chunk))
            {
                chunk.SetCell(worldPosition, value);
            }
        }
        #endregion
    }

    public class OnNewChunkGenerationArgs
    {
        public Vector2Int chunkCoords;
        public int height, width;
        public OnNewChunkGenerationArgs(Vector2Int chunkCoords, int width, int height)
        {
            this.chunkCoords = chunkCoords;
            this.height = height;
            this.width = width;
        }
    }
}