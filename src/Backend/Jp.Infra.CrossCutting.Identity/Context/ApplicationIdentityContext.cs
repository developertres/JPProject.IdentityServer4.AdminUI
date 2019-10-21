﻿using Jp.Infra.CrossCutting.Identity.Constants;
using Jp.Infra.CrossCutting.Identity.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Jp.Infra.CrossCutting.Identity.Context
{
    public class ApplicationIdentityContext : IdentityDbContext<UserIdentity, UserIdentityRole, Guid, UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken>
    {
        private readonly ILoggerFactory loggerFactory;

        public ApplicationIdentityContext(
            DbContextOptions<ApplicationIdentityContext> options,
            ILoggerFactory loggerFactory)
            : base(options)
        {
            this.loggerFactory = loggerFactory;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ConfigureIdentityContext(builder);

        }


        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development"))
                optionsBuilder.UseLoggerFactory(this.loggerFactory).EnableSensitiveDataLogging();
        }

        private void ConfigureIdentityContext(ModelBuilder builder)
        {
            builder.Entity<UserIdentityRole>().ToTable(TableConsts.IdentityRoles);
            builder.Entity<UserIdentityRoleClaim>().ToTable(TableConsts.IdentityRoleClaims);
            builder.Entity<UserIdentityUserRole>().ToTable(TableConsts.IdentityUserRoles);

            builder.Entity<UserIdentity>().ToTable(TableConsts.IdentityUsers);
            builder.Entity<UserIdentityUserLogin>().ToTable(TableConsts.IdentityUserLogins);
            builder.Entity<UserIdentityUserClaim>().ToTable(TableConsts.IdentityUserClaims);
            builder.Entity<UserIdentityUserToken>().ToTable(TableConsts.IdentityUserTokens);
        }
    }
}
