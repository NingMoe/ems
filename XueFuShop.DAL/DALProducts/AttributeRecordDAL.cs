using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.DAL
{
    public sealed class AttributeRecordDAL : IAttributeRecord
    {
        public void AddAttributeRecord(AttributeRecordInfo attributeRecord)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@attributeID", SqlDbType.Int), new SqlParameter("@productID", SqlDbType.Int), new SqlParameter("@value", SqlDbType.NVarChar) };
            pt[0].Value = attributeRecord.AttributeID;
            pt[1].Value = attributeRecord.ProductID;
            pt[2].Value = attributeRecord.Value;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "AddAttributeRecord", pt);
        }

        public void DeleteAttributeRecordByProductID(string strProductID)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@strProductID", SqlDbType.NVarChar) };
            pt[0].Value = strProductID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteAttributeRecordByProductID", pt);
        }

        public void PrepareAttributeRecordModel(SqlDataReader dr, List<AttributeRecordInfo> attributeRecordList)
        {
            while (dr.Read())
            {
                AttributeRecordInfo item = new AttributeRecordInfo();
                item.AttributeID = dr.GetInt32(0);
                item.ProductID = dr.GetInt32(1);
                item.Value = dr[2].ToString();
                attributeRecordList.Add(item);
            }
        }

        public List<AttributeRecordInfo> ReadAttributeRecordByProduct(int productID)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@productID", SqlDbType.Int) };
            pt[0].Value = productID;
            List<AttributeRecordInfo> attributeRecordList = new List<AttributeRecordInfo>();
            using (SqlDataReader reader = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadAttributeRecordByProduct", pt))
            {
                this.PrepareAttributeRecordModel(reader, attributeRecordList);
            }
            return attributeRecordList;
        }

        public List<AttributeRecordInfo> ReadList(string attributeID, string productID)
        {
            SqlParameter[] pt = new SqlParameter[] {
                new SqlParameter("@attributeID", SqlDbType.NVarChar),
                new SqlParameter("@productID", SqlDbType.NVarChar)
            };
            pt[0].Value = attributeID;
            pt[1].Value = productID;
            List<AttributeRecordInfo> attributeRecordList = new List<AttributeRecordInfo>();
            using (SqlDataReader reader = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadAttributeRecordList", pt))
            {
                this.PrepareAttributeRecordModel(reader, attributeRecordList);
            }
            return attributeRecordList;
        }

        public List<AttributeRecordInfo> ReadAttributeRecordAllList()
        {
            List<AttributeRecordInfo> attributeRecordList = new List<AttributeRecordInfo>();
            using (SqlDataReader reader = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadAttributeRecordAllList"))
            {
                this.PrepareAttributeRecordModel(reader, attributeRecordList);
            }
            return attributeRecordList;
        }
    }
}
