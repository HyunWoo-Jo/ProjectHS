using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay {

    public interface IPathStrategy {
        List<Vector2Int> CreatePathPoints(int x, int y);
    }

    /// <summary>
    /// S 자 모양으로 Path를 생성
    /// </summary>
    public class SLinePathStrategy : IPathStrategy {
        public List<Vector2Int> CreatePathPoints(int x, int y) {
            List<Vector2Int> pathWaypointList = new ();
            pathWaypointList.Add(new Vector2Int(0, y / 2));                 // 시작: 왼쪽 중앙
            pathWaypointList.Add(new Vector2Int(x / 3, y / 2));         // 오른쪽으로 이동
            pathWaypointList.Add(new Vector2Int(x / 3, y / 4));         // 아래로 이동
            pathWaypointList.Add(new Vector2Int(2 * x / 3, y / 4));     // 오른쪽으로 이동
            pathWaypointList.Add(new Vector2Int(2 * x / 3, 3 * y / 4)); // 위로 이동
            pathWaypointList.Add(new Vector2Int(x - 1, 3 * y / 4));     // 도착: 오른쪽 위쪽 사분면

            // 혹시 모를 오류를 대비해 좌표를 맵 경계 내로 제한
            for (int i = 0; i < pathWaypointList.Count; i++) {
                pathWaypointList[i] = new Vector2Int(
                    Mathf.Clamp(pathWaypointList[i].x, 0, x - 1),
                    Mathf.Clamp(pathWaypointList[i].y, 0, y - 1)
                );
            }

            return pathWaypointList;
        }
    }
}
