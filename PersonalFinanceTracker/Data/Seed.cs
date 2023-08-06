using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Areas.Identity.Data;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Data
{
    public class Seed
    {
        public static async Task SeedDataAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await SeedRolesAsync(serviceScope);
                await SeedUsersAsync(serviceScope);
                await SeedTransactionCategoriesAsync(dbContext);
                await SeedTransactionTypesAsync(dbContext);
                await SeedSavingsStatusAsync(dbContext);
                await SeedTransactionsAsync(dbContext, serviceScope);
                await SeedSavingsAsync(dbContext, serviceScope);
            }
        }

        private static async Task SeedRolesAsync(IServiceScope serviceScope)
        {
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var rolesToCreate = new List<string>
            {
                UserRoles.Admin,
                UserRoles.User
            };

            foreach (var role in rolesToCreate)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedUsersAsync(IServiceScope serviceScope)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
            string adminUserEmail = "admin@gmail.com";
            string appUserEmail = "user@gmail.com";

            await CreateUserIfNotExist(userManager, adminUserEmail, "Jan", "Kowalski");
            await CreateUserIfNotExist(userManager, appUserEmail, "Janusz", "Nowak");
        }

        private static async Task CreateUserIfNotExist(UserManager<User> userManager, string email, string firstName,
            string lastName)
        {
            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                var newUser = new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                };
                await userManager.CreateAsync(newUser, "EbK^zyEzs^ib@4y6*6H");
                await userManager.AddToRoleAsync(newUser,
                    email == "admin@gmail.com" ? UserRoles.Admin : UserRoles.User);
            }
        }

        private static async Task SeedTransactionCategoriesAsync(ApplicationDbContext dbContext)
        {
            var categoriesToAdd = new List<TransactionCategory>
            {
                new() { Name = "Groceries" },
                new() { Name = "Entertainment" },
                new() { Name = "Utilities" },
                new() { Name = "Dining Out" },
                new() { Name = "Shopping" },
                new() { Name = "Salary" },
                new() { Name = "Freelance" },
                new() { Name = "Rental Income" },
                new() { Name = "Bonus" },
                new() { Name = "Investment Returns" },
                new() { Name = "Healthcare" },
                new() { Name = "Travel" },
                new() { Name = "Charity" },
                new() { Name = "Books" },
                new() { Name = "Gifts" },
                new() { Name = "Transportation" },
                new() { Name = "Home Repairs" },
                new() { Name = "Education" },
                new() { Name = "Gym Membership" },
                new() { Name = "Pets" }
            };

            foreach (var category in categoriesToAdd)
            {
                var existingCategory =
                    await dbContext.TransactionCategories.FirstOrDefaultAsync(c => c.Name == category.Name);
                if (existingCategory == null)
                {
                    dbContext.TransactionCategories.Add(category);
                }
            }

            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedTransactionTypesAsync(ApplicationDbContext dbContext)
        {
            var typesToAdd = new List<TransactionType>
            {
                new() { Name = "Expense" },
                new() { Name = "Income" },
            };

            foreach (var type in typesToAdd)
            {
                var existingType = await dbContext.TransactionTypes.FirstOrDefaultAsync(c => c.Name == type.Name);
                if (existingType == null)
                {
                    dbContext.TransactionTypes.Add(type);
                }
            }

            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedSavingsStatusAsync(ApplicationDbContext dbContext)
        {
            var savingsToAdd = new List<SavingsStatus>
            {
                new() { Name = "In Progress" },
                new() { Name = "Completed" },
            };

            foreach (var savings in savingsToAdd)
            {
                var existingType = await dbContext.SavingsStatus.FirstOrDefaultAsync(c => c.Name == savings.Name);
                if (existingType == null)
                {
                    dbContext.SavingsStatus.Add(savings);
                }
            }

            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedTransactionsAsync(ApplicationDbContext dbContext, IServiceScope serviceScope)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
            string appUserEmail = "user@gmail.com";
            var appUser = await userManager.FindByEmailAsync(appUserEmail);
            string userId = appUser.Id;

            var transactionsToAdd = new List<Transaction>
            {
                // September 2022
                new()
                {
                    Id = "116",
                    UserId = userId,
                    Category = "Groceries",
                    Date = new DateTime(2022, 9, 1, 12, 30, 0),
                    Description = "Bought groceries at the local supermarket.",
                    Type = "Expense",
                    Amount = 130.50m
                },
                new()
                {
                    Id = "117",
                    UserId = userId,
                    Category = "Entertainment",
                    Date = new DateTime(2022, 9, 2, 18, 15, 0),
                    Description = "Purchased movie tickets for a new release.",
                    Type = "Expense",
                    Amount = 95.20m
                },
                new()
                {
                    Id = "118",
                    UserId = userId,
                    Category = "Utilities",
                    Date = new DateTime(2022, 9, 3, 9, 0, 0),
                    Description = "Paid electricity bill for the month.",
                    Type = "Expense",
                    Amount = 115.80m
                },
                new()
                {
                    Id = "119",
                    UserId = userId,
                    Category = "Dining Out",
                    Date = new DateTime(2022, 9, 4, 20, 0, 0),
                    Description = "Had dinner at a fancy restaurant.",
                    Type = "Expense",
                    Amount = 140.00m
                },
                new()
                {
                    Id = "120",
                    UserId = userId,
                    Category = "Shopping",
                    Date = new DateTime(2022, 9, 5, 14, 45, 0),
                    Description = "Bought new clothes at the mall.",
                    Type = "Expense",
                    Amount = 220.25m
                },
                new()
                {
                    Id = "121",
                    UserId = userId,
                    Category = "Salary",
                    Date = new DateTime(2022, 9, 10, 15, 0, 0),
                    Description = "Received monthly salary.",
                    Type = "Income",
                    Amount = 3700.00m
                },
                new()
                {
                    Id = "122",
                    UserId = userId,
                    Category = "Freelance",
                    Date = new DateTime(2022, 9, 15, 11, 30, 0),
                    Description = "Received payment for freelance work.",
                    Type = "Income",
                    Amount = 600.75m
                },
                new()
                {
                    Id = "123",
                    UserId = userId,
                    Category = "Rental Income",
                    Date = new DateTime(2022, 9, 20, 9, 0, 0),
                    Description = "Received rental income from property.",
                    Type = "Income",
                    Amount = 1500.50m
                },
                new()
                {
                    Id = "124",
                    UserId = userId,
                    Category = "Bonus",
                    Date = new DateTime(2022, 9, 22, 14, 30, 0),
                    Description = "Received a bonus at work.",
                    Type = "Income",
                    Amount = 900.00m
                },
                new()
                {
                    Id = "125",
                    UserId = userId,
                    Category = "Investment Returns",
                    Date = new DateTime(2022, 12, 25, 16, 45, 0),
                    Description = "Received returns from investments.",
                    Type = "Income",
                    Amount = 750.25m
                },
                new()
                {
                    Id = "126",
                    UserId = userId,
                    Category = "Vacation",
                    Date = new DateTime(2022, 9, 26, 12, 0, 0),
                    Description = "Booked a holiday package for vacation.",
                    Type = "Expense",
                    Amount = 800.00m
                },
                new()
                {
                    Id = "127",
                    UserId = userId,
                    Category = "Electronics",
                    Date = new DateTime(2022, 9, 27, 9, 30, 0),
                    Description = "Purchased a new smartphone.",
                    Type = "Expense",
                    Amount = 350.00m
                },
                new()
                {
                    Id = "128",
                    UserId = userId,
                    Category = "Sports",
                    Date = new DateTime(2022, 9, 28, 16, 30, 0),
                    Description = "Bought sports equipment and gear.",
                    Type = "Expense",
                    Amount = 120.00m
                },
                new()
                {
                    Id = "129",
                    UserId = userId,
                    Category = "Subscriptions",
                    Date = new DateTime(2022, 9, 29, 14, 0, 0),
                    Description = "Paid for streaming and magazine subscriptions.",
                    Type = "Expense",
                    Amount = 20.00m
                },
                new()
                {
                    Id = "130",
                    UserId = userId,
                    Category = "Car Maintenance",
                    Date = new DateTime(2022, 9, 30, 11, 45, 0),
                    Description = "Serviced the car and changed oil.",
                    Type = "Expense",
                    Amount = 100.00m
                },

                // October 2022
                new()
                {
                    Id = "86",
                    UserId = userId,
                    Category = "Groceries",
                    Date = new DateTime(2022, 10, 1, 12, 30, 0),
                    Description = "Bought groceries at the local supermarket.",
                    Type = "Expense",
                    Amount = 120.50m
                },
                new()
                {
                    Id = "87",
                    UserId = userId,
                    Category = "Entertainment",
                    Date = new DateTime(2022, 10, 2, 18, 15, 0),
                    Description = "Purchased movie tickets for a new release.",
                    Type = "Expense",
                    Amount = 75.20m
                },
                new()
                {
                    Id = "88",
                    UserId = userId,
                    Category = "Utilities",
                    Date = new DateTime(2022, 10, 3, 9, 0, 0),
                    Description = "Paid electricity bill for the month.",
                    Type = "Expense",
                    Amount = 95.80m
                },
                new()
                {
                    Id = "89",
                    UserId = userId,
                    Category = "Dining Out",
                    Date = new DateTime(2022, 10, 4, 20, 0, 0),
                    Description = "Had dinner at a fancy restaurant.",
                    Type = "Expense",
                    Amount = 150.00m
                },
                new()
                {
                    Id = "90",
                    UserId = userId,
                    Category = "Shopping",
                    Date = new DateTime(2022, 10, 5, 14, 45, 0),
                    Description = "Bought new clothes at the mall.",
                    Type = "Expense",
                    Amount = 200.25m
                },
                new()
                {
                    Id = "91",
                    UserId = userId,
                    Category = "Salary",
                    Date = new DateTime(2022, 10, 10, 15, 0, 0),
                    Description = "Received monthly salary.",
                    Type = "Income",
                    Amount = 3500.00m
                },
                new()
                {
                    Id = "92",
                    UserId = userId,
                    Category = "Freelance",
                    Date = new DateTime(2022, 10, 15, 11, 30, 0),
                    Description = "Received payment for freelance work.",
                    Type = "Income",
                    Amount = 700.75m
                },
                new()
                {
                    Id = "93",
                    UserId = userId,
                    Category = "Rental Income",
                    Date = new DateTime(2022, 10, 20, 9, 0, 0),
                    Description = "Received rental income from property.",
                    Type = "Income",
                    Amount = 1800.50m
                },
                new()
                {
                    Id = "94",
                    UserId = userId,
                    Category = "Bonus",
                    Date = new DateTime(2022, 10, 22, 14, 30, 0),
                    Description = "Received a bonus at work.",
                    Type = "Income",
                    Amount = 1200.00m
                },
                new()
                {
                    Id = "95",
                    UserId = userId,
                    Category = "Investment Returns",
                    Date = new DateTime(2022, 10, 25, 16, 45, 0),
                    Description = "Received returns from investments.",
                    Type = "Income",
                    Amount = 800.25m
                },
                new()
                {
                    Id = "96",
                    UserId = userId,
                    Category = "Healthcare",
                    Date = new DateTime(2022, 10, 27, 10, 30, 0),
                    Description = "Paid for a medical check-up and prescriptions.",
                    Type = "Expense",
                    Amount = 85.00m
                },
                new()
                {
                    Id = "97",
                    UserId = userId,
                    Category = "Travel",
                    Date = new DateTime(2022, 10, 28, 8, 0, 0),
                    Description = "Booked flight tickets for a vacation.",
                    Type = "Expense",
                    Amount = 500.00m
                },
                new()
                {
                    Id = "98",
                    UserId = userId,
                    Category = "Charity",
                    Date = new DateTime(2022, 10, 29, 14, 0, 0),
                    Description = "Donated to a local non-profit organization.",
                    Type = "Expense",
                    Amount = 70.00m
                },
                new()
                {
                    Id = "99",
                    UserId = userId,
                    Category = "Books",
                    Date = new DateTime(2022, 10, 30, 11, 0, 0),
                    Description = "Purchased novels from a bookstore.",
                    Type = "Expense",
                    Amount = 40.00m
                },
                new()
                {
                    Id = "100",
                    UserId = userId,
                    Category = "Gifts",
                    Date = new DateTime(2022, 10, 31, 17, 30, 0),
                    Description = "Bought presents for a friend's birthday.",
                    Type = "Expense",
                    Amount = 80.00m
                },
                // November 2022
                new()
                {
                    Id = "101",
                    UserId = userId,
                    Category = "Groceries",
                    Date = new DateTime(2022, 11, 1, 12, 30, 0),
                    Description = "Bought groceries at the local supermarket.",
                    Type = "Expense",
                    Amount = 110.50m
                },
                new()
                {
                    Id = "102",
                    UserId = userId,
                    Category = "Entertainment",
                    Date = new DateTime(2022, 11, 2, 18, 15, 0),
                    Description = "Purchased movie tickets for a new release.",
                    Type = "Expense",
                    Amount = 85.20m
                },
                new()
                {
                    Id = "103",
                    UserId = userId,
                    Category = "Utilities",
                    Date = new DateTime(2022, 11, 3, 9, 0, 0),
                    Description = "Paid electricity bill for the month.",
                    Type = "Expense",
                    Amount = 105.80m
                },
                new()
                {
                    Id = "104",
                    UserId = userId,
                    Category = "Dining Out",
                    Date = new DateTime(2022, 11, 4, 20, 0, 0),
                    Description = "Had dinner at a fancy restaurant.",
                    Type = "Expense",
                    Amount = 160.00m
                },
                new()
                {
                    Id = "105",
                    UserId = userId,
                    Category = "Shopping",
                    Date = new DateTime(2022, 11, 5, 14, 45, 0),
                    Description = "Bought new clothes at the mall.",
                    Type = "Expense",
                    Amount = 210.25m
                },
                new()
                {
                    Id = "106",
                    UserId = userId,
                    Category = "Salary",
                    Date = new DateTime(2022, 11, 10, 15, 0, 0),
                    Description = "Received monthly salary.",
                    Type = "Income",
                    Amount = 3200.00m
                },
                new()
                {
                    Id = "107",
                    UserId = userId,
                    Category = "Freelance",
                    Date = new DateTime(2022, 11, 15, 11, 30, 0),
                    Description = "Received payment for freelance work.",
                    Type = "Income",
                    Amount = 550.75m
                },
                new()
                {
                    Id = "108",
                    UserId = userId,
                    Category = "Rental Income",
                    Date = new DateTime(2022, 11, 20, 9, 0, 0),
                    Description = "Received rental income from property.",
                    Type = "Income",
                    Amount = 1300.50m
                },
                new()
                {
                    Id = "109",
                    UserId = userId,
                    Category = "Bonus",
                    Date = new DateTime(2022, 11, 22, 14, 30, 0),
                    Description = "Received a bonus at work.",
                    Type = "Income",
                    Amount = 850.00m
                },
                new()
                {
                    Id = "110",
                    UserId = userId,
                    Category = "Investment Returns",
                    Date = new DateTime(2022, 11, 25, 16, 45, 0),
                    Description = "Received returns from investments.",
                    Type = "Income",
                    Amount = 650.25m
                },
                new()
                {
                    Id = "111",
                    UserId = userId,
                    Category = "Transportation",
                    Date = new DateTime(2022, 11, 26, 8, 30, 0),
                    Description = "Paid for monthly public transportation pass.",
                    Type = "Expense",
                    Amount = 80.00m
                },
                new()
                {
                    Id = "112",
                    UserId = userId,
                    Category = "Home Repairs",
                    Date = new DateTime(2022, 11, 27, 16, 0, 0),
                    Description = "Hired a plumber to fix a leak.",
                    Type = "Expense",
                    Amount = 180.00m
                },
                new()
                {
                    Id = "113",
                    UserId = userId,
                    Category = "Education",
                    Date = new DateTime(2022, 11, 28, 13, 0, 0),
                    Description = "Enrolled in a language course.",
                    Type = "Expense",
                    Amount = 230.00m
                },
                new()
                {
                    Id = "114",
                    UserId = userId,
                    Category = "Gym Membership",
                    Date = new DateTime(2022, 11, 29, 7, 45, 0),
                    Description = "Paid for a monthly gym membership.",
                    Type = "Expense",
                    Amount = 60.00m
                },
                new()
                {
                    Id = "115",
                    UserId = userId,
                    Category = "Pets",
                    Date = new DateTime(2022, 11, 30, 15, 30, 0),
                    Description = "Bought pet supplies for the month.",
                    Type = "Expense",
                    Amount = 55.00m
                },
                // December 2022
                new()
                {
                    Id = "116",
                    UserId = userId,
                    Category = "Groceries",
                    Date = new DateTime(2022, 12, 1, 12, 30, 0),
                    Description = "Bought groceries at the local supermarket.",
                    Type = "Expense",
                    Amount = 130.50m
                },
                new()
                {
                    Id = "117",
                    UserId = userId,
                    Category = "Entertainment",
                    Date = new DateTime(2022, 12, 2, 18, 15, 0),
                    Description = "Purchased movie tickets for a new release.",
                    Type = "Expense",
                    Amount = 95.20m
                },
                new()
                {
                    Id = "118",
                    UserId = userId,
                    Category = "Utilities",
                    Date = new DateTime(2022, 12, 3, 9, 0, 0),
                    Description = "Paid electricity bill for the month.",
                    Type = "Expense",
                    Amount = 115.80m
                },
                new()
                {
                    Id = "119",
                    UserId = userId,
                    Category = "Dining Out",
                    Date = new DateTime(2022, 12, 4, 20, 0, 0),
                    Description = "Had dinner at a fancy restaurant.",
                    Type = "Expense",
                    Amount = 140.00m
                },
                new()
                {
                    Id = "120",
                    UserId = userId,
                    Category = "Shopping",
                    Date = new DateTime(2022, 12, 5, 14, 45, 0),
                    Description = "Bought new clothes at the mall.",
                    Type = "Expense",
                    Amount = 220.25m
                },
                new()
                {
                    Id = "121",
                    UserId = userId,
                    Category = "Salary",
                    Date = new DateTime(2022, 12, 10, 15, 0, 0),
                    Description = "Received monthly salary.",
                    Type = "Income",
                    Amount = 3700.00m
                },
                new()
                {
                    Id = "122",
                    UserId = userId,
                    Category = "Freelance",
                    Date = new DateTime(2022, 12, 15, 11, 30, 0),
                    Description = "Received payment for freelance work.",
                    Type = "Income",
                    Amount = 600.75m
                },
                new()
                {
                    Id = "123",
                    UserId = userId,
                    Category = "Rental Income",
                    Date = new DateTime(2022, 12, 20, 9, 0, 0),
                    Description = "Received rental income from property.",
                    Type = "Income",
                    Amount = 1500.50m
                },
                new()
                {
                    Id = "124",
                    UserId = userId,
                    Category = "Bonus",
                    Date = new DateTime(2022, 12, 22, 14, 30, 0),
                    Description = "Received a bonus at work.",
                    Type = "Income",
                    Amount = 900.00m
                },
                new()
                {
                    Id = "125",
                    UserId = userId,
                    Category = "Investment Returns",
                    Date = new DateTime(2022, 12, 25, 16, 45, 0),
                    Description = "Received returns from investments.",
                    Type = "Income",
                    Amount = 750.25m
                },
                new()
                {
                    Id = "126",
                    UserId = userId,
                    Category = "Vacation",
                    Date = new DateTime(2022, 12, 26, 12, 0, 0),
                    Description = "Booked a holiday package for vacation.",
                    Type = "Expense",
                    Amount = 800.00m
                },
                new()
                {
                    Id = "127",
                    UserId = userId,
                    Category = "Electronics",
                    Date = new DateTime(2022, 12, 27, 9, 30, 0),
                    Description = "Purchased a new smartphone.",
                    Type = "Expense",
                    Amount = 350.00m
                },
                new()
                {
                    Id = "128",
                    UserId = userId,
                    Category = "Sports",
                    Date = new DateTime(2022, 12, 28, 16, 30, 0),
                    Description = "Bought sports equipment and gear.",
                    Type = "Expense",
                    Amount = 120.00m
                },
                new()
                {
                    Id = "129",
                    UserId = userId,
                    Category = "Subscriptions",
                    Date = new DateTime(2022, 12, 29, 14, 0, 0),
                    Description = "Paid for streaming and magazine subscriptions.",
                    Type = "Expense",
                    Amount = 20.00m
                },
                new()
                {
                    Id = "130",
                    UserId = userId,
                    Category = "Car Maintenance",
                    Date = new DateTime(2022, 12, 30, 11, 45, 0),
                    Description = "Serviced the car and changed oil.",
                    Type = "Expense",
                    Amount = 100.00m
                },
                // January 2023
                new()
                {
                    Id = "131",
                    UserId = userId,
                    Category = "Groceries",
                    Date = new DateTime(2023, 1, 1, 12, 30, 0),
                    Description = "Bought groceries at the local supermarket.",
                    Type = "Expense",
                    Amount = 110.50m
                },
                new()
                {
                    Id = "132",
                    UserId = userId,
                    Category = "Entertainment",
                    Date = new DateTime(2023, 1, 2, 18, 15, 0),
                    Description = "Purchased movie tickets for a new release.",
                    Type = "Expense",
                    Amount = 85.20m
                },
                new()
                {
                    Id = "133",
                    UserId = userId,
                    Category = "Utilities",
                    Date = new DateTime(2023, 1, 3, 9, 0, 0),
                    Description = "Paid electricity bill for the month.",
                    Type = "Expense",
                    Amount = 105.80m
                },
                new()
                {
                    Id = "134",
                    UserId = userId,
                    Category = "Dining Out",
                    Date = new DateTime(2023, 1, 4, 20, 0, 0),
                    Description = "Had dinner at a fancy restaurant.",
                    Type = "Expense",
                    Amount = 160.00m
                },
                new()
                {
                    Id = "135",
                    UserId = userId,
                    Category = "Shopping",
                    Date = new DateTime(2023, 1, 5, 14, 45, 0),
                    Description = "Bought new clothes at the mall.",
                    Type = "Expense",
                    Amount = 210.25m
                },
                new()
                {
                    Id = "136",
                    UserId = userId,
                    Category = "Salary",
                    Date = new DateTime(2023, 1, 10, 15, 0, 0),
                    Description = "Received monthly salary.",
                    Type = "Income",
                    Amount = 3200.00m
                },
                new()
                {
                    Id = "137",
                    UserId = userId,
                    Category = "Freelance",
                    Date = new DateTime(2023, 1, 15, 11, 30, 0),
                    Description = "Received payment for freelance work.",
                    Type = "Income",
                    Amount = 550.75m
                },
                new()
                {
                    Id = "138",
                    UserId = userId,
                    Category = "Rental Income",
                    Date = new DateTime(2023, 1, 20, 9, 0, 0),
                    Description = "Received rental income from property.",
                    Type = "Income",
                    Amount = 1300.50m
                },
                new()
                {
                    Id = "139",
                    UserId = userId,
                    Category = "Bonus",
                    Date = new DateTime(2023, 1, 22, 14, 30, 0),
                    Description = "Received a bonus at work.",
                    Type = "Income",
                    Amount = 850.00m
                },
                new()
                {
                    Id = "140",
                    UserId = userId,
                    Category = "Investment Returns",
                    Date = new DateTime(2023, 1, 25, 16, 45, 0),
                    Description = "Received returns from investments.",
                    Type = "Income",
                    Amount = 650.25m
                },
                new()
                {
                    Id = "141",
                    UserId = userId,
                    Category = "Transportation",
                    Date = new DateTime(2023, 1, 26, 8, 30, 0),
                    Description = "Paid for monthly public transportation pass.",
                    Type = "Expense",
                    Amount = 80.00m
                },
                new()
                {
                    Id = "142",
                    UserId = userId,
                    Category = "Home Repairs",
                    Date = new DateTime(2023, 1, 27, 16, 0, 0),
                    Description = "Hired a plumber to fix a leak.",
                    Type = "Expense",
                    Amount = 180.00m
                },
                new()
                {
                    Id = "143",
                    UserId = userId,
                    Category = "Education",
                    Date = new DateTime(2023, 1, 28, 13, 0, 0),
                    Description = "Enrolled in a language course.",
                    Type = "Expense",
                    Amount = 230.00m
                },
                new()
                {
                    Id = "144",
                    UserId = userId,
                    Category = "Gym Membership",
                    Date = new DateTime(2023, 1, 29, 7, 45, 0),
                    Description = "Paid for a monthly gym membership.",
                    Type = "Expense",
                    Amount = 60.00m
                },
                new()
                {
                    Id = "145",
                    UserId = userId,
                    Category = "Pets",
                    Date = new DateTime(2023, 1, 30, 15, 30, 0),
                    Description = "Bought pet supplies for the month.",
                    Type = "Expense",
                    Amount = 55.00m
                },
                // February 2023
                new()
                {
                    Id = "146",
                    UserId = userId,
                    Category = "Groceries",
                    Date = new DateTime(2023, 2, 1, 12, 30, 0),
                    Description = "Bought groceries at the local supermarket.",
                    Type = "Expense",
                    Amount = 90.50m
                },
                new()
                {
                    Id = "147",
                    UserId = userId,
                    Category = "Entertainment",
                    Date = new DateTime(2023, 2, 2, 18, 15, 0),
                    Description = "Purchased movie tickets for a new release.",
                    Type = "Expense",
                    Amount = 65.20m
                },
                new()
                {
                    Id = "148",
                    UserId = userId,
                    Category = "Utilities",
                    Date = new DateTime(2023, 2, 3, 9, 0, 0),
                    Description = "Paid electricity bill for the month.",
                    Type = "Expense",
                    Amount = 115.80m
                },
                new()
                {
                    Id = "149",
                    UserId = userId,
                    Category = "Dining Out",
                    Date = new DateTime(2023, 2, 4, 20, 0, 0),
                    Description = "Had dinner at a fancy restaurant.",
                    Type = "Expense",
                    Amount = 130.00m
                },
                new()
                {
                    Id = "150",
                    UserId = userId,
                    Category = "Shopping",
                    Date = new DateTime(2023, 2, 5, 14, 45, 0),
                    Description = "Bought new clothes at the mall.",
                    Type = "Expense",
                    Amount = 180.25m
                },
                new()
                {
                    Id = "151",
                    UserId = userId,
                    Category = "Salary",
                    Date = new DateTime(2023, 2, 10, 15, 0, 0),
                    Description = "Received monthly salary.",
                    Type = "Income",
                    Amount = 3400.00m
                },
                new()
                {
                    Id = "152",
                    UserId = userId,
                    Category = "Freelance",
                    Date = new DateTime(2023, 2, 15, 11, 30, 0),
                    Description = "Received payment for freelance work.",
                    Type = "Income",
                    Amount = 600.75m
                },
                new()
                {
                    Id = "153",
                    UserId = userId,
                    Category = "Rental Income",
                    Date = new DateTime(2023, 2, 20, 9, 0, 0),
                    Description = "Received rental income from property.",
                    Type = "Income",
                    Amount = 1400.50m
                },
                new()
                {
                    Id = "154",
                    UserId = userId,
                    Category = "Bonus",
                    Date = new DateTime(2023, 2, 22, 14, 30, 0),
                    Description = "Received a bonus at work.",
                    Type = "Income",
                    Amount = 900.00m
                },
                new()
                {
                    Id = "155",
                    UserId = userId,
                    Category = "Investment Returns",
                    Date = new DateTime(2023, 2, 25, 16, 45, 0),
                    Description = "Received returns from investments.",
                    Type = "Income",
                    Amount = 750.25m
                },
                new()
                {
                    Id = "156",
                    UserId = userId,
                    Category = "Healthcare",
                    Date = new DateTime(2023, 2, 27, 10, 30, 0),
                    Description = "Paid for a medical check-up and prescriptions.",
                    Type = "Expense",
                    Amount = 85.00m
                },
                new()
                {
                    Id = "157",
                    UserId = userId,
                    Category = "Travel",
                    Date = new DateTime(2023, 2, 28, 8, 0, 0),
                    Description = "Booked flight tickets for a vacation.",
                    Type = "Expense",
                    Amount = 500.00m
                },
                new()
                {
                    Id = "158",
                    UserId = userId,
                    Category = "Charity",
                    Date = new DateTime(2023, 2, 28, 14, 0, 0),
                    Description = "Donated to a local non-profit organization.",
                    Type = "Expense",
                    Amount = 70.00m
                },
                // March 2023
                new()
                {
                    Id = "159",
                    UserId = userId,
                    Category = "Groceries",
                    Date = new DateTime(2023, 3, 1, 12, 30, 0),
                    Description = "Bought groceries at the local supermarket.",
                    Type = "Expense",
                    Amount = 100.50m
                },
                new()
                {
                    Id = "160",
                    UserId = userId,
                    Category = "Entertainment",
                    Date = new DateTime(2023, 3, 2, 18, 15, 0),
                    Description = "Purchased movie tickets for a new release.",
                    Type = "Expense",
                    Amount = 75.20m
                },
                new()
                {
                    Id = "161",
                    UserId = userId,
                    Category = "Utilities",
                    Date = new DateTime(2023, 3, 3, 9, 0, 0),
                    Description = "Paid electricity bill for the month.",
                    Type = "Expense",
                    Amount = 125.80m
                },
                new()
                {
                    Id = "162",
                    UserId = userId,
                    Category = "Dining Out",
                    Date = new DateTime(2023, 3, 4, 20, 0, 0),
                    Description = "Had dinner at a fancy restaurant.",
                    Type = "Expense",
                    Amount = 120.00m
                },
                new()
                {
                    Id = "163",
                    UserId = userId,
                    Category = "Shopping",
                    Date = new DateTime(2023, 3, 5, 14, 45, 0),
                    Description = "Bought new clothes at the mall.",
                    Type = "Expense",
                    Amount = 230.25m
                },
                new()
                {
                    Id = "164",
                    UserId = userId,
                    Category = "Salary",
                    Date = new DateTime(2023, 3, 10, 15, 0, 0),
                    Description = "Received monthly salary.",
                    Type = "Income",
                    Amount = 3100.00m
                },
                new()
                {
                    Id = "165",
                    UserId = userId,
                    Category = "Freelance",
                    Date = new DateTime(2023, 3, 15, 11, 30, 0),
                    Description = "Received payment for freelance work.",
                    Type = "Income",
                    Amount = 500.75m
                },
                new()
                {
                    Id = "166",
                    UserId = userId,
                    Category = "Rental Income",
                    Date = new DateTime(2023, 3, 20, 9, 0, 0),
                    Description = "Received rental income from property.",
                    Type = "Income",
                    Amount = 1600.50m
                },
                new()
                {
                    Id = "167",
                    UserId = userId,
                    Category = "Bonus",
                    Date = new DateTime(2023, 3, 22, 14, 30, 0),
                    Description = "Received a bonus at work.",
                    Type = "Income",
                    Amount = 1100.00m
                },
                new()
                {
                    Id = "168",
                    UserId = userId,
                    Category = "Investment Returns",
                    Date = new DateTime(2023, 3, 25, 16, 45, 0),
                    Description = "Received returns from investments.",
                    Type = "Income",
                    Amount = 850.25m
                },
                new()
                {
                    Id = "169",
                    UserId = userId,
                    Category = "Transportation",
                    Date = new DateTime(2023, 3, 26, 8, 30, 0),
                    Description = "Paid for monthly public transportation pass.",
                    Type = "Expense",
                    Amount = 80.00m
                },
                new()
                {
                    Id = "170",
                    UserId = userId,
                    Category = "Home Repairs",
                    Date = new DateTime(2023, 3, 27, 16, 0, 0),
                    Description = "Hired a plumber to fix a leak.",
                    Type = "Expense",
                    Amount = 180.00m
                },
                new()
                {
                    Id = "171",
                    UserId = userId,
                    Category = "Education",
                    Date = new DateTime(2023, 3, 28, 13, 0, 0),
                    Description = "Enrolled in a language course.",
                    Type = "Expense",
                    Amount = 230.00m
                },
                new()
                {
                    Id = "172",
                    UserId = userId,
                    Category = "Gym Membership",
                    Date = new DateTime(2023, 3, 29, 7, 45, 0),
                    Description = "Paid for a monthly gym membership.",
                    Type = "Expense",
                    Amount = 60.00m
                },
                new()
                {
                    Id = "173",
                    UserId = userId,
                    Category = "Pets",
                    Date = new DateTime(2023, 3, 30, 15, 30, 0),
                    Description = "Bought pet supplies for the month.",
                    Type = "Expense",
                    Amount = 55.00m
                },

                // April 2023
                new()
                {
                    UserId = userId,
                    Category = "Groceries",
                    Date = new DateTime(2023, 4, 1, 12, 30, 0),
                    Description = "Bought groceries at the local supermarket.",
                    Type = "Expense",
                    Amount = 95.50m
                },
                new()
                {
                    UserId = userId,
                    Category = "Entertainment",
                    Date = new DateTime(2023, 4, 2, 18, 15, 0),
                    Description = "Purchased movie tickets for a new release.",
                    Type = "Expense",
                    Amount = 80.20m
                },
                new()
                {
                    UserId = userId,
                    Category = "Utilities",
                    Date = new DateTime(2023, 4, 3, 9, 0, 0),
                    Description = "Paid electricity bill for the month.",
                    Type = "Expense",
                    Amount = 135.80m
                },
                new()
                {
                    UserId = userId,
                    Category = "Dining Out",
                    Date = new DateTime(2023, 4, 4, 20, 0, 0),
                    Description = "Had dinner at a fancy restaurant.",
                    Type = "Expense",
                    Amount = 110.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Shopping",
                    Date = new DateTime(2023, 4, 5, 14, 45, 0),
                    Description = "Bought new clothes at the mall.",
                    Type = "Expense",
                    Amount = 250.25m
                },
                new()
                {
                    UserId = userId,
                    Category = "Salary",
                    Date = new DateTime(2023, 4, 10, 15, 0, 0),
                    Description = "Received monthly salary.",
                    Type = "Income",
                    Amount = 3000.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Freelance",
                    Date = new DateTime(2023, 4, 15, 11, 30, 0),
                    Description = "Received payment for freelance work.",
                    Type = "Income",
                    Amount = 480.75m
                },
                new()
                {
                    UserId = userId,
                    Category = "Rental Income",
                    Date = new DateTime(2023, 4, 20, 9, 0, 0),
                    Description = "Received rental income from property.",
                    Type = "Income",
                    Amount = 1700.50m
                },
                new()
                {
                    UserId = userId,
                    Category = "Bonus",
                    Date = new DateTime(2023, 4, 22, 14, 30, 0),
                    Description = "Received a bonus at work.",
                    Type = "Income",
                    Amount = 950.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Investment Returns",
                    Date = new DateTime(2023, 4, 25, 16, 45, 0),
                    Description = "Received returns from investments.",
                    Type = "Income",
                    Amount = 800.25m
                },
                new()
                {
                    UserId = userId,
                    Category = "Healthcare",
                    Date = new DateTime(2023, 4, 27, 10, 30, 0),
                    Description = "Paid for a medical check-up and prescriptions.",
                    Type = "Expense",
                    Amount = 90.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Travel",
                    Date = new DateTime(2023, 4, 28, 8, 0, 0),
                    Description = "Booked flight tickets for a vacation.",
                    Type = "Expense",
                    Amount = 600.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Charity",
                    Date = new DateTime(2023, 4, 29, 14, 0, 0),
                    Description = "Donated to a local non-profit organization.",
                    Type = "Expense",
                    Amount = 50.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Books",
                    Date = new DateTime(2023, 4, 30, 11, 0, 0),
                    Description = "Purchased novels from a bookstore.",
                    Type = "Expense",
                    Amount = 30.00m
                },
                // May 2023
                new()
                {
                    UserId = userId,
                    Category = "Groceries",
                    Date = new DateTime(2023, 5, 1, 12, 30, 0),
                    Description = "Bought groceries at the local supermarket.",
                    Type = "Expense",
                    Amount = 120.50m
                },
                new()
                {
                    UserId = userId,
                    Category = "Entertainment",
                    Date = new DateTime(2023, 5, 2, 18, 15, 0),
                    Description = "Purchased movie tickets for a new release.",
                    Type = "Expense",
                    Amount = 85.20m
                },
                new()
                {
                    UserId = userId,
                    Category = "Utilities",
                    Date = new DateTime(2023, 5, 3, 9, 0, 0),
                    Description = "Paid electricity bill for the month.",
                    Type = "Expense",
                    Amount = 95.80m
                },
                new()
                {
                    UserId = userId,
                    Category = "Dining Out",
                    Date = new DateTime(2023, 5, 4, 20, 0, 0),
                    Description = "Had dinner at a fancy restaurant.",
                    Type = "Expense",
                    Amount = 150.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Shopping",
                    Date = new DateTime(2023, 5, 5, 14, 45, 0),
                    Description = "Bought new clothes at the mall.",
                    Type = "Expense",
                    Amount = 200.25m
                },
                new()
                {
                    UserId = userId,
                    Category = "Salary",
                    Date = new DateTime(2023, 5, 10, 15, 0, 0),
                    Description = "Received monthly salary.",
                    Type = "Income",
                    Amount = 3500.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Freelance",
                    Date = new DateTime(2023, 5, 15, 11, 30, 0),
                    Description = "Received payment for freelance work.",
                    Type = "Income",
                    Amount = 700.75m
                },
                new()
                {
                    UserId = userId,
                    Category = "Rental Income",
                    Date = new DateTime(2023, 5, 20, 9, 0, 0),
                    Description = "Received rental income from property.",
                    Type = "Income",
                    Amount = 1800.50m
                },
                new()
                {
                    UserId = userId,
                    Category = "Bonus",
                    Date = new DateTime(2023, 5, 22, 14, 30, 0),
                    Description = "Received a bonus at work.",
                    Type = "Income",
                    Amount = 1200.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Investment Returns",
                    Date = new DateTime(2023, 5, 25, 16, 45, 0),
                    Description = "Received returns from investments.",
                    Type = "Income",
                    Amount = 800.25m
                },
                new()
                {
                    UserId = userId,
                    Category = "Healthcare",
                    Date = new DateTime(2023, 5, 27, 10, 30, 0),
                    Description = "Paid for a medical check-up and prescriptions.",
                    Type = "Expense",
                    Amount = 85.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Travel",
                    Date = new DateTime(2023, 5, 28, 8, 0, 0),
                    Description = "Booked flight tickets for a vacation.",
                    Type = "Expense",
                    Amount = 500.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Charity",
                    Date = new DateTime(2023, 5, 29, 14, 0, 0),
                    Description = "Donated to a local non-profit organization.",
                    Type = "Expense",
                    Amount = 70.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Books",
                    Date = new DateTime(2023, 5, 30, 11, 0, 0),
                    Description = "Purchased novels from a bookstore.",
                    Type = "Expense",
                    Amount = 40.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Gifts",
                    Date = new DateTime(2023, 5, 31, 17, 30, 0),
                    Description = "Bought presents for a friend's birthday.",
                    Type = "Expense",
                    Amount = 80.00m
                },
                // June 2023
                new()
                {
                    UserId = userId,
                    Category = "Groceries",
                    Date = new DateTime(2023, 6, 1, 12, 30, 0),
                    Description = "Bought groceries at the local supermarket.",
                    Type = "Expense",
                    Amount = 130.50m
                },
                new()
                {
                    UserId = userId,
                    Category = "Entertainment",
                    Date = new DateTime(2023, 6, 2, 18, 15, 0),
                    Description = "Purchased movie tickets for a new release.",
                    Type = "Expense",
                    Amount = 95.20m
                },
                new()
                {
                    UserId = userId,
                    Category = "Utilities",
                    Date = new DateTime(2023, 6, 3, 9, 0, 0),
                    Description = "Paid electricity bill for the month.",
                    Type = "Expense",
                    Amount = 115.80m
                },
                new()
                {
                    UserId = userId,
                    Category = "Dining Out",
                    Date = new DateTime(2023, 6, 4, 20, 0, 0),
                    Description = "Had dinner at a fancy restaurant.",
                    Type = "Expense",
                    Amount = 140.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Shopping",
                    Date = new DateTime(2023, 6, 5, 14, 45, 0),
                    Description = "Bought new clothes at the mall.",
                    Type = "Expense",
                    Amount = 220.25m
                },
                new()
                {
                    UserId = userId,
                    Category = "Salary",
                    Date = new DateTime(2023, 6, 10, 15, 0, 0),
                    Description = "Received monthly salary.",
                    Type = "Income",
                    Amount = 3700.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Freelance",
                    Date = new DateTime(2023, 6, 15, 11, 30, 0),
                    Description = "Received payment for freelance work.",
                    Type = "Income",
                    Amount = 600.75m
                },
                new()
                {
                    UserId = userId,
                    Category = "Rental Income",
                    Date = new DateTime(2023, 6, 20, 9, 0, 0),
                    Description = "Received rental income from property.",
                    Type = "Income",
                    Amount = 1500.50m
                },
                new()
                {
                    UserId = userId,
                    Category = "Bonus",
                    Date = new DateTime(2023, 6, 22, 14, 30, 0),
                    Description = "Received a bonus at work.",
                    Type = "Income",
                    Amount = 900.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Investment Returns",
                    Date = new DateTime(2023, 6, 25, 16, 45, 0),
                    Description = "Received returns from investments.",
                    Type = "Income",
                    Amount = 750.25m
                },
                new()
                {
                    UserId = userId,
                    Category = "Vacation",
                    Date = new DateTime(2023, 6, 26, 12, 0, 0),
                    Description = "Booked a holiday package for vacation.",
                    Type = "Expense",
                    Amount = 800.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Electronics",
                    Date = new DateTime(2023, 6, 27, 9, 30, 0),
                    Description = "Purchased a new smartphone.",
                    Type = "Expense",
                    Amount = 350.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Sports",
                    Date = new DateTime(2023, 6, 28, 16, 30, 0),
                    Description = "Bought sports equipment and gear.",
                    Type = "Expense",
                    Amount = 120.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Subscriptions",
                    Date = new DateTime(2023, 6, 29, 14, 0, 0),
                    Description = "Paid for streaming and magazine subscriptions.",
                    Type = "Expense",
                    Amount = 20.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Car Maintenance",
                    Date = new DateTime(2023, 6, 30, 11, 45, 0),
                    Description = "Serviced the car and changed oil.",
                    Type = "Expense",
                    Amount = 100.00m
                },
                // July 2023
                new()
                {
                    UserId = userId,
                    Category = "Groceries",
                    Date = new DateTime(2023, 7, 1, 12, 30, 0),
                    Description = "Bought groceries at the local supermarket.",
                    Type = "Expense",
                    Amount = 110.50m
                },
                new()
                {
                    UserId = userId,
                    Category = "Entertainment",
                    Date = new DateTime(2023, 7, 2, 18, 15, 0),
                    Description = "Purchased movie tickets for a new release.",
                    Type = "Expense",
                    Amount = 85.20m
                },
                new()
                {
                    UserId = userId,
                    Category = "Utilities",
                    Date = new DateTime(2023, 7, 3, 9, 0, 0),
                    Description = "Paid electricity bill for the month.",
                    Type = "Expense",
                    Amount = 105.80m
                },
                new()
                {
                    Id = "221",
                    UserId = userId,
                    Category = "Dining Out",
                    Date = new DateTime(2023, 7, 4, 20, 0, 0),
                    Description = "Had dinner at a fancy restaurant.",
                    Type = "Expense",
                    Amount = 160.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Shopping",
                    Date = new DateTime(2023, 7, 5, 14, 45, 0),
                    Description = "Bought new clothes at the mall.",
                    Type = "Expense",
                    Amount = 210.25m
                },
                new()
                {
                    UserId = userId,
                    Category = "Salary",
                    Date = new DateTime(2023, 7, 10, 15, 0, 0),
                    Description = "Received monthly salary.",
                    Type = "Income",
                    Amount = 3200.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Freelance",
                    Date = new DateTime(2023, 7, 15, 11, 30, 0),
                    Description = "Received payment for freelance work.",
                    Type = "Income",
                    Amount = 550.75m
                },
                new()
                {
                    Id = "225",
                    UserId = userId,
                    Category = "Rental Income",
                    Date = new DateTime(2023, 7, 20, 9, 0, 0),
                    Description = "Received rental income from property.",
                    Type = "Income",
                    Amount = 1300.50m
                },
                new()
                {
                    UserId = userId,
                    Category = "Bonus",
                    Date = new DateTime(2023, 7, 22, 14, 30, 0),
                    Description = "Received a bonus at work.",
                    Type = "Income",
                    Amount = 850.00m
                },
                new()
                {
                    Id = "227",
                    UserId = userId,
                    Category = "Investment Returns",
                    Date = new DateTime(2023, 7, 25, 16, 45, 0),
                    Description = "Received returns from investments.",
                    Type = "Income",
                    Amount = 650.25m
                },
                new()
                {
                    UserId = userId,
                    Category = "Transportation",
                    Date = new DateTime(2023, 7, 26, 8, 30, 0),
                    Description = "Paid for monthly public transportation pass.",
                    Type = "Expense",
                    Amount = 80.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Home Repairs",
                    Date = new DateTime(2023, 7, 27, 16, 0, 0),
                    Description = "Hired a plumber to fix a leak.",
                    Type = "Expense",
                    Amount = 180.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Education",
                    Date = new DateTime(2023, 7, 28, 13, 0, 0),
                    Description = "Enrolled in a language course.",
                    Type = "Expense",
                    Amount = 230.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Gym Membership",
                    Date = new DateTime(2023, 7, 29, 7, 45, 0),
                    Description = "Paid for a monthly gym membership.",
                    Type = "Expense",
                    Amount = 60.00m
                },
                new()
                {
                    UserId = userId,
                    Category = "Pets",
                    Date = new DateTime(2023, 7, 30, 15, 30, 0),
                    Description = "Bought pet supplies for the month.",
                    Type = "Expense",
                    Amount = 55.00m
                },
                // August 2023
                new()
                {
                    Id = "233",
                    UserId = userId,
                    Category = "Groceries",
                    Date = new DateTime(2023, 8, 1, 12, 30, 0),
                    Description = "Bought groceries at the local supermarket.",
                    Type = "Expense",
                    Amount = 95.50m
                },
                new()
                {
                    Id = "234",
                    UserId = userId,
                    Category = "Entertainment",
                    Date = new DateTime(2023, 8, 2, 18, 15, 0),
                    Description = "Purchased movie tickets for a new release.",
                    Type = "Expense",
                    Amount = 80.20m
                },
                new()
                {
                    Id = "235",
                    UserId = userId,
                    Category = "Utilities",
                    Date = new DateTime(2023, 8, 3, 9, 0, 0),
                    Description = "Paid electricity bill for the month.",
                    Type = "Expense",
                    Amount = 135.80m
                },
                new()
                {
                    Id = "236",
                    UserId = userId,
                    Category = "Dining Out",
                    Date = new DateTime(2023, 8, 4, 20, 0, 0),
                    Description = "Had dinner at a fancy restaurant.",
                    Type = "Expense",
                    Amount = 110.00m
                },
                new()
                {
                    Id = "237",
                    UserId = userId,
                    Category = "Shopping",
                    Date = new DateTime(2023, 8, 5, 14, 45, 0),
                    Description = "Bought new clothes at the mall.",
                    Type = "Expense",
                    Amount = 250.25m
                },
                new()
                {
                    Id = "238",
                    UserId = userId,
                    Category = "Salary",
                    Date = new DateTime(2023, 8, 10, 15, 0, 0),
                    Description = "Received monthly salary.",
                    Type = "Income",
                    Amount = 3500.00m
                },
                new()
                {
                    Id = "239",
                    UserId = userId,
                    Category = "Freelance",
                    Date = new DateTime(2023, 8, 15, 11, 30, 0),
                    Description = "Received payment for freelance work.",
                    Type = "Income",
                    Amount = 480.75m
                },
                new()
                {
                    Id = "240",
                    UserId = userId,
                    Category = "Rental Income",
                    Date = new DateTime(2023, 8, 20, 9, 0, 0),
                    Description = "Received rental income from property.",
                    Type = "Income",
                    Amount = 1700.50m
                },
                new()
                {
                    Id = "241",
                    UserId = userId,
                    Category = "Bonus",
                    Date = new DateTime(2023, 8, 22, 14, 30, 0),
                    Description = "Received a bonus at work.",
                    Type = "Income",
                    Amount = 950.00m
                },
                new()
                {
                    Id = "242",
                    UserId = userId,
                    Category = "Investment Returns",
                    Date = new DateTime(2023, 8, 25, 16, 45, 0),
                    Description = "Received returns from investments.",
                    Type = "Income",
                    Amount = 800.25m
                },
                new()
                {
                    Id = "243",
                    UserId = userId,
                    Category = "Healthcare",
                    Date = new DateTime(2023, 8, 27, 10, 30, 0),
                    Description = "Paid for a medical check-up and prescriptions.",
                    Type = "Expense",
                    Amount = 90.00m
                },
                new()
                {
                    Id = "244",
                    UserId = userId,
                    Category = "Travel",
                    Date = new DateTime(2023, 8, 28, 8, 0, 0),
                    Description = "Booked flight tickets for a vacation.",
                    Type = "Expense",
                    Amount = 600.00m
                },
                new()
                {
                    Id = "245",
                    UserId = userId,
                    Category = "Charity",
                    Date = new DateTime(2023, 8, 29, 14, 0, 0),
                    Description = "Donated to a local non-profit organization.",
                    Type = "Expense",
                    Amount = 50.00m
                },
                new()
                {
                    Id = "246",
                    UserId = userId,
                    Category = "Books",
                    Date = new DateTime(2023, 8, 30, 11, 0, 0),
                    Description = "Purchased novels from a bookstore.",
                    Type = "Expense",
                    Amount = 30.00m
                },
                // September 2023
                new()
                {
                    Id = "247",
                    UserId = userId,
                    Category = "Groceries",
                    Date = new DateTime(2023, 9, 1, 12, 30, 0),
                    Description = "Bought groceries at the local supermarket.",
                    Type = "Expense",
                    Amount = 120.50m
                },
                new()
                {
                    Id = "248",
                    UserId = userId,
                    Category = "Entertainment",
                    Date = new DateTime(2023, 9, 2, 18, 15, 0),
                    Description = "Purchased movie tickets for a new release.",
                    Type = "Expense",
                    Amount = 85.20m
                },
                new()
                {
                    Id = "249",
                    UserId = userId,
                    Category = "Utilities",
                    Date = new DateTime(2023, 9, 3, 9, 0, 0),
                    Description = "Paid electricity bill for the month.",
                    Type = "Expense",
                    Amount = 95.80m
                },
                new()
                {
                    Id = "250",
                    UserId = userId,
                    Category = "Dining Out",
                    Date = new DateTime(2023, 9, 4, 20, 0, 0),
                    Description = "Had dinner at a fancy restaurant.",
                    Type = "Expense",
                    Amount = 150.00m
                },
                new()
                {
                    Id = "251",
                    UserId = userId,
                    Category = "Shopping",
                    Date = new DateTime(2023, 9, 5, 14, 45, 0),
                    Description = "Bought new clothes at the mall.",
                    Type = "Expense",
                    Amount = 200.25m
                },
                new()
                {
                    Id = "252",
                    UserId = userId,
                    Category = "Salary",
                    Date = new DateTime(2023, 9, 10, 15, 0, 0),
                    Description = "Received monthly salary.",
                    Type = "Income",
                    Amount = 3500.00m
                },
                new()
                {
                    Id = "253",
                    UserId = userId,
                    Category = "Freelance",
                    Date = new DateTime(2023, 9, 15, 11, 30, 0),
                    Description = "Received payment for freelance work.",
                    Type = "Income",
                    Amount = 700.75m
                },
                new()
                {
                    Id = "254",
                    UserId = userId,
                    Category = "Rental Income",
                    Date = new DateTime(2023, 9, 20, 9, 0, 0),
                    Description = "Received rental income from property.",
                    Type = "Income",
                    Amount = 1800.50m
                },
                new()
                {
                    Id = "255",
                    UserId = userId,
                    Category = "Bonus",
                    Date = new DateTime(2023, 9, 22, 14, 30, 0),
                    Description = "Received a bonus at work.",
                    Type = "Income",
                    Amount = 1200.00m
                },
                new()
                {
                    Id = "256",
                    UserId = userId,
                    Category = "Investment Returns",
                    Date = new DateTime(2023, 9, 25, 16, 45, 0),
                    Description = "Received returns from investments.",
                    Type = "Income",
                    Amount = 800.25m
                },
                new()
                {
                    Id = "257",
                    UserId = userId,
                    Category = "Healthcare",
                    Date = new DateTime(2023, 9, 27, 10, 30, 0),
                    Description = "Paid for a medical check-up and prescriptions.",
                    Type = "Expense",
                    Amount = 85.00m
                },
                new()
                {
                    Id = "258",
                    UserId = userId,
                    Category = "Travel",
                    Date = new DateTime(2023, 9, 28, 8, 0, 0),
                    Description = "Booked flight tickets for a vacation.",
                    Type = "Expense",
                    Amount = 500.00m
                },
                new()
                {
                    Id = "259",
                    UserId = userId,
                    Category = "Charity",
                    Date = new DateTime(2023, 9, 29, 14, 0, 0),
                    Description = "Donated to a local non-profit organization.",
                    Type = "Expense",
                    Amount = 70.00m
                },
                new()
                {
                    Id = "260",
                    UserId = userId,
                    Category = "Books",
                    Date = new DateTime(2023, 9, 30, 11, 0, 0),
                    Description = "Purchased novels from a bookstore.",
                    Type = "Expense",
                    Amount = 40.00m
                },
            };

            foreach (var transaction in transactionsToAdd)
            {
                transaction.Date = new DateTime(transaction.Date.Year, transaction.Date.Month, transaction.Date.Day, 0, 0, 0, DateTimeKind.Utc);

                var existingTransaction = await dbContext.Transactions.FindAsync(transaction.Id);
                if (existingTransaction == null)
                {
                    dbContext.Transactions.Add(transaction);
                }
            }

            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedSavingsAsync(ApplicationDbContext dbContext, IServiceScope serviceScope)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
            string appUserEmail = "user@gmail.com";
            var appUser = await userManager.FindByEmailAsync(appUserEmail);
            string userId = appUser.Id;

            var savingsToAdd = new List<Savings>
            {
                new()
                {
                    Id = "1",
                    UserId = userId,
                    TargetAmount = 1500.00m,
                    CurrentAmount = 500.00m,
                    Deadline = new DateTime(2023, 09, 20),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 05),
                    Title = "Vacation Fund"
                },
                new()
                {
                    Id = "2",
                    UserId = userId,
                    TargetAmount = 800.00m,
                    CurrentAmount = 100.00m,
                    Deadline = new DateTime(2023, 08, 31),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 06),
                    Title = "New Laptop"
                },
                new()
                {
                    Id = "3",
                    UserId = userId,
                    TargetAmount = 3000.00m,
                    CurrentAmount = 3000.00m,
                    Deadline = new DateTime(2023, 12, 15),
                    Status = "Completed",
                    Date = new DateTime(2023, 08, 07),
                    Title = "Emergency Fund"
                },
                new()
                {
                    Id = "4",
                    UserId = userId,
                    TargetAmount = 2500.00m,
                    CurrentAmount = 1000.00m,
                    Deadline = new DateTime(2023, 10, 30),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 08),
                    Title = "Home Renovation"
                },
                new()
                {
                    Id = "5",
                    UserId = userId,
                    TargetAmount = 500.00m,
                    CurrentAmount = 50.00m,
                    Deadline = new DateTime(2023, 08, 15),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 09),
                    Title = "Gifts for Family"
                },
                new()
                {
                    Id = "6",
                    UserId = userId,
                    TargetAmount = 1200.00m,
                    CurrentAmount = 800.00m,
                    Deadline = new DateTime(2023, 11, 30),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 10),
                    Title = "Car Repairs"
                },
                new()
                {
                    Id = "7",
                    UserId = userId,
                    TargetAmount = 10000.00m,
                    CurrentAmount = 6000.00m,
                    Deadline = new DateTime(2023, 12, 31),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 11),
                    Title = "Education Fund"
                },
                new()
                {
                    Id = "8",
                    UserId = userId,
                    TargetAmount = 4000.00m,
                    CurrentAmount = 2000.00m,
                    Deadline = new DateTime(2023, 11, 15),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 12),
                    Title = "Dream Vacation"
                },
                new()
                {
                    Id = "9",
                    UserId = userId,
                    TargetAmount = 6000.00m,
                    CurrentAmount = 2000.00m,
                    Deadline = new DateTime(2023, 10, 10),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 13),
                    Title = "Home Theater System"
                },
                new()
                {
                    Id = "10",
                    UserId = userId,
                    TargetAmount = 2000.00m,
                    CurrentAmount = 1000.00m,
                    Deadline = new DateTime(2023, 09, 10),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 14),
                    Title = "Gym Membership"
                },
                new()
                {
                    Id = "11",
                    UserId = userId,
                    TargetAmount = 3000.00m,
                    CurrentAmount = 1500.00m,
                    Deadline = new DateTime(2023, 12, 31),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 15),
                    Title = "Emergency Fund"
                },
                new()
                {
                    Id = "12",
                    UserId = userId,
                    TargetAmount = 1800.00m,
                    CurrentAmount = 500.00m,
                    Deadline = new DateTime(2023, 10, 31),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 16),
                    Title = "Vacation Fund"
                },
                new()
                {
                    Id = "13",
                    UserId = userId,
                    TargetAmount = 4000.00m,
                    CurrentAmount = 3500.00m,
                    Deadline = new DateTime(2023, 09, 20),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 17),
                    Title = "New Laptop"
                },
                new()
                {
                    Id = "14",
                    UserId = userId,
                    TargetAmount = 800.00m,
                    CurrentAmount = 200.00m,
                    Deadline = new DateTime(2023, 08, 31),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 18),
                    Title = "Emergency Fund"
                },
                new()
                {
                    Id = "15",
                    UserId = userId,
                    TargetAmount = 1000.00m,
                    CurrentAmount = 1000.00m,
                    Deadline = new DateTime(2023, 12, 15),
                    Status = "Completed",
                    Date = new DateTime(2023, 08, 19),
                    Title = "Home Renovation"
                },
                new()
                {
                    Id = "16",
                    UserId = userId,
                    TargetAmount = 5000.00m,
                    CurrentAmount = 2500.00m,
                    Deadline = new DateTime(2023, 10, 30),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 20),
                    Title = "Gifts for Family"
                },
                new()
                {
                    Id = "17",
                    UserId = userId,
                    TargetAmount = 800.00m,
                    CurrentAmount = 50.00m,
                    Deadline = new DateTime(2023, 08, 15),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 21),
                    Title = "Car Repairs"
                },
                new()
                {
                    Id = "18",
                    UserId = userId,
                    TargetAmount = 2000.00m,
                    CurrentAmount = 1000.00m,
                    Deadline = new DateTime(2023, 11, 30),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 22),
                    Title = "Education Fund"
                },
                new()
                {
                    Id = "19",
                    UserId = userId,
                    TargetAmount = 7000.00m,
                    CurrentAmount = 4000.00m,
                    Deadline = new DateTime(2023, 12, 31),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 23),
                    Title = "Dream Vacation"
                },
                new()
                {
                    Id = "20",
                    UserId = userId,
                    TargetAmount = 3000.00m,
                    CurrentAmount = 1500.00m,
                    Deadline = new DateTime(2023, 11, 15),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 24),
                    Title = "Home Theater System"
                },
                new()
                {
                    Id = "21",
                    UserId = userId,
                    TargetAmount = 1000.00m,
                    CurrentAmount = 200.00m,
                    Deadline = new DateTime(2023, 09, 10),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 25),
                    Title = "Gym Membership"
                },
                new()
                {
                    Id = "22",
                    UserId = userId,
                    TargetAmount = 5000.00m,
                    CurrentAmount = 1000.00m,
                    Deadline = new DateTime(2023, 12, 31),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 26),
                    Title = "Emergency Fund"
                },
                new()
                {
                    Id = "23",
                    UserId = userId,
                    TargetAmount = 3800.00m,
                    CurrentAmount = 500.00m,
                    Deadline = new DateTime(2023, 10, 31),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 27),
                    Title = "Vacation Fund"
                },
                new()
                {
                    Id = "24",
                    UserId = userId,
                    TargetAmount = 10000.00m,
                    CurrentAmount = 2000.00m,
                    Deadline = new DateTime(2023, 09, 20),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 28),
                    Title = "New Laptop"
                },
                new()
                {
                    Id = "25",
                    UserId = userId,
                    TargetAmount = 1500.00m,
                    CurrentAmount = 100.00m,
                    Deadline = new DateTime(2023, 08, 31),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 29),
                    Title = "Emergency Fund"
                },
                new()
                {
                    Id = "26",
                    UserId = userId,
                    TargetAmount = 2500.00m,
                    CurrentAmount = 1000.00m,
                    Deadline = new DateTime(2023, 12, 15),
                    Status = "Completed",
                    Date = new DateTime(2023, 08, 30),
                    Title = "Home Renovation"
                },
                new()
                {
                    Id = "27",
                    UserId = userId,
                    TargetAmount = 500.00m,
                    CurrentAmount = 50.00m,
                    Deadline = new DateTime(2023, 10, 30),
                    Status = "In Progress",
                    Date = new DateTime(2023, 08, 31),
                    Title = "Gifts for Family"
                },
                new()
                {
                    Id = "28",
                    UserId = userId,
                    TargetAmount = 1200.00m,
                    CurrentAmount = 800.00m,
                    Deadline = new DateTime(2023, 08, 15),
                    Status = "In Progress",
                    Date = new DateTime(2023, 09, 01),
                    Title = "Car Repairs"
                },
                new()
                {
                    Id = "29",
                    UserId = userId,
                    TargetAmount = 3000.00m,
                    CurrentAmount = 3000.00m,
                    Deadline = new DateTime(2023, 11, 30),
                    Status = "In Progress",
                    Date = new DateTime(2023, 09, 02),
                    Title = "Education Fund"
                },
                new()
                {
                    Id = "30",
                    UserId = userId,
                    TargetAmount = 10000.00m,
                    CurrentAmount = 6000.00m,
                    Deadline = new DateTime(2023, 12, 31),
                    Status = "In Progress",
                    Date = new DateTime(2023, 09, 03),
                    Title = "Dream Vacation"
                },
                new()
                {
                    Id = "31",
                    UserId = userId,
                    TargetAmount = 4000.00m,
                    CurrentAmount = 2000.00m,
                    Deadline = new DateTime(2023, 11, 15),
                    Status = "In Progress",
                    Date = new DateTime(2023, 09, 04),
                    Title = "Home Theater System"
                },
                new()
                {
                    Id = "32",
                    UserId = userId,
                    TargetAmount = 6000.00m,
                    CurrentAmount = 2000.00m,
                    Deadline = new DateTime(2023, 10, 10),
                    Status = "In Progress",
                    Date = new DateTime(2023, 09, 05),
                    Title = "Gym Membership"
                },
                new()
                {
                    Id = "33",
                    UserId = userId,
                    TargetAmount = 2000.00m,
                    CurrentAmount = 1000.00m,
                    Deadline = new DateTime(2023, 09, 10),
                    Status = "In Progress",
                    Date = new DateTime(2023, 09, 06),
                    Title = "Emergency Fund"
                },
                new()
                {
                    Id = "34",
                    UserId = userId,
                    TargetAmount = 3000.00m,
                    CurrentAmount = 1500.00m,
                    Deadline = new DateTime(2023, 12, 31),
                    Status = "In Progress",
                    Date = new DateTime(2023, 09, 07),
                    Title = "Vacation Fund"
                },
                new()
                {
                    Id = "35",
                    UserId = userId,
                    TargetAmount = 1800.00m,
                    CurrentAmount = 500.00m,
                    Deadline = new DateTime(2023, 10, 31),
                    Status = "In Progress",
                    Date = new DateTime(2023, 09, 08),
                    Title = "New Laptop"
                },
                new()
                {
                    Id = "36",
                    UserId = userId,
                    TargetAmount = 4000.00m,
                    CurrentAmount = 3500.00m,
                    Deadline = new DateTime(2023, 09, 20),
                    Status = "In Progress",
                    Date = new DateTime(2023, 09, 09),
                    Title = "Emergency Fund"
                },
                new()
                {
                    Id = "37",
                    UserId = userId,
                    TargetAmount = 800.00m,
                    CurrentAmount = 200.00m,
                    Deadline = new DateTime(2023, 08, 31),
                    Status = "In Progress",
                    Date = new DateTime(2023, 09, 10),
                    Title = "Home Renovation"
                }
            };

            foreach (var saving in savingsToAdd)
            {
                saving.Deadline = new DateTime(saving.Deadline.Year, saving.Deadline.Month, saving.Deadline.Day, 0, 0, 0, DateTimeKind.Utc);
                saving.Date = new DateTime(saving.Date.Year, saving.Date.Month, saving.Date.Day, 0, 0, 0, DateTimeKind.Utc);
            }

            await dbContext.Savings.AddRangeAsync(savingsToAdd);
            await dbContext.SaveChangesAsync();
        }
    }
}