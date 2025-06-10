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
    }
}
