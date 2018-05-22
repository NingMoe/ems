using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using YXTSMS;
using LetvAPI;

namespace XueFuShop.API
{
    public partial class LetvCallBack : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string code, message, payer_status = string.Empty;
            SortedDictionary<string, string> sPara = GetRequestPost();

            code = "1";
            message = "��Ƶ����ʧ��";
            payer_status = "0";

            //if (LetvCore.Verify(sPara, RequestHelper.GetQueryString<string>("sign")))
            //{
            //    string UserName = RequestHelper.GetQueryString<string>("payer_name");
            //    string CheckCode = RequestHelper.GetQueryString<string>("check_code");
            //    if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(CheckCode))
            //    {
            //        message = "����Դ��ȡʧ��";
            //    }
            //    else
            //    {
            //        try
            //        {
            //            string[] CodeArray = CheckCode.Split('_');
            //            UserInfo user = UserBLL.ReadUser(int.Parse(CodeArray[0]));
            //            if (user.Status == 2 && CompanyBLL.ReadCompany(user.CompanyID).State == 0 && user.Mobile == UserName)
            //            {
            //                int TimeOutSeconds = SMSConfig.CodeTimeOut * 60;
            //                //��ȡ�γ����ݣ���֤�Ƿ��ж����Ƶ
            //                ProductInfo product = ProductBLL.ReadProduct(int.Parse(CodeArray[2]));
            //                //if (TestCateModel.CateCode.Contains("|"))
            //                //{
            //                //    //�����Ƶ����֤��Ч���ӳ������Сʱ
            //                //    TimeOutSeconds = 5 * 3600;
            //                //}
            //                SMSRecordInfo SMSRecordModel = SMSRecordBLL.ReadSMSRecord(UserName, CodeArray[1]);
            //                if (SMSRecordModel != null)
            //                {
            //                    if ((DateTime.Now - SMSRecordModel.DataCreateDate).TotalSeconds <= TimeOutSeconds)
            //                    {
            //                        //if (SMSRecordModel.VerCode == CodeArray[1])
            //                        //{
            //                        code = "0";
            //                        message = "��֤�ɹ�";
            //                        payer_status = "1";
            //                        //}
            //                        //else
            //                        //{
            //                        //    message = "��֤�벻��ȷ";
            //                        //}
            //                    }
            //                    else
            //                    {
            //                        message = "��֤�볬ʱ";
            //                    }
            //                }
            //                else
            //                {
            //                    message = "��֤�벻��ȷ";
            //                }
            //            }
            //            else
            //            {
            //                message = "�ʺ���Ϣ�쳣";
            //            }
            //        }
            //        catch
            //        {
            //            message = "��֤�쳣";
            //        }
            //    }
            //}
            //ResponseHelper.Write("{\"code\":" + code + ",\"message\":\"" + message + "\",\"payer_status\":" + payer_status + "}");
            ResponseHelper.Write("{\"code\":0,\"message\":\"��֤�ɹ�\",\"payer_status\":1}");
        }

        /// <summary>
        /// ��ȡ���ӹ��������󣬲��ԡ�������=����ֵ������ʽ�������
        /// </summary>
        /// <returns>request��������Ϣ��ɵ�����</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }
    }
}
