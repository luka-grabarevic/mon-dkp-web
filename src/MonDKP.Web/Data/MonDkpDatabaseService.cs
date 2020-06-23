using System;
using System.IO;
using System.Threading.Tasks;
using GridMvc.Server;
using GridShared;
using GridShared.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using MonDKP.Entities;
using MonDKP.Lib;

namespace MonDKP.Web.Data
{
    public class MonDkpDatabaseService
    {
        private const String DatabaseFileName = "MonolithDKP.lua";
        private MonDkpDatabase cachedData;
        private DateTime lastWriteTimeUtc = DateTime.MinValue;

        public async Task<ItemsDTO<DkpEntry>> GetDkpGridRowsAsync(Action<IGridColumnCollection<DkpEntry>> columns,
                                                                  QueryDictionary<StringValues> query)
        {
            var dataBase = await GetMonDkpDatabaseAsync();
            var server = new GridServer<DkpEntry>(dataBase.DkpTable.DkpEntries,
                                                  new QueryCollection(query),
                                                  true,
                                                  "dkpGrid",
                                                  columns)
                         .Sortable(true)
                         .Searchable(true, true, false)
                         .Filterable(true);

            // return items to displays
            return server.ItemsToDisplay;
        }

        public async Task<ItemsDTO<LootEntry>> GetLootGridRowsAsync(Action<IGridColumnCollection<LootEntry>> columns,
                                                                    QueryDictionary<StringValues> query)
        {
            var dataBase = await GetMonDkpDatabaseAsync();
            var server = new GridServer<LootEntry>(dataBase.LootHistory.LootEntries,
                                                   new QueryCollection(query),
                                                   true,
                                                   "lootGrid",
                                                   columns)
                         .Sortable(true)
                         .Searchable(true, true, false)
                         .Filterable(true);

            // return items to displays
            return server.ItemsToDisplay;
        }

        private async Task<MonDkpDatabase> GetMonDkpDatabaseAsync()
        {
            var writeTimeUtc = File.GetLastWriteTimeUtc(DatabaseFileName);
            if (writeTimeUtc > this.lastWriteTimeUtc)
            {
                var data = await MonDkpFileLoader.LoadMonDkpDatabaseAsync(DatabaseFileName);
                this.cachedData = data;
                this.lastWriteTimeUtc = writeTimeUtc;
            }

            return this.cachedData;
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