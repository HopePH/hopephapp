namespace Yol.Punla.Authentication
{
    public interface IAppUser
    {
        bool IsAuthenticated { get; }
        bool SignUpCompleted { get;}
    }
}
