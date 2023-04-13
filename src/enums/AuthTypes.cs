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
    /// NationShards provides all the available nation shards on the NationStates API
    /// </summary>
    public class AuthType
    {
        readonly public string HeaderKey;
        private AuthType(string HeaderKey) => this.HeaderKey = HeaderKey;

        public static readonly AuthType Pin = new AuthType("x-pin");
        public static readonly AuthType Password = new AuthType("x-password");
        public static readonly AuthType Autologin = new AuthType("x-autologin");

        public static implicit operator AuthType(string ls) {
            switch(ls.ToLower())
            {
                case "x-pin":
                    return Pin;
                case "x-password":
                    return Password;
                case "x-autologin":
                    return Autologin;
                default:
                    throw new InvalidCastException("Invalid AuthType.");
            }
        }

        public override string ToString() => HeaderKey;
    }
}