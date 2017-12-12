using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Logging
{
    public class Log
    {
       private static readonly string connectionString = null;
       static Log()
       {
           connectionString = ConfigurationManager.ConnectionStrings["LoggingDB"].ConnectionString;
           if (String.IsNullOrEmpty(connectionString))
               throw new ApplicationException("Connection string has not been set.");
       }

       public static void Create(string message, LogEntryLevel level)
       {
           Create(message, level, null);
       }

       public static void Create(string message, LogEntryLevel level, System.Exception exception)
       {
           Create(message, level, exception,null );
       }


       public static void Create(string message, LogEntryLevel level, System.Exception exception, params LogEntryAttribute[] logEntries)
       {
           Create(new LogEntry(message, level, exception, logEntries));
       }
       public static void Create(LogEntry le)
       {
           if (le != null)
               WriteToDB(le);
       }

       private static void WriteToDB(LogEntry le)
       {
           //if (connection == null)
           //    throw new ApplicationException("Connection to the Logging database hasn't been established yet");

           try
           {
               using (MySqlConnection connection = new MySqlConnection(connectionString))
               {
                   connection.Open();

                   MySqlCommand command = new MySqlCommand("sp_LogEntry_Ins", connection);
                   command.CommandType = System.Data.CommandType.StoredProcedure;
             
                   command.Parameters.Add(new MySqlParameter("@pMessage", MySqlDbType.VarChar)).Value = le.Message;
                   command.Parameters.Add(new MySqlParameter("@pLevel", MySqlDbType.Int32)).Value = (int)le.Level;
                   command.Parameters.Add(new MySqlParameter("@pExceptionText", MySqlDbType.Text)).Value = le.ExceptionText;
                   command.Parameters.Add(new MySqlParameter("@pId", MySqlDbType.VarChar)).Direction = System.Data.ParameterDirection.Output;

                   command.ExecuteNonQuery();
                   int logEntryId = int.Parse(command.Parameters["@pId"].Value.ToString());
                   le.ID = logEntryId;
               }
              
               WriteLogEntryAttributes(le);
           }
           catch
           {
               //throw;
           }
       }

       private static void WriteLogEntryAttributes(LogEntry le)
       {
           if (le.Attributes == null || le.Attributes.Count == 0)
               return;

           try
           {
               using (MySqlConnection connection = new MySqlConnection(connectionString))
               {
                   connection.Open();

                   MySqlCommand command = new MySqlCommand("sp_LogEntryAttribute_Ins", connection);
                   command.CommandType = System.Data.CommandType.StoredProcedure;

                   command.Parameters.Add(new MySqlParameter("@pLogEntryId", MySqlDbType.Int32));
                   command.Parameters.Add(new MySqlParameter("@pName", MySqlDbType.VarChar));
                   command.Parameters.Add(new MySqlParameter("@pValue", MySqlDbType.VarChar));
                   command.Parameters.Add(new MySqlParameter("@pId", MySqlDbType.Int32));

                   foreach (LogEntryAttribute lea in le.Attributes)
                   {
                       command.Parameters["@pLogEntryId"].Value = le.ID;
                       command.Parameters["@pName"].Value = lea.Name;
                       command.Parameters["@pValue"].Value = lea.Value;
                       command.ExecuteNonQuery();
                   }

               }
           }
           catch
           {
               throw;
           }
       }
    }
}
