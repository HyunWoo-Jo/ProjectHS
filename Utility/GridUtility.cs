using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace CustomUtility
{
    public static class GridUtility
    {
        /// <summary>
        /// ������Ʈ �̹����� ���� ��ġ�� �������ִ� �Լ�
        /// </summary>
        public static Vector3 GridToWorldPosition(int x, int y) {
            return new Vector3((x * 1.25f) + (y * 1.25f), (y * 0.75f) - (x * 0.75f), 0);
        }
        /// <summary>
        /// world ��ǥ�� grid ��ǥ�� ��ȯ
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public static Vector2Int WorldToGridPosition(Vector2 worldPos) {
            float p = worldPos.x / 1.25f;   // = x + y
            float q = worldPos.y / 0.75f;   // = y - x

            float fx = (p - q) * 0.5f;      // �Ǽ� ��ǥ
            float fy = (p + q) * 0.5f;

            int x = Mathf.RoundToInt(fx);
            int y = Mathf.RoundToInt(fy);

            return new Vector2Int(x, y);
        }

        /// <summary>
        /// a -> b �߰� grid ����
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static IEnumerable<Vector2Int> StepCells(Vector2Int a, Vector2Int b) {
            int dx = Math.Sign(b.x - a.x);   // -1, 0, +1
            int dy = Math.Sign(b.y - a.y);   // -1, 0, +1
            int len = Mathf.Abs(b.x - a.x) + Mathf.Abs(b.y - a.y);

            // ����
            return Enumerable.Range(1, len).Select(i => new Vector2Int(a.x + dx * i, a.y + dy * i));
        }
    }
}
