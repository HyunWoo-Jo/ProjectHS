using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay {

    public interface IPathStrategy {
        List<Vector2Int> CreatePathPoints(int x, int y);
    }

    /// <summary>
    /// S �� ������� Path�� ����
    /// </summary>
    public class SLinePathStrategy : IPathStrategy {
        public List<Vector2Int> CreatePathPoints(int x, int y) {
            List<Vector2Int> pathWaypointList = new ();
            pathWaypointList.Add(new Vector2Int(0, y / 2));                 // ����: ���� �߾�
            pathWaypointList.Add(new Vector2Int(x / 3, y / 2));         // ���������� �̵�
            pathWaypointList.Add(new Vector2Int(x / 3, y / 4));         // �Ʒ��� �̵�
            pathWaypointList.Add(new Vector2Int(2 * x / 3, y / 4));     // ���������� �̵�
            pathWaypointList.Add(new Vector2Int(2 * x / 3, 3 * y / 4)); // ���� �̵�
            pathWaypointList.Add(new Vector2Int(x - 1, 3 * y / 4));     // ����: ������ ���� ��и�

            // Ȥ�� �� ������ ����� ��ǥ�� �� ��� ���� ����
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
