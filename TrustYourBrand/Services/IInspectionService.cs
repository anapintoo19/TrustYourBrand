using TrustYourBrand.Models;

namespace TrustYourBrand.Services
{
    public interface IInspectionService
    {
        Task<List<InspectionDto>> GetInspectionsAsync();
        Task<InspectionDto> GetInspectionByIdAsync(int id);
        Task<InspectionResult> CreateInspectionAsync(CreateInspectionDto inspectionDto);
        Task<InspectionResult> UpdateInspectionAsync(int id, UpdateInspectionDto inspectionDto);
        Task<InspectionResult> DeleteInspectionAsync(int id);
        Task<List<BrandDto>> GetBrands();
        Task<List<TemplateDto>> GetTemplates();
        Task<List<SectionDto>> GetSection();
    }
}
