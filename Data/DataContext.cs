using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using webapi_dotnet6.Entity;

namespace webapi_dotnet6.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> opts) : base(opts) { }

    public DbSet<Course> Courses { get; set; }
}
