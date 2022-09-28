using Apcitas.WebService.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Apcitas.WebService.Data;

public class Seed
{
    public static async Task SeedUsers(DataContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var usersDat = await System.IO.File.ReadAllTextAsync("Data/UsersSeedData.js");
        var users = System.Text.Json.JsonSerializer.Deserialize<List<AppUser>>(usersDat);
        foreach(var user in users)
        {
            using var hamc = new HMACSHA512();
            user.UserName = user.UserName.ToLower();
            user.PasswordHash = hamc.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            user.PasswordSalt = hamc.Key;

            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }
}
