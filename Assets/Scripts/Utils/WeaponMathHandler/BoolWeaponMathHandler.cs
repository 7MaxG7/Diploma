using System;
using Infrastructure;

namespace Utils
{
    internal class BoolWeaponMathHandler : AbstractWeaponMathHandler<bool>
    {
        protected override bool CountSpecificUpgradedStat(bool baseStat, ArithmeticType arithmeticType,
            float deltaValue)
        {
            switch (arithmeticType)
            {
                case ArithmeticType.Plus:
                    return Convert.ToBoolean(Convert.ToSingle(baseStat) + deltaValue);
                case ArithmeticType.Minus:
                    return Convert.ToBoolean(Convert.ToSingle(baseStat) - deltaValue);
                case ArithmeticType.Multiply:
                    return Convert.ToBoolean(Convert.ToSingle(baseStat) * deltaValue);
                case ArithmeticType.Divide:
                    return Convert.ToBoolean(Convert.ToSingle(baseStat) / deltaValue);
                case ArithmeticType.Equal:
                    return Convert.ToBoolean(deltaValue);
                default:
                    return baseStat;
            }
        }
    }
}