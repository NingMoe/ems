using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IVoteItem
    {
        
        int AddVoteItem(VoteItemInfo voteItem);
        void ChangeVoteItemCount(string strID, ChangeAction action);
        void ChangeVoteItemCountByGeneral(string strID, ChangeAction action);
        void ChangeVoteItemOrder(ChangeAction action, int id);
        void DeleteVoteItem(string strID);
        void DeleteVoteItemByVoteID(string strVoteID);
        VoteItemInfo ReadVoteItem(int id);
        List<VoteItemInfo> ReadVoteItemAllList();
        List<VoteItemInfo> ReadVoteItemByVote(int voteID);
        void UpdateVoteItem(VoteItemInfo voteItem);
    }
}
