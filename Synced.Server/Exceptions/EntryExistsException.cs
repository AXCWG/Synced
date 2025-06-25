using Microsoft.Data.Sqlite;

namespace Synced.Server.Exceptions
{
    public class EntryExistsException : Exception
    {
        public SqliteException BaseSqliteExecption { get;  }
        public EntryExistsException(SqliteException e) 
        {
            BaseSqliteExecption = e; 
        }
        public override string Message { get; } = "Entry already exists in Db. "; 
    }
}
