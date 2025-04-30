using TrustYourBrand.Models;
using TrustYourBrand.Pages;

namespace TrustYourBrand.Services
{
    public interface ITemplateService
    {
        Task<List<TemplateDto>> GetTemplates();
        Task<List<CustomQuestion>> GetTemplateQuestions(int templateId);
    }
}
