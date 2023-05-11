﻿using System.Security.Claims;

namespace Election2023.Application.Interfaces.Services;

public interface ICurrentUserService
{
    string UserId { get; }

    IReadOnlyCollection<KeyValuePair<string, string>> Claims { get; }

    void SetUser(ClaimsPrincipal user);
}

