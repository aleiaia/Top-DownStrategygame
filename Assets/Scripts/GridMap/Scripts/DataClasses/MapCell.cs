using UnityEngine;

namespace Assets.GridMap
{
    public class MapCell
    {

        public float height;
        public float heightNormalised;

        public void SetHeight(float h)
        {
            this.height = h;
        }

        public override string ToString()
        {
            return height.ToString();
        }
    }
}