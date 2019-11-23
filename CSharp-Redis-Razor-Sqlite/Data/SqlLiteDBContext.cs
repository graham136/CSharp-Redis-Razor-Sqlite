using CSharp_Redis_Razor_Sqlite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp_Redis_Razor_Sqlite.Data
{
    public class SqlLiteDBContext: DbContext
    {
        public DbSet<Entry> Entries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=RedisBackup.db");
    }
}
