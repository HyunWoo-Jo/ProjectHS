using Data;
using GamePlay;
using System;
using System.Linq;
using System.Collections.Generic;   
using UnityEngine;
using CustomUtility;


namespace GamePlay {
    /*
    ���� ���� ����	    �ε� �� / ���� �� / �Ͻ����� / Ŭ���� ��
    �� ��ȯ ó��	    �� �̵� ��û ó��, Transition UI ���� ��
    ���� ����/�� ó��	���� �ʱ�ȭ, ���� ���� üũ, Ŭ����/���� ���� üũ
    ������ �ε�	        SaveData, PlayerData �� �ҷ�����
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
        /// ���� ���� ����
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
            tileMapper.LoadDataAsync(temaType, () => { // Data Load�� �Ϸ� �Ǹ�
                _mapGenerator.InstanceMap(mapDataList, tileMapper); // �� Instance
            });
        }




    }

}
