using UnityEngine;
using CustomUtility;
namespace Data
{
    public class WaveStatusModel
    {
        public ObservableValue<int> waveLevelObservable { get; } = new(0); // 스테이지 레벨
        public ObservableValue<float> waveTimeObservable { get; } = new(0); // 웨이브 타임
    }
}
