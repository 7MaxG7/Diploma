using Infrastructure;

namespace Utils
{
    internal interface IWeaponMathHandler
    {
        object CountUpgradedStat(object baseStat, ArithmeticType arithmeticType, float deltaValue);
    }
}