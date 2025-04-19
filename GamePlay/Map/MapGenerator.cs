using NUnit.Framework;
using UnityEngine;
using Data;
using System.Collections.Generic;
using Utility;
using UnityEngine.Tilemaps;
using Cysharp.Threading.Tasks;
using static Codice.Client.BaseCommands.QueryParser;
namespace GamePlay
{
    /// <summary>
    /// ���� �������ִ� Ŭ����
    /// </summary>
    public class MapGenerator {

        private enum TempTileType {
            Ground,     // ��
            PathMarker  // ���
        }

        // --- �� ���� �Լ� ---
        /// <summary>
        /// �־��� ũ��� Ÿ�� ���潺 �� �����͸� ����
        /// </summary>
        /// <param name="sizeX">���� ���� ũ��</param>
        /// <param name="sizeY">���� ���� ũ��</param>
        /// <returns>������ �� Ÿ�� ������ ����Ʈ</returns>
        public List<MapData> GenerateMap(int sizeX, int sizeY, out List<Vector2Int> pathWaypointList) {
            pathWaypointList = new List<Vector2Int>();
            if (sizeX <= 0 || sizeY <= 0) {
                return new List<MapData>(); // �� ����Ʈ ��ȯ
            }

            // �ӽ� �׸��带 �����ϰ� �ʱ�ȭ
            TempTileType[,] tempGrid = new TempTileType[sizeX, sizeY];
            for (int x = 0; x < sizeX; x++) {
                for (int y = 0; y < sizeY; y++) {
                    tempGrid[x, y] = TempTileType.Ground;
                }
            }

            // ��� ������ ���� ������ ����

            pathWaypointList.Add(new Vector2Int(0, sizeY / 2));                 // ����: ���� �߾�
            pathWaypointList.Add(new Vector2Int(sizeX / 3, sizeY / 2));         // ���������� �̵�
            pathWaypointList.Add(new Vector2Int(sizeX / 3, sizeY / 4));         // �Ʒ��� �̵�
            pathWaypointList.Add(new Vector2Int(2 * sizeX / 3, sizeY / 4));     // ���������� �̵�
            pathWaypointList.Add(new Vector2Int(2 * sizeX / 3, 3 * sizeY / 4)); // ���� �̵�
            pathWaypointList.Add(new Vector2Int(sizeX - 1, 3 * sizeY / 4));     // ����: ������ ���� ��и�

            // Ȥ�� �� ������ ����� ������ ��ǥ�� �� ��� ���� ����
            for (int i = 0; i < pathWaypointList.Count; i++) {
                pathWaypointList[i] = new Vector2Int(
                    Mathf.Clamp(pathWaypointList[i].x, 0, sizeX - 1),
                    Mathf.Clamp(pathWaypointList[i].y, 0, sizeY - 1)
                );
            }

            // ������ ���̸� �������� ����
            for (int i = 0; i < pathWaypointList.Count - 1; i++) {
                Vector2Int start = pathWaypointList[i];
                Vector2Int end = pathWaypointList[i + 1];

                // �������� ���� ������ Ÿ���� ��η� ǥ��
                // ���� ��� �׸���
                if (start.y == end.y) {
                    int y = start.y;
                    int direction = (start.x < end.x) ? 1 : -1; // �̵� ���� (������: 1, ����: -1)

                    for (int x = start.x; x != end.x; x += direction) {
                        if (IsInBounds(x, y, sizeX, sizeY)) {
                            tempGrid[x, y] = TempTileType.PathMarker;
                        }
                    }
                }
                // ���� ��� �׸���
                else if (start.x == end.x) {
                    int x = start.x;
                    int direction = (start.y < end.y) ? 1 : -1; // �̵� ���� (��: 1, �Ʒ�: -1)
                                                                // ���������� ���� '�ٷ� ��'���� �ݺ�
                    for (int y = start.y; y != end.y; y += direction) {
                        if (IsInBounds(x, y, sizeX, sizeY)) {
                            tempGrid[x, y] = TempTileType.PathMarker;
                        }
                    }
                } else {
                    // �밢�� ��� ����
                    Debug.Log("�밢�� ���");
                }
                // end  ���� ���� ����
                if (IsInBounds(end.x, end.y, sizeX, sizeY)) {
                    tempGrid[end.x, end.y] = TempTileType.PathMarker;
                }
            }

            // �ӽ� �׸��带 ������� ���� Ÿ�� Ÿ�� ����
            TileType[,] finalGrid = new TileType[sizeX, sizeY];
            for (int x = 0; x < sizeX; x++) {
                for (int y = 0; y < sizeY; y++) {
                    // ���� Ÿ���� ��� ǥ�� Ÿ���� ���
                    if (tempGrid[x, y] == TempTileType.PathMarker) {
                        // �����¿� ���� Ÿ���� ������� Ȯ�� (��� �˻� ����)
                        bool hasUp = IsPath(tempGrid, x, y + 1, sizeX, sizeY);
                        bool hasDown = IsPath(tempGrid, x, y - 1, sizeX, sizeY);
                        bool hasLeft = IsPath(tempGrid, x - 1, y, sizeX, sizeY);
                        bool hasRight = IsPath(tempGrid, x + 1, y, sizeX, sizeY);

                        // ���� ���¿� ���� ���� ���� Ÿ�� Ÿ�� ����
                        finalGrid[x, y] = GetRoadTileType(hasUp, hasDown, hasLeft, hasRight);
                    } else // ��ΰ� �ƴ� Ÿ���� �״�� '��' Ÿ�� ����
                      {
                        finalGrid[x, y] = TileType.Ground;
                    }
                }
            }

            // ���� �׸��� ���� ��ȯ
            List<MapData> mapDataList = new List<MapData>(sizeX * sizeY);

            const int BASE_ORDER = 10000;
            const int ORDER_PER_ROW = 100;

            // ��ȸ
            for (int y = 0; y < sizeY; y++) {
                for (int x = 0; x < sizeX; x++) {
                    int calculatedOrder = BASE_ORDER - (y * ORDER_PER_ROW) + x;
                    MapData data = new() {
                        position = new Vector3((x * 1.25f) + (y * 1.25f), (y * 0.75f)- (x * 0.75f), 0),
                        type = finalGrid[x, y],
                        orderBy = calculatedOrder
                    };

                    mapDataList.Add(data); // ����Ʈ�� �߰�
                }
            }

            return mapDataList; // �ϼ��� �� ������ ����Ʈ ��ȯ
        }


