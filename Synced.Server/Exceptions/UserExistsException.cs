namespace Synced.Server.Exceptions
{
    public class UserExistsException : Exception
    {
        public override string Message { get; } = "The username is already exists occupied "; 
    }
}
