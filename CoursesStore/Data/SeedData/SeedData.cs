using CoursesStore.Data;
using Microsoft.EntityFrameworkCore;

namespace CoursesStore.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new CoursesStoreContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<CoursesStoreContext>>()))
        {
            if (context.Course.Any())
            {
                return;   // DB has been seeded
            }

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Course.AddRange(
                new Course
                {
                    Name = "All in the world",
                    Description = "All in the world project description",
                    EffectCount = 100,
                    Price = 4.99m
                },
                new Course
                {
                    Name = "Swipe",
                    Description = "Swipe project description",
                    EffectCount = 100,
                    Price = 5.99m
                },
                new Course
                {
                    Name = "WITCHBLADES",
                    Description = "WITCHBLADES project description",
                    EffectCount = 100,
                    Price = 6.99m
                },
                new Course
                {
                    Name = "VFX",
                    Description = "VFX pack description",
                    EffectCount = 100,
                    Price = 6.99m
                },
                new Course
                {
                    Name = "DarkLight",
                    Description = "DarkLight project description",
                    EffectCount = 100,
                    Price = 6.99m
                },
                new Course
                {
                    Name = "SFX",
                    Description = "SFX pack description",
                    EffectCount = 100,
                    Price = 6.99m
                }
            );
            context.SaveChanges();
        }
    }
}