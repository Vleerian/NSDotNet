using System.Xml.Serialization;

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
    [Serializable(), XmlRoot("REGION")]
    public struct RegionAPI
    {
        //These are values parsed from the data dump
        [XmlElement("NAME")]
        public string Name { get; init; }
        
        public string name => Helpers.SanitizeName(Name);

        [XmlElement("NUMNATIONS")]
        public int NumNations { get; init; }
        [XmlElement("NATIONS")]
        public string nations { get; init; }
        [XmlElement("DELEGATE")]
        public string Delegate { get; init; }
        [XmlElement("DELEGATEVOTES")]
        public int DelegateVotes { get; init; }
        [XmlElement("DELEGATEAUTH")]
        public string DelegateAuth { get; init; }
        [XmlElement("FOUNDER")]
        public string Founder { get; init; }
        [XmlElement("FOUNDERAUTH")]
        public string FounderAuth { get; init; }
        [XmlElement("FACTBOOK")]
        public string Factbook { get; init; }
        [XmlArray("OFFICERS"), XmlArrayItem("OFFICER", typeof(Officer))]
        public Officer[] Officers { get; init; }
        [XmlArray("EMBASSIES"), XmlArrayItem("EMBASSY", typeof(string))]
        public string[] Embassies { get; init; }
        [XmlElement("LASTUPDATE")]
        public double? LastUpdate { get; init; }

        //These are values added after the fact by ARCore
        public string[] Nations
        {
            get {
                return nations
                    .Replace(' ','_')
                    .ToLower()
                    .Split(":", StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public long Index;
        public bool hasPassword;
        public bool hasFounder;
        public string FirstNation;
    }
}