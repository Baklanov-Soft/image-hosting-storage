using CommunityToolkit.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ImageHosting.Persistence.DbContexts;

public class ImageHostingContextFactory : IDesignTimeDbContextFactory<ImageHostingDbContext>
{
    public ImageHostingDbContext CreateDbContext(string[] args)
    {
        Guard.HasSizeEqualTo(args, 1);

        var optionsBuilder = new DbContextOptionsBuilder<ImageHostingDbContext>();
        optionsBuilder.UseNpgsql(args[0]);

        return new ImageHostingDbContext(optionsBuilder.Options);
    }
}
