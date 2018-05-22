using System;

namespace XueFuShop.Common
{
    /// <summary>
    /// ʹ��Random������α�����
    /// </summary>
    public sealed class RandomHelper
    {
        //���������
        private static Random _random = new Random();

        #region ����һ��ָ����Χ���������
        /// <summary>
        /// ����һ��ָ����Χ��������������������Χ������Сֵ�������������ֵ
        /// </summary>
        /// <param name="minNum">��Сֵ</param>
        /// <param name="maxNum">���ֵ</param>
        public static int GetRandomInt(int minNum, int maxNum)
        {
            return _random.Next(minNum, maxNum);
        }
        #endregion
        #region ����һ��0.0��1.0�����С��
        /// <summary>
        /// ����һ��0.0��1.0�����С��
        /// </summary>
        public static double GetRandomDouble()
        {
            return _random.NextDouble();
        }
        #endregion
        #region ��һ����άѡ����������������
        /// <summary>
        /// ��һ����άѡ����������������
        /// </summary>
        /// <typeparam name="T">���������</typeparam>
        /// <param name="arr">��Ҫ������������</param>
        public static T[,] GetRandomOptionArray<T>(T[,] arr, ref string answer)
        {
            //������������������㷨:��λ�͵�λ�����һλ���н�����������λ���ϵ�ֵ����
            //��ʼ����
            for (int i = arr.GetUpperBound(0); i > 0; i--)
            {
                //���������λ��
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
                //�������������λ�õ�ֵ
                T temp = arr[randomNum, 1];
                arr[randomNum, 1] = arr[i, 1];
                arr[i, 1] = temp;
            }
            return arr;
        }
        #endregion

        #region ��һ����������������
        /// <summary>
        /// ��һ����������������
        /// </summary>
        /// <typeparam name="T">���������</typeparam>
        /// <param name="arr">��Ҫ������������</param>
        public static T[] GetRandomOptionArray<T>(T[] arr)
        {
            //������������������㷨:��λ�͵�λ�����һλ���н�����������λ���ϵ�ֵ����
            //��ʼ����
            for (int i = arr.GetUpperBound(0); i > 0; i--)
            {
                //���������λ��
                int randomNum = GetRandomInt(0, i);

                //�������������λ�õ�ֵ
                T temp = arr[randomNum];
                arr[randomNum] = arr[i];
                arr[i] = temp;
            }
            return arr;
        }
        #endregion
    }
}
