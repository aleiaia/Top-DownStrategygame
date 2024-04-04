using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GridMap.Utils
{
    public static class MeshUtils
    {
        public static void CreateEmptyMeshArrays(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)
        {
            vertices = new Vector3[4 * quadCount];
            uvs = new Vector2[4 * quadCount];
            triangles = new int[6 * quadCount];

        }

        public static void AddToMeshArrays(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 position, float height, Vector3 baseSize, Vector2 uv, Vector2 v2)
        {
            vertices[index * 4 + 0] = new Vector3(position.x, position.y, height);
            vertices[index * 4 + 1] = new Vector3(position.x, position.y + baseSize.y, height);
            vertices[index * 4 + 2] = new Vector3(position.x + baseSize.x, position.y + baseSize.y, height);
            vertices[index * 4 + 3] = new Vector3(position.x + baseSize.x, position.y, height);

            uvs[index * 4 + 0] = uv;
            uvs[index * 4 + 1] = uv;
            uvs[index * 4 + 2] = uv;
            uvs[index * 4 + 3] = uv;

            triangles[index * 6 + 0] = index * 4 + 0;
            triangles[index * 6 + 1] = index * 4 + 1;
            triangles[index * 6 + 2] = index * 4 + 2;

            triangles[index * 6 + 3] = index * 4 + 0;
            triangles[index * 6 + 4] = index * 4 + 2;
            triangles[index * 6 + 5] = index * 4 + 3;
        }
    }
}
