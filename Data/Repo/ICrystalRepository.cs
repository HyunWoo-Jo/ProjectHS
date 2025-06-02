using System;
using UnityEngine;

namespace Data
{

    public interface ICrystalRepository {


        void AddChangeHandler(Action<int> handler);
        void RemoveChangeHandler(Action<int> handler);

        int GetValue();
        void SetValue(int value);

        void LoadValue();
    }

  
}
