using UnityEngine;

namespace UI
{
    /// <summary>
    /// Pointer가 Up 될때 호출하게 하는 인터페이스로 UI에 사용
    /// </summary>
    public interface IPointerUP
    {
        /// <summary>
        /// Pointer가 Up 될때 호출
        /// </summary>
        void OnPointerUP();
    }
}
