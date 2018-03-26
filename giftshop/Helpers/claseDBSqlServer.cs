using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;

namespace giftshop.Helpers
{
    public class claseDBSqlServer : IDisposable
    {
        private bool disposed = false;

        private string _connectionString = string.Empty;
        private string _connectionStringDefault = "Server=localhost;User Id=root;password=root;Database=Giftshop;";
        private string _procedureName = string.Empty;
        private List<SqlParameter> _parametersArray;
        private ArrayList _dataListArray;
        private int _bulkLoaderNumberOfLinesToSkip = 1;

        public string ConnectionString
        {
            get
            {
                if (_connectionString == string.Empty)
                {
                    return _connectionStringDefault;
                }
                return _connectionString;
            }
            set { _connectionString = value; }
        }

        public string Procedure
        {
            get { return _procedureName; }
            set
            {
                if (_parametersArray == null)
                {
                    _parametersArray = new List<SqlParameter>();
                }
                else
                {
                    _parametersArray.Clear();
                }
                _procedureName = value;
            }
        }


        public ArrayList DataListArray
        {
            get { return _dataListArray; }
        }

        public int BulkLoaderNumberOfLinesToSkip
        {
            get { return _bulkLoaderNumberOfLinesToSkip; }
            set { _bulkLoaderNumberOfLinesToSkip = value; }
        }

       


        public claseDBSqlServer()
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (_parametersArray != null) _parametersArray.Clear();
                if (_dataListArray != null) _dataListArray.Clear();
                _parametersArray = null;
                _dataListArray = null;
                _connectionString = null;
                _connectionStringDefault = null;
                _procedureName = null;
            }
            disposed = true;
        }

        ~claseDBSqlServer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void ClearDataListArray()
        {
            _dataListArray = new ArrayList();
        }        

        public void AddParameter(string name, object value)
        {
            SqlParameter parameterTmp = new SqlParameter();
            parameterTmp.ParameterName = name;
            parameterTmp.Value = value;
            parameterTmp.Direction = ParameterDirection.Input;
            _parametersArray.Add(parameterTmp);
        }

        public void AddParameter(string name, object value, DbType DBFieldType)
        {
            SqlParameter parameterTmp = new SqlParameter();
            parameterTmp.ParameterName = name;
            parameterTmp.Value = value;
            parameterTmp.Direction = ParameterDirection.Input;
            parameterTmp.DbType = DBFieldType;
            _parametersArray.Add(parameterTmp);
        }

        public void AddParameter(string name, object value, System.Data.ParameterDirection direction)
        {
            SqlParameter parameterTmp = new SqlParameter();
            parameterTmp.ParameterName = name;
            parameterTmp.Direction = direction;
            if (parameterTmp.Direction == ParameterDirection.Input | parameterTmp.Direction == ParameterDirection.InputOutput)
            {
                parameterTmp.Value = value;
            }
            _parametersArray.Add(parameterTmp);
        }

        public void AddParameter(string name, object value, System.Data.ParameterDirection direction, DbType DBFieldType)
        {
            SqlParameter parameterTmp = new SqlParameter();
            parameterTmp.ParameterName = name;
            parameterTmp.Direction = direction;
            parameterTmp.DbType = DBFieldType;
            if (parameterTmp.Direction == ParameterDirection.Input | parameterTmp.Direction == ParameterDirection.InputOutput)
            {
                parameterTmp.Value = value;
            }
            _parametersArray.Add(parameterTmp);
        }

        public object GetParameter(string name)
        {
            foreach (SqlParameter p in _parametersArray)
            {
                if (p.ParameterName == name)
                {
                    if (!(p.Value is DBNull))
                    {
                        return p.Value;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }
        

        public void ClearParameters()
        {
            _parametersArray.Clear();
        }

        public void ExecuteProcedureNonQuery()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(_procedureName, conn);
            cmd.CommandTimeout = 600;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            if ((_parametersArray != null))
            {
                foreach (SqlParameter p in _parametersArray)
                {
                    cmd.Parameters.Add(p);
                }
                _parametersArray.Clear();
            }
            conn.Open();
            cmd.ExecuteNonQuery();
            if ((cmd.Parameters != null))
            {
                foreach (SqlParameter p in cmd.Parameters)
                {
                    if (p.Direction == ParameterDirection.InputOutput | p.Direction == ParameterDirection.Output | p.Direction == ParameterDirection.ReturnValue)
                    {
                        _parametersArray.Add(p);
                    }
                }
            }
            conn.Close();
            conn.Dispose();
        }

        public DataRow ExecuteProcedureDataRow()
        {
            DataTable r = ExecuteProcedureDataTable("tmp");
            if (!(r.Rows.Count == 0))
            {
                return r.Rows[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable ExecuteProcedureDataTable()
        {
            return ExecuteProcedureDataTable("tmp");
        }

        public DataTable ExecuteProcedureDataTable(string tableName)
        {
            return ExecuteProcedureDataSet(tableName).Tables[tableName];
        }

        public DataSet ExecuteProcedureDataSet()
        {
            return ExecuteProcedureDataSet("tmp");
        }

        public List<Dictionary<string, object>> ExecuteProcedureDataList()
        {
            return GetDataList(ExecuteProcedureDataTable("tmp"));
        }

        public List<Dictionary<string, object>> GetDataList(DataTable dt)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows.Add(row);
            }
            return rows;
        }

        public DataSet ExecuteProcedureDataSet(string tableName)
        {
            DataSet dtTmp = new DataSet();
            SqlDataAdapter dataAdpt = new SqlDataAdapter();
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(_procedureName, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            if ((_parametersArray != null))
            {
                foreach (SqlParameter p in _parametersArray)
                {
                    cmd.Parameters.Add(p);
                }
                _parametersArray.Clear();
            }
            
            conn.Open();
            dataAdpt.SelectCommand = cmd;
            dataAdpt.Fill(dtTmp, tableName);
            if ((cmd.Parameters != null))
            {
                foreach (SqlParameter p in cmd.Parameters)
                {
                    if (p.Direction == ParameterDirection.InputOutput | p.Direction == ParameterDirection.Output | p.Direction == ParameterDirection.ReturnValue)
                    {
                        _parametersArray.Add(p);
                    }
                }
            }
            conn.Close();
            conn.Dispose();
            return dtTmp;
        }

        public void AddDataListArray(List<Dictionary<string, object>> DataList, string DataListName)
        {
            var o = new
            {
                DataName = DataListName,
                Data = DataList
            };
            _dataListArray.Add(o);
        }
    }
}