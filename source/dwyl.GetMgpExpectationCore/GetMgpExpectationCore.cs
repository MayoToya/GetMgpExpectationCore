using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using MathNet.Numerics.Statistics;

namespace dwyl.GetMgpExpectationCore
{
    public static class GetMgpExpectationCore
    {
        public static KeyValuePair<string, (double Mean, double Median)>[] Calculate(string numbers)
        {
            var chars = numbers.AsSpan();

            if (numbers.Length != 9)
            {
                throw new ArgumentException();
            }

            var (intArrayFromInput, hiddenNumbers, hiddenNumbersPositions) = ParseInputForUnsafe(chars);

            return GetPermutationEnumerable(hiddenNumbers)
                    .Select(x => GetValidArrayUsingUnsafeClass(intArrayFromInput, x, hiddenNumbersPositions).CalculateFromBytes())
                    .ToArray()
                    .ToKeyValuePairs();
        }

        private static (byte[], byte[] hiddenNumbers, byte[] hiddenNumbersPositions) ParseInputForUnsafe(ReadOnlySpan<char> span)
        {
            var oneToNine = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var array = new byte[9];
            var hiddenNumbersPositions = new byte[5];
            byte counter = 0;
            var hiddenNumberCounter = 0;

            foreach (var c in span)
            {

                if (c < 48 || 57 < c)
                {
                    throw new InvalidDataException();
                }

                if (c == 48)
                {
                    hiddenNumbersPositions[hiddenNumberCounter] = counter;
                    hiddenNumberCounter++;
                }
                else
                {
                    array[counter] = (byte)(c - 48);
                }

                counter++;
            }

            return (array, oneToNine.Except(array).ToArray(), hiddenNumbersPositions);
        }

        private static byte[] GetValidArrayUsingUnsafeClass(byte[] value, in Span<byte> correction, in Span<byte> hiddenNumbersPositions)
        {
            if (value.Length != 9 || correction.Length != 5)
            {
                throw new ArgumentException();
            }

            var array = new byte[9];
            Unsafe.CopyBlock(ref array[0], ref value[0], 9);

            for (var i = 0; i < correction.Length; i++)
            {
                array[hiddenNumbersPositions[i]] = correction[i];
            }

            return array;
        }

