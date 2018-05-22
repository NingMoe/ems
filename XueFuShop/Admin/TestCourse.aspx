<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true"
    Codebehind="TestCourse.aspx.cs" Inherits="XueFuShop.Admin.TestCourse" Title="无标题页" %>

<%@ Import Namespace="XueFuShop.BLL" %>
<%@ Import Namespace="XueFuShop.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <script type="text/javascript" src="js/jquery.1.12.4.min.js"></script>

    <script type="text/javascript" src="My97DatePicker/WdatePicker.js"></script>

    <div class="position">
        <img src="Style/Images/PositionIcon.png" alt="" />课程列表</div>
    <ul class="search">
        <li>分类：<asp:DropDownList ID="ClassID" runat="server" />
            <%--品牌：<asp:DropDownList ID="BrandID" runat="server" />--%>
            关键字：<asp:TextBox CssClass="input" ID="Key" runat="server" Width="100px" title="课程名称，名称拼音首字母或者课程编码" />
            是否特价：<asp:DropDownList ID="IsSpecial" runat="server">
                <asp:ListItem Value="">全部</asp:ListItem>
                <asp:ListItem Value="0">否</asp:ListItem>
                <asp:ListItem Value="1">是</asp:ListItem>
            </asp:DropDownList>
            是否新品：<asp:DropDownList ID="IsNew" runat="server">
                <asp:ListItem Value="">全部</asp:ListItem>
                <asp:ListItem Value="0">否</asp:ListItem>
                <asp:ListItem Value="1">是</asp:ListItem>
            </asp:DropDownList>
            是否热销：<asp:DropDownList ID="IsHot" runat="server">
                <asp:ListItem Value="">全部</asp:ListItem>
                <asp:ListItem Value="0">否</asp:ListItem>
                <asp:ListItem Value="1">是</asp:ListItem>
            </asp:DropDownList>
            是否推荐：<asp:DropDownList ID="IsTop" runat="server">
                <asp:ListItem Value="">全部</asp:ListItem>
                <asp:ListItem Value="0">否</asp:ListItem>
                <asp:ListItem Value="1">是</asp:ListItem>
            </asp:DropDownList>
            <asp:Button CssClass="button" ID="SearchButton" Text=" 搜 索 " runat="server" OnClick="SearchButton_Click" />
        </li>
    </ul>
    <table class="listTable" cellpadding="0">
        <thead>
            <tr class="listTableHead">
                <th style="width: 3%">
                    ID</th>
                <th style="width: 5%">
                    图片</th>
                <th style="width: 25%; text-align: left; text-indent: 8px;" data-sort="string">
                    课程</th>
                <th style="width: 15%" data-sort="string">
                    分类</th>
                <th style="width: 5%">
                    更新时间</th>
                <th style="width: 2%">
                    视频</th>
                <th style="width: 2%">
                    题库</th>
                <th style="width: 4%">
                    试题数量</th>
                <th style="width: 2%">
                    特价</th>
                <th style="width: 2%">
                    新品</th>
                <th style="width: 2%">
                    热销</th>
                <th style="width: 2%">
                    推荐</th>
                <th style="width: 5%">
                    管理</th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="RecordList" runat="server">
                <ItemTemplate>
                    <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
                        <td>
                            <input type="checkbox" name="SelectID" value="<%# Eval("ID") %>" title="<%# Eval("ID") %>" /></td>
                        <td>
                            <%# string.IsNullOrEmpty(Eval("Photo").ToString()) || Eval("Photo").ToString() == "/" ? "<i class=\"iconfont icon-xuefu-nopic\" style=\"font-size:22px;\"></i>" : "<img class=\"smallphoto\" id=\"photo" + Eval("ID") + "\" data-origin=\"" + Eval("Photo").ToString() + "\" src=" + Eval("Photo").ToString().Replace("Original", "60-60") + "  onload=\"photoLoad(this,40,40)\"/>"%>
                        </td>
                        <td style="text-align: left; text-indent: 8px;">
                            <%# int.Parse(Eval("CompanyID").ToString()) > 0 ? "【" + CompanyBLL.ReadCompany(int.Parse(Eval("CompanyID").ToString())).CompanySimpleName + "】" : ""%>
                            <a href="/ProductDetail-I<%# Eval("ID") %>.aspx" target="_blank">
                                <%# Eval("Name") %>
                            </a>
                            <%# GetSpecialTestTime(int.Parse(Eval("CompanyID").ToString()), int.Parse(Eval("ID").ToString()))%>
                        </td>
                        <td>
                            <%# ProductClassBLL.ProductClassNameList(Eval("ClassID").ToString()) %>
                        </td>
                        <td>
                            <%# AttributeRecordBLL.ReadAttributeRecord(attributeRecordList, 3, int.Parse(Eval("ID").ToString())).Value%>
                        </td>
                        <td>
                            <%# CalcItemNum(Eval("ProductNumber").ToString(),'|') %>
                        </td>
                        <td>
                            <%# CalcItemNum(Eval("Accessory").ToString(),',')%>
                        </td>
                        <td>
                            <%# QuestionBLL.ReadQuestionNum(Eval("Accessory").ToString())%>
                        </td>
                        <td>
                            <span id="IsSpecial<%#Eval("ID") %>" style="cursor: pointer" onclick="updateStatus(<%#Eval("ID") %>,'IsSpecial')">
                                <%# XueFuShop.Common.ShopCommon.GetBoolString(Eval("IsSpecial"))%>
                            </span>
                        </td>
                        <td>
                            <span id="IsNew<%#Eval("ID") %>" style="cursor: pointer" onclick="updateStatus(<%#Eval("ID") %>,'IsNew')">
                                <%# XueFuShop.Common.ShopCommon.GetBoolString(Eval("IsNew"))%>
                            </span>
                        </td>
                        <td>
                            <span id="IsHot<%#Eval("ID") %>" style="cursor: pointer" onclick="updateStatus(<%#Eval("ID") %>,'IsHot')">
                                <%# XueFuShop.Common.ShopCommon.GetBoolString(Eval("IsHot"))%>
                            </span>
                        </td>
                        <td>
                            <span id="IsTop<%#Eval("ID") %>" style="cursor: pointer" onclick="updateStatus(<%#Eval("ID") %>,'IsTop')">
                                <%# XueFuShop.Common.ShopCommon.GetBoolString(Eval("IsTop"))%>
                            </span>
                        </td>
                        <td>
                            <a href="javascript:pop('TestCourseAdd.aspx?ID=<%# Eval("ID") %>',1000,600,'<%# Eval("Name") %>','TestCourseAdd')"
                                alt="修改" title="修改"><i class="iconfont icon-bianji"></i></a><a href="javascript:deleteProduct(<%#Eval("ID") %>)"
                                    alt="删除" title="删除"><i class="iconfont icon-iconfontshanchu"></i></a><a href="javascript:popPageOnly('QuestionXls.aspx?CateId=<%#Eval("ID") %>&Action=TestCateFileOut',800,220,'试题导出','QuestionAdd25')"
                                        alt="导出试题" title="导出试题"><i class="iconfont icon-excelbiaodaochu"></i></a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <div class="listPage">
        <XueFu:CommonPager ID="MyPager" runat="server" />
    </div>
    <div class="action">
        <input type="button" value=" 添 加 " class="button" onclick="pop('TestCourseAdd.aspx',1000,600,'课程添加','TestCourseAdd')" />&nbsp;
        <input type="button" value="课程导出" class="button" onclick="popPageOnly('ProductExport.aspx?<%=HttpContext.Current.Request.QueryString%>',500,330,'课程导出','ProductAdd')" />&nbsp;
        <%if (isSale == 2)
      {%>
        <asp:Button CssClass="button" ID="OnSaleButton" Text=" 上 架 " OnClientClick="return checkSelect()"
            runat="server" OnClick="OnSaleButton_Click" />&nbsp;<%} %>
        <asp:Button CssClass="button" ID="OffSaleButton" Text=" 下 架 " OnClientClick="return checkSelect()"
            runat="server" OnClick="OffSaleButton_Click" />&nbsp;
        <input type="button" id="SetTimeButton" style="width: 90px;" value="设置考试参数" class="button" />&nbsp;
        <input type="checkbox" name="All" onclick="selectAll(this)" />全选/取消
    </div>
    <textarea id="TestSettingContent" style="display: none;">    <div class="add">		    <ul>		    <li class="left">考试时间：</li>		    <li class="right">			    <input id="TestStartTime" name="TestStartTime" class="input Wdate" value="" style="width:140px" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss',readOnly:true,maxDate:'#F{$dp.$D(\'TestEndTime\')}'})" /> 至 			    <input id="TestEndTime" name="TestEndTime" class="input Wdate" value="" style="width:140px" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss',readOnly:true,minDate:'#F{$dp.$D(\'TestStartTime\')}'})" />		    </li>	    </ul>	    <ul>		    <li class="left">试卷总分：</li>		    <li class="right">		        <input id="PaperScore" name="PaperScore" class="input" value="" />	        </li>	    </ul>	    <ul>		    <li class="left">考试时长：</li>		    <li class="right">		        <input id="TestTimeLength" name="TestTimeLength" class="input" value="" />		        		    </li>	    </ul>	    <ul>		    <li class="left">试题数量：</li>		    <li class="right">		        <input id="QuestionsNum" name="QuestionsNum" class="input" value="" />		    </li>	    </ul>	    <ul>		    <li class="left">及格分数：</li>		    <li class="right">		        <input id="LowScore" name="LowScore" class="input" value="" />		    </li>	    </ul>	    <div class="action">            <input type="button" id="SettingOK"  value=" 确定 " class="button"/>        </div>    </div></textarea>
    <style>.smallphoto { cursor: pointer;}.layui-layer-tips { width: auto !important; height: 205px;}.layui-layer-content { padding: 8px !important;}.layui-layer-msg .layui-layer-content { color: #fff;}</style>

    <script type="text/javascript" src="js/ProductAdd.js"></script>

    <script type="text/javascript" src="js/stupidtable.min.js"></script>

    <script type="text/javascript">$(function() {    $("[data-sort]").closest("table").stupidtable();    $(".smallphoto").click(function(){        layer.tips("<img src='"+$(this).data("origin")+"' >", "#"+$(this).attr("id"), {            tips: [1, '#DFEFFE'], //还可配置颜色        });    })    $("#SetTimeButton").click(function(){    	var $selectID=$("[name='SelectID']:checked");    	if($selectID.length>0){	        layer.open({	          type: 1,	          title:"设置考试参数",	          skin: 'layui-layer-rim', //加上边框	          area: ['600px', '350px'], //宽高	          content: $("#TestSettingContent").val()	        });        }    	else{    		layer.msg('请选择课程！');    	}    });    $(document).on("click","#SettingOK",function(){    	var $selectID=$("[name='SelectID']:checked");		var selectID = [],        paperScore = $("#PaperScore").val(),        testTimeLength = $("#TestTimeLength").val(),        testQuestionsCount = $("#QuestionsNum").val(),        lowScore = $("#LowScore").val(),        testStartTime = $("#TestStartTime").val(),        testEndTime = $("#TestEndTime").val();        console.log(paperScore+"|"+testTimeLength+"|"+testQuestionsCount+"|"+lowScore+"|"+testStartTime+"|"+testEndTime);        if(paperScore == "" && testTimeLength == "" && testQuestionsCount == "" && lowScore == "" && testStartTime == "" && testEndTime == ""){        	if(!confirm("确定要清空课程的考试规则吗(清空后将使用系统默认的规则)?")){        		return false;        	}        }    	$selectID.each(function(index, el) {		    selectID.push($(el).val())	    })	    selectID = selectID.join();        $.post("Ajax.aspx?Action=UpdateTestSetting",{SelectID:selectID,PaperScore:paperScore,TestTimeLength:testTimeLength,TestQuestionsCount:testQuestionsCount,LowScore:lowScore,TestStartTime:testStartTime,TestEndTime:testEndTime},            function(data){            data=JSON.parse(data);            if(data.Success){            	layer.closeAll();            	layer.msg('设置成功！');            }            else{                layer.msg(data.Msg);            }        })      })})</script>

</asp:Content>
