using System;

namespace XueFuShop.Models
{
    /// <summary>
    /// ʵ����MXItem��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    public class MXAdminSysItem
    {
        #region MXAdminSysItemModel

        /// <summary>
        /// ϵͳ�˵� ��ʶ
        /// </summary>
        private int _ItemId;
        public int ItemId
        {
            set { _ItemId = value; }
            get { return _ItemId; }
        }

        /// <summary>
        /// ϵͳ�˵� ����
        /// </summary>
        private string _ItemName;
        public string ItemName
        {
            set { _ItemName = value; }
            get { return _ItemName; }
        }

        /// <summary>
        /// ϵͳ�˵� ����ʶ
        /// </summary>
        private int _ParentItemId;
        public int ParentItemId
        {
            set { _ParentItemId = value; }
            get { return _ParentItemId; }
        }


        /// <summary>
        /// �������
        /// </summary>
        private int _OrderIndex;
        public int OrderIndex
        {
            set { _OrderIndex = value; }
            get { return _OrderIndex; }
        }

        /// <summary>
        /// ͼ���ַ
        /// </summary>
        private string _ItemImage;
        public string ItemImage
        {
            set { _ItemImage = value; }
            get { return _ItemImage; }
        }

        /// <summary>
        /// ָ��URL
        /// </summary>
        private string _ItemURL;
        public string ItemURL
        {
            set { _ItemURL = value; }
            get { return _ItemURL; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        private string _CreateDate;
        public string CreateDate
        {
            set { _CreateDate = value; }
            get { return _CreateDate; }
        }

        #endregion MXAdminSysItemModel
    }
}
