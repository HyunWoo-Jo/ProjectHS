using CustomUtility;
using UnityEngine;
using R3;
namespace Data
{
    public class HpModel
    {
        public ReactiveProperty<int> curHpObservable = new(20);
        public ReactiveProperty<int> maxHpObservable = new(20);
    }
}
