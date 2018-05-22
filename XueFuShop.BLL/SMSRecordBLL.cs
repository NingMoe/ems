using System;
using System.Data;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.BLL
{
    public class SMSRecordBLL
    {
        private static readonly ISMSRecord dal = FactoryHelper.Instance<ISMSRecord>(Global.DataProvider, "SMSRecordDAL");

        public static int AddSMSRecord(SMSRecordInfo Model)
        {
            return dal.AddSMSRecord(Model);
        }

        public static SMSRecordInfo ReadSMSRecord(string Mobile, string Code)
        {
            return dal.ReadSMSRecord(Mobile, Code);
        }

        public static SMSRecordInfo ReadSMSRecord(string Mobile)
        {
            return ReadSMSRecord(Mobile, null);
        }
    }
}
