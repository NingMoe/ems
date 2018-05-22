using System;

namespace XueFuShop.Common
{
    /// <summary>
    /// 使用Random类生成伪随机数
    /// </summary>
    public sealed class RandomHelper
    {
        //随机数对象
        private static Random _random = new Random();

        #region 生成一个指定范围的随机整数
        /// <summary>
        /// 生成一个指定范围的随机整数，该随机数范围包括最小值，但不包括最大值
        /// </summary>
        /// <param name="minNum">最小值</param>
        /// <param name="maxNum">最大值</param>
        public static int GetRandomInt(int minNum, int maxNum)
        {
            return _random.Next(minNum, maxNum);
        }
        #endregion
        #region 生成一个0.0到1.0的随机小数
        /// <summary>
        /// 生成一个0.0到1.0的随机小数
        /// </summary>
        public static double GetRandomDouble()
        {
            return _random.NextDouble();
        }
        #endregion
        #region 对一个二维选项数组进行随机排序
        /// <summary>
        /// 对一个二维选项数组进行随机排序
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="arr">需要随机排序的数组</param>
        public static T[,] GetRandomOptionArray<T>(T[,] arr, ref string answer)
        {
            //对数组进行随机排序的算法:逐位和低位中随机一位进行交换，将两个位置上的值交换
            //开始交换
            for (int i = arr.GetUpperBound(0); i > 0; i--)
            {
                //生成随机数位置
                int randomNum = GetRandomInt(0, i);

                if (arr[randomNum, 0].ToString() == answer || arr[i, 0].ToString() == answer)
                {
                    if (arr[randomNum, 0].ToString() == answer)
                    {
                        answer = arr[i, 0].ToString();
                    }
                    else
                    {
                        answer = arr[randomNum, 0].ToString();
                    }
                }
                //交换两个随机数位置的值
                T temp = arr[randomNum, 1];
                arr[randomNum, 1] = arr[i, 1];
                arr[i, 1] = temp;
            }
            return arr;
        }
        #endregion

        #region 对一个数组进行随机排序
        /// <summary>
        /// 对一个数组进行随机排序
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="arr">需要随机排序的数组</param>
        public static T[] GetRandomOptionArray<T>(T[] arr)
        {
            //对数组进行随机排序的算法:逐位和低位中随机一位进行交换，将两个位置上的值交换
            //开始交换
            for (int i = arr.GetUpperBound(0); i > 0; i--)
            {
                //生成随机数位置
                int randomNum = GetRandomInt(0, i);

                //交换两个随机数位置的值
                T temp = arr[randomNum];
                arr[randomNum] = arr[i];
                arr[i] = temp;
            }
            return arr;
        }
        #endregion
    }
}
