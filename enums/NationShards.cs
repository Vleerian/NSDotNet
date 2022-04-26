#region License
/*
Nationstates DotNet Core Library
Copyright (C) 2022 Vleerian R

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
    public class NationShards : Shards
    {
        private NationShards(string Shard) : base(Shard, "?nation=") {}

        public static NationShards ADMIRABLE = new("admirable");
        public static NationShards ADMIRABLES = new("admirables");
        public static NationShards ANIMAL = new("animal");
        public static NationShards ANIMALTRAIT = new("animaltrait");
        public static NationShards ANSWERED = new("answered");
        public static NationShards BANNER = new("banner");
        public static NationShards BANNERS = new("banners");
        public static NationShards CAPITAL = new("capital");
        public static NationShards CATEGORY = new("category");
        public static NationShards CENSUS = new("census");
        public static NationShards CRIME = new("crime");
        public static NationShards CURRENCY = new("currency");
        public static NationShards CUSTOMLEADER = new("customleader");
        public static NationShards CUSTOMCAPITAL = new("customcapital");
        public static NationShards CUSTOMRELIGION = new("customreligion");
        public static NationShards DBID = new("dbid");
        public static NationShards DEATHS = new("deaths");
        public static NationShards DEMONYM = new("demonym");
        public static NationShards DEMONYM2 = new("demonym2");
        public static NationShards DEMONYM2PLURAL = new("demonym2plural");
        public static NationShards DISPATCHES = new("dispatches");
        public static NationShards DISPATCHLIST = new("dispatchlist");
        public static NationShards ENDORSEMENTS = new("endorsements");
        public static NationShards FACTBOOKS = new("factbooks");
        public static NationShards FACTBOOKLIST = new("factbooklist");
        public static NationShards FIRSTLOGIN = new("firstlogin");
        public static NationShards FLAG = new("flag");
        public static NationShards FOUNDED = new("founded");
        public static NationShards FOUNDEDTIME = new("foundedtime");
        public static NationShards FREEDOM = new("freedom");
        public static NationShards FULLNAME = new("fullname");
        public static NationShards GAVOTE = new("gavote");
        public static NationShards GDP = new("gdp");
        public static NationShards GOVT = new("govt");
        public static NationShards GOVTDESC = new("govtdesc");
        public static NationShards GOVTPRIORITY = new("govtpriority");
        public static NationShards HAPPENINGS = new("happenings");
        public static NationShards INCOME = new("income");
        public static NationShards INDUSTRYDESC = new("industrydesc");
        public static NationShards INFLUENCE = new("influence");
        public static NationShards LASTACTIVITY = new("lastactivity");
        public static NationShards LASTLOGIN = new("lastlogin");
        public static NationShards LEADER = new("leader");
        public static NationShards LEGISLATION = new("legislation");
        public static NationShards MAJORINDUSTRY = new("majorindustry");
        public static NationShards MOTTO = new("motto");
        public static NationShards NAME = new("name");
        public static NationShards NOTABLE = new("notable");
        public static NationShards NOTABLES = new("notables");
        public static NationShards POLICIES = new("policies");
        public static NationShards POOREST = new("poorest");
        public static NationShards POPULATION = new("population");
        public static NationShards PUBLICSECTOR = new("publicsector");
        public static NationShards RCENSUS = new("rcensus");
        public static NationShards REGION = new("region");
        public static NationShards RELIGION = new("religion");
        public static NationShards RICHEST = new("richest");
        public static NationShards SCVOTE = new("scvote");
        public static NationShards SECTORS = new("sectors");
        public static NationShards SENSIBILITIES = new("sensibilities");
        public static NationShards TAX = new("tax");
        public static NationShards TGCANRECRUIT = new("tgcanrecruit");
        public static NationShards TGCANCAMPAIGN = new("tgcancampaign");
        public static NationShards TYPE = new("type");
        public static NationShards WA = new("wa");
        public static NationShards WABADGES = new("wabadges");
        public static NationShards WCENSUS = new("wcensus");
        public static NationShards ZOMBIE = new("zombie");
    }
}