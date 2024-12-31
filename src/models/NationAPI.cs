using System.Xml.Serialization;

using NSDotnet.Enums;

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

namespace NSDotnet.Models
{
    [Serializable, XmlRoot("NATION")]
    public struct NationAPI
    {
        public string GetCSV() => $"{name},{Endos},{IsWA},{InfluenceLevel},{Influence}";

        [XmlAttribute("id")]
        public string id { get; init; }

        [XmlElement("NAME")]
        public string? name { get; init; }
        [XmlElement("TYPE")]
        public string type { get; init; }

        [XmlElement("FULLNAME")]
        public string fullname { get; init; }
        [XmlElement("CATEGORY")]
        public string? category { get; init; }
        [XmlElement("POPULATION")]
        public int? population;
        [XmlElement("REGION")]
        public string? region { get; init; }

        [XmlElement("ENDORSEMENTS")]
        public string? endorsements { get; init; }

        [XmlIgnore]
        public string[] Endorsements => endorsements?.Split(",") ?? new string[0];

        [XmlIgnore]
        public int Endos => Endorsements.Count();

        [XmlElement("UNSTATUS")]
        public string unstatus { get; init; }

        [XmlElement("INFLUENCE")]
        public string InfluenceLevel { get; init; }
        [XmlElement("INFLUENCENUM")]
        public int Influence { get; init; }
        [XmlElement("TGCANRECRUIT")]
        public int TGCanRecruit { get; init; }
        [XmlElement("TGCANCAMPAIGN")]
        public int TGCanCampaign { get; init; }

        [XmlIgnore]
        public int Residency => (int)CensusData[CensusScore.Residency].CensusScore;

        [XmlIgnore]
        public bool IsWA => unstatus.StartsWith("WA");

        [XmlIgnore]
        public bool EndorsingDel;
        [XmlIgnore]
        public bool EndorsingRO;

        [XmlElement("FLAG")]
        public string flag { get; init; }

        [XmlArray("CENSUS"), XmlArrayItem("SCALE", typeof(CensusAPI))]
        public List<CensusAPI> CensusScores { get; init; }

        [XmlElement("GAVOTE")]
        public string GAVote { get; init; }

        [XmlElement("SCVOTE")]
        public string SCVote { get; init; }

        [XmlElement("PACKS")]
        public int Packs { get; init; }

        [XmlIgnore]
        public Dictionary<CensusScore, CensusAPI> CensusData =>
            CensusScores.ToDictionary(C => C.Census);

        [XmlArray("ISSUES")]
        [XmlArrayItem("ISSUE")]
        public Issue[] Issues { get; init; }
    }

    public struct Issue
    {
        [XmlAttribute("id")]
        public int IssueID { get; init; }
        [XmlElement("TITLE")]
        public string Title { get; init; }
        [XmlElement("TEXT")]
        public string Text { get; init; }
        [XmlElement("AUTHOR")]
        public string Author { get; init; }
        [XmlElement("PIC1")]
        public string Pic1 { get; init; }
        [XmlElement("PIC2")]
        public string Pic2 { get; init; }

        [XmlElement("OPTION")]
        public IssueOption[] Options { get; init; }
    }

    public struct IssueOption
    {
        [XmlText]
        public string OptionText { get; init; }
        [XmlAttribute("id")]
        public int OptionID { get; init; }
    }
}
