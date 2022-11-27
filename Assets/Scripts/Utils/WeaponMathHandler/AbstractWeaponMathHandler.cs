using Infrastructure;

namespace Utils
{
    internal abstract class AbstractWeaponMathHandler<T> : IWeaponMathHandler
    {
        protected T WeaponCharacteristicValue;


        public object CountUpgradedStat(object baseStat, ArithmeticType arithmeticType, float deltaValue)
        {
            return CountSpecificUpgradedStat((T)baseStat, arithmeticType, deltaValue);
        }

        protected abstract T CountSpecificUpgradedStat(T baseStat, ArithmeticType arithmeticType, float deltaValue);
    }
}