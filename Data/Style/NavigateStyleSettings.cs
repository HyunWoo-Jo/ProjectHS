using DG.Tweening;
using UnityEngine;
using System;
namespace Data
{
    [CreateAssetMenu(fileName = "NavigateStyleSettings", menuName = "Scriptable Objects/NavigateStyleSettings")]
    [Serializable]
    public class NavigateStyleSettings : ScriptableObject
    {
        public float panelMoveDuration = 0.5f;
        public Ease panelMoveEase = Ease.InOutCirc;
        public Vector2 buttonOriginalSize = new Vector2(400, 250);
        public Vector2 buttonCloseUpSize = new Vector2(500, 300);
        public float buttonAnimDuration  = 0.2f;
        public Ease buttonAnimEase = Ease.InOutElastic;
    }
}
