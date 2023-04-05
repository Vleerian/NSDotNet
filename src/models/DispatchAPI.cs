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
    [Serializable()]
    public struct DispatchAPI
    {
        [XmlAttribute("id")]
        public int DispatchID { get; init; }
        [XmlElement("TITLE")]
        public string Title { get; init; }
        [XmlElement("AUTHOR")]
        public string Author { get; init; }
        [XmlElement("CATEGORY")]
        public string Category { get; init; }
        [XmlElement("CREATED")]
        public long Created { get; init; }
        [XmlElement("EDITED")]
        public long Edited { get; init; }
        [XmlElement("VIEWS")]
        public int views { get; init; }
        [XmlElement("SCORE")]
        public int score { get; init; }
        [XmlElement("TEXT")]
        public string Text { get; init; }
    }
}