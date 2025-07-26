using UnityEngine;
using GamePlay;
using NUnit.Framework;
using System.Collections.Generic;
using CustomUtility;
using Data;
using System.Linq;
using System.IO;
namespace Tests.Unit
{
    [TestFixture]
    public class MapGeneratorTests {
        private MapGenerator _gen;

        [SetUp]
        public void Setup() => _gen = new MapGenerator();



        /// <summary>
        /// SStrategy 검증 
        /// </summary>
        [Test]
        public void GenerateMap_Assert_SStrategy() {
            Assert_Path(new SLinePathStrategy(), "SLine");

        }
        /// <summary>
        /// SStrategy 검증 
        /// </summary>
        [Test]
        public void GenerateMap_Assert_UStrategy() {
            Assert_Path(new ULinePathStrategy(), "ULine");
        }

        /// <summary>
        /// 맵이 제대로 생성됬나 검증
        /// path 이외에 Road가 생성되었나 검증
        /// path 개수 그대로 생성이 되었나 검증
        /// </summary>
        /// <param name="pathStrategy"></param>
        private void Assert_Path(IPathStrategy pathStrategy, string assertionMessage) {
            _gen.SetPathStrategy(pathStrategy);
            var map = _gen.GenerateMap(10, 10, out var pathWorld);
            IEnumerable<Vector2Int> gridPath = pathWorld.Select(data => GridUtility.WorldToGridPosition(data)); // 그리드 좌표로 변환
            IEnumerable<Vector2Int> mapRoadList = map.Where(data => data.type != TileType.Ground).Select(data => GridUtility.WorldToGridPosition(data.position));
            // map에 Path이외에 Road가 있는지 검증
            var gridCellPath = gridPath.Take(1).Concat(gridPath.Zip(gridPath.Skip(1), GridUtility.StepCells).SelectMany(c => c)).Distinct();
            bool isValid = !mapRoadList.Except(gridCellPath).Any();
            Assert.IsTrue(isValid, assertionMessage + "포함되지 않은 경로가 존재함");

            // path 가 연결되어있나 검증
            bool isConnected = gridCellPath.Zip(gridCellPath.Skip(1), (a, b) => Mathf.Abs((a.x + a.y) - (b.x + b.y))).All(diff => diff == 1);
            Assert.IsTrue(isConnected, assertionMessage + "Path가 연결되어 있지 않음"); 
        }
    }
}
