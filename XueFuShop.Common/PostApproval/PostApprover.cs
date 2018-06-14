using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.Common
{
    public class PostApprover
    {
        private string approvalPostString = "|478|5|484|$|4|8|";
        private List<PostPassInfo> postPassList = null;

        public int NextPostID { get; private set; }

        public PostApprover(List<PostPassInfo> postPassList)
        {
            this.postPassList = postPassList;
        }

        public bool IsTest(int postID)
        {
            bool result = true;
            string currentPostApprovalString = GetCurrentPostApprovalString(postID);
            if (currentPostApprovalString != null)
            {
                var approvalPostArray = currentPostApprovalString.Split('|');
                for (int i = 1; i < approvalPostArray.Length; i++)
                {
                    if (approvalPostArray[i] == postID.ToString())
                    {
                        if (i - 1 > 0 && !IsPass(int.Parse(approvalPostArray[i - 1])))
                        {
                            this.NextPostID = int.Parse(approvalPostArray[i - 1]);
                            result = false;
                        }
                        break;
                    }
                }
            }
            return result;
        }

        private bool IsPass(int postID)
        {
            return postPassList.Exists(t => t.PostId == postID);
        }

        private string GetCurrentPostApprovalString(int postID)
        {
            string result = null;
            if (approvalPostString.IndexOf(string.Format("|{0}|", postID.ToString())) >= 0)
            {
                if (approvalPostString.IndexOf('$') > 0)
                {
                    foreach (var item in approvalPostString.Split('$'))
                    {
                        if (item.IndexOf(string.Format("|{0}|", postID.ToString())) >= 0)
                        {
                            result = item;
                            break;
                        }
                    }
                }
                else
                {
                    result = this.approvalPostString;
                }
            }
            return result;
        }
    }
}
