using TrustYourBrand.Models;
using TrustYourBrand.Pages;

namespace TrustYourBrand.Services
{
    public interface IDepartmentService
    {
        Task<List<DepartmentResponseDto>> GetDepartmentsAsync();
        Task<DepartmentResult> CreateDepartmentAsync(CreateDepartmentDto departmentDto);
        Task<DepartmentResult> DeleteDepartmentAsync(int id);
        Task<DepartmentResult> UpdateDepartmentAsync(int id, UpdateDepartmentDto departmentDto);
    }

    public class DepartmentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public DepartmentResponseDto CreatedDepartment { get; set; }
        public DepartmentResponseDto UpdatedDepartment { get; set; }
    }
}
