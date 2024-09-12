using Ehrms.Web.Models;

namespace Ehrms.Web.Client.EmployeeApi.Skill;

public interface ISkillServiceClient
{
    Task<Response<SkillModel>> GetSkillAsync(Guid id);
    Task<Response<IEnumerable<SkillModel>>> GetSkillsAsync();
    Task<Response<SkillModel>> CreateSkillAsync(SkillModel skill);
    Task<Response<Guid>> DeleteEmployeeAsync(Guid id);
    Task<Response<SkillModel>> UpdateSkillAsync(SkillModel skill);
}