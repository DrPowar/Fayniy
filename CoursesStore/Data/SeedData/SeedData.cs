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
                    Name = "Test course 1",
                    Description = "Test description 1",
                    Duration = TimeSpan.FromHours(2),
                    Price = 4.99m,
                    ImageUrl = "https://sproutsocial.com/insights/social-media-image-sizes-guide/"
                },
                new Course
                {
                    Name = "Test course 2",
                    Description = "Test description 2",
                    Duration = TimeSpan.FromHours(3),
                    Price = 5.99m,
                    ImageUrl = "https://sproutsocial.com/insights/social-media-image-sizes-guide/"
                },
                new Course
                {
                    Name = "Test course 3",
                    Description = "Test description 3",
                    Duration = TimeSpan.FromHours(1.5),
                    Price = 6.99m,
                    ImageUrl = "https://sproutsocial.com/insights/social-media-image-sizes-guide/"
                },
                new Course
                {
                    Name = "Test course 4",
                    Description = "Test description 4",
                    Duration = TimeSpan.FromHours(2.3),
                    Price = 7.99m,
                    ImageUrl = "https://sproutsocial.com/insights/social-media-image-sizes-guide/"
                }
            );
            context.SaveChanges();
        }
    }
}