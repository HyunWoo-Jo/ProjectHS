using UnityEngine;
using System;
using System.Collections;
using Unity.Collections;
using Unity.Mathematics;
namespace Data {
    public interface IEnemyDataStore {
        EnemyData GetEnemyData(int index);
        int EnemiesLength();
        bool IsEnemyData();
        void SetEnemyData(int index, EnemyData enemyData);
        NativeArray<EnemyData> GetEnemiesData();
    }
}
