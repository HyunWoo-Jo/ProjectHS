using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Diagnostics;
using UnityEngine;

namespace Contracts {

    public interface ICrystalRepository {

        UniTask<int> GetAsyncValue();

        UniTask<bool> AsyncTrySpend(int price);

        UniTask<bool> AsyncTryEarn(int value);
    }

  
}
