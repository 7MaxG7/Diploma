using Infrastructure;

namespace Abstractions.Utils
{
    internal interface IWeaponMathHandler
    {
        object CountUpgradedStat(object baseStat, ArithmeticType arithmeticType, float deltaValue);
    }
}