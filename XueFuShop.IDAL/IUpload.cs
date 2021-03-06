using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IUpload
    {        
        void AddUpload(UploadInfo upload);
        void DeleteUploadByClassID(int tableID, string strClassID);
        void DeleteUploadByRecordID(int tableID, string strRecordID);
        List<UploadInfo> ReadUploadByClassID(int tableID, string strClassID);
        List<UploadInfo> ReadUploadByRecordID(int tableID, string strRecordID);
        void UpdateUpload(int tableID, int classID, int recordID, string randomNumber);
    }
}
