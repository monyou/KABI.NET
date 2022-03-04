using KABINET_Domain.Entities;
using KABINET_Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;

namespace KABINET_Persistance
{
    public static class KabinetDbContextSeed
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<User>().
                HasData(new User(Guid.Parse("20a48303-a321-4da4-8cd0-520c1aee58be"), "admin@kabi.net", "Admin", "Admin", "999", UserRoles.Admin));
        }
    }
}
