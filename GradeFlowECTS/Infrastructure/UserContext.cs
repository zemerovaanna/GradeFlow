using GradeFlowECTS.Data;

namespace GradeFlowECTS.Interfaces
{
    public class UserContext : IUserContext
    {
        public LocalUser CurrentUser { get; private set; }

        public void SetUser(LocalUser user)
        {
            CurrentUser = user;
        }
    }
}