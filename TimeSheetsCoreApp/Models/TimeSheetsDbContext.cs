using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TimeSheetsCoreApp.Models
{
    static class DbContextDbName 
    {
        public static string DataSourceDbName = "Data Source=timesheets.db";
    }

    // Design Time Factory
    public class TimeSheetsContextDesignTimeFactory : IDesignTimeDbContextFactory<TimeSheetsDbContext>
    {
        TimeSheetsDbContext IDesignTimeDbContextFactory<TimeSheetsDbContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TimeSheetsDbContext>();
            optionsBuilder.UseSqlite(DbContextDbName.DataSourceDbName);

            return new TimeSheetsDbContext(optionsBuilder.Options);
        }
    }

    public static class TimeSheetsDbContextFactory
    {
        public static TimeSheetsDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TimeSheetsDbContext>();
            optionsBuilder.UseSqlite(DbContextDbName.DataSourceDbName);

            return new TimeSheetsDbContext(optionsBuilder.Options);
        }
    }

    public class TimeSheetsDbContext : DbContext
    {
        public TimeSheetsDbContext(DbContextOptions<TimeSheetsDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LoggedInUser> LoggedInUsers { get; set; }
    }
}
