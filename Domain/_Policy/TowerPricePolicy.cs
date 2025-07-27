using UnityEngine;
using Zenject;
namespace Domain
{
    public class TowerPricePolicy : ITowerPricePolicy {
        public int AdvancePrice(int curPrice) {
            return curPrice + 1;
        }
    }
}
