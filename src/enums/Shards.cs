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
    /// Shards is the base class for API sharding, and can be used as a function to build requests
    /// </summary>
    public class Shards
    {
        public string Shard { get; init; }

        public string APIQuery { get; init; }

        protected Shards(string Shard, string Query)
        {
            this.Shard = Shard;
            APIQuery = Query;
        }

        public override string ToString() => Shard;

        public static implicit operator string(Shards shard) => shard.Shard;

        public override bool Equals(object? o) => base.Equals(o);

        public override int GetHashCode() =>
            base.GetHashCode();

        public static bool operator ==(Shards t, Shards o) =>
            t.Shard == o.Shard;

        public static bool operator !=(Shards t, Shards o) =>
            t.Shard != o.Shard;
    }
}