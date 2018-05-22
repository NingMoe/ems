using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface ILink
    {
        
        int AddLink(LinkInfo link);
        void ChangeLinkOrder(ChangeAction action, int id);
        void DeleteLink(string strID);
        List<LinkInfo> ReadLinkAllList();
        void UpdateLink(LinkInfo link);
    }
}
