using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace MBA_DevXpert_PEO.Api.Identity
{
    public interface IUser
    {
        string Name { get; }
        Guid GetUserId();
        string GetUserEmail();
        bool IsAuthenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetClaimsIdentity();
    }
}
