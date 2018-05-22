using System;
using System.Collections.Generic;
using System.Text;

namespace XueFuShop.Models.DTO
{
    public sealed class CourseDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ClassID { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public string Url { get; set; }
        public string SUrl { get; set; }
        public string RCUrl { get; set; }
        public string Teacher { get; set; }
        public string Summary { get; set; }
        public bool IsTest { get; set; }
    }
}
