using Microsoft.Data.Sqlite;

namespace Synced.Server
{
    public static class DbConnections
    {
        public static void DbInitialize(this SqliteConnection sqliteConnection)
        {
            var command = sqliteConnection.CreateCommand();
            command.CommandText = "select name from sqlite_master where type='table' and name='users';";
            command.ExecuteNonQuery(); 
            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                command = sqliteConnection.CreateCommand();
                command.CommandText = "create table users(uuid text primary key unique not null, username text unique not null, passwrod text not null, role integer not null);";
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
                command.CommandText = "create table devices (uuid text primary key unique not null, master text not null); ";
                command.ExecuteNonQuery();

            }
        }
    }
}
