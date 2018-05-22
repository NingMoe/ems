using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class ActiveUser : CommonBasePage
    {
        
        protected string result = string.Empty;

        
        protected override void PageLoad()
        {
            base.PageLoad();
            string queryString = RequestHelper.GetQueryString<string>("CheckCode");
            if (queryString != string.Empty)
            {
                string str2 = StringHelper.Decode(queryString, ShopConfig.ReadConfigInfo().SecureKey);
                if (str2.IndexOf('|') > 0)
                {
                    int id = Convert.ToInt32(str2.Split(new char[] { '|' })[0]);
                    string str3 = str2.Split(new char[] { '|' })[1];
                    string str4 = str2.Split(new char[] { '|' })[2];
                    UserInfo info = UserBLL.ReadUser(id);
                    if (((info.ID > 0) && (info.UserName == str4)) && (info.Email == str3))
                    {
                        if (info.Status == 1)
                        {
                            UserBLL.ChangeUserStatus(info.ID.ToString(), 2);
                            this.result = "��ϲ�����ɹ������û�";
                        }
                        else
                        {
                            this.result = "���û��Ѿ�������";
                        }
                    }
                    else
                    {
                        this.result = "����ļ�����Ϣ";
                    }
                }
                else
                {
                    this.result = "����ļ�����Ϣ";
                }
            }
            else
            {
                this.result = "������Ϣ��ʽ����";
            }
        }
    }
}
