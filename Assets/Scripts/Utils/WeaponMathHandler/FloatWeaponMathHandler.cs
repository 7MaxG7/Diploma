using Infrastructure;

namespace Utils.WeaponMathHandler
{
    internal class FloatWeaponMathHandler : AbstractWeaponMathHandler<float>
    {
        protected override float CountSpecificUpgradedStat(float baseStat, ArithmeticType arithmeticType, float deltaValue)
        {
            switch (arithmeticType)
            {
                case ArithmeticType.Plus:
                    return baseStat + deltaValue;
                case ArithmeticType.Minus:
                    return baseStat - deltaValue;
                case ArithmeticType.Multiply:
                    return baseStat * deltaValue;
                case ArithmeticType.Divide:
                    return baseStat / deltaValue;
                case ArithmeticType.Equal:
                    return  deltaValue;
                default:
                    return baseStat;
            }
        }

        // protected override void HandleSpecificValueArithmetic(ArithmeticType arithmeticType, float deltaValue)
        // {
        //     switch (arithmeticType)
        //     {
        //         case ArithmeticType.Plus:
        //             WeaponCharacteristicValue += deltaValue;
        //             break;
        //         case ArithmeticType.Minus:
        //             WeaponCharacteristicValue -= deltaValue;
        //             break;
        //         case ArithmeticType.Multiply:
        //             WeaponCharacteristicValue *= deltaValue;
        //             break;
        //         case ArithmeticType.Divide:
        //             WeaponCharacteristicValue /= deltaValue;
        //             break;
        //         case ArithmeticType.Equal:
        //             WeaponCharacteristicValue = deltaValue;
        //             break;
        //     }
        // }
    }
}