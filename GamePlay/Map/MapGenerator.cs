using NUnit.Framework;
using UnityEngine;
using Data;
using System.Collections.Generic;
namespace GamePlay
{
    /// <summary>
    /// 맵을 생성해주는 클레스
    /// </summary>
    public class MapGenerator {

        public List<MapData> GenerateMap(int sizeX, int sizeY) {
            List<MapData> mapDataList = new();
            mapDataList.Capacity = sizeX * sizeY;

            // 맵 생성하는 로직
        }
    }
}
