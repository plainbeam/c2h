using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Configuration;
using MySqlConnector;

namespace C2H.Data.Utility
{
    public class Database
    {
        public MySqlConnection m_conn;
        String MyConn = "";

        public Database(String con)
        {
            String connString = con;
            MyConn = con;
            m_conn = new MySqlConnection(connString);
            m_conn.Open(); 
        }

        public void close()
        {
            m_conn.Dispose();
            m_conn.Close(); 
        }

        public DataSet executeStatement(String sql)
        {
            try
            {
                DataSet data = new DataSet();
                MySqlCommand command = new MySqlCommand(sql, m_conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                command.CommandTimeout = 15000;
                adapter.SelectCommand = command;
                adapter.Fill(data);
                command.Dispose();
                adapter.Dispose();
                return data;
            }
            catch (Exception ex){

                throw new ApplicationException("Error running query: " + sql, ex);
            }
        }

        public DataSet executeStatement(String sql, Hashtable parameters)
        {
            try{
                
                DataSet data = new DataSet();
                MySqlCommand command = new MySqlCommand(sql, m_conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();

                foreach (DictionaryEntry entry in parameters){
                    
                    command.Parameters.AddWithValue(entry.Key.ToString(), entry.Value);
                }
                
                adapter.SelectCommand = command;
                adapter.Fill(data);


                command.Dispose();
                adapter.Dispose();
                return data;
            }
            catch (Exception ex){
               
                throw new ApplicationException("Error running query: " + sql, ex);
            }
        }

    }
}
