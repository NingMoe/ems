using System;
using System.Collections.Generic;

using System.Text;

namespace XueFuShop.Models
{
    /// <summary>
    /// 实体类MXAddress。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
   public class MXAddress
    {
       
        /// <summary>
        /// 送货地址ID
        /// </summary>
       private int _AddressId;
       public int AddressId
       {
           get { return _AddressId; }
           set { _AddressId = value; }
       }
       /// <summary>
       /// 会员ID
       /// </summary>
       private int _UserId;
       public int UserId
       {
           get { return _UserId; }
           set { _UserId = value; }
       }
       /// <summary>
       /// 收货电话
       /// </summary>
       private string _Tel;
       public string Tel
       {
           get { return _Tel; }
           set { _Tel = value; }
       }
       /// <summary>
       /// 收货手机
       /// </summary>
       private string _Mobile;
       public string Mobile
       {
           get { return _Mobile; }
           set { _Mobile = value; }
       }
       /// <summary>
       /// 收货地址
       /// </summary>
       private string _Address;
       public string Address
       {
           get { return _Address; }
           set { _Address = value; }
       }
       /// <summary>
       /// 收货名字
       /// </summary>
       private string _Name;
       public string Name
       {
           get { return _Name; }
           set { _Name = value; }
       }
       /// <summary>
       /// 收货邮编
       /// </summary>       
       private string _Post;
       public string Post
       {
           get { return _Post; }
           set { _Post = value; }
       }

    }
}
