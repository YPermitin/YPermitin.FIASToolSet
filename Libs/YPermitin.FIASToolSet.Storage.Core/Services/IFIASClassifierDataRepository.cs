using YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;

namespace YPermitin.FIASToolSet.Storage.Core.Services;

public interface IFIASClassifierDataRepository
{
    #region AddressObjects

    Task<List<AddressObject>> GetAddressObjects();
    Task<AddressObject> GetAddressObject(int id);
    Task<bool> AddressObjectExists(int id);
    void AddAddressObject(AddressObject objectLevel);
    void UpdateAddressObject(AddressObject objectLevel);
    void RemoveAddressObject(AddressObject objectLevel);

    #endregion
    
    Task<bool> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<bool> SaveAsync();
    Task SaveWithIdentityInsertAsync<T>();
}