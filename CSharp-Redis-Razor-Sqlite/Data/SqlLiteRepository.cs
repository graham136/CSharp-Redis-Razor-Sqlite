using CSharp_Redis_Razor_Sqlite.Interfaces;
using CSharp_Redis_Razor_Sqlite.Models;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_Redis_Razor_Sqlite.Data
{
    public class SqlLiteRepository : IDataRepository<Entry>
    {

        public IEnumerable<Entry> Entries;
        private SqlLiteDBContext _sqlLiteDBContext;        

        public SqlLiteRepository()
        {
            _sqlLiteDBContext = new SqlLiteDBContext();
            _sqlLiteDBContext.Database.EnsureCreated();
            Entries = _sqlLiteDBContext.Entries;
            
            if (!Entries.Any())
            {
                _sqlLiteDBContext.Entries.Add(new Entry { EntryKey = "Default1", EntryValue = "DefaultValue1" });
                _sqlLiteDBContext.SaveChanges();
                Entries = _sqlLiteDBContext.Entries;
            }

        }

        public IEnumerable<Entry> Add(Entry entity)
        {

            if (!_sqlLiteDBContext.Entries.Contains(entity)) { _sqlLiteDBContext.Entries.Add(entity); }
            _sqlLiteDBContext.SaveChanges();

            var result = _sqlLiteDBContext.Entries.First(entry => entry.EntryId == entity.EntryId);
            return (IEnumerable<Entry>)result;
        }

        public IEnumerable<Entry> Delete(Entry entity)
        {
            _sqlLiteDBContext.Entries.Remove(entity);
            _sqlLiteDBContext.SaveChanges();
            var result = _sqlLiteDBContext.Entries.FirstOrDefault(entry => entry.EntryId == entity.EntryId);
            return (IEnumerable<Entry>)result;
        }

        public IEnumerable<Entry> GetAllKeys()
        {
            return _sqlLiteDBContext.Entries;
        }

        public IEnumerable<Entry> GetKey(string value)
        {
            var result = _sqlLiteDBContext.Entries.FirstOrDefault(entry => entry.EntryValue == value);
            return (IEnumerable<Entry>)result;
        }

        public IEnumerable<Entry> GetValue(string key)
        {
            var result = _sqlLiteDBContext.Entries.FirstOrDefault(entry => entry.EntryKey == key);
            return (IEnumerable<Entry>)result;
        }

        public IEnumerable<Entry> SaveAllKeys()
        {
            _sqlLiteDBContext.SaveChanges();
            return _sqlLiteDBContext.Entries;
        }

        public IEnumerable<Entry> Update(Entry oldEntity, Entry newEntity)
        {
            var result = _sqlLiteDBContext.Entries.Update(newEntity);
            return (IEnumerable<Entry>)result;
        }
    }
}
