using Ehrms.EmployeeInfo.API.Models;
using Ehrms.EmployeeInfo.API.Context;

namespace Ehrms.EmployeeInfo.API.IntegrationTests;

internal class Seed
{
    internal static void InitializeTestDb(EmployeeInfoDbContext dbContext)
    {
        dbContext.Skills.AddRange(GetSkills());
        dbContext.Employees.AddRange(GetEmployees());
    }

    private static List<Employee> GetEmployees()
    {
        var employeeFaker = new EmployeeFaker();
        return
        [
            employeeFaker.Generate(),
            employeeFaker.Generate(),
        ];
    }

    private static List<Skill> GetSkills()
    {
        var skillFaker = new SkillFaker();
        return
        [
            skillFaker.Generate(),
            skillFaker.Generate()   
        ];
    }
}