using NUnit.Framework;
using UnityEngine;
using Data;
using System.Collections.Generic;
namespace GamePlay
{
    /// <summary>
    /// ���� �������ִ� Ŭ����
    /// </summary>
    public class MapGenerator {

        public List<MapData> GenerateMap(int sizeX, int sizeY) {
            List<MapData> mapDataList = new();
            mapDataList.Capacity = sizeX * sizeY;

            // �� �����ϴ� ����
        }
    }
}
