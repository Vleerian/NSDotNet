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
    public struct Officer
    {
        [XmlElement("NATION")]
        public string Nation { get; init; }
        [XmlElement("OFFICE")]
        public string Office { get; init; }
        [XmlElement("AUTHORITY")]
        public string OfficerAuth { get; init; }
        [XmlElement("TIME")]
        public int AssingedTimestamp { get; init; }
        [XmlElement("BY")]
        public string AssignedBy { get; init; }
        [XmlElement("ORDER")]
        public int Order { get; init; }
    }
}