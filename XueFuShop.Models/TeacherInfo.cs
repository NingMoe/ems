using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace XueFuShop.Models
{
    [Table("Teacher")] 
    public sealed class TeacherInfo : BaseModel
    {
        private string name;
        private SexType sex;
        private string photo;
        private string introduction;

        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// �Ա�
        /// </summary>
        public SexType Sex
        {
            get { return this.sex; }
            set { this.sex = value; }
        }

        /// <summary>
        /// ��Ƭ·��
        /// </summary>
        public string Photo
        {
            get { return this.photo; }
            set { this.photo = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Introduction
        {
            get { return this.introduction; }
            set { this.introduction = value; }
        }
    }
}
