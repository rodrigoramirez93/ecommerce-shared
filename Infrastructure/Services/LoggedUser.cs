﻿using Infrastructure.Interfaces;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class LoggedUserService : ILoggedUserService
    {
        private readonly LoggedUser LoggedUser;

        public LoggedUserService()
        {
            LoggedUser = new LoggedUser();
        }

        public LoggedUser GetLoggedUser() => LoggedUser;

        public void SetEmail(string email)
            => LoggedUser.Email = email;

        public void SetClaims(IEnumerable<Claim> claims)
            => LoggedUser.Claims = claims;

        public void SetTenants(IEnumerable<Claim> tenants)
            => LoggedUser.Tenants = tenants;

        public void SetUserId(Claim userIdClaim)
        {
            var canParse = int.TryParse(userIdClaim.Value, out int id);
            if (canParse) LoggedUser.Id = id;
        }

        public void SetRequestTenant(string tenantHeader)
        {
            LoggedUser.ActiveTenant = Guid.Parse(tenantHeader);
        }
    }
}
