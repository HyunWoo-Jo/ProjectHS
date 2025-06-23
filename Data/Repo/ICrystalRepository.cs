using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Data
{

    public interface ICrystalRepository {


        void AddChangedHandler(Action<int> handler);
        void RemoveChangedHandler(Action<int> handler);

        int GetValue();
        void SetValue(int value);

        UniTask LoadValue();
    }

  
}
