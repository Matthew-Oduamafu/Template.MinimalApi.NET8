using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Template.MinimalApi.NET8.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "76b2a881-be6d-4d6b-a108-09646e63938c",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            },
            new IdentityRole
            {
                Id = "a7e6783e-c29e-4489-b582-9dc974865547",
                Name = "Super Administrator",
                NormalizedName = "SUPER ADMINISTRATOR"
            },
            new IdentityRole
            {
                Id = "6916d5e6-debd-4611-a34a-463aef71ae54",
                Name = "User",
                NormalizedName = "USER"
            }
        );
    }
}