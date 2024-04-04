using System.Collections.Generic;
using UnityEngine;

namespace Assets.GridMap
{
    public enum ActionType
    {
        Hill,
        Pit,
        Range,
    }
    public class HeightMapAction
    {

        public Dictionary<Vector2Int, float> height;
        public Vector2Int center;

        private AffectedArea actionBoredrs;


        public HeightMapAction(ActionType type, Vector2Int center, int maxSize, float startHeight, float power)
        {
            this.center = center;
            this.height = new Dictionary<Vector2Int, float>();
            actionBoredrs = new AffectedArea(center);

            switch (type)
            {
                case ActionType.Hill:
                    AddOneHill(maxSize, startHeight, power);
                    break;
                case ActionType.Pit:
                    AddOnePit(maxSize, startHeight, power);
                    break;
                case ActionType.Range:
                    break;
                default:
                    break;
            }
        }

        public float GetValue(Vector2Int point)
        {
            if (height.TryGetValue(point - center, out float h))
            {
                return h;
            }
            return 0;
        }

        public bool IsInBoundaries(Vector2Int point)
        {
            return actionBoredrs.IsInBoundaries(point);
        }

        private void AddOneHill(int maxSize, float startHeight, float power)
        {
            Vector2Int start = Vector2Int.zero;
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            height.Add(start, startHeight);
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                Vector2Int previus = queue.Dequeue();
                height.TryGetValue(previus, out float h);
                foreach (var offset in neighborOffset.Values)
                {
                    Vector2Int current = previus + offset;
                    if (current.x < maxSize && current.y < maxSize && current.x > -maxSize && current.y > -maxSize)
                    {
                        if (!height.TryGetValue(current, out float old_h))
                        {
                            float currentHeight = Mathf.Pow(h, power) * (Random.value * 0.2f + 0.85f);
                            height.Add(current, currentHeight);
                            actionBoredrs.UpdateBoundaries(current);
                            if (currentHeight > 2f) { queue.Enqueue(current); }
                        }
                        else
                        {
                            height[current] = (old_h + Mathf.Pow(h, power) * (Random.value * 0.2f + 0.85f)) / 2f;
                        }
                    }
                }
            }
        }

        private void AddOnePit(int maxSize, float startDepth, float power)
        {
            Vector2Int start = Vector2Int.zero;
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            float h = startDepth;
            height.Add(start, h);

            while (queue.Count > 0)
            {
                Vector2Int previus = queue.Dequeue();
                h = Mathf.Pow(h, power) * (Random.value * 0.2f + 0.85f);
                if (h > 2f)
                {
                    foreach (var offset in neighborOffset.Values)
                    {
                        Vector2Int current = previus + offset;
                        if (!height.TryGetValue(current, out float old_h))
                        {
                            float currentHeight = (-h) * (Random.value * 0.2f + 0.85f);
                            height.Add(current, currentHeight);
                            queue.Enqueue(current);
                        }
                        else
                        {
                            height[current] = -(-old_h + h * (Random.value * 0.2f + 0.85f)) / 2f;
                        }
                    }
                }

                height.TryGetValue(previus, out _);
            }
        }

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
}