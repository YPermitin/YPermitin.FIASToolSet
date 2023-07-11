using YPermitin.FIASToolSet.Storage.Core.Models;

namespace YPermitin.FIASToolSet.Storage.Core.Services;

public interface IFIASInstallationManagerRepository
{
    Task<List<FIASVersionInstallation>> GetInstallations(Guid? statusId = null, Guid? typeId = null);
    Task<FIASVersionInstallation> GetInstallation(Guid id);
    Task<FIASVersionInstallation> GetLastInstallation();
    Task<FIASVersionInstallation> GetPreviousInstallation(Guid installationId);
    void AddInstallation(FIASVersionInstallation installation);
    void UpdateInstallation(FIASVersionInstallation installation);
    
    Task<bool> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<bool> SaveAsync();
}