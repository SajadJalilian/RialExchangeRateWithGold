using Microsoft.EntityFrameworkCore;

namespace DatabaseSaver;

public class AppDbContext : DbContext
{
    public string DbPath { get; }

    public AppDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "goldprice.db");
    }


    public DbSet<GoldDataEntity> GoldDatas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}