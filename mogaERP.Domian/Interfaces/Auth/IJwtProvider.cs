﻿using mogaERP.Domain.Entities;

namespace mogaERP.Domain.Interfaces.Auth;
public interface IJwtProvider
{
    (string token, int expiresIn) GenerateToken(ApplicationUser user);
    string? ValidateToken(string token);
}