        /// <summary>
        /// �־��� ��ǥ�� �� ���� ���� �ִ��� Ȯ��
        /// </summary>
        private bool IsInBounds(int x, int y, int sizeX, int sizeY) {
            return x >= 0 && x < sizeX && y >= 0 && y < sizeY;
        }

        /// <summary>
        /// �־��� �׸���� ��ǥ�� �ִ� Ÿ���� ��� ǥ������ Ȯ�� 
        /// </summary>
        private bool IsPath(TempTileType[,] grid, int x, int y, int sizeX, int sizeY) {
            // ���� ���� ���� �ִ��� Ȯ���ϰ�, �� ���� ��� ǥ������ Ȯ��
            return IsInBounds(x, y, sizeX, sizeY) && grid[x, y] == TempTileType.PathMarker;
        }

        /// <summary>
        /// �����¿� ���� ���¸� �������� ������ ���� Ÿ�� Ÿ���� ��ȯ
        /// </summary>
        private TileType GetRoadTileType(bool up, bool down, bool left, bool right) {
            // ����� ������ �� ���� ���
            int connections = (up ? 1 : 0) + (down ? 1 : 0) + (left ? 1 : 0) + (right ? 1 : 0);

            // ���� ������ ���� �б�
            switch (connections) {
                case 4: // 4���� ����: ����(+)
                return TileType.Road_Cross;

                case 3: // 3���� ����: T��
                if (!up) return TileType.Road_HorizontalDown; // ���ʸ� ���� '��' 
                if (!down) return TileType.Road_HorizontalUp;   // �Ʒ��ʸ� ���� '��' 
                if (!left) return TileType.Road_VerticalRight;  // ���ʸ� ���� '��' 
                if (!right) return TileType.Road_VerticalLeft; // �����ʸ� ���� '��'
                break; 

                case 2: // 2���� ����: ���� �Ǵ� �ڳ�
                if (up && down) return TileType.Road_Vertical;      // ���� ����  '|'
                if (left && right) return TileType.Road_Horizontal;  // �¿� ���� '��'
                if (up && right) return TileType.Road_RightUp;       // ��� ���� '��'
                if (up && left) return TileType.Road_LeftUp;        // ���� ���� '��'
                if (down && right) return TileType.Road_RightDown;     // �Ͽ� ���� '��'
                if (down && left) return TileType.Road_LeftDown;      // ���� ���� '��' 
                break;

                case 1: // 1���� ����: ����� �� ����
                        // �� ������ Ÿ�� ����� ��� ������ ������ ���� �����Դϴ�.
                        // ���⼭�� ����� ���⿡ ���� �ܼ��� ���� Ÿ�Ϸ� ǥ���մϴ�.
                if (up || down) return TileType.Road_Vertical; // �� �Ǵ� �Ʒ��� ����� ��� ���� ��
                if (left || right) return TileType.Road_Horizontal; // ���� �Ǵ� �����ʸ� ����� ��� ���� ��
                break;

                case 0: // 0���� ����: ���� ��� Ÿ��
                        // ���� �߻��Ѵٸ� �⺻ ���� �Ǵ� ������ ó��
                return TileType.Road_Horizontal; // ����: �⺻ ���� ���η� ó��
            }
            return TileType.Ground; // ���� �߻� �� �⺻ �� Ÿ�� ��ȯ
        }

        /// <summary>
        /// �ʵ����� ��� �� Instance
        /// </summary>
        public void InstanceMap(List<MapData> mapDataList, TileSpriteMapper tileMapper) {
            Debug.Log("instacnce");
            //DataManager.Instance.LoadAssetAsync<GameObject>("Field.prefab").ContinueWith(fieldPrefab => {
            //    var spriteDictionary = tileMapper.GetSpriteDictionary();
            //    foreach (MapData mapData in mapDataList) {
            //        GameObject obj = GameObject.Instantiate(fieldPrefab);
            //        var spriteRenderer = obj.GetComponent<SpriteRenderer>();
            //        spriteRenderer.sprite = spriteDictionary[mapData.type];
            //        spriteRenderer.sortingOrder = mapData.orderBy;
            //        obj.transform.position = mapData.position;
            //    }
            //});
        }
    }
}
