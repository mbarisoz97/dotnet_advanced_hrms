﻿namespace Ehrms.Administration.API.Database.Models;

public class Employee : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public virtual ICollection<PaymentCriteria> PaymentCriteria { get; set; } = [];
}