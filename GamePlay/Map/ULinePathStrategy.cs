using UnityEngine;
using System.Collections.Generic;
namespace GamePlay
{
    public class ULinePathStrategy : IPathStrategy {
        public List<Vector2Int> CreatePathPoints(int x, int y) {
            List<Vector2Int> path = new();
            int midY = y / 2;

            path.Add(new Vector2Int(0, 0));             // ����
            path.Add(new Vector2Int(x - 1, 0));         // ������ ������
            path.Add(new Vector2Int(x - 1, midY));      // �Ʒ���
            path.Add(new Vector2Int(0, midY));          // ���� ������
            path.Add(new Vector2Int(0, y - 1));         // �Ʒ���
            path.Add(new Vector2Int(x - 1, y - 1));     // ������ ������ (����)

            return path;
        }
    }
}
