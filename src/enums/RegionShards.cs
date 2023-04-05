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
    /// RegionShards provides all the available region shards on the NationStates API
    /// </summary>
    public class RegionShards : Shards
    {
        private RegionShards(string Shard) : base(Shard, "?region=") {}

        public static RegionShards CENSUS = new("census");
        public static RegionShards CENSUSRANKS = new("censusranks");
        public static RegionShards DBID = new("dbid");
        public static RegionShards DELEGATE = new("delegate");
        public static RegionShards DELEGATEAUTH = new("delegateauth");
        public static RegionShards DELEGATEVOTES = new("delegatevotes");
        public static RegionShards DISPATCHES = new("dispatches");
        public static RegionShards EMBASSIES = new("embassies");
        public static RegionShards EMBASSYRMB = new("embassyrmb");
        public static RegionShards FACTBOOK = new("factbook");
        public static RegionShards FLAG = new("flag");
        public static RegionShards FOUNDED = new("founded");
        public static RegionShards FOUNDEDTIME = new("foundedtime");
        public static RegionShards FOUNDER = new("founder");
        public static RegionShards FOUNDERAUTH = new("founderauth");
        public static RegionShards GAVOTE = new("gavote");
        public static RegionShards HAPPENINGS = new("happenings");
        public static RegionShards HISTORY = new("history");
        public static RegionShards LASTUPDATE = new("lastupdate");
        public static RegionShards MESSAGES = new("messages");
        public static RegionShards NAME = new("name");
        public static RegionShards NATIONS = new("nations");
        public static RegionShards NUMNATIONS = new("numnations");
        public static RegionShards OFFICERS = new("officers");
        public static RegionShards POLL = new("poll");
        public static RegionShards POWER = new("power");
        public static RegionShards SCVOTE = new("scvote");
        public static RegionShards TAGS = new("tags");
        public static RegionShards WABADGES = new("wabadges");
        public static RegionShards ZOMBIE = new("zombie");
    }
}