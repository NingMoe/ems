using System;
using System.Collections.Generic;
using System.Text;

namespace XueFuShop.Models
{
    public class RenZhengCateInfo
    {

        /// <summary>
        /// 
        /// </summary>
        private int _Id;
        public int Id
        {
            set { _Id = value; }
            get { return _Id; }
        }

        /// <summary>
        /// ��֤�γ�
        /// </summary>
        private int _CateId;
        public int CateId
        {
            set { _CateId = value; }
            get { return _CateId; }
        }

        /// <summary>
        /// ��λ
        /// </summary>
        private int _PostId;
        public int PostId
        {
            set { _PostId = value; }
            get { return _PostId; }
        }

        /// <summary>
        /// ��λ
        /// </summary>
        private string _inPostID;
        public string InPostID
        {
            set { _inPostID = value; }
            get { return _inPostID; }
        }

        /// <summary>
        /// �Ƿ�ɾ��
        /// </summary>
        private int _Del;
        public int Del
        {
            set { _Del = value; }
            get { return _Del; }
        }

        /// <summary>
        /// ����
        /// </summary>
        private int _Order;
        public int Order
        {
            set { _Order = value; }
            get { return _Order; }
        }
    }
}
