using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using XueFuShop.Models;
using System.Web.UI;
using System.Web;

namespace XueFuShop.Common
{
    /// <summary>
    /// ��optgroup�������˵�
    /// </summary>
    public class NewDropDownList : DropDownList
    {
        //���캯��
        //public NewDropDownList(DataTable dt)
        //{
        //    ListItem item = new ListItem();
        //    foreach (DataRow row in dt.Rows)
        //    {                
        //        item.Value = row[0].ToString();
        //        item.Text = row[1].ToString();
        //        if (row[3].ToString() == "0")
        //            item.Attributes.Add("optgroup", item.Text);
        //        Items.Add(item);
        //    }
        //}

        public NewDropDownList(List<PostInfo> PostList)
        {
            Items.Add(new ListItem("��ѡ��ѧϰ��λ", "-1"));
            foreach (PostInfo row in PostList)
            {
                ListItem item = new ListItem();
                item.Value = row.PostId.ToString();
                item.Text = row.PostName;
                if (row.ParentId == 0) item.Attributes.Add("optgroup", item.Text);
                item.Selected = false;
                Items.Add(item);
            }
        }

        //public NewDropDownList(List<TrainingCateInfo> trainingCateList)
        //{
        //    Items.Add(new ListItem("��ѡ����ѵ���", "-1"));
        //    foreach (TrainingCateInfo info in trainingCateList)
        //    {
        //        ListItem item = new ListItem();
        //        item.Value = info.ID.ToString();
        //        item.Text = info.TrainingName;
        //        if (info.ParentID==0) item.Attributes.Add("optgroup", item.Text);
        //        item.Selected = false;
        //        Items.Add(item);
        //    }
        //}
        
        //��д����
        protected override void RenderContents(HtmlTextWriter writer)
        {
            string optgroup;
            ArrayList optOptionGroups = new ArrayList();
            foreach (ListItem item in this.Items)
            {
                

                if (item.Attributes["optgroup"] == null)
                {
                    RenderListItem(item, writer);
                }
                else
                {
                    optgroup = item.Attributes["optgroup"];
                    if (optOptionGroups.Contains(optgroup))
                    {
                        RenderListItem(item, writer);
                    }
                    else
                    {
                        if (optOptionGroups.Count > 0)
                        {
                            optgroupEndTag(writer);
                        }
                        optgroupBeginTag(optgroup, writer);
                        optOptionGroups.Add(optgroup);
                    }
                }
            }
            if (optOptionGroups.Count > 0)
            {
                optgroupEndTag(writer);
            }
        } 
        
        //option �����style
        private void RenderListItem(ListItem item, HtmlTextWriter writer)
        {
            writer.WriteBeginTag("option");
            writer.WriteAttribute("value", item.Value, true);
            //writer.WriteAttribute("style", "color:#7395c1", true);//�˴����Ը���ѡ���������ɫ
            if (item.Selected)
            {
                writer.WriteAttribute("selected", "selected", false);
            }
            foreach (string key in item.Attributes.Keys)
            {
                writer.WriteAttribute(key, item.Attributes[key]);
            }
            writer.Write(HtmlTextWriter.TagRightChar);
            HttpUtility.HtmlEncode(item.Text, writer);
            writer.WriteEndTag("option");
            writer.WriteLine(); 
        } 
        
        //option ���optgroup
        private void optgroupBeginTag(string name, HtmlTextWriter writer)
        {
            writer.WriteBeginTag("optgroup");
            writer.WriteAttribute("label", name);
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteLine();
        }


        private void optgroupEndTag(HtmlTextWriter writer)
        {
            writer.WriteEndTag("optgroup");
            writer.WriteLine();
        }
    }
}
