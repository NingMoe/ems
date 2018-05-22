using System;

namespace XueFuShop.Models
{
    /// <summary>
    /// ʵ����MXItem��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    public class Item
    {
        #region ItemModel

        /// <summary>
        /// 
        /// </summary>
        private int _MXItemId;
        public int ItemId
        {
            set { _MXItemId = value; }
            get { return _MXItemId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _ItemName;
        public string ItemName
        {
            set { _ItemName = value; }
            get { return _ItemName; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _ParentId;
        public int ParentId
        {
            set { _ParentId = value; }
            get { return _ParentId; }
        }


        /// <summary>
        /// 
        /// </summary>
        private int _OrderNum;
        public int OrderNum
        {
            set { _OrderNum = value; }
            get { return _OrderNum; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _State;
        public int State
        {
            set { _State = value; }
            get { return _State; }
        }
       
        #endregion ItemModel
    }
}
