using Infrastructure;

namespace Utils
{
    internal class IntArrayWeaponMathHandler : AbstractWeaponMathHandler<int[]>
    {
        protected override int[] CountSpecificUpgradedStat(int[] baseStat, ArithmeticType arithmeticType,
            float deltaValue)
        {
            var stat = new int[baseStat.Length];
            baseStat.CopyTo(stat, 0);
            for (var i = 0; i < stat.Length; i++)
            {
                switch (arithmeticType)
                {
                    case ArithmeticType.Plus:
                        stat[i] += (int)deltaValue;
                        break;
                    case ArithmeticType.Minus:
                        stat[i] -= (int)deltaValue;
                        break;
                    case ArithmeticType.Multiply:
                        stat[i] = (int)(stat[i] * deltaValue);
                        break;
                    case ArithmeticType.Divide:
                        stat[i] = (int)(stat[i] / deltaValue);
                        break;
                    case ArithmeticType.Equal:
                        stat[i] = (int)deltaValue;
                        break;
                    case ArithmeticType.None:
                    default:
                        break;
                }
            }

            return stat;
        }

        // protected override void HandleSpecificValueArithmetic(ArithmeticType arithmeticType, float deltaValue)
        // {
        //     for (var i = 0; i < WeaponCharacteristicValue.Length; i++)
        //     {
        //         switch (arithmeticType)
        //         {
        //             case ArithmeticType.Plus:
        //                 WeaponCharacteristicValue[i] += (int)deltaValue;
        //                 break;
        //             case ArithmeticType.Minus:
        //                 WeaponCharacteristicValue[i] -= (int)deltaValue;
        //                 break;
        //             case ArithmeticType.Multiply:
        //                 WeaponCharacteristicValue[i] = (int)(WeaponCharacteristicValue[i] * deltaValue);
        //                 break;
        //             case ArithmeticType.Divide:
        //                 WeaponCharacteristicValue[i] = (int)(WeaponCharacteristicValue[i] / deltaValue);
        //                 break;
        //             case ArithmeticType.Equal:
        //                 WeaponCharacteristicValue[i] = (int)deltaValue;
        //                 break;
        //             case ArithmeticType.None:
        //             default:
        //                 return;
        //         }
        //     }
        // }
    }
}