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
    [Serializable, XmlRoot("CARD")]
    public struct CardAPI
    {
        [XmlElement("ID")]
        public int ID { get; init; }
        [XmlElement("NAME")]
        public string name { get; init; }
        [XmlIgnore]
        public string Name {
            get { return name.ToLower().Replace(" ", "_"); }
        }
        [XmlElement("TYPE")]
        public string Type { get; init; }
        [XmlElement("REGION")]
        public string Region { get; init; }
        [XmlElement("FLAG")]
        public string Flag { get; init; }

        /// <remarks>
        /// It should be noted that when parsing the cards data dump, CATEGORY is the nation's Category, not it's rarity
        /// When requested from the API, Category will be the card's rarity.
        /// </remarks>
        [XmlElement("CATEGORY")]
        public string Category { get; init; }

        /// <remarks>
        /// It should be noted that when parsing the cards data dump, CATEGORY is the nation's Category, not it's rarity
        /// When requested from the API, Category will be the card's rarity.
        /// </remarks>
        [XmlElement("CARDCATEGORY")]
        public string Rarity { get; init; }
        [XmlElement("MARKET_VALUE")]
        public float MarketValue { get; init; }
        [XmlElement("DESCRIPTION")]
        public string Description { get; init; }
        [XmlArray("BADGE")]
        [XmlArrayItem("BADGE", typeof(string))]
        public string[] Badges { get; init; }

        [XmlArray("TROPHIES")]
        [XmlArrayItem("TROPHY", typeof(Trophy))]
        public Trophy[] Trophies { get; init; }

        [XmlArray("OWNERS")]
        [XmlArrayItem("OWNER", typeof(string))]
        public string[] Owners { get; init; }

        [XmlArray("MARKETS")]
        [XmlArrayItem("MARKET", typeof(CardListing))]
        public CardListing[] Markets { get; init; }
    }

    [Serializable()]
    public class Trophy
    {
        [XmlAttribute("type")]
        public string TrophyType { get; init; }

        [XmlText]
        public string Score { get; init; }
    }

    [Serializable()]
    public class CardListing
    {
        [XmlElement("NATION")]
        public string nation { get; init; }
        [XmlElement("PRICE")]
        public float price { get; init; }
        [XmlElement("TIMESTAMP")]
        public long timestamp { get; init; }
        [XmlElement("TYPE")]
        public string type { get; init; }
    }
}
