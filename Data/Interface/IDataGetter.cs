using UnityEngine;

namespace Data
{
    public interface IDataGetter<T> 
    {
        T GetValue();
    }
    public interface IDataGetterKey<T, Key> {
        T GetValue(Key key);
    }
}
