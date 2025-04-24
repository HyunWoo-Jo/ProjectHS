using UnityEngine;

namespace UI
{
    public interface IUIFactory
    {
        T InstanceUI<T>(Transform parent, int orderBy) where T : Object;
    }
}
