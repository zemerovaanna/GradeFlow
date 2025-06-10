namespace GradeFlowECTS.Interfaces
{
    public interface IUserSettingsService
    {
        bool IsFirstLaunch { get; set; }
        void ResetToDefault();
    }
}