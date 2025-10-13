using InventorAi_api.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace InventorAi_api.Data
{
    public partial class InventorAiContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
         //   modelBuilder.Entity<Store>()
         //.HasOne(s => s.License)
         //.WithOne(l => l.Store)
         //.HasForeignKey<License>(l => l.StoreId)
         //.OnDelete(DeleteBehavior.Cascade);
        }
    }
}
