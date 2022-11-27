using Infrastructure;

namespace Utils
{
    internal class FloatWeaponMathHandler : AbstractWeaponMathHandler<float>
    {
        protected override float CountSpecificUpgradedStat(float baseStat, ArithmeticType arithmeticType,
            float deltaValue)
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
                    return deltaValue;
                default:
                    return baseStat;
            }
        }
    }
}