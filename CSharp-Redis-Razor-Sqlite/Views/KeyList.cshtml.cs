using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using CSharp_Redis_Razor_Sqlite.Data;
using CSharp_Redis_Razor_Sqlite.Interfaces;
using CSharp_Redis_Razor_Sqlite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CSharp_Redis_Razor_Sqlite.Views
{
    public class KeyListModel : PageModel
    {
        public IEnumerable<Entry> sqlLiteEntries;
        public IEnumerable<Entry> redisEntries;

        private IDataRepository<Entry> sqlLiteDataRepository;
        private IDataRepository<Entry> redisDataRepository;
        
        public void OnGet()
        {
            sqlLiteDataRepository = new SqlLiteRepository();
            redisDataRepository = new RedisRepository();
            sqlLiteEntries = sqlLiteDataRepository.GetAllKeys();
            redisEntries = redisDataRepository.GetAllKeys();
        }
    }
}
