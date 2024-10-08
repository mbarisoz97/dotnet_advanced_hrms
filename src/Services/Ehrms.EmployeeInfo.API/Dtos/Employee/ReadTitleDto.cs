﻿using Ehrms.EmployeeInfo.API.Dtos.Title;

namespace Ehrms.EmployeeInfo.API.Dtos.Employee;

public sealed class ReadTitleDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    //public Guid TitleId { get; set; }
    
    public Title.ReadTitleDto? Title { get; set; }
    public ICollection<Guid> Skills { get; set; } = [];
    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}