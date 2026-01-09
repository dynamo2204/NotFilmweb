using System.Data;
using Microsoft.AspNetCore.Identity;
using NotFilmweb.Constants;
using NotFilmweb.Models;

namespace NotFilmweb.Data
{
    public class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<IdentityUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();
            var context = service.GetService<ApplicationDbContext>();

            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            await roleManager.CreateAsync(new IdentityRole(Roles.User));


            var adminEmail = "admin@admin.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            // niekryty krytyk
            var criticUser = new IdentityUser { UserName = "krytyk@kino.pl", Email = "krytyk@kino.pl", EmailConfirmed = true };
            if (await userManager.FindByEmailAsync(criticUser.Email) == null)
            {
                await userManager.CreateAsync(criticUser, "User123!");
                await userManager.AddToRoleAsync(criticUser, Roles.User);
            }
            var criticInDb = await userManager.FindByEmailAsync("krytyk@kino.pl");

            if (adminUser == null)
            {
                var newAdmin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, Roles.Admin);
                }
            }

            if (!context.Categories.Any())
            {
                var catSciFi = new Category { Name = "Sci-Fi" };
                var catAction = new Category { Name = "Akcja" };
                var catDrama = new Category { Name = "Dramat" };
                var catComedy = new Category { Name = "Komedia" };

                var movies = new List<Resource>
                {
                    new Resource { Title = "Interstellar", Description = "Epicka podróż w poszukiwaniu nowego domu dla ludzkości.", Category = catSciFi },
                    new Resource { Title = "Matrix", Description = "Haker odkrywa, że rzeczywistość jest symulacją.", Category = catSciFi },
                    new Resource { Title = "Diuna", Description = "Walka o władzę na pustynnej planecie Arrakis.", Category = catSciFi },

                    new Resource { Title = "Mroczny Rycerz", Description = "Batman staje do walki z Jokerem w Gotham.", Category = catAction },
                    new Resource { Title = "John Wick", Description = "Emerytowany zabójca wraca do gry.", Category = catAction },
                    new Resource { Title = "Gladiator", Description = "Generał, który stał się niewolnikiem. Niewolnik, który stał się gladiatorem.", Category = catAction },

                    new Resource { Title = "Skazani na Shawshank", Description = "Historia przyjaźni dwóch więźniów.", Category = catDrama },
                    new Resource { Title = "Ojciec Chrzestny", Description = "Saga rodu Corleone.", Category = catDrama },
                    new Resource { Title = "Forrest Gump", Description = "Życie jest jak pudełko czekoladek.", Category = catDrama },

                    new Resource { Title = "Nietykalni", Description = "Przyjaźń milionera i chłopaka z przedmieścia.", Category = catComedy },
                    new Resource { Title = "Kac Vegas", Description = "Co się stało w Vegas, zostaje w Vegas... chyba że zgubisz pana młodego.", Category = catComedy },
                    new Resource { Title = "Grand Budapest Hotel", Description = "Przygody konsjerża w fikcyjnym państwie Zubrowka.", Category = catComedy }
                };


                context.Categories.AddRange(catSciFi, catAction, catDrama, catComedy);
                context.Resources.AddRange(movies);

                var random = new Random();
                var comments = new[]
                {
                    "Absolutne arcydzieło!", "Trochę nudny momentami.", "Świetna gra aktorska.",
                    "Można obejrzeć, ale bez szału.", "Totalna strata czasu.", "Klasyk kina.",
                    "Efekty specjalne robią robotę.", "Wzruszająca historia.", "Polecam każdemu!"
                };

                foreach (var movie in movies)
                {
                    int reviewsCount = random.Next(1, 3);
                    for (int i = 0; i < reviewsCount; i++)
                    {
                        context.Reviews.Add(new Review
                        {
                            Resource = movie,
                            User = criticInDb,
                            UserId = criticInDb.Id,
                            Rating = random.Next(2, 6),
                            Comment = comments[random.Next(comments.Length)],
                            CreatedAt = DateTime.Now.AddDays(-random.Next(1, 100))
                        });
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
