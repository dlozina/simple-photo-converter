using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PhotoConverterWebAppv2.Models
{
    public class appDBContext : DbContext
    {
        // Think of DbContext as Database
        // Think of DbSet as a table in Database
        public DbSet<ConvertedPhoto> ConvertedPho { get; set; }
    }
}