using UnityEngine;

namespace CustomUtility {
    public enum VarType {
        Int,
        Float,
    }

    /// <summary>
    /// int 또는 float 하나만 담는 variant.
    /// </summary>ant {
    public class ValueVariant {
        private readonly VarType _type;
        private int _i;
        private float _f;

        public void SetValue(int value) {
            _i = value;
        }
        public void SetValue(float value) {
            _f = value;
        }

        public ValueVariant(int i) {
            _type = VarType.Int;
            this._i = i;  
        }
        public ValueVariant(float f) {
            _type = VarType.Int;
            this._f = f;
        }



        public VarType Type => _type;

        public T Get<T>() {
            T value = default;
            if (typeof(T) == typeof(int) && _type == VarType.Int) {
                value = (T)(object)_i;
            }

            if (typeof(T) == typeof(float) && _type == VarType.Float) {
                value = (T)(object)_f;
            }
            return value;
        }
        public static bool TryCreate(object src, out ValueVariant vv) {
            switch (src) {
                case int i:
                vv = new ValueVariant(i);
                return true;

                case long l:
                vv = new ValueVariant((int)l);
                return true;

                case float f:
                vv = new ValueVariant(f);
                return true;

                case double d:             
                vv = new ValueVariant((float)d);
                return true;

                // 지원하지 않는 타입
                default:
                vv = default;
                UnityEngine.Debug.Log(src.GetType());
                return false;
            }
        }
    }
}
