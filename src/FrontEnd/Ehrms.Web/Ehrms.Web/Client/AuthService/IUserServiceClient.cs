﻿using Ehrms.Web.Models.User;

namespace Ehrms.Web.Client.AuthService;

internal interface IUserServiceClient
{
    Task<Response<ReadUserModel>> GetUserByIdAsync(Guid id);
    Task<Response<IEnumerable<ReadUserModel>>> GetUsersAsync();
    Task<Response<ReadUserModel>> UpdateUserAsync(ReadUserModel model);
    Task<Response<ReadUserModel>> RegisterUserAsync(RegisterUserModel model);
}