using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoursesStore.Models;

namespace CoursesStore.Data
{
    public class CoursesStoreContext : DbContext
    {
        public CoursesStoreContext (DbContextOptions<CoursesStoreContext> options)
            : base(options)
        {
        }

        public DbSet<CoursesStore.Models.Course> Course { get; set; } = default!;
    }
}
