using InventorAi_api.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace InventorAi_api.Data
{
    public partial class InventorAiContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            // Ignore scaffolded collection to avoid conflict
            modelBuilder.Entity<User>()
                .Ignore(u => u.Licenses);
            // One-to-one User ↔ License
            modelBuilder.Entity<User>()
                .HasOne(u => u.License)
                .WithOne(l => l.User)
                .HasForeignKey<License>(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add more custom relationships or overrides here
        }
    }
}
