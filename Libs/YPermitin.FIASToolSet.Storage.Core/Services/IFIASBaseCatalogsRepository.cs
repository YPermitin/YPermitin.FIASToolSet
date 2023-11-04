using YPermitin.FIASToolSet.Storage.Core.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.Storage.Core.Services;

public interface IFIASBaseCatalogsRepository
{
    #region ObjectLevel
    
    Task<List<ObjectLevel>> GetObjectLevels();
    Task<ObjectLevel> GetObjectLevel(int level);
    Task<bool> ObjectLevelExists(int level);
    void AddObjectLevel(ObjectLevel objectLevel);
    void UpdateObjectLevel(ObjectLevel objectLevel);
    void RemoveObjectLevel(ObjectLevel objectLevel);
    
    #endregion
    
    #region AddressObjectType
    
    Task<List<AddressObjectType>> GetAddressObjectTypes();
    Task<AddressObjectType> GetAddressObjectType(int id);
    Task<bool> AddressObjectTypeExists(int id);
    void AddAddressObjectType(AddressObjectType addressObjectType);
    void UpdateAddressObjectType(AddressObjectType addressObjectType);
    void RemoveAddressObjectType(AddressObjectType addressObjectType);
    
    #endregion
    
    #region ApartmentType
    
    Task<List<ApartmentType>> GetApartmentTypes();
    Task<ApartmentType> GetApartmentType(int id);
    Task<bool> ApartmentTypeExists(int id);
    void AddApartmentType(ApartmentType apartmentType);
    void UpdateApartmentType(ApartmentType apartmentType);
    void RemoveApartmentType(ApartmentType apartmentType);
    
    #endregion
    
    #region HouseType
    
    Task<List<HouseType>> GetHouseTypes();
    Task<HouseType> GetHouseType(int id);
    Task<bool> HouseTypeExists(int id);
    void AddHouseType(HouseType houseType);
    void UpdateHouseType(HouseType houseType);
    void RemoveHouseType(HouseType houseType);
    
    #endregion
    
    #region NormativeDocKind
    
    Task<List<NormativeDocKind>> GetNormativeDocKinds();
    Task<NormativeDocKind> GetNormativeDocKind(int id);
    Task<bool> NormativeDocKindExists(int id);
    void AddNormativeDocKind(NormativeDocKind normativeDocKind);
    void UpdateNormativeDocKind(NormativeDocKind normativeDocKind);
    void RemoveNormativeDocKind(NormativeDocKind normativeDocKind);
    
    #endregion
    
    #region NormativeDocType
    
    Task<List<NormativeDocType>> GetNormativeDocTypes();
    Task<NormativeDocType> GetNormativeDocType(int id);
    Task<bool> NormativeDocTypeExists(int id);
    void AddNormativeDocType(NormativeDocType normativeDocType);
    void UpdateNormativeDocType(NormativeDocType normativeDocType);
    void RemoveNormativeDocType(NormativeDocType normativeDocType);
    
    #endregion
    
    #region OperationType
    
    Task<List<OperationType>> GetOperationTypes();
    Task<OperationType> GetOperationType(int id);
    Task<bool> OperationTypeExists(int id);
    void AddOperationType(OperationType operationType);
    void UpdateOperationType(OperationType operationType);
    void RemoveOperationType(OperationType operationType);
    
    #endregion
    
    #region ParameterType
    
    Task<List<ParameterType>> GetParameterTypes();
    Task<ParameterType> GetParameterType(int id);
    Task<bool> ParameterTypeExists(int id);
    void AddParameterType(ParameterType parameterType);
    void UpdateParameterType(ParameterType parameterType);
    void RemoveParameterType(ParameterType parameterType);
    
    #endregion
    
    #region RoomType
    
    Task<List<RoomType>> GetRoomTypes();
    Task<RoomType> GetRoomType(int id);
    Task<bool> RoomTypeExists(int id);
    void AddRoomType(RoomType roomType);
    void UpdateRoomType(RoomType roomType);
    void RemoveRoomType(RoomType roomType);
    
    #endregion
    
    Task<bool> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<bool> SaveAsync();
    Task SaveWithIdentityInsertAsync<T>();
}