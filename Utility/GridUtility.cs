using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace CustomUtility
{
    public static class GridUtility
    {
        /// <summary>
        /// 오브젝트 이미지에 맞춰 위치를 변경해주는 함수
        /// </summary>
        public static Vector3 GridToWorldPosition(int x, int y) {
            return new Vector3((x * 1.25f) + (y * 1.25f), (y * 0.75f) - (x * 0.75f), 0);
        }
        /// <summary>
        /// world 좌표를 grid 좌표로 변환
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public static Vector2Int WorldToGridPosition(Vector2 worldPos) {
            float p = worldPos.x / 1.25f;   // = x + y
            float q = worldPos.y / 0.75f;   // = y - x

            float fx = (p - q) * 0.5f;      // 실수 좌표
            float fy = (p + q) * 0.5f;

            int x = Mathf.RoundToInt(fx);
            int y = Mathf.RoundToInt(fy);

            return new Vector2Int(x, y);
        }

        /// <summary>
        /// a -> b 중간 grid 생성
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static IEnumerable<Vector2Int> StepCells(Vector2Int a, Vector2Int b) {
            int dx = Math.Sign(b.x - a.x);   // -1, 0, +1
            int dy = Math.Sign(b.y - a.y);   // -1, 0, +1
            int len = Mathf.Abs(b.x - a.x) + Mathf.Abs(b.y - a.y);

            // 생성
            return Enumerable.Range(1, len).Select(i => new Vector2Int(a.x + dx * i, a.y + dy * i));
        }
    }
}
