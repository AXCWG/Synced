using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace Synced.Server
{
    public static class DbConnections
    {
        public static SqliteDataReader DbExecute(this SqliteConnection conn, string sql)
        {
            var command = conn.CreateCommand();
            command.CommandText = sql;
            command.ExecuteNonQuery();
            return command.ExecuteReader();
        }

        public static SqliteDataReader DbExecute(this SqliteConnection conn, string sql,
            List<(string?, object?)> parameters
        )
        {
            var command = conn.CreateCommand();
            command.CommandText = sql;
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
            }

            command.ExecuteNonQuery();
            return command.ExecuteReader();
        }

        public static void DbInitialize(this SqliteConnection sqliteConnection)
        {
            var command = sqliteConnection.CreateCommand();
            command.CommandText = "select name from sqlite_master where type='table' and name='users';";
            command.ExecuteNonQuery();
            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                command = sqliteConnection.CreateCommand();
                command.CommandText =
                    "create table users(uuid text primary key unique not null, username text unique not null, passwrod text not null, role integer not null);";
                command.ExecuteNonQuery();
            }

            command = sqliteConnection.CreateCommand();
            command.CommandText = "select name from sqlite_master where type='table' and name='devices'; ";
            command.ExecuteNonQuery();
            reader.DisposeAsync().GetAwaiter().GetResult();
            reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                command = sqliteConnection.CreateCommand();
                command.CommandText =
                    "create table devices (uuid text primary key unique not null, master text not null); ";
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Check if username exists in the database. 
        /// </summary>
        /// <param name="sqliteConnection"></param>
        /// <param name="username"></param>
        /// <param name="uuid"></param>
        public static void CheckUsername(this SqliteConnection sqliteConnection, string username, out List<string> uuid)
        {
            var command = sqliteConnection.CreateCommand();
            command.CommandText = "select uuid from users where username='$username';";
            command.Parameters.AddWithValue("$username", username);
            using var reader = command.ExecuteReader();
            var rows = new List<string>();
            while (reader.Read())
            {
                rows.Add(reader.GetString(0));
            }

            uuid = rows;
        }

        public static void AddUser(this SqliteConnection sqliteConnection, string username, string password,
            string? email, int role)
        {
            sqliteConnection.DbExecute(
                "insert into users (uuid, username, password, role) values ($uuid, $username, $password, $role);",
                [
                    ("$uuid", Guid.NewGuid().ToString()),
                    ("$username", username),
                    ("$password", password), ("$email", email), ("$role", role)
                ]);
            ;
        }
    }
}