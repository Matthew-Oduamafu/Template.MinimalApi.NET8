using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Template.MinimalApi.NET8.Data.Configurations;

public class UserRolesConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(
            new IdentityUserRole<string>
            {
                RoleId = "76b2a881-be6d-4d6b-a108-09646e63938c",
                UserId = "46b4340e-6500-420b-9d9e-f04a516e7f19"
            },
            new IdentityUserRole<string>
            {
                RoleId = "a7e6783e-c29e-4489-b582-9dc974865547",
                UserId = "46b4340e-6500-420b-9d9e-f04a516e7f19"
            },
            new IdentityUserRole<string>
            {
                RoleId = "6916d5e6-debd-4611-a34a-463aef71ae54",
                UserId = "46b4340e-6500-420b-9d9e-f04a516e7f19"
            }
        );
    }
}