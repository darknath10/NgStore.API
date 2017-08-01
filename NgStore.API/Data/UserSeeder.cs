using Microsoft.AspNetCore.Identity;
using NgStore.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgStore.API.Data
{
    public class UserSeeder
    {
        private UserManager<User> _usrMgr;

        public UserSeeder(UserManager<User> usrMgr)
        {
            _usrMgr = usrMgr;
        }

        public async Task Seed()
        {
            if (!_usrMgr.Users.Any())
            {
                var user = new User()
                {
                    UserName = "elikopter",
                    Email = "elikopter@ngstore.com"
                };

                var userResult = await _usrMgr.CreateAsync(user, "P@ssw0rd");
                var claimResult = await _usrMgr.AddClaimAsync(user, new Claim("NgStoreWorker", "true"));

                if (!userResult.Succeeded || !claimResult.Succeeded) throw new InvalidOperationException("Failed to add user");
            }
        }
    }
}
