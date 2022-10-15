using Abstractions.Utils;
using Infrastructure;

namespace Utils.WeaponMathHandler
{
    internal abstract class AbstractWeaponMathHandler<T> : IWeaponMathHandler
    {
        protected T WeaponCharacteristicValue;
        

        public object CountUpgradedStat(object baseStat, ArithmeticType arithmeticType, float deltaValue)
        {
            return CountSpecificUpgradedStat((T)baseStat, arithmeticType, deltaValue);
        }
        
        // public void HandleArithmetics(ArithmeticType arithmeticType, float deltaValue)
        // {
        //     HandleSpecificValueArithmetic(arithmeticType, deltaValue);
        // }
        //
        // public object GetResult()
        // {
        //     return WeaponCharacteristicValue;
        // }

        protected abstract T CountSpecificUpgradedStat(T baseStat, ArithmeticType arithmeticType, float deltaValue);

        // protected abstract void HandleSpecificValueArithmetic(ArithmeticType arithmeticType, float deltaValue);
    }
}