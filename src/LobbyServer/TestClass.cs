using MySqlConnector;
using System.Data;
using System.Data.Common;

namespace LobbyServer
{
    public class TestClass
    {

        public static void Test()
        {
            HashSet<Type> NullableTypes = new HashSet<Type> {
               typeof(int),
               typeof(short),
               typeof(long),
               typeof(double),
               typeof(decimal),
               typeof(float),
               typeof(bool),
               typeof(DateTime)
       };
            string dbString = "server=127.0.0.1; port=33063; user id=lumen;password=lumen!@!@;persistsecurityinfo=True;database={0}";
            string dbSchema = "account_db";

            System.Data.Common.DbConnection connection = new MySqlConnector.MySqlConnection(string.Format(dbString, dbSchema));
            connection.Open();

            string sql = "select TABLE_NAME from information_schema.tables where table_type IN ('BASE TABLE', 'VIEW') and TABLE_SCHEMA = '" + dbSchema + "'";

            List<string> tables = new List<string>();

            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = sql;
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string t = reader.GetString(0);
                        tables.Add(t);
                    }
                }
            }
        }

    }
}
