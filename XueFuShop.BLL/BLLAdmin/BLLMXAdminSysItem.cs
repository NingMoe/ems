using System;
using System.Text;
using System.Data;
using XueFuShop.DAL;
using XueFuShop.Models;

namespace XueFuShop.BLL
{
    public class BLLMXAdminSysItem
    {
        private DALAdminSysItem dal = new DALAdminSysItem();
        private MXAdminSysItem model = new MXAdminSysItem();
        private string _ErrMessage;//����Ҫ���صĴ�����Ϣ

        public string ErrMessage
        {
            get { return (this._ErrMessage); }
            set { this._ErrMessage = value; }
        }

        public DataTable GetItemInforByItemId(int ItemId)
        {
            DataSet ds = dal.GetItemInforByItemId(ItemId);
            return ds.Tables[0];
        }

        //public SqlDataReader GetItemInforByParentItemId(int ParentItemId)
        //{
        //    return dal.GetItemInforByParentId(ParentItemId);
        //}

        public DataTable GetItemInforByParentItemId(int ParentItemId)
        {
            DataSet ds = dal.GetItemInforByParentId(ParentItemId);
            return ds.Tables[0];
        }

        public bool UpdateItem(int ItemId)
        {
            model.ItemId = ItemId;
            int result = dal.UpdateItem(model);
            if (result > 0)
            {
                return true;
            }
            else
            {
                _ErrMessage = "���³����쳣��";
                return false;
            }
        }

        public bool DeleteItem(int ItemId)
        {
            model.ItemId = ItemId;
            int result = dal.DeleteItem(ItemId);
            if (result > 0)
            {
                return true;
            }
            else
            {
                _ErrMessage = "ɾ�������쳣��";
                return false;
            }
        }
    }
}
