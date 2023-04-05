using System.IO.Compression;
using System.Linq;
using System.Text;

using NSDotnet.Enums;
using NSDotnet.Models;

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
    public class NSAPI
    {
        // Singleton stuff.
        private NSAPI() {}

        private static NSAPI? instance;

        /// <summary>
        /// For NSAPI to properly rate-limit requests it must be the only
        /// Instance in the program. For this reason, it implements the
        /// singleton design pattern, and may only be accessed through
        /// a static member
        /// </summary>
        public static NSAPI Instance
        {
            get {
                if(instance == null)
                    instance = new();
                return instance;
            }
        }

        // The absolute number of requests the API will allow
        const int Max_Requests = 50;
        // NSDotnet undershoots the max API speed
        const int NSDotnet_Max_Requests = 45;

        // The last-seen result of X-RateLimit-Requests-Seen
        public int Requests_Seen = 0;
        private DateTimeOffset Ratelimit_Reference_Date;
        private DateTime Last_Request = DateTime.Now;
        private int Next_Delay = 0;

        private DateTime NextTG = DateTime.Now + new TimeSpan(0,0,Recruitment_Span);

        private readonly HttpClient client = new();
        private string? _userAgent;
        public string UserAgent
        {
            get => _userAgent ?? string.Empty;
            set {
                if(client.DefaultRequestHeaders.Contains("User-Agent"))
                    client.DefaultRequestHeaders.Remove("User-Agent");
                client.DefaultRequestHeaders.Add("User-Agent", value);
                _userAgent = value;
            }
        }

        /// <summary>
        /// This method checks the X-ratelimit-requests-seen header and returns how long the application should wait for it's next request
        /// </summary>
        void Get_Rate_Limit_Delay(HttpResponseMessage r)
        {
            int RatelimitSeen = Int32.Parse(r.Headers.GetValues("X-ratelimit-requests-seen").First());
            DateTimeOffset Request_Date = (DateTimeOffset)r.Headers.Date!;
            if(RatelimitSeen == 1)
                Ratelimit_Reference_Date = Request_Date;
            else if(RatelimitSeen > NSDotnet_Max_Requests)
            {
                TimeSpan Reference_Delta = Request_Date - Ratelimit_Reference_Date;
                int Time_to_Wait = 31 - ((int)Reference_Delta.TotalSeconds);
                if(Time_to_Wait < 0 || Time_to_Wait > 31)
                    Time_to_Wait = 31;
                this.Next_Delay = Time_to_Wait;
            }
            this.Requests_Seen = RatelimitSeen;
        }

        /// <summary>
        /// Makes a request to the speicifed URI - note that requests may be delayed by
        /// up to 30 seconds depending on if the rate-limit ceiling has been hit
        /// <param name="Address" type="URI">The URI to request from</param>
        /// <returns>The HttpResponseMessage from the host</returns>
        /// </summary>
        public async Task<HttpResponseMessage?> MakeRequest(string Address)
        {
            if(_userAgent == null || _userAgent.Trim() == string.Empty)
                throw new InvalidOperationException("No User-Agent set.");

            // If you've waited a minute already, don't wait another minute for no reason
            TimeSpan Delta = DateTime.Now - Last_Request;
            if(Delta.TotalSeconds < Next_Delay)
            {
                await Task.Delay(Next_Delay * 1000);
                Next_Delay = 0;
            }

            // Make the request
            var Req = await client.GetAsync(Address);
            
            // Do rate limit calculations
            Get_Rate_Limit_Delay(Req);
            Last_Request = DateTime.Now;
            return Req;
        }

        /// <summary>
        /// Makes a request to the speicifed URI - note that requests may be delayed by
        /// up to 30 seconds depending on if the rate-limit ceiling has been hit
        /// <param name="Address" type="URI">The URI to request from</param>
        /// <param name="Cancellation" type="CancellationToken">A cancellation token for prematurely cancelling the task</param>
        /// <returns>The HttpResponseMessage from the host</returns>
        /// </summary>
        public async Task<HttpResponseMessage?> MakeRequest(string Address, CancellationToken Cancellation)
        {
            if(_userAgent == null || _userAgent.Trim() == string.Empty)
                throw new InvalidOperationException("No User-Agent set.");

            TimeSpan Delta = DateTime.Now - Last_Request;
            if(Delta.TotalSeconds < Next_Delay)
            {
                await Task.Delay(Next_Delay * 1000);
                Next_Delay = 0;
            }

            // Make the request
            var Req = await client.GetAsync(Address);
            
            // Do rate limit calculations
            Get_Rate_Limit_Delay(Req);
            return Req;
        }

        /// <summary>
        /// Donwloads the latest data dump and returns the filename
        /// </summary>
        /// <param name="datadumptype">The type of data dump to download</param>
        /// <returns>Filename of the downloaded dump</returns>
        public async Task<string> DownloadDataDump(DataDumpType datadumptype)
        {
            // An easy way to select either regions or nations
            string DumpType = datadumptype == DataDumpType.Regions ? "regions" : "nations";
            string DumpFilename = $"{DumpType}.{DateTime.Now.ToString("MM.dd.yyyy")}.xml.gz";

            // If the dump file already exists, we do not download it again - return the filename
            if (File.Exists(DumpFilename))
                return DumpFilename;

            // Fetch the data dump, and open a filestream
            var response = await client.GetAsync($"https://www.nationstates.net/pages/{DumpType}.xml.gz");
            using(var fs = new FileStream(DumpFilename, FileMode.CreateNew))
            {
                // Copy the data from the request into the file
                await response.Content.CopyToAsync(fs);
            }

            // Return the filename
            return DumpFilename;
        }

        /// <summary>
        /// Unzips nation.xml.gz and region.xml.gz files
        /// </summary>
        /// <param name="Filename">the .xml.gz file to unzip</param>
        /// <returns>The uncompressed XML plaintext</returns>
        public static string UnzipDump(string Filename)
        {
            // Open a filestream for the data dump
            using (var fileStream = new FileStream(Filename, FileMode.Open))
            {
                // Open a GZip stream to decompress - this takes the Filestream and decompresses it
                using (var gzStream = new GZipStream(fileStream, CompressionMode.Decompress))
                {
                    // Open an output stream for the resulting plaintext
                    using (var outputStream = new MemoryStream())
                    {
                        // Copy the unzipped contents to the output stream
                        gzStream.CopyTo(outputStream);
                        // Convert  the resulting output stream into a string and return
                        byte[] outputBytes = outputStream.ToArray();
                        return Encoding.Default.GetString(outputBytes);
                    }
                }
            }
        }
    }
}