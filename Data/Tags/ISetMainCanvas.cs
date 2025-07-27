using UnityEngine;
using Zenject;

namespace Data
{
    public interface ISetMainCanvas {
        void SetMainCanvas(IMainCanvasTag mainCanvas, DiContainer container);
    }
}
