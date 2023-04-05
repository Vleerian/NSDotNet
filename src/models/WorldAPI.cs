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
    [XmlRoot("WORLD")]
    public struct WorldAPI
    {
        [XmlArray("HAPPENINGS")]
        [XmlArrayItem("EVENT", typeof(HappeningsAPI))]
        public HappeningsAPI[] Happenings { get; init; }

        [XmlElement("REGIONS")]
        public string Regions { get; init; }

        [XmlElement("FEATUREDREGION")]
        public string Featured { get; init; }

        [XmlElement("NATIONS")]
        public string Nations { get; init; }

        [XmlElement("NEWNATIONS")]
        public string NewNations { get; init; }

        [XmlElement("NUMNATIONS")]
        public int NumNations { get; init; }

        [XmlElement("NUMREGIONS")]
        public int NumRegions { get; init; }

        [XmlElement("DISPATCH")]
        public DispatchAPI Dispatch { get; init; }
    }
}