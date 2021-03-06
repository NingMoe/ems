using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IVote
    {
        int AddVote(VoteInfo vote);
        void ChangeVoteCount(int id, ChangeAction action);
        void ChangeVoteCountByGeneral(string strID, ChangeAction action);
        void DeleteVote(string strID);
        VoteInfo ReadVote(int id);
        List<VoteInfo> ReadVoteList(int currentPage, int pageSize, ref int count);
        void UpdateVote(VoteInfo vote);
    }
}
