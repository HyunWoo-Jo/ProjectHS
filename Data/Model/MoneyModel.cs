using UnityEngine;

namespace Data
{
    public class MoneyModel
    {
        public long Value {  get; private set; }
     
        public void Set(long value) {
            Value = value;
        }
    }
}
