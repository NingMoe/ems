using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

/// <summary>
/// SQLHelper类封装对SQL Server数据库的添加、删除、修改和选择等操作
/// </summary>
public class SQLHelper
{
	/// 连接数据源
	private SqlConnection conn = null;
	private readonly string RETURNVALUE = "RETURNVALUE";
	//记录错误日志位置
	private static string m_fileName = "c:\\Systemlog.txt";

	/// <summary>
	/// 打开数据库连接.
	/// </summary>
	private void Open()
	{
		// 打开数据库连接
		if(conn == null)
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString);
		}

		if(conn.State == ConnectionState.Closed)
		{
			try
			{
				///打开数据库连接
				conn.Open();
			}
			catch(Exception ex)
			{
				SQLHelper.CreateErrorMsg(ex.Message);
			}
			finally
			{
				///关闭已经打开的数据库连接				
			}
		}
	}

	/// <summary>
	/// 关闭数据库连接
	/// </summary>
	public void Close()
	{
		///判断连接是否已经创建
		if(conn != null)
		{
			///判断连接的状态是否打开
			if(conn.State == ConnectionState.Open)
			{
				conn.Close();
			}
		}
	}

	/// <summary>
	/// 释放资源
	/// </summary>
	public void Dispose()
	{
		// 确认连接是否已经关闭
		if(conn != null)
		{
			conn.Dispose();
			conn = null;
		}
	}

	/// <summary>
	/// 执行存储过程
	/// </summary>
	/// <param name="procName">存储过程的名称</param>
	/// <returns>返回存储过程返回值</returns>
	public int RunProc(string procName)
	{
		SqlCommand cmd = CreateProcCommand(procName,null);
		///执行存储过程
		cmd.ExecuteNonQuery();
		///关闭数据库的连接
		Close();

		///返回存储过程的参数值
		return (int)cmd.Parameters[RETURNVALUE].Value;
	}

	/// <summary>
	/// 执行存储过程
	/// </summary>
	/// <param name="procName">存储过程名称</param>
	/// <param name="prams">存储过程所需参数</param>
	/// <returns>返回存储过程返回值</returns>
	public int RunProc(string procName,SqlParameter[] prams)
	{
		SqlCommand cmd = CreateProcCommand(procName,prams);
		///执行存储过程
		cmd.ExecuteNonQuery();
		///关闭数据库的连接
		Close();

		///返回存储过程的参数值
		return (int)cmd.Parameters[RETURNVALUE].Value;
	}

	/// <summary>
	/// 执行存储过程
	/// </summary>
	/// <param name="procName">存储过程的名称</param>
    /// <param name="dataReader">返回DataReader对象</param>
	public void RunProc(string procName,out SqlDataReader dataReader)
	{
		///创建Command
		SqlCommand cmd = CreateProcCommand(procName,null);

		///读取数据
		dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
	}

	/// <summary>
	/// 执行存储过程
	/// </summary>
	/// <param name="procName">存储过程的名称</param>
	/// <param name="prams">存储过程所需参数</param>
    /// <param name="dataReader">返回DataReader对象</param>
	public void RunProc(string procName,SqlParameter[] prams,out SqlDataReader dataReader)
	{
		///创建Command
		SqlCommand cmd = CreateProcCommand(procName,prams);

		///读取数据
		dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
	}

	/// <summary>
	/// 执行存储过程
	/// </summary>
	/// <param name="procName">存储过程的名称</param>
	/// <param name="dataSet">返回DataSet对象</param>
	public void RunProc(string procName,ref DataSet dataSet)
	{
		if(dataSet == null)
		{
			dataSet = new DataSet();
		}
		///创建SqlDataAdapter
		SqlDataAdapter da = CreateProcDataAdapter(procName,null);

		///读取数据
		da.Fill(dataSet);
		///关闭数据库的连接
		Close();
	}

	/// <summary>
	/// 执行存储过程
	/// </summary>
	/// <param name="procName">存储过程的名称</param>
	/// <param name="prams">存储过程所需参数</param>
	/// <param name="dataSet">返回DataSet对象</param>
	public void RunProc(string procName,SqlParameter[] prams,ref DataSet dataSet)
	{
		if(dataSet == null)
		{
			dataSet = new DataSet();
		}
		///创建SqlDataAdapter
		SqlDataAdapter da = CreateProcDataAdapter(procName,prams);

		///读取数据
		da.Fill(dataSet);
		///关闭数据库的连接
		Close();
	}

	
	/// <summary>
	/// 创建一个SqlCommand对象以此来执行存储过程
	/// </summary>
	/// <param name="procName">存储过程的名称</param>
	/// <param name="prams">存储过程所需参数</param>
	/// <returns>返回SqlCommand对象</returns>
	private SqlCommand CreateProcCommand(string procName,SqlParameter[] prams)
	{
		///打开数据库连接
		Open();

		///设置Command
		SqlCommand cmd = new SqlCommand(procName,conn);
		cmd.CommandType = CommandType.StoredProcedure;

		///添加把存储过程的参数
		if(prams != null)
		{
			foreach(SqlParameter parameter in prams)
			{
				cmd.Parameters.Add(parameter);
			}
		}

		///添加返回参数ReturnValue
		cmd.Parameters.Add(
			new SqlParameter(RETURNVALUE,SqlDbType.Int,4,ParameterDirection.ReturnValue,
			false,0,0,string.Empty,DataRowVersion.Default,null));

		///返回创建的SqlCommand对象
		return cmd;
	}	

	/// <summary>
	/// 创建一个SqlDataAdapter对象，用此来执行存储过程
	/// </summary>
	/// <param name="procName">存储过程的名称</param>
	/// <param name="prams">存储过程所需参数</param>
	/// <returns>返回SqlDataAdapter对象</returns>
	private SqlDataAdapter CreateProcDataAdapter(string procName,SqlParameter[] prams)
	{
		///打开数据库连接
		Open();

		///设置SqlDataAdapter对象
		SqlDataAdapter da = new SqlDataAdapter(procName,conn);
		da.SelectCommand.CommandType = CommandType.StoredProcedure;

		///添加把存储过程的参数
		if(prams != null)
		{
			foreach(SqlParameter parameter in prams)
			{
				da.SelectCommand.Parameters.Add(parameter);
			}
		}

		///添加返回参数ReturnValue
		da.SelectCommand.Parameters.Add(
			new SqlParameter(RETURNVALUE,SqlDbType.Int,4,ParameterDirection.ReturnValue,
			false,0,0,string.Empty,DataRowVersion.Default,null));

		///返回创建的SqlDataAdapter对象
		return da;
	}
	
	/// <summary>
	/// 生成存储过程参数
	/// </summary>
	/// <param name="ParamName">存储过程名称</param>
	/// <param name="DbType">参数类型</param>
	/// <param name="Size">参数大小</param>
	/// <param name="Direction">参数方向</param>
	/// <param name="Value">参数值</param>
	/// <returns>新的 parameter 对象</returns>
	public SqlParameter CreateParam(string ParamName,SqlDbType DbType,Int32 Size,ParameterDirection Direction,object Value)
	{
		SqlParameter param;

		///当参数大小为0时，不使用该参数大小值
		if(Size > 0)
		{
			param = new SqlParameter(ParamName,DbType,Size);
		}
		else
		{
			///当参数大小为0时，不使用该参数大小值
			param = new SqlParameter(ParamName,DbType);
		}

		///创建输出类型的参数
		param.Direction = Direction;
		if(!(Direction == ParameterDirection.Output && Value == null))
		{
			param.Value = Value;
		}

		///返回创建的参数
		return param;
	}

	/// <summary>
	/// 传入输入参数
	/// </summary>
	/// <param name="ParamName">存储过程名称</param>
	/// <param name="DbType">参数类型</param></param>
	/// <param name="Size">参数大小</param>
	/// <param name="Value">参数值</param>
	/// <returns>新的parameter 对象</returns>
	public SqlParameter CreateInParam(string ParamName,SqlDbType DbType,int Size,object Value)
	{
		return CreateParam(ParamName,DbType,Size,ParameterDirection.Input,Value);
	}

	/// <summary>
	/// 传入返回值参数
	/// </summary>
	/// <param name="ParamName">存储过程名称</param>
	/// <param name="DbType">参数类型</param>
	/// <param name="Size">参数大小</param>
	/// <returns>新的 parameter 对象</returns>
	public SqlParameter CreateOutParam(string ParamName,SqlDbType DbType,int Size)
	{
		return CreateParam(ParamName,DbType,Size,ParameterDirection.Output,null);
	}

	/// <summary>
	/// 传入返回值参数
	/// </summary>
	/// <param name="ParamName">存储过程名称</param>
	/// <param name="DbType">参数类型</param>
	/// <param name="Size">参数大小</param>
	/// <returns>新的 parameter 对象</returns>
	public SqlParameter CreateReturnParam(string ParamName,SqlDbType DbType,int Size)
	{
		return CreateParam(ParamName,DbType,Size,ParameterDirection.ReturnValue,null);
	}

	/// <summary>
		/// 将DataReader转为DataTable
		/// </summary>
		/// <param name="DataReader">DataReader</param>
	public static DataTable ConvertdrTodt(SqlDataReader dataReader)
	{
		///定义DataTable
		DataTable datatable = new DataTable();

		///动态添加表的数据列
		for(int i = 0; i < dataReader.FieldCount; i++)
		{
			DataColumn mydc = new DataColumn();
			mydc.DataType = dataReader.GetFieldType(i);
			mydc.ColumnName = dataReader.GetName(i);
			datatable.Columns.Add(mydc);
		}

		///添加表的数据
		while(dataReader.Read())
		{
			DataRow mydr = datatable.NewRow();
			for(int i = 0; i < dataReader.FieldCount; i++)
			{
				mydr[i] = dataReader[i].ToString();
			}
			datatable.Rows.Add(mydr);
			mydr = null;
		}
		///关闭数据读取器
		dataReader.Close();
		return datatable;
	}

	/// <summary>
	/// 记录日志至文本文件
	/// </summary>
	/// <param name="message">记录的内容</param>
	public static void CreateErrorMsg(string message)
	{
		//if(File.Exists(m_fileName))
		//{
		//    ///如果日志文件已经存在，则直接写入日志文件
		//    StreamWriter sr = File.AppendText(FileName);
		//    sr.WriteLine("\n");
		//    sr.WriteLine(DateTime.Now.ToString() + message);
		//    sr.Close();
		//}
		//else
		//{
		//    ///创建日志文件
		//    StreamWriter sr = File.CreateText(FileName);
		//    sr.Close();
		//}	
	}
}
