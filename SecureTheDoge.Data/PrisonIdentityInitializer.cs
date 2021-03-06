﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SecureTheDoge.Data.Entities;

namespace SecureTheDoge.Data
{
  public class PrisonIdentityInitializer
  {
    private RoleManager<IdentityRole> _roleMgr;
    private UserManager<PrisonUser> _userMgr;

    public PrisonIdentityInitializer(UserManager<PrisonUser> userMgr, RoleManager<IdentityRole> roleMgr)
    {
      _userMgr = userMgr;
      _roleMgr = roleMgr;
    }

    public async Task Seed()
    {
      var user = await _userMgr.FindByNameAsync("shawnwildermuth");

      // Add User
      if (user == null)
      {
        if (!(await _roleMgr.RoleExistsAsync("Admin")))
        {
          var role = new IdentityRole("Admin");
          await _roleMgr.CreateAsync(role);
        }

        user = new PrisonUser()
        {
          UserName = "shawnwildermuth",
          FirstName = "Shawn",
          LastName = "Wildermuth",
          Email = "shawn@wildermuth.com"
        };
      
        var userResult = await _userMgr.CreateAsync(user, "P@ssw0rd!");
        var roleResult = await _userMgr.AddToRoleAsync(user, "Admin");
        var claimResult = await _userMgr.AddClaimAsync(user, new Claim("SuperUser", "True"));

        if (!userResult.Succeeded || !roleResult.Succeeded || !claimResult.Succeeded)
        {
          throw new InvalidOperationException("Failed to build user and roles");
        }

      }
    }
  }
}
