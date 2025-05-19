using UnityEngine;
using System.Collections.Generic;
namespace GamePlay
{
    public class ULinePathStrategy : IPathStrategy {
        public List<Vector2Int> CreatePathPoints(int x, int y) {
            List<Vector2Int> path = new();
            int midY = y / 2;

            path.Add(new Vector2Int(0, 0));             // 시작
            path.Add(new Vector2Int(x - 1, 0));         // 오른쪽 끝까지
            path.Add(new Vector2Int(x - 1, midY));      // 아래로
            path.Add(new Vector2Int(0, midY));          // 왼쪽 끝까지
            path.Add(new Vector2Int(0, y - 1));         // 아래로
            path.Add(new Vector2Int(x - 1, y - 1));     // 오른쪽 끝까지 (도착)

            return path;
        }
    }
}
