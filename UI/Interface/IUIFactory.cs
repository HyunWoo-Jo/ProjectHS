using UnityEngine;

namespace UI
{
    public interface IUIFactory
    {
        T InstanceUI<T>(int orderBy) where T : Object;
    }
}
