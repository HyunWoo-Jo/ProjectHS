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
    /// 맵을 생성해주는 클레스
    /// </summary>
    public class MapGenerator {

        private enum TempTileType {
            Ground,     // 땅
            PathMarker  // 경로
        }

        // --- 맵 생성 함수 ---
        /// <summary>
        /// 주어진 크기로 타워 디펜스 맵 데이터를 생성
        /// </summary>
        /// <param name="sizeX">맵의 가로 크기</param>
        /// <param name="sizeY">맵의 세로 크기</param>
        /// <returns>생성된 맵 타일 데이터 리스트</returns>
        public List<MapData> GenerateMap(int sizeX, int sizeY, out List<Vector2Int> pathWaypointList) {
            pathWaypointList = new List<Vector2Int>();
            if (sizeX <= 0 || sizeY <= 0) {
                return new List<MapData>(); // 빈 리스트 반환
            }

            // 임시 그리드를 생성하고 초기화
            TempTileType[,] tempGrid = new TempTileType[sizeX, sizeY];
            for (int x = 0; x < sizeX; x++) {
                for (int y = 0; y < sizeY; y++) {
                    tempGrid[x, y] = TempTileType.Ground;
                }
            }

            // 경로 생성을 위한 경유지 정의

            pathWaypointList.Add(new Vector2Int(0, sizeY / 2));                 // 시작: 왼쪽 중앙
            pathWaypointList.Add(new Vector2Int(sizeX / 3, sizeY / 2));         // 오른쪽으로 이동
            pathWaypointList.Add(new Vector2Int(sizeX / 3, sizeY / 4));         // 아래로 이동
            pathWaypointList.Add(new Vector2Int(2 * sizeX / 3, sizeY / 4));     // 오른쪽으로 이동
            pathWaypointList.Add(new Vector2Int(2 * sizeX / 3, 3 * sizeY / 4)); // 위로 이동
            pathWaypointList.Add(new Vector2Int(sizeX - 1, 3 * sizeY / 4));     // 도착: 오른쪽 위쪽 사분면

            // 혹시 모를 오류를 대비해 경유지 좌표를 맵 경계 내로 제한
            for (int i = 0; i < pathWaypointList.Count; i++) {
                pathWaypointList[i] = new Vector2Int(
                    Mathf.Clamp(pathWaypointList[i].x, 0, sizeX - 1),
                    Mathf.Clamp(pathWaypointList[i].y, 0, sizeY - 1)
                );
            }

            // 경유지 사이를 직선으로 연결
            for (int i = 0; i < pathWaypointList.Count - 1; i++) {
                Vector2Int start = pathWaypointList[i];
                Vector2Int end = pathWaypointList[i + 1];

                // 시작점과 끝점 사이의 타일을 경로로 표시
                // 가로 경로 그리기
                if (start.y == end.y) {
                    int y = start.y;
                    int direction = (start.x < end.x) ? 1 : -1; // 이동 방향 (오른쪽: 1, 왼쪽: -1)

                    for (int x = start.x; x != end.x; x += direction) {
                        if (IsInBounds(x, y, sizeX, sizeY)) {
                            tempGrid[x, y] = TempTileType.PathMarker;
                        }
                    }
                }
                // 세로 경로 그리기
                else if (start.x == end.x) {
                    int x = start.x;
                    int direction = (start.y < end.y) ? 1 : -1; // 이동 방향 (위: 1, 아래: -1)
                                                                // 시작점에서 끝점 '바로 전'까지 반복
                    for (int y = start.y; y != end.y; y += direction) {
                        if (IsInBounds(x, y, sizeX, sizeY)) {
                            tempGrid[x, y] = TempTileType.PathMarker;
                        }
                    }
                } else {
                    // 대각선 경로 예외
                    Debug.Log("대각선 경로");
                }
                // end  최종 구역 설정
                if (IsInBounds(end.x, end.y, sizeX, sizeY)) {
                    tempGrid[end.x, end.y] = TempTileType.PathMarker;
                }
            }

            // 임시 그리드를 기반으로 최종 타일 타입 결정
            TileType[,] finalGrid = new TileType[sizeX, sizeY];
            for (int x = 0; x < sizeX; x++) {
                for (int y = 0; y < sizeY; y++) {
                    // 현재 타일이 경로 표시 타일인 경우
                    if (tempGrid[x, y] == TempTileType.PathMarker) {
                        // 상하좌우 인접 타일이 경로인지 확인 (경계 검사 포함)
                        bool hasUp = IsPath(tempGrid, x, y + 1, sizeX, sizeY);
                        bool hasDown = IsPath(tempGrid, x, y - 1, sizeX, sizeY);
                        bool hasLeft = IsPath(tempGrid, x - 1, y, sizeX, sizeY);
                        bool hasRight = IsPath(tempGrid, x + 1, y, sizeX, sizeY);

                        // 연결 상태에 따라 최종 도로 타일 타입 결정
                        finalGrid[x, y] = GetRoadTileType(hasUp, hasDown, hasLeft, hasRight);
                    } else // 경로가 아닌 타일은 그대로 '땅' 타입 유지
                      {
                        finalGrid[x, y] = TileType.Ground;
                    }
                }
            }

            // 최종 그리드 정보 변환
            List<MapData> mapDataList = new List<MapData>(sizeX * sizeY);

            const int BASE_ORDER = 10000;
            const int ORDER_PER_ROW = 100;

            // 순회
            for (int y = 0; y < sizeY; y++) {
                for (int x = 0; x < sizeX; x++) {
                    int calculatedOrder = BASE_ORDER - (y * ORDER_PER_ROW) + x;
                    MapData data = new() {
                        position = new Vector3((x * 1.25f) + (y * 1.25f), (y * 0.75f)- (x * 0.75f), 0),
                        type = finalGrid[x, y],
                        orderBy = calculatedOrder
                    };

                    mapDataList.Add(data); // 리스트에 추가
                }
            }

            return mapDataList; // 완성된 맵 데이터 리스트 반환
        }


        /// <summary>
        /// 주어진 좌표가 맵 범위 내에 있는지 확인
        /// </summary>
        private bool IsInBounds(int x, int y, int sizeX, int sizeY) {
            return x >= 0 && x < sizeX && y >= 0 && y < sizeY;
        }

        /// <summary>
        /// 주어진 그리드와 좌표에 있는 타일이 경로 표시인지 확인 
        /// </summary>
        private bool IsPath(TempTileType[,] grid, int x, int y, int sizeX, int sizeY) {
            // 먼저 범위 내에 있는지 확인하고, 그 다음 경로 표시인지 확인
            return IsInBounds(x, y, sizeX, sizeY) && grid[x, y] == TempTileType.PathMarker;
        }

        /// <summary>
        /// 상하좌우 연결 상태를 바탕으로 적절한 도로 타일 타입을 반환
        /// </summary>
        private TileType GetRoadTileType(bool up, bool down, bool left, bool right) {
            // 연결된 방향의 총 개수 계산
            int connections = (up ? 1 : 0) + (down ? 1 : 0) + (left ? 1 : 0) + (right ? 1 : 0);

            // 연결 개수에 따라 분기
            switch (connections) {
                case 4: // 4방향 연결: 십자(+)
                return TileType.Road_Cross;

                case 3: // 3방향 연결: T자
                if (!up) return TileType.Road_HorizontalDown; // 위쪽만 없음 'ㅜ' 
                if (!down) return TileType.Road_HorizontalUp;   // 아래쪽만 없음 'ㅗ' 
                if (!left) return TileType.Road_VerticalRight;  // 왼쪽만 없음 'ㅏ' 
                if (!right) return TileType.Road_VerticalLeft; // 오른쪽만 없음 'ㅓ'
                break; 

                case 2: // 2방향 연결: 직선 또는 코너
                if (up && down) return TileType.Road_Vertical;      // 상하 연결  '|'
                if (left && right) return TileType.Road_Horizontal;  // 좌우 연결 '─'
                if (up && right) return TileType.Road_RightUp;       // 상우 연결 'ㄴ'
                if (up && left) return TileType.Road_LeftUp;        // 상좌 연결 '┘'
                if (down && right) return TileType.Road_RightDown;     // 하우 연결 '┌'
                if (down && left) return TileType.Road_LeftDown;      // 하좌 연결 'ㄱ' 
                break;

                case 1: // 1방향 연결: 경로의 끝 지점
                        // 끝 지점의 타일 모양을 어떻게 할지는 디자인 결정 사항입니다.
                        // 여기서는 연결된 방향에 맞춰 단순한 직선 타일로 표시합니다.
                if (up || down) return TileType.Road_Vertical; // 위 또는 아래만 연결된 경우 세로 길
                if (left || right) return TileType.Road_Horizontal; // 왼쪽 또는 오른쪽만 연결된 경우 가로 길
                break;

                case 0: // 0방향 연결: 고립된 경로 타일
                        // 만약 발생한다면 기본 도로 또는 땅으로 처리
                return TileType.Road_Horizontal; // 예시: 기본 가로 도로로 처리
            }
            return TileType.Ground; // 오류 발생 시 기본 땅 타일 반환
        }

        /// <summary>
        /// 맵데이터 기반 맵 Instance
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
