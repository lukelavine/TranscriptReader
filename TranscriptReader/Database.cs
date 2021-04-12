using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace TranscriptReader
{
    //Should probably add some try catch loops here
    static class Database
    {
        public static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = new SQLiteConnection("Data Source=classes.db;Version=3;");
            try
            {
                sqlite_conn.Open();
                Console.WriteLine("Connection Succesful");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return sqlite_conn;

        }

        public static void SetUpDatabase(SQLiteConnection conn)
        {
            SQLiteCommand sQLiteCommand;
            string createTable = @"CREATE TABLE Classes(
                District_Code CHARACTER(6) PRIMARY KEY, 
                Name VARCHAR(64),
                Credit VARCHAR(64)
            )";
            sQLiteCommand = conn.CreateCommand();
            sQLiteCommand.CommandText = createTable;
            sQLiteCommand.ExecuteNonQuery();

        }

        public static void AddClass(SQLiteConnection conn, string code, string description, string[] credits)
        {
            SQLiteCommand sQLiteCommand;
            sQLiteCommand = conn.CreateCommand();
            string values = "('" + code + "', '" + description + "', '";
            foreach (string credit in credits) {
                if (credit != "")
                {
                    values += credit + ",";
                }
            }
            values = values.Remove(values.Length - 1, 1);
            values += "');";
            sQLiteCommand.CommandText = "INSERT OR REPLACE INTO Classes (District_Code, Name, Credit) VALUES " + values;
            sQLiteCommand.ExecuteNonQuery();
        }

        public static List<string[]> ReadClass(SQLiteConnection conn)
        {
            List<string[]> data = new List<string[]>();
            SQLiteDataReader sQLiteDataReader;
            SQLiteCommand sQLiteCommand;
            sQLiteCommand = conn.CreateCommand();
            sQLiteCommand.CommandText = "SELECT * FROM Classes";

            sQLiteDataReader = sQLiteCommand.ExecuteReader();
            while (sQLiteDataReader.Read())
            {
                string[] credits = sQLiteDataReader.GetString(2).Split(',');
                string[] arr = new string[2+credits.Length];
                arr[0] = sQLiteDataReader.GetString(0);
                arr[1] = sQLiteDataReader.GetString(1);
                for (int i = 0; i<credits.Length; i++)
                {
                    arr[2 + i] = credits[i];
                }
                data.Add(arr);
            }

            return data;
        }

        public static string CheckClass(SQLiteConnection conn, string code)
        {
            SQLiteDataReader sQLiteDataReader;
            SQLiteCommand sQLiteCommand;
            sQLiteCommand = conn.CreateCommand();
            sQLiteCommand.CommandText = "SELECT Credit FROM Classes WHERE District_Code = '" + code + "'";

            sQLiteDataReader = sQLiteCommand.ExecuteReader();
            if (sQLiteDataReader.Read())
            {
                return sQLiteDataReader.GetString(0);
            }
            else
            {
                return "";
            }

            
     
        }

        public static void RemoveClass(SQLiteConnection conn, string districtCode)
        {
            SQLiteCommand sQLiteCommand;
            sQLiteCommand = conn.CreateCommand();
            sQLiteCommand.CommandText = "DELETE FROM Classes WHERE District_Code = '" + districtCode + "'";
            sQLiteCommand.ExecuteNonQuery();
        }
    }
}
