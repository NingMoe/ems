<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductAjax.aspx.cs" Inherits="XueFuShop.Admin.ProductAjax" %>
<%@ Import Namespace="XueFuShop.BLL" %>
<%@ Import Namespace="XueFuShop.Common" %>
<%@ Import Namespace="XueFuShop.Models" %>
 <select name='<%=NamePrefix %><%=controlName %>' id='<%=IDPrefix%><%=controlName %>' <%=cssContent %> <%=dobuleClickContent %>>
 <%if ("SearchProductAccessory,SearchRelationProduct,SearchProductByName".IndexOf(action) > -1)
   {
        foreach (ProductInfo product in productList){ %>
        <option value="<%=product.ID %>"><%=product.Name %></option>
        <%}
    }
    else if (action == "SearchProductQuestionBank")
   {
       foreach (CourseInfo course in courseList){ %>
    <option value="<%=course.CourseId %>"><%=course.CourseName%></option>
      <%}
   } 
   else if (action == "SearchRelationArticle")
   {
       foreach (ArticleInfo article in articleList){ %>
    <option value="<%=article.ID %>"><%=article.Title%></option>
      <%}
   } 
    else if (action == "SearchUser")
   {
       foreach (UserInfo user in userList){ %>
    <option value="<%=user.ID %>|<%=user.UserName%>"><%=user.UserName%></option>
      <%}
   }
   %>
</select>