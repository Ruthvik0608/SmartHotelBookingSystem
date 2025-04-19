using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotelBookingSystem.DataAccess.ADO
{
    public class DB1
    {
        static SqlConnection connection = new SqlConnection("Server=localhost;Database=SmartHotelDB;Integrated Security=True;TrustServerCertificate=true");

        #region Connection
        public SqlConnection GetConnection()
        {
            connection.Open();
            return connection;
        }
        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

        }
        #endregion

        #region Private Methods 
        private SqlParameter createSqlParameter(string name, object value)
        {
            SqlParameter objSqlParameter = new SqlParameter(name, value);
            return objSqlParameter;
        }
        public DataSet ConvertDataReaderToDataSet(SqlDataReader reader)
        {
            DataSet dataSet = new DataSet();
            do
            {
                // Create new data table

                DataTable schemaTable = reader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    // A query returning records was executed

                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        // Create a column name that is unique in the data table
                        string columnName = (string)dataRow["ColumnName"]; //+ "<C" + i + "/>";
                                                                           // Add the column definition to the data table
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }

                    dataSet.Tables.Add(dataTable);

                    // Fill the data table we just created

                    while (reader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < reader.FieldCount; i++)
                            dataRow[i] = reader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                else
                {
                    // No records were returned

                    DataColumn column = new DataColumn("RowsAffected");
                    dataTable.Columns.Add(column);
                    dataSet.Tables.Add(dataTable);
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = reader.RecordsAffected;
                    dataTable.Rows.Add(dataRow);
                }
            }
            while (reader.NextResult());
            return dataSet;
        }
        #endregion

        #region SQL Specific
        public int InsertUpdateOrDelete(string QueryOrSPName, nameValuePairList NameValuePairObject, bool IsSP = false)
        {
            SqlCommand cmdObject = new SqlCommand(QueryOrSPName, GetConnection());

            if (IsSP)
            {
                cmdObject.CommandType = CommandType.StoredProcedure;
            }

            foreach (nameValuePair objList in NameValuePairObject)
            {
                cmdObject.Parameters.Add(createSqlParameter(objList.ColName, objList.Value));
            }

            int status = 0;

            try
            {
                status = cmdObject.ExecuteNonQuery();

                return status;
            }
            catch (Exception exp)
            {
                return status;
            }
            finally
            {
                CloseConnection();
            }


        }
        public int InsertUpdateOrDelete(string InsertQuery)
        {
            SqlCommand cmdObject = new SqlCommand(InsertQuery, GetConnection());

            int status = 0;

            try
            {
                status = cmdObject.ExecuteNonQuery();
                if (connection.State == ConnectionState.Open)
                {
                    CloseConnection();
                }
                return status;
            }
            catch (Exception exp)
            {
                return status;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    CloseConnection();
                }
            }


        }

        public object FetchCount(string Query)
        {
            SqlCommand sqlCommand = new SqlCommand(Query, GetConnection());

            object Count = sqlCommand.ExecuteScalar();
            return Count;
        }
        public DataSet FillMultipleTables(string FirstQuery, string SecondQuery)
        {
            string Query = FirstQuery + ";" + SecondQuery;
            SqlCommand cmdObject = new SqlCommand(Query, GetConnection());
            SqlDataAdapter adapter = new SqlDataAdapter(cmdObject);
            DataSet dsQuery = new DataSet();
            adapter.Fill(dsQuery);
            CloseConnection();
            return dsQuery;
        }
        public DataTable FillAndReturnDataTable(string SelectQuery, nameValuePairList nvp)
        {

            SqlDataAdapter adp = new SqlDataAdapter(SelectQuery, GetConnection());
            DataTable dt = new DataTable();
            adp.Fill(dt);
            CloseConnection();
            return dt;

        }
        public DataTable FillAndReturnDataSet(string Query, nameValuePairList NameValuePairObject)
        {
            try
            {
                SqlCommand CommandObject = new SqlCommand(Query, GetConnection());

                //CommandObject.CommandType = CommandType.StoredProcedure;

                foreach (nameValuePair objList in NameValuePairObject)
                {
                    CommandObject.Parameters.Add(createSqlParameter(objList.ColName, objList.Value));
                }

                SqlDataReader ReaderObject = CommandObject.ExecuteReader();

                DataSet ds = ConvertDataReaderToDataSet(ReaderObject);

                if (ds.Tables.Count > 0)
                    return ds.Tables[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataSet FillAndReturnDataSet(string Query)
        {
            SqlDataAdapter adp = new SqlDataAdapter(Query, GetConnection());
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds;
        }

        public DataTable FetchData(string query, List<SqlParameter> parameters = null)
        {
            SqlCommand cmd = new SqlCommand(query, GetConnection());

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);
                }
            }

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();

            try
            {
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return dataTable;
        }

        internal DataTable FetchData(string query, nameValuePairList nvp)
        {
            SqlCommand cmd = new SqlCommand(query, GetConnection());

            foreach (nameValuePair objList in nvp)
            {
                cmd.Parameters.Add(createSqlParameter(objList.ColName, objList.Value));
            }

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();

            try
            {
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return dataTable;
        }

        #endregion
    }

    #region Name Value Pair and List...
    public class nameValuePairList : List<Dictionary<string, object>>
    {

    }


    public class nameValuePair : Dictionary<string, object>
    {
        public string ColName { get; set; }
        public object Value { get; set; }
        public nameValuePair(string Name, object ColValue)
        {
            ColName = Name;
            Value = ColValue;
        }
    }
    #endregion
}
