using YPermitin.FIASToolSet.Storage.Core.Models.Versions;

namespace YPermitin.FIASToolSet.Storage.Core.Services;

public interface IFIASInstallationManagerRepository
{
    #region FIASVersionInstallation
    
    Task<List<FIASVersionInstallation>> GetInstallations(Guid? statusId = null, Guid? typeId = null,
        bool includeDetails = false);
    Task<FIASVersionInstallation> GetInstallation(Guid id);
    Task<FIASVersionInstallation> GetLastInstallation();
    Task<FIASVersionInstallation> GetPreviousInstallation(Guid installationId);
    void AddInstallation(FIASVersionInstallation installation);
    void UpdateInstallation(FIASVersionInstallation installation);
    
    #endregion
    
    #region FIASVersionInstallationStep

    Task<List<FIASVersionInstallationStep>> GetVersionInstallationSteps(Guid installationId);

    Task<FIASVersionInstallationStep> GetVersionInstallationStep(Guid installationId, string fileFullName);

    void AddInstallationStep(FIASVersionInstallationStep installationStep);

    void UpdateInstallationStep(FIASVersionInstallationStep installationStep);
    
    #endregion
    
    Task<bool> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<bool> SaveAsync();
}