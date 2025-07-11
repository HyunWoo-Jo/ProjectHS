using Cysharp.Threading.Tasks;
using System;
using System.Diagnostics;
using UnityEngine;

namespace Data
{

    public interface ICrystalRepository {


        void AddChangedListener(Action<int> handler);
        void RemoveChangedListener(Action<int> handler);
        int GetValue();

        bool TrySpend(int price);

        bool TryEarn(int value);
        void SetValue(int value);

        UniTask LoadValue();
    }

  
}
