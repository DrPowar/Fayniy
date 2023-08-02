﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoursesStore.Data
{
    public class CoursesStoreContext : IdentityDbContext<IdentityUser>
    {
        public CoursesStoreContext(DbContextOptions<CoursesStoreContext> options)
            : base(options)
        {
        }

        public DbSet<CoursesStore.Models.Course> Course { get; set; } = default!;
    }
}
