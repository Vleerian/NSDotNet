using System.Xml.Serialization;

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

namespace NSDotnet
{
    public static class Helpers
    {
        /// <summary>
        /// Sanitizes region and nation names
        /// </summary>
        /// <param name="name">The nation/region name to sanitize</param>
        /// <returns>The trimmed and lowercase nation name with spaces converted to underscores</returns>
        public static string SanitizeName(string name) => name.Trim().ToLower().Replace(' ', '_');
        
        /// <summary>
        /// This method makes deserializing XML less painful
        /// <param name="response">The HttpResponseMessage deserialize out of</param>
        /// <returns>The parsed return from the request.</returns>
        /// </summary>
        public static async Task<T> BetterDeserialize<T>(HttpResponseMessage response) =>
            (T)new XmlSerializer(typeof(T))!.Deserialize(new StringReader(await response.Content.ReadAsStringAsync()))!;

        /// <summary>
        /// This method makes deserializing XML less painful
        /// <param name="url">The URL to request from</param>
        /// <returns>The parsed return from the request.</returns>
        /// </summary>
        public static T BetterDeserialize<T>(string XML) =>
            (T)new XmlSerializer(typeof(T))!.Deserialize(new StringReader(XML))!;
    }
}