        private static byte[][] GetPermutationEnumerable(byte[] hiddenNumbers)
        {
            if (hiddenNumbers.Length != 5)
            {
                throw new ArgumentException();
            }

            var first = hiddenNumbers[0];
            var second = hiddenNumbers[1];
            var third = hiddenNumbers[2];
            var fourth = hiddenNumbers[3];
            var fifth = hiddenNumbers[4];

            return new[]
            {
                new[] {first, second, third, fourth, fifth},
                new[] {first, second, third, fifth, fourth},
                new[] {first, second, fourth, third, fifth},
                new[] {first, second, fourth, fifth, third},
                new[] {first, second, fifth, third, fourth},
                new[] {first, second, fifth, fourth, third},
                new[] {first, third, second, fourth, fifth},
                new[] {first, third, second, fifth, fourth},
                new[] {first, third, fourth, second, fifth},
                new[] {first, third, fourth, fifth, second},
                new[] {first, third, fifth, second, fourth},
                new[] {first, third, fifth, fourth, second},
                new[] {first, fourth, second, third, fifth},
                new[] {first, fourth, second, fifth, third},
                new[] {first, fourth, third, second, fifth},
                new[] {first, fourth, third, fifth, second},
                new[] {first, fourth, fifth, second, third},
                new[] {first, fourth, fifth, third, second},
                new[] {first, fifth, second, third, fourth},
                new[] {first, fifth, second, fourth, third},
                new[] {first, fifth, third, second, fourth},
                new[] {first, fifth, third, fourth, second},
                new[] {first, fifth, fourth, second, third},
                new[] {first, fifth, fourth, third, second},
                new[] {second, first, third, fourth, fifth},
                new[] {second, first, third, fifth, fourth},
                new[] {second, first, fourth, third, fifth},
                new[] {second, first, fourth, fifth, third},
                new[] {second, first, fifth, third, fourth},
                new[] {second, first, fifth, fourth, third},
                new[] {second, third, first, fourth, fifth},
                new[] {second, third, first, fifth, fourth},
                new[] {second, third, fourth, first, fifth},
                new[] {second, third, fourth, fifth, first},
                new[] {second, third, fifth, first, fourth},
                new[] {second, third, fifth, fourth, first},
                new[] {second, fourth, first, third, fifth},
                new[] {second, fourth, first, fifth, third},
                new[] {second, fourth, third, first, fifth},
                new[] {second, fourth, third, fifth, first},
                new[] {second, fourth, fifth, first, third},
                new[] {second, fourth, fifth, third, first},
                new[] {second, fifth, first, third, fourth},
                new[] {second, fifth, first, fourth, third},
                new[] {second, fifth, third, first, fourth},
                new[] {second, fifth, third, fourth, first},
                new[] {second, fifth, fourth, first, third},
                new[] {second, fifth, fourth, third, first},
                new[] {third, first, second, fourth, fifth},
                new[] {third, first, second, fifth, fourth},
                new[] {third, first, fourth, second, fifth},
                new[] {third, first, fourth, fifth, second},
                new[] {third, first, fifth, second, fourth},
                new[] {third, first, fifth, fourth, second},
                new[] {third, second, first, fourth, fifth},
                new[] {third, second, first, fifth, fourth},
                new[] {third, second, fourth, first, fifth},
                new[] {third, second, fourth, fifth, first},
                new[] {third, second, fifth, first, fourth},
                new[] {third, second, fifth, fourth, first},
                new[] {third, fourth, first, second, fifth},
                new[] {third, fourth, first, fifth, second},
                new[] {third, fourth, second, first, fifth},
                new[] {third, fourth, second, fifth, first},
                new[] {third, fourth, fifth, first, second},
                new[] {third, fourth, fifth, second, first},
                new[] {third, fifth, first, second, fourth},
                new[] {third, fifth, first, fourth, second},
                new[] {third, fifth, second, first, fourth},
                new[] {third, fifth, second, fourth, first},
                new[] {third, fifth, fourth, first, second},
                new[] {third, fifth, fourth, second, first},
                new[] {fourth, first, second, third, fifth},
                new[] {fourth, first, second, fifth, third},
                new[] {fourth, first, third, second, fifth},
                new[] {fourth, first, third, fifth, second},
                new[] {fourth, first, fifth, second, third},
                new[] {fourth, first, fifth, third, second},
                new[] {fourth, second, first, third, fifth},
                new[] {fourth, second, first, fifth, third},
                new[] {fourth, second, third, first, fifth},
                new[] {fourth, second, third, fifth, first},
                new[] {fourth, second, fifth, first, third},
                new[] {fourth, second, fifth, third, first},
                new[] {fourth, third, first, second, fifth},
                new[] {fourth, third, first, fifth, second},
                new[] {fourth, third, second, first, fifth},
                new[] {fourth, third, second, fifth, first},
                new[] {fourth, third, fifth, first, second},
                new[] {fourth, third, fifth, second, first},
                new[] {fourth, fifth, first, second, third},
                new[] {fourth, fifth, first, third, second},
                new[] {fourth, fifth, second, first, third},
                new[] {fourth, fifth, second, third, first},
                new[] {fourth, fifth, third, first, second},
                new[] {fourth, fifth, third, second, first},
                new[] {fifth, first, second, third, fourth},
                new[] {fifth, first, second, fourth, third},
                new[] {fifth, first, third, second, fourth},
                new[] {fifth, first, third, fourth, second},
                new[] {fifth, first, fourth, second, third},
                new[] {fifth, first, fourth, third, second},
                new[] {fifth, second, first, third, fourth},
                new[] {fifth, second, first, fourth, third},
                new[] {fifth, second, third, first, fourth},
                new[] {fifth, second, third, fourth, first},
                new[] {fifth, second, fourth, first, third},
                new[] {fifth, second, fourth, third, first},
                new[] {fifth, third, first, second, fourth},
                new[] {fifth, third, first, fourth, second},
                new[] {fifth, third, second, first, fourth},
                new[] {fifth, third, second, fourth, first},
                new[] {fifth, third, fourth, first, second},
                new[] {fifth, third, fourth, second, first},
                new[] {fifth, fourth, first, second, third},
                new[] {fifth, fourth, first, third, second},
                new[] {fifth, fourth, second, first, third},
                new[] {fifth, fourth, second, third, first},
                new[] {fifth, fourth, third, first, second},
                new[] {fifth, fourth, third, second, first}
            };
        }
    }

