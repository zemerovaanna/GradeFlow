using GradeFlowECTS.Data;

namespace GradeFlowECTS.Interfaces
{
    public interface IUserContext
    {
        LocalUser CurrentUser { get; }

        void SetUser(LocalUser user);
    }
}