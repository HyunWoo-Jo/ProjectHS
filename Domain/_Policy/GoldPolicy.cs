namespace Domain {
    // 인게임에 사용되는 골드 정보를 저장하는 클레스
    public class GoldPolicy : IGoldPolicy
    {
        public double DiscountRatio { get; set; } = 1f;   // 할인 비율
        public double AdditionalRatio { get; set; } = 1f; // 추가 비율

        /// <summary>
        /// 골드 획득 정책
        /// </summary>
        public bool TryEarnGold(int curGold, int earnGold, out int value) {
            if (earnGold <= 0) { value = 0; return false; }
            value = (int)(earnGold * AdditionalRatio);
            return true;
        }

        /// <summary>
        /// 골드 소비 정책
        /// </summary>
        public bool TrySpendGold(int curGold, int spendGold, out int value) {
            value = (int)(spendGold * DiscountRatio);
            if (curGold < value) return false;
            return true;
        }
    }
}
