using System;
using System.Data;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface ISMSRecord
    {
        int AddSMSRecord(SMSRecordInfo Model);
        SMSRecordInfo ReadSMSRecord(string Mobile, string Code);
    }
}
