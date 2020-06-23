using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MonDKP.Entities;
using MonDKP.Lib;

namespace MonDKP.Web.Data
{
    public class MonDkpDatabaseService
    {
        private DateTime lastWriteTimeUtc = DateTime.MinValue;
        private MonDkpDatabase cachedData;
        private const String DatabaseFileName = "MonolithDKP.lua";

        public async Task<MonDkpDatabase> GetMonDkpDatabaseAsync()
        {
            var writeTimeUtc = File.GetLastWriteTimeUtc(DatabaseFileName);
            if (writeTimeUtc > lastWriteTimeUtc)
            {
                var data = await MonDkpFileLoader.LoadMonDkpDatabaseAsync(DatabaseFileName);
                this.cachedData = data;
                this.lastWriteTimeUtc = writeTimeUtc;
            }

            return this.cachedData;
        }

        public async Task<List<DkpEntry>> GetDkpListAsync()
        {
            var dataBase = await GetMonDkpDatabaseAsync();

            return dataBase.DkpTable.DkpEntries;
        }

        public async Task<List<LootEntry>> GetLootListAsync()
        {
            var dataBase = await GetMonDkpDatabaseAsync();

            //await UpdateItemQuality(dataBase.LootHistory.LootEntries);

            return dataBase.LootHistory.LootEntries;
        }

        //private async Task UpdateItemQuality(List<LootEntry> lootHistoryLootEntries)
        //{
        //    foreach (var lootEntry in lootHistoryLootEntries)
        //    {
        //        if (!qualityCache.ContainsKey(lootEntry.ItemNumber))
        //        {
        //            var url = $"https://classic.wowhead.com/item={lootEntry.ItemNumber}&xml";

        //            using (var client = new HttpClient())
        //            {
        //                var result = await client.GetAsync(url);

        //                var xml = await result.Content.ReadAsStringAsync();

        //                var wowHeadItem = XmlSerializerHelper.Deserialize<WowHead>(xml);

        //                qualityCache[lootEntry.ItemNumber] = wowHeadItem.Item.Quality.Id;
        //            }
        //        }

        //        lootEntry.ItemQuality = $"q{qualityCache[lootEntry.ItemNumber]}";
        //    }
        //}
    }
}