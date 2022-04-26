using NSDotnet.Enums;

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
    public class RequestBuilder<T> where T : Shards
    {
        const string BaseURI = "https://www.nationstates.net/cgi-bin/api.cgi";

        private List<T> _shards = new();

        public T[] Shards => _shards.ToArray();

        public RequestBuilder<T> AddShard(T shard)
        {
            if(!_shards.Contains(shard))
                _shards.Add(shard);
            return this;
        }

        public RequestBuilder<T> RemoveShard(T shard)
        {
            if(_shards.Contains(shard))
                _shards.Remove(shard);
            return this;
        }

        public string BuildRequest()
        {
            string Request = "";
            Request += _shards[0].APIQuery;
            Request += string.Join("+", _shards);
            return BaseURI + Request;
        }
    }
}