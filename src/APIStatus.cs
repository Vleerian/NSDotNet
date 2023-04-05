using System.IO.Compression;
using System.Linq;
using System.Text;

using NSDotnet.Enums;
using NSDotnet.Models;

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

namespace NSDotnet
{
    public class APIStatus
    {
        ///<summary>The raw policy string returned by the NSAPI</summary>
        public readonly string RateLimit_Policy;
        ///<summary>The window size of the API</summary>
        public readonly int Window;
        ///<summary>The raw request limit returned by the NSAPI</summary>
        public readonly int Limit;
        ///<summary>The raw number of requests seen, as returned by the API</summary>
        public readonly int RequestsSeen;
        ///<summary>The raw remaining requests, as returned by the API</summary>
        public readonly int Remaining;
        ///<summary>The raw number of sectonds until the window resets, as returned by the API</summary>
        public readonly int Reset;
        ///<summary>Once blocked from the API, wait this long before trying again</summary>
        public readonly int RetryAfter;

        public APIStatus(HttpResponseMessage r)
        {
            RateLimit_Policy = "";
            Limit = 0;
            Remaining = 0;
            Reset = 0;
            if(r.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                RetryAfter = Int32.Parse(r.Headers.GetValues("retry-after").First());
            }
            RateLimit_Policy = r.Headers.GetValues("ratelimit-policy").First();
            Limit = Int32.Parse(r.Headers.GetValues("ratelimit-limit").First());
            Remaining = Int32.Parse(r.Headers.GetValues("ratelimit-remaining").First());
            Reset = Int32.Parse(r.Headers.GetValues("ratelimit-reset").First());
            Window = Int32.Parse(RateLimit_Policy.Split(";w=")[1]);
            RequestsSeen = Int32.Parse(r.Headers.GetValues("x-ratelimit-requests-seen").First());
        }
    }
}