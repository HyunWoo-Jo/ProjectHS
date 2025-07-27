namespace Domain {
    public interface IGoldPolicy
    {

        /// <summary>
        /// 골드 획득
        /// </summary>
        bool TryEarnGold(int curGold, int earnGold, out int value);

        /// <summary>
        /// 골드 소모 
        /// </summary>

        bool TrySpendGold(int curGold, int spendGold, out int value);

    }
}
