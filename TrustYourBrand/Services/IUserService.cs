using TrustYourBrand.Models;

namespace TrustYourBrand.Services
{
    public interface IUserService
    {
        Task<List<User>> GetUsers(int? createdByUserId = null);
        Task<List<RoleDto>> GetRoles();
        Task<List<DepartmentDto>> GetDepartments();

        Task<List<FunctionDto>> GetFunctions();

        Task<LoginResult> CreateUser(User user); // Ajustado para Task<UserCreationResult>
        Task<LoginResult> UpdateUser(int id, UpdateUserDto userDto);
        // Ajustado para Task<UserOperationResult>
        Task<LoginResult> DeactivateUser(int id); // Ajustado para Task<UserOperationResult>
        Task<LoginResult> ChangePin(int userId, string newPin);
        //Task<LoginResult> RoleUser(int id, string role); // Ajustado para Task<UserOperationResult>
        
    }
}