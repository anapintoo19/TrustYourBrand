using TrustYourBrand.Models;
using TrustYourBrand.Pages;

namespace TrustYourBrand.Services
{
    public interface IStoreService
    {
        Task<List<StoreDto>> GetStoresAsync();
        Task<StoreResult> CreateStoreAsync(CreateStoreDto storeDto);
        Task<StoreResult> DeleteStoreAsync(int id);
        Task<StoreResult> UpdateStoreAsync(int id, UpdateStoreDto storeDto);
    }
    public class StoreResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public StoreDto CreatedStore { get; set; }
        public StoreDto UpdatedStore { get; set; }
    }
}
