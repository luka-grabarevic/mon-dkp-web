using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MonDKP.Entities;
using NLua;

namespace MonDKP.Lib
{
    public static class MonDkpFileLoader
    {
        private static readonly Regex itemNameRegEx = new Regex("(?<=\\[).+?(?=\\])");
        private static readonly Regex itemNumberRegEx = new Regex("(?<=\\:)[0-9]{5}?(?=\\:)");
        private static readonly Regex itemIdRegEx = new Regex("(?<=Hitem\\:).+?(?=\\|)");

        public static async Task<MonDkpDatabase> LoadMonDkpDatabaseAsync(string filePath)
        {
            using (var lua = new Lua())
            {
                lua.State.Encoding = Encoding.UTF8;

                var str =  await File.ReadAllTextAsync(filePath);
                lua.DoString(str);
                var db = new MonDkpDatabase
                {
                    DkpHistory = new DkpHistory
                    {
                        HistoryEntries = GetDkpHistory(lua).ToList()
                    },
                    LootHistory = new LootHistory
                    {
                        LootEntries = GetLootHistory(lua).ToList()
                    },
                    DkpTable = new DkpTable
                    {
                        DkpEntries = GetDkpEntries(lua).ToList()
                    }
                };

                CleanUpDkpList(db);

                return db;
            }
        }

        private static void CleanUpDkpList(MonDkpDatabase db)
        {
            var validPlayers =
                db.DkpTable.DkpEntries.Select(a => a.Player).Distinct().ToList();
            var invalidPlayers =
                db.DkpHistory.HistoryEntries.SelectMany(a => a.Players).Distinct().Where(a => !validPlayers.Contains(a))
                    .ToList();

            db.LootHistory.LootEntries.RemoveAll(a => !validPlayers.Contains(a.Player));

            foreach (var dkpEntry in db.DkpHistory.HistoryEntries)
            foreach (var invalidPlayer in invalidPlayers)
                dkpEntry.PlayerString = dkpEntry.PlayerString.Replace($"{invalidPlayer},", string.Empty);

            db.DkpHistory.HistoryEntries.RemoveAll(a => string.IsNullOrEmpty(a.PlayerString));
        }

        private static IEnumerable<DkpEntry> GetDkpEntries(Lua lua)
        {
            var dkpTable = lua.GetTable("MonDKP_DKPTable");
            foreach (var dkpEntry in dkpTable.Values.OfType<LuaTable>())
                yield return new DkpEntry()
                {
                    Player = dkpEntry["player"].ToString(),
                    Dkp = int.Parse(dkpEntry["dkp"].ToString()),
                    LifetimeSpent = int.Parse(dkpEntry["lifetime_spent"].ToString()),
                    LifetimeGained = int.Parse(dkpEntry["lifetime_gained"].ToString()),
                    Class = dkpEntry["class"].ToString(),
                };
        }

        private static IEnumerable<LootEntry> GetLootHistory(Lua lua)
        {
            var lootHistory = lua.GetTable("MonDKP_Loot");
            foreach (var lootValue in lootHistory.Values.OfType<LuaTable>())
            {
                var loot = lootValue["loot"].ToString();
                yield return new LootEntry()
                {
                    Player = lootValue["player"].ToString(),
                    Zone = lootValue["zone"].ToString(),
                    Boss = lootValue["boss"].ToString(),
                    DeletedBy = lootValue["deletedby"]?.ToString(),
                    Deletes = lootValue["deletes"]?.ToString(),
                    Cost = int.Parse(lootValue["cost"].ToString()),
                    TimeStamp = long.Parse(lootValue["date"].ToString()),
                    ItemName = itemNameRegEx.Match(loot).Value,
                    ItemNumber = int.Parse(itemNumberRegEx.Match(loot).Value),
                    ItemId = itemIdRegEx.Match(loot).Value
                };
            }
        }

        private static IEnumerable<HistoryEntry> GetDkpHistory(Lua lua)
        {
            var dkpHistory = lua.GetTable("MonDKP_DKPHistory");
            foreach (var historyValue in dkpHistory.Values.OfType<LuaTable>())
                yield return new HistoryEntry()
                {
                    PlayerString = historyValue["players"].ToString(),
                    Dkp = int.Parse(historyValue["dkp"].ToString()),
                    Reason = historyValue["reason"].ToString(),
                    TimeStamp = long.Parse(historyValue["date"].ToString())
                };
        }
    }
}