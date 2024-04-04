using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Assets.GridMap
{
    public class ActionBasedHeightMapGenerator
    {
        ActionBasedHeightMapSettings settings;
        private List<HeightMapAction> actions;
        private List<AffectedArea> generatedArea;
        private int hillAmout;

        private int chunkWidth, chunkHeight;

        public ActionBasedHeightMapGenerator(ActionBasedHeightMapSettings settings)
        {
            this.settings = settings;
            actions = new List<HeightMapAction>();
            generatedArea = new List<AffectedArea>();
        }

        public void OnNewChunkGeneration(OnNewChunkGenerationArgs args)
        {
            chunkWidth = args.width;
            chunkHeight = args.height;
            Vector2Int center = new Vector2Int(args.chunkCoords.x * chunkWidth, args.chunkCoords.y * chunkHeight);
            int maxX = (args.chunkCoords.x + 1) * chunkWidth + settings.maxActionRange;
            int maxY = (args.chunkCoords.y + 1) * chunkHeight + settings.maxActionRange;
            int minX = (args.chunkCoords.x) * chunkWidth - settings.maxActionRange;
            int minY = (args.chunkCoords.y) * chunkHeight - settings.maxActionRange;
            AffectedArea chunkBorder = new AffectedArea(maxX, minX, maxY, minY);
            generatedArea.Add(chunkBorder);
            hillAmout = Random.Range(1, (int)((float)settings.HillDensity / 100f * chunkWidth * chunkHeight));
            CreateActions();
        }

        private void CreateActions()
        {
            foreach (var quad in generatedArea.Where(i => !i.isProcessed))
            {
                List<Vector2Int> points = quad.GetRandomPoints(hillAmout);
                bool toAdd = true;

                foreach (Vector2Int point in points)
                {
                    foreach (var otherQuad in generatedArea)
                    {
                        if (otherQuad != quad)
                        {
                            toAdd = toAdd && !otherQuad.IsInBoundaries(point);
                        }
                    }
                    if (toAdd)
                    {
                        actions.Add(new HeightMapAction(ActionType.Hill, point, settings.maxActionRange, Random.value * settings.maxHillHeight, settings.blobPower));
                    }
                    toAdd = true;
                }
                quad.isProcessed = true;
            }
        }

        public (float, float ) CalculateHeightOnPoint(OnNewGridObjectGenerationArgs args)
        {
            Vector2Int worldCoords = args.cellAbsoluteCoords;
            float h = 0;
            foreach (HeightMapAction action in actions)
            {
                if (action.IsInBoundaries(worldCoords))
                {
                    h += action.GetValue(worldCoords);
                }
            }
            return (h, h/settings.maxHeight);
        }
    }

    public class AffectedArea
    {
        public bool isProcessed;
        private Vector2Int center;
        private int maxX;
        private int minX;
        private int maxY;
        private int minY;

        public AffectedArea(int maxX, int minX, int maxY, int minY)
        {
            this.maxX = maxX;
            this.minX = minX;
            this.maxY = maxY;
            this.minY = minY;
            this.center = Vector2Int.zero;
            isProcessed = false;
        }
        public AffectedArea(Vector2Int center)
        {
            this.center = center;
            isProcessed = false;
        }
        public void SetBoundaries(int maxX, int minX, int maxY, int minY)
        {
            this.maxX = maxX;
            this.minX = minX;
            this.maxY = maxY;
            this.minY = minY;
        }
        public void UpdateBoundaries(Vector2Int point)
        {
            maxX = maxX > point.x ? maxX : point.x;
            minX = minX < point.x ? minX : point.x;
            maxY = maxY > point.y ? maxY : point.y;
            minY = minY < point.y ? minY : point.y;
        }
        public bool IsInBoundaries(Vector2Int point)
        {
            return
                point.x < center.x + maxX &&
                point.x > center.x + minX &&
                point.y <= center.y + maxY &&
                point.y >= center.y + minY;
        }
        public List<Vector2Int> GetRandomPoints(int count)
        {
            List<Vector2Int> points = new List<Vector2Int>();
            for (int i = 0; i < count; i++)
            {
                int x = Random.Range(minX, maxX);
                int y = Random.Range(minY, maxY);
                points.Add(new Vector2Int(x, y));
            }
            return points;
        }
    }
}