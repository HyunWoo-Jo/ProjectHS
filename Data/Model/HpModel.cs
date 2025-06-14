using CustomUtility;
using UnityEngine;

namespace Data
{
    public class HpModel
    {
        public ObservableValue<int> curHpObservable = new(20);
        public ObservableValue<int> maxHpObservable = new(20);
    }
}
