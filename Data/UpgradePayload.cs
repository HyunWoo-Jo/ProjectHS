using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class UpgradePayload {
        public UpgradeType type;
        public float? value = null;    // 통상 수치
        public string abilityId = null;    // 특수 능력 식별
        public IReadOnlyDictionary<string, object> args = null; // 세부 파라미터
    }
}
