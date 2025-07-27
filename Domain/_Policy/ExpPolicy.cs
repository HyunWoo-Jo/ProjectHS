namespace Domain {
    public class ExpPolicy : IExpPolicy
    {
        private const float _StartPlayerExp = 0;

        public int GetNextLevelExp(int level) {
            return (level + 1) * 20;
        }

        public float CalculateExp(float exp) {
            return exp;
        }

    }
}
