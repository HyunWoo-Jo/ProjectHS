using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Data
{

    public interface ICrystalRepository {


        void AddChangedListener(Action<int> handler);
        void RemoveChangedListener(Action<int> handler);

        int GetValue();
        void SetValue(int value);

        UniTask LoadValue();
    }

  
}
