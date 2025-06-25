using System.Data.Common;
using System.Runtime.InteropServices;
using Microsoft.Data.Sqlite;
using Synced.Server.Exceptions;

namespace Synced.Server
{
    public static class DbConnections
    {
        public static SqliteDataReader DbExecute(this SqliteConnection conn, string sql)
        {
            var command = conn.CreateCommand();
            command.CommandText = sql;
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
            try
            {
                return command.ExecuteReader();

            }
            catch (SqliteException e)
            {
                if (e.SqliteErrorCode == 19)
                {
                    throw new EntryExistsException(e);
                }
                throw;
            }
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
                    "create table users(uuid text primary key unique not null, username text unique not null, password text not null, role integer not null);";
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

            command = sqliteConnection.CreateCommand();
            command.CommandText = "select name from sqlite_master where type='table' and name='file_table'; ";
            command.ExecuteNonQuery();
            reader.DisposeAsync().GetAwaiter().GetResult();
            reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                command = sqliteConnection.CreateCommand();
                command.CommandText =
                    """
                                        CREATE TABLE file_table (
                    	uuid TEXT(36) NOT NULL,
                    	belongs TEXT(36) NOT NULL,
                    	hash TEXT NOT NULL,
                        name TEXT NOT NULL, 
                    	CONSTRAINT file_table_pk PRIMARY KEY (uuid)
                    );
                    """;
                command.ExecuteNonQuery();
            }
            command = sqliteConnection.CreateCommand();
            command.CommandText = "select name from sqlite_master where type='table' and name='root_table'; ";
            command.ExecuteNonQuery();
            reader.DisposeAsync().GetAwaiter().GetResult();
            reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                command = sqliteConnection.CreateCommand();
                command.CommandText =
                    """
                                        CREATE TABLE root_table (
                    	uuid TEXT(36) NOT NULL,
                    	belongs_user TEXT(36) NOT NULL,
                    	name TEXT NOT NULL, 
                    	CONSTRAINT file_table_pk PRIMARY KEY (uuid)
                    );
                    """;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Check if username exists in the database. 
        /// </summary>
        /// <param name="sqliteConnection"></param>
        /// <param name="username"></param>
        /// <param name="uuid"></param>
        public static bool CheckUsernameIfExists(this SqliteConnection sqliteConnection, string username, out List<string> uuid)
        {
            var command = sqliteConnection.CreateCommand();
            command.CommandText = "select uuid from users where username=$username;";
            command.Parameters.AddWithValue("$username", username);
            using var reader = command.ExecuteReader();
            var rows = new List<string>();
            while (reader.Read())
            {
                rows.Add(reader.GetString(0));
            }
            uuid = rows;
            return uuid.Count != 0;

        }
        /// <summary>
        /// Add user entry in database. 
        /// </summary>
        /// <param name="sqliteConnection"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        public static string AddUser(this SqliteConnection sqliteConnection, string username, string password,
            string? email, int role)
        {
            var guid = Guid.NewGuid().ToString();
            if (sqliteConnection.CheckUsernameIfExists(username, out _))
            {
                throw new UserExistsException();
            }
            var reader = sqliteConnection.DbExecute(
                "insert into users (uuid, username, password, role) values ($uuid, $username, $password, $role);",
                [
                    ("$uuid",guid ),
                    ("$username", username),
                    ("$password", password.ToSHA256HexHashString()), ("$email", email), ("$role", role)
                ]);
            return guid;


        }
        public static void GetUser(this SqliteConnection sqliteConnection, string username)
        {

        }
    }
}