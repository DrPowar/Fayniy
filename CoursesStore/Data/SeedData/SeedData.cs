using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CoursesStore.Data;
using System;
using System.Linq;
using CoursesStore.Models;

namespace CoursesStore.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new CoursesStoreContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<CoursesStoreContext>>()))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Course.Any())
            {
                return;   // DB has been seeded
            }
            context.Course.AddRange(
                new Course
                {
                    Name = "All in the world",
                    Description = "All in the world project description",
                    Duration = TimeSpan.FromHours(2),
                    Price = 4.99m
                },
                new Course
                {
                    Name = "Swipe",
                    Description = "Swipe project description",
                    Duration = TimeSpan.FromHours(3),
                    Price = 5.99m
                },
                new Course
                {
                    Name = "WITCHBLADES",
                    Description = "WITCHBLADES project description",
                    Duration = TimeSpan.FromHours(1.5),
                    Price = 6.99m
                },
                new Course
                {
                    Name = "VFX",
                    Description = "VFX pack description",
                    Duration = TimeSpan.FromHours(1.5),
                    Price = 6.99m
                },
                new Course
                {
                    Name = "SFX",
                    Description = "SFX pack description",
                    Duration = TimeSpan.FromHours(1.5),
                    Price = 6.99m
                },
                new Course
                {
                    Name = "DarkLight",
                    Description = "DarkLight project description",
                    Duration = TimeSpan.FromHours(1.5),
                    Price = 6.99m
                }
            );
            context.SaveChanges();
        }
    }
}