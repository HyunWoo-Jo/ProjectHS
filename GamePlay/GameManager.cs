using Data;
using GamePlay;
using System;
using System.Linq;
using System.Collections.Generic;   
using UnityEngine;
using CustomUtility;


namespace GamePlay {
    /*
    게임 상태 관리	    로딩 중 / 게임 중 / 일시정지 / 클리어 등
    씬 전환 처리	    씬 이동 요청 처리, Transition UI 연결 등
    게임 시작/끝 처리	게임 초기화, 시작 조건 체크, 클리어/실패 조건 체크
    데이터 로딩	        SaveData, PlayerData 등 불러오기
    */
    public class GameManager : MonoBehaviour {
        public enum GameState {
            Running,
            Pause,
            Clear,
            Loading,
        }

        public GameState State { get; private set; }
        public MapTema MapTema { get; private set; }

        private MapGenerator _mapGenerator;

        /// <summary>
        /// 게임 상태 변경
        /// </summary>
        /// <param name="state"></param>
        public void ChangeGameState(GameState state) {
            State = state;
        }

        private void Awake() {
            //var temaList = Enum.GetValues(typeof(MapTema)).Cast<MapTema>().ToList();
            //MapTema = temaList[UnityEngine.Random.Range(0, temaList.Count)];

            //_mapGenerator = new MapGenerator();
            //List<MapData> mapDataList = _mapGenerator.GenerateMap(20, 20, out List<Vector2Int> pathWaypointList);

            //LoadRoadData(MapTema, mapDataList);

        }

        private void LoadRoadData(MapTema temaType, List<MapData> mapDataList) {
            TileSpriteMapper tileMapper = new TileSpriteMapper(); // Tile Mapper
            tileMapper.LoadDataAsync(temaType, () => { // Data Load가 완료 되면
                _mapGenerator.InstanceMap(mapDataList, tileMapper); // 맵 Instance
            });
        }




    }

}
