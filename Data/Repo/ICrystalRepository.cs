using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Diagnostics;
using UnityEngine;

namespace Data
{

    public interface ICrystalRepository {


        ReadOnlyReactiveProperty<int> GetRO_ReactiveObservable();
        void Notify();
        int GetValue();

        bool TrySpend(int price);

        bool TryEarn(int value);
        void SetValue(int value);

        UniTask LoadValue();
    }

  
}
