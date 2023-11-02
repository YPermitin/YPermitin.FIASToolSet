using YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;

namespace YPermitin.FIASToolSet.Storage.Core.Services;

public interface IFIASClassifierDataRepository
{
    #region AddressObjects

    Task<List<AddressObject>> GetAddressObjects(List<int> ids = null);
    Task<AddressObject> GetAddressObject(int id);
    Task<bool> AddressObjectExists(int id);
    void AddAddressObject(AddressObject objectLevel);
    void UpdateAddressObject(AddressObject objectLevel);
    void RemoveAddressObject(AddressObject objectLevel);

    #endregion
    
    #region AddressObjectDivision

    Task<List<AddressObjectDivision>> GetAddressObjectDivisions();
    Task<AddressObjectDivision> GetAddressObjectDivision(int id);
    Task<bool> AddressObjectDivisionExists(int id);
    void AddAddressObjectDivision(AddressObjectDivision objectLevel);
    void UpdateAddressObjectDivision(AddressObjectDivision objectLevel);
    void RemoveAddressObjectDivision(AddressObjectDivision objectLevel);

    #endregion
    
    #region AddressObjectParameter

    Task<List<AddressObjectParameter>> GetAddressObjectParameters(List<int> ids = null);
    Task<AddressObjectParameter> GetAddressObjectParameter(int id);
    Task<bool> AddressObjectParameterExists(int id);
    void AddAddressObjectParameter(AddressObjectParameter addressObjectParameter);
    void UpdateAddressObjectParameter(AddressObjectParameter addressObjectParameter);
    void RemoveAddressObjectParameter(AddressObjectParameter addressObjectParameter);

    #endregion
    
    Task<bool> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<bool> SaveAsync();
    Task SaveWithIdentityInsertAsync<T>();
    void ClearChangeTracking();
}