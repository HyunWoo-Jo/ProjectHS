using UnityEngine;
using System;
using System.Collections;
using Unity.Collections;
using Unity.Mathematics;
namespace Data {
    public interface IEnemyDataService {
        EnemyData GetEnemyData(int index);
        void SetEnemyData(int index, EnemyData enemyData);
        NativeArray<EnemyData> GetEnemiesData();
    }
}
