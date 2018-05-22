using System;
using System.Collections.Generic;
using System.Reflection;

namespace XueFu.EntLib
{
    public sealed class EnumHelper
    {
        public static string ReadEnumChineseName<T>(int value)
        {
            string str = string.Empty;
            List<EnumInfo> list = ReadEnumList<T>();
            foreach (EnumInfo info in list)
            {
                if (info.Value == value)
                {
                    return info.ChineseName;
                }
            }
            return str;
        }

        public static List<EnumInfo> ReadEnumList<T>()
        {
            List<EnumInfo> list = new List<EnumInfo>();
            FieldInfo[] fields = typeof(T).GetFields();
            foreach (FieldInfo info in fields)
            {
                if (info.GetCustomAttributes(typeof(EnumAttribute), false).Length > 0)
                {
                    EnumInfo item = new EnumInfo();
                    item.ChineseName = ((EnumAttribute)info.GetCustomAttributes(typeof(EnumAttribute), false)[0]).ChineseName;
                    item.EnglishName = info.Name;
                    item.Value = Convert.ToInt32(info.GetRawConstantValue());
                    list.Add(item);
                }
            }
            return list;
        }
    }

    public sealed class EnumInfo
    {
        private string chineseName = string.Empty;
        private string englishName = string.Empty;
        private int value = 0;

        public string ChineseName
        {
            get
            {
                return this.chineseName;
            }
            set
            {
                this.chineseName = value;
            }
        }

        public string EnglishName
        {
            get
            {
                return this.englishName;
            }
            set
            {
                this.englishName = value;
            }
        }

        public int Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }

    public class EnumAttribute : Attribute
    {
        private string chineseName;

        public EnumAttribute(string chineseName)
        {
            this.chineseName = chineseName;
        }

        public string ChineseName
        {
            get
            {
                return this.chineseName;
            }
            set
            {
                this.chineseName = value;
            }
        }
    }
}
