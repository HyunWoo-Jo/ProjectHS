using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Data
{
    [CreateAssetMenu(fileName = "RarityColorStyle", menuName = "Scriptable Objects/Style/RarityColorStyleSO")]
    public class RarityColorStyleSO : ScriptableObject
    {
        private Dictionary<Rarity, Color> _colorDictionary = new();
        [SerializeField] private Color _common = Color.white;
        [SerializeField] private Color _rare = Color.green;
        [SerializeField] private Color _epic = new Color(0.5f, 0f, 0.5f); // Purple
        [SerializeField] private Color _legendary = new Color(1f, 0.65f, 0f); // Orange

        public Color GetColor(Rarity rarity) => _colorDictionary[rarity];

        private void OnEnable() {
            BuildDictionary();
#if UNITY_EDITOR
            DataAssert();
#endif
        }
        /// <summary>
        /// Dictionary 생성
        /// </summary>
        private void BuildDictionary() {
            _colorDictionary.Clear(); // 중복 예외 방지
            _colorDictionary[Rarity.Common] = _common;
            _colorDictionary[Rarity.Rare] = _rare;
            _colorDictionary[Rarity.Epic] = _epic;
            _colorDictionary[Rarity.Legendary] = _legendary;
        }


#if UNITY_EDITOR
        /// <summary>
        /// Enum 모든 값이 매핑돼 있는지 검증
        /// </summary>
        private void DataAssert() {
            var excludeList = ((Rarity[])Enum.GetValues(typeof(Rarity))).ToList().Where(key => !_colorDictionary.ContainsKey(key)); // 검출
            Assert.IsTrue(excludeList.Count() == 0);
            excludeList.ToList().ForEach(key => Debug.LogError(key.ToString() + " 색상이 정의되지 않았습니다."));
        }
#endif
    }
}