    public static class MyExtensions
    {
        public static ushort[] MgpDic => new ushort[19] { 10000, 36, 720, 360, 80, 252, 108, 72, 54, 180, 72, 180, 119, 36, 306, 1080, 144, 1800, 3600 };

        public static ushort ToMgp(this byte sum)
        {
            if (sum < 6 || 24 < sum)
            {
                throw new ArgumentOutOfRangeException();
            }

            return MgpDic[sum - 6];
        }

        public static (ushort topRow, ushort middleRow, ushort bottomRow, ushort leftColumn, ushort middleColumn, ushort rightColumn, ushort downwardSloping, ushort upwardSloping) CalculateFromBytes(this byte[] array)
        {
            if (array.Length != 9)
            {
                throw new ArgumentException("not 9");
            }

            return (((byte)(array[0] + array[1] + array[2])).ToMgp(), ((byte)(array[3] + array[4] + array[5])).ToMgp(), ((byte)(array[6] + array[7] + array[8])).ToMgp(), ((byte)(array[0] + array[3] + array[6])).ToMgp(), ((byte)(array[1] + array[4] + array[7])).ToMgp(), ((byte)(array[2] + array[5] + array[8])).ToMgp(), ((byte)(array[0] + array[4] + array[8])).ToMgp(), ((byte)(array[2] + array[4] + array[6])).ToMgp());
        }

        public static KeyValuePair<string, (double mean, double median)>[] ToKeyValuePairs(this (ushort topRow, ushort middleRow, ushort bottomRow, ushort leftColumn, ushort middleColumn, ushort rightColumn, ushort downwardSloping, ushort upwardSloping)[] resultsOfCalculations)
        {
            return new[]
            {
                new KeyValuePair<string, (double, double)>("Top Row", resultsOfCalculations.Select(x => x.topRow).ToMeanAndMedian()),
                new KeyValuePair<string, (double, double)>("Middle Row", resultsOfCalculations.Select(x => x.middleRow).ToMeanAndMedian()),
                new KeyValuePair<string, (double, double)>("BottomRow", resultsOfCalculations.Select(x => x.bottomRow).ToMeanAndMedian()),
                new KeyValuePair<string, (double, double)>("Left Column", resultsOfCalculations.Select(x => x.leftColumn).ToMeanAndMedian()),
                new KeyValuePair<string, (double, double)>("Middle Column", resultsOfCalculations.Select(x => x.middleColumn).ToMeanAndMedian()),
                new KeyValuePair<string, (double, double)>("Right Column", resultsOfCalculations.Select(x => x.rightColumn).ToMeanAndMedian()),
                new KeyValuePair<string, (double, double)>("Downward Sloping", resultsOfCalculations.Select(x => x.downwardSloping).ToMeanAndMedian()),
                new KeyValuePair<string, (double, double)>("Upward Sloping", resultsOfCalculations.Select(x => x.upwardSloping).ToMeanAndMedian())
            };
        }

        public static (double mean, double median) ToMeanAndMedian(this IEnumerable<ushort> x)
        {
            var d = x.Select(y => (double)y).ToArray();
            return (d.Mean(), d.Median());
        }

    }

}
