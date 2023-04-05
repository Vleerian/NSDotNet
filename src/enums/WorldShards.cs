#region License
/*
Nationstates DotNet Core Library
Copyright (C) 2023 Vleerian R

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
#endregion

namespace NSDotnet.Enums
{
    /// <summary>
    /// WorldShards provides all the available world shards on the NationStates API
    /// </summary>
    public class WorldShards : Shards
    {
        private WorldShards(string Shard) : base(Shard, "?q=") {}

        public static WorldShards BANNER = new("banner");
        public static WorldShards CENSUS = new("census");
        public static WorldShards CENSUSID = new("censusid");
        public static WorldShards CENSUSDESC = new("censusdesc");
        public static WorldShards CENSUSNAME = new("censusname");
        public static WorldShards CENSUSRANKS = new("censusranks");
        public static WorldShards CENSUSSCALE = new("censusscale");
        public static WorldShards CENSUSTITLE = new("censustitle");
        public static WorldShards DISPATCH = new("dispatch");
        public static WorldShards DISPATCHLIST = new("dispatchlist");
        public static WorldShards FACTION = new("faction");
        public static WorldShards FACTIONS = new("factions");
        public static WorldShards FEATUREDREGION = new("featuredregion");
        public static WorldShards HAPPENINGS = new("happenings");
        public static WorldShards LASTEVENTID = new("lasteventid");
        public static WorldShards NATIONS = new("nations");
        public static WorldShards NEWNATIONS = new("newnations");
        public static WorldShards NUMNATIONS = new("numnations");
        public static WorldShards NUMREGIONS = new("numregions");
        public static WorldShards POLL = new("poll");
        public static WorldShards REGIONS = new("regions");
        public static WorldShards REGIONSBYTAG = new("regionsbytag");
        public static WorldShards TGQUEUE = new("tgqueue");
    }
}