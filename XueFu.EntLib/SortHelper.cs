using System;
using System.Collections.Generic;
using System.Text;

namespace XueFu.EntLib
{
    #region ÅÅÐò²Ù×÷Àà
    public sealed class SortHelper
    {
        public static Dictionary<string, string> DictionarySort(Dictionary<string, string> dic)
        {
            //List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>(dic);
            //if (dic.Count > 0)
            //{
            //    lst.Sort(delegate(KeyValuePair<string, string> s1, KeyValuePair<string, string> s2)
            //    {
            //        //return s2.Value.CompareTo(s1.Value);
            //        //return s2.Key.CompareTo(s1.Key);
            //        return s1.Key.CompareTo(s2.Key);
            //    });
            //    dic.Clear();

            //    //foreach (KeyValuePair<string, int> kvp in lst)
            //    //    Response.Write(kvp.Key + "£º" + kvp.Value + "<br />");
            //}

            //return lst.to;
            return (SortedDictionary<string, string>)dic;
        }
    }
    #endregion
}
