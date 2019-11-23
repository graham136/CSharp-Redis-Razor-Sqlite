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
using Microsoft.Extensions.Configuration;

namespace CSharp_Redis_Razor_Sqlite.Views.Home
{
    public class KeysModel : PageModel
    {
        //public IEnumerable<Entry> sqlLiteEntries;
        //public IEnumerable<Entry> redisEntries;

        private IDataRepository<Entry> sqlLiteDataRepository;
        private IDataRepository<Entry> redisDataRepository;

        public IList<Entry> sqlLiteEntries { get; set; }
        public IList<Entry> redisEntries { get; set; }

        [BindProperty]
        public Entry Entry { get; set; }

        public KeysModel()
        {
            sqlLiteDataRepository = new SqlLiteRepository();
            redisDataRepository = new RedisRepository();
            sqlLiteEntries = sqlLiteDataRepository.GetAllKeys().ToList();
            redisEntries = sqlLiteDataRepository.GetAllKeys().ToList();
            redisEntries = redisDataRepository.GetAllKeys().ToList();
            if(!redisEntries.Any())
            {
                foreach (var entry in sqlLiteEntries)
                {
                    redisDataRepository.Add(entry);
                }

                redisEntries = redisDataRepository.GetAllKeys().ToList();
            }                      
        }
        
        public void OnGet()
        {
            sqlLiteDataRepository = new SqlLiteRepository();
            redisDataRepository = new RedisRepository();
            sqlLiteEntries = sqlLiteDataRepository.GetAllKeys().ToList();
            redisEntries = redisDataRepository.GetAllKeys().ToList();           
        }

        protected void btnAddEntry(object sender, EventArgs e)
        {
            sqlLiteDataRepository.Add(Entry);
            sqlLiteDataRepository.SaveAllKeys();
            sqlLiteEntries = sqlLiteDataRepository.GetAllKeys().ToList();
            redisEntries = sqlLiteDataRepository.GetAllKeys().ToList();
        }

        protected void btnRefreshFromSQL_Click(object sender, EventArgs e)
        {            
            foreach(var entry in sqlLiteEntries)
            {
                redisDataRepository.Add(entry);
            }

            redisEntries = redisDataRepository.GetAllKeys().ToList();
        }
    }
}
