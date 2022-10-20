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

        // protected override void HandleSpecificValueArithmetic(ArithmeticType arithmeticType, float deltaValue)
        // {
        //     switch (arithmeticType)
        //     {
        //         case ArithmeticType.Plus:
        //             WeaponCharacteristicValue = Convert.ToBoolean(Convert.ToSingle(WeaponCharacteristicValue) + deltaValue);
        //             break;
        //         case ArithmeticType.Minus:
        //             WeaponCharacteristicValue = Convert.ToBoolean(Convert.ToSingle(WeaponCharacteristicValue) - deltaValue);
        //             break;
        //         case ArithmeticType.Multiply:
        //             WeaponCharacteristicValue = Convert.ToBoolean(Convert.ToSingle(WeaponCharacteristicValue) * deltaValue);
        //             break;
        //         case ArithmeticType.Divide:
        //             WeaponCharacteristicValue = Convert.ToBoolean(Convert.ToSingle(WeaponCharacteristicValue) / deltaValue);
        //             break;
        //         case ArithmeticType.Equal:
        //             WeaponCharacteristicValue = Convert.ToBoolean(deltaValue);
        //             break;
        //     }
        // }
    }
}