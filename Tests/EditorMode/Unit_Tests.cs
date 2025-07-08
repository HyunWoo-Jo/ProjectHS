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
        /// SStrategy ���� 
        /// </summary>
        [Test]
        public void GenerateMap_Assert_SStrategy() {
            Assert_Path(new SLinePathStrategy(), "SLine");

        }
        /// <summary>
        /// SStrategy ���� 
        /// </summary>
        [Test]
        public void GenerateMap_Assert_UStrategy() {
            Assert_Path(new ULinePathStrategy(), "ULine");
        }

        /// <summary>
        /// ���� ����� �����糪 ����
        /// path �̿ܿ� Road�� �����Ǿ��� ����
        /// path ���� �״�� ������ �Ǿ��� ����
        /// </summary>
        /// <param name="pathStrategy"></param>
        private void Assert_Path(IPathStrategy pathStrategy, string assertionMessage) {
            _gen.SetPathStrategy(pathStrategy);
            var map = _gen.GenerateMap(10, 10, out var pathWorld);
            IEnumerable<Vector2Int> gridPath = pathWorld.Select(data => GridUtility.WorldToGridPosition(data)); // �׸��� ��ǥ�� ��ȯ
            IEnumerable<Vector2Int> mapRoadList = map.Where(data => data.type != TileType.Ground).Select(data => GridUtility.WorldToGridPosition(data.position));
            // map�� Path�̿ܿ� Road�� �ִ��� ����
            var gridCellPath = gridPath.Take(1).Concat(gridPath.Zip(gridPath.Skip(1), GridUtility.StepCells).SelectMany(c => c)).Distinct();
            bool isValid = !mapRoadList.Except(gridCellPath).Any();
            Assert.IsTrue(isValid, assertionMessage + "���Ե��� ���� ��ΰ� ������");

            // path �� ����Ǿ��ֳ� ����
            bool isConnected = gridCellPath.Zip(gridCellPath.Skip(1), (a, b) => Mathf.Abs((a.x + a.y) - (b.x + b.y))).All(diff => diff == 1);
            Assert.IsTrue(isConnected, assertionMessage + "Path�� ����Ǿ� ���� ����"); 
        }
    }
}
