using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp_Redis_Razor_Sqlite.Models
{
    public class Entry
    {
        public int EntryId { get; set; }
        [Required]     
        public string EntryKey { get; set; }
        [Required]        
        public string EntryValue { get; set; }
    }
}
