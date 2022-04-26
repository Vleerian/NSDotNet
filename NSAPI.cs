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
        // The "seconds" component in "Requests per second"
        const int Time_Span = 30;

        // The amount of time between recruitment and non-recruitment TGs respectively
        const int Recruitment_Span = 180;
        const int Non_RecruitNormal_Span = 30;

        private DateTime NextTG = DateTime.Now + new TimeSpan(0,0,Recruitment_Span);

        private readonly HttpClient client = new();

        private readonly LeakyBucket stampLog = new(NSDotnet_Max_Requests, Time_Span);

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
        /// Makes a request to the NationStates URI as outlined by the request builder provided
        /// </summary>
        public async Task<HttpResponseMessage?> MakeRequest<T>(RequestBuilder<T> requestBuilder) where T : Shards
            => await MakeRequest(requestBuilder.BuildRequest());

        /// <summary>
        /// Sends a telegram using the NationStates API
        /// IMPORTANT: IF YOU ARE SENDING A RECRUITMENT TELEGRAM, ENSURE `bool Recruitment` IS SET TO TRUE
        /// NSDotNet cannot know activity that has taken place BEFORE it's runtime, and assumes a recruitment TG was sent
        /// prior to it's startup.
        /// <param name="Recruitment" type="bool">Whether or not this is a recruitment telegram</param>
        /// <returns>The HttpResponseMessage from the host</returns>
        /// </summary>
        public async Task<HttpResponseMessage?> SendTelegram(bool Recruitment, string ClientKey, string SecretKey,string TGID, string Recipient)
        {
            while(DateTime.Now < NextTG) await Task.Delay(500);
            NextTG = DateTime.Now + new TimeSpan(0,0,(Recruitment?Recruitment_Span:Non_RecruitNormal_Span));
            return await MakeRequest($"https://www.nationstates.net/cgi-bin/api.cgi?a=sendTG&client={ClientKey}&key={SecretKey}&tgid={TGID}&to={Recipient}");
        }

        /// <summary>
        /// Makes a request to the speicifed URI - note that requests may be delayed by
        /// up to 30 seconds depending on if the rate-limit ceiling has been hit
        /// <param name="Address" type="URI">The URI to request from</param>
        /// <returns>The HttpResponseMessage from the host</returns>
        /// </summary>
        private async Task<HttpResponseMessage?> MakeRequest(string Address)
        {
            if(_userAgent == null || _userAgent.Trim() == string.Empty)
                throw new InvalidOperationException("No User-Agent set.");

            // If the request cannot be safely made, wait
            while(!stampLog.CanRequest )
                await Task.Delay(600);

            // Add the current time to the request history
            stampLog.Enqueue();

            // Make the request
            var Req = await client.GetAsync(Address);
            
            // If the site has seen more rate-limited requests than the application, equalize it
            int RateLimitSeen = Helpers.CheckRatelimit(Req);
            while(RateLimitSeen < stampLog.Count) stampLog.Enqueue();

            return Req;
        }

        /// <summary>
        /// Makes a request to the speicifed URI - note that requests may be delayed by
        /// up to 30 seconds depending on if the rate-limit ceiling has been hit
        /// <param name="Address" type="URI">The URI to request from</param>
        /// <param name="Cancellation" type="CancellationToken">A cancellation token for prematurely cancelling the task</param>
        /// <returns>The HttpResponseMessage from the host</returns>
        /// </summary>
        private async Task<HttpResponseMessage?> MakeRequest(string Address, CancellationToken Cancellation)
        {
            if(_userAgent == null || _userAgent.Trim() == string.Empty)
                throw new InvalidOperationException("No User-Agent set.");

            // If the request cannot be safely made, wait
            while(!stampLog.CanRequest && !Cancellation.IsCancellationRequested)
                await Task.Delay(600);
            // If cancellation was request, exit
            if(Cancellation.IsCancellationRequested)
                return null;

            // Add the current time to the request history
            stampLog.Enqueue();

            Console.WriteLine("Requset made.");
            // Make the request
            var Req = await client.GetAsync(Address);
            
            // If the site has seen more rate-limited requests than the application, equalize it
            int RateLimitSeen = Helpers.CheckRatelimit(Req);
            while(RateLimitSeen < stampLog.Count) stampLog.Enqueue();

            return Req;
        }

        /// <summary>
        /// Donwloads the latest data dump and returns the filename
        /// </summary>
        /// <param name="datadumptype">The type of data dump to download</typeparam>
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

        private sealed class LeakyBucket
        {
            // The limit to how many timestamps can be stored
            public int Limit { get; init; }

            // How long a timestamp can sit in the queue for
            public int Fresh_Window { get; init; }

            // The collection of timestamps being stored
            private List<DateTime> RequestTimestamps= new();

            public LeakyBucket(int limit, int fresh_window)
            {
                Limit = limit;
                Fresh_Window = fresh_window;
            }

            // How many timestamps are being stored
            public int Count => RequestTimestamps.Count;

            // If a request can be made
            public bool CanRequest {
                get {
                    CleanTimestamps();

                    if(RequestTimestamps.Count == Limit)
                    {
                        var Difference = DateTime.Now - RequestTimestamps.Last();
                        if(Difference.TotalSeconds > Time_Span)
                            return true;
                        return false;
                    }
                    return true;
                }
            }

            // Clears out stale timestamps
            private void CleanTimestamps() {
                // Clear any timestamps from more than 30s ago
                RequestTimestamps = RequestTimestamps.Where(Stamp=>{
                    var Difference = DateTime.Now - Stamp;
                    if(Difference.TotalSeconds < Fresh_Window)
                        return true;
                    return false;
                }).ToList();
            }

            public void Enqueue(){
                RequestTimestamps.Add(DateTime.Now);
            }
        }

    }
}