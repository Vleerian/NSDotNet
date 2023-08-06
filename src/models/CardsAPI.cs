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
    [XmlRoot("CARDS")]
    public struct CardsAPI
    {
        [XmlArray("DECK")]
        [XmlArrayItem("CARD", typeof(DeckItem))]
        public DeckItem[] Deck { get; init; }
        [XmlElement("INFO")]
        public DeckInfo Deck_Info { get; init; }
    }

    [Serializable()]
    public class DeckItem
    {
        [XmlElement("CARDID")]
        public int ID { get; init; }
        [XmlElement("CATEGORY")]
        public string Rarity { get; init; }
        [XmlElement("SEASON")]
        public int Season { get; init; }
    }

    [Serializable()]
    public class DeckInfo
    {
        [XmlElement("BANK")]
        public float Bank { get; init; }
        [XmlElement("DECK_CAPACITY_RAW")]
        public int DeckCapacity { get; init; }
        [XmlElement("DECK_VALUE")]
        public float DeckValue { get; init; }
        [XmlElement("ID")]
        public int ID { get; init; }
        [XmlElement("LAST_PACK_OPENED")]
        public int Last_Pack_Opened { get; init; }
        [XmlElement("LAST_VALUED")]
        public int Last_Valued { get; init; }
        [XmlElement("NAME")]
        public string Name { get; init; }
        [XmlElement("NUM_CARDS")]
        public int Num_Cards { get; init; }
        [XmlElement("RANK")]
        public int Rank { get; init; }
        [XmlElement("REGION_RANK")]
        public int Region_Rank { get; init; }
    }
}
