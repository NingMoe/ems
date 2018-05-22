<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JiaoHao.aspx.cs" Inherits="XueFuShop.JiaoHao" %>
<!DOCTYPE html>
<html lang="zh-CN">
<head>
  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>转饭抽号</title>
  <link href="http://cdn.bootcss.com/bootstrap/3.3.6/css/bootstrap.css" rel="stylesheet">
  <style type="text/css">
    body {
      font-family: "微软雅黑", "宋体";
      -webkit-text-size-adjust: 100%;
      -ms-text-size-adjust: 100%;
      color: #575755;
      font-size: 16px;
      padding-top: 30px;
    }
    h3,h4 {
      margin: 5px;
    }
    .kouhao { 
      border-top: 1px solid #b2acac;
      border-bottom: 1px solid #b2acac;
      padding: 10px 20px;
      width: 250px;
      margin-top: 10px;
      margin-bottom: 10px;
    }
    .rule {
      margin-top: 50px;
    }
   .rule dd {
      line-height: 35px;
      margin-left: 10px;
    }
    .rule dd span,h3,h4 { color: #fc3651;  }
    .rule dt {
	  font-weight: normal;
	  border-radius: 20px;
	  background: #fc3651;
	  color: #fff;
	  padding: 3px 20px;
	  margin-top: 15px;
	  margin-bottom: 10px;
	  width: 110px;
    }
    .inlineblock { display: inline-block; }
    #UserName { width: 150px;}
    @media screen and (max-width:640px){
      #UserName { width: 200px;}
      .form-group { margin-bottom: 0; }
    }
    @media screen and (max-width:400px){
      #UserName { width: 180px;}
    }
    @media screen and (max-width:360px){
      #UserName { width: 170px;}
      .form-group { margin-bottom: 0; }
    }
    @media screen and (max-width:320px){
      #UserName { width: 155px;}
      .form-group { margin-bottom: 0; }
    }
    </style>
</head>
<body>
  <div class="container-fluid col-sm-offset-2 col-sm-8">
    <div class="">
  		<h4>感恩微波炉</h4>
  		<h3 class="tips"><asp:Literal ID="Tips" runat="server"></asp:Literal></h3>
  	</div>
  	<%if(optionType==1) {%>
    <form class="form-inline row" action="" method="post">
      <div class="form-group col-xs-6 col-lg-2">
        <label class="sr-only" for="UserName">姓名</label>
        <input type="text" class="form-control" id="UserName" name="UserName" placeholder="姓名">
      </div>
      <div class="col-xs-2">
        <button type="submit" class="btn btn-default">报名</button>
      </div>
    </form>
    <%} %>
  	<div class="kouhao">一样的号码牌，不一样的感觉</div>
    <%if(optionType==2) {%>
    <div class="rule">
      <dl>
        <dt>抽号结果</dt>
        <%=resultNameList %>
      </dl>
    </div>
    <%}%>
    <div class="rule">
      <dl>
        <dt class="inlineblock">使用时间</dt>
        <dd class="inlineblock"><span>工作日11:30 — 13:00</span></dd>
        <dt>使用规则</dt>
        <dd><span>01/</span>每天上午11:30分起开始报名。</dd>
        <dd><span>02/</span>每天上午11:45分开始随机排序。</dd>
        <dd><span>03/</span>请按生成顺序逐个进行。</dd>
        <dd><span>04/</span>错过报名时间的人员，请自觉排在最后。</dd>
      </dl>
    </div>
  </div>
</body>
</html>
