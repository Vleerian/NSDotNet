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
    /// <summary>
    /// NSAPI is the primary API-Facing class of NSDotNet, and contains
    /// a rate-limiting mechanism based on the NSAPI's headers. It implments
    /// the singleton design pattern to avoid multiple instances of it being
    /// created in a program.
    /// </summary>
    public class NSAPI
    {
        private NSAPI()
        {}

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

        /// <summary>A semaphore lock to prevent simultaneous requsts.</summary>
        /// <remarks>Technically speaking, simultaneous requests aren't a problem with
        /// the API, however being too cautious is the name of the game. While an API
        /// overrun isn't the end of the world, it's still an unfavorable outcome.</remarks>
        private readonly SemaphoreSlim Lock = new SemaphoreSlim(1, 1);

        /// <summary>The current API status, as reported by the most recent request</summary>
        public APIStatus? Status { get; private set; }

        /// <summary>The timestamp for the last request made</summary>
        private DateTime LastRequest = DateTime.Now;

        /// <summary>The max number of requests as reported by the NS API</summary>
        /// <seealso cref="NSAPI.status">status</seealso>
        public int limit {
            get => Status!.Limit;
        }

        /// <summary>The max number of requests NSDotNet will allow - this is 90% of the maximum the API itself allows</summary>
        public int Limit {
            get => (int)Math.Floor(Status!.Limit * 0.9);
        }

        /// <summary>The number of remaining requests as reported by the NS API</summary>
        /// <seealso cref="NSAPI.status">status</seealso>
        private int remaining {
            get => Status!.Remaining;
        }
        
        /// <summary>The max number of remaining requests NSDotNet will allow</summary>
        public int Remaining {
            get {
                int limit_difference = limit - Limit;
                int adjusted_remaining = remaining - limit_difference;
                return adjusted_remaining > 0 ? adjusted_remaining : 0;
            }
        }

        /// <summary>A boolean that determines if NSDotNet can make a request, and is used internally by Make_Request</summary>
        /// <remarks>
        /// By default, NSDotNet will allow a single request to be made if status has not been set, as there
        /// is no way to get the status of the client's NSAPI window status otherwise.
        /// </remarks>
        /// <seealso cref="status"/>
        public bool Can_Request {
            get {
                if(Status == null)
                    return true;

                int Time_Between = (int)(DateTime.Now - LastRequest).TotalSeconds;
                if(Time_Between > Status!.Window)
                    return true;

                return Remaining > 0;
            }
        }

        /// <summary>This method waits until the current window (as defined by the http headers) has reset</summary>
        /// <seealso cref="NSAPI.status"/>
        public async Task Wait_For_Reset() =>
            await Task.Delay(Status!.Reset * 1000);

        /// <summary>This method waits until API access has been unlocked</summary>
        /// <seealso cref="NSAPI.status"/>
        public async Task Wait_For_Unlock() =>
            await Task.Delay(Status!.RetryAfter * 1000);

        private readonly HttpClient client = new();

        private string? _userAgent;
        /// <summary>
        /// The UserAgent NSDotNet uses to make requests. The UserAgent <b>MUST</b> be set
        /// prior <b>ANY</b> requests being made to adhere the the NationStates API rules.
        /// </summary>
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

        private NSAuth? _auth;
        /// <summary>
        /// The Auth parameters that NSDotNet will use when making requests. This <b>MUST</b>
        /// be set for private shards to function correctly.
        /// </summary>
        public NSAuth? Auth
        {
            get => _auth ?? null;
            set {
                if(_auth != null)
                    client.DefaultRequestHeaders.Remove(_auth.HeaderKey);
                _auth = value;
                client.DefaultRequestHeaders.Add(_auth!.HeaderKey, _auth.Value);
            }
        }

        /// <summary>
        /// This methods wraps MakeRequest and will attempt to deserialize the return to the specified type
        /// <param name="Address" type="string">The URI to request from</param>
        /// <param name="Data" type="T">The data to be returned from the API</param>
        /// <returns>The HttpResponseMessage from the host</returns>
        /// <seealso cref="NSAPI.MakeRequest">status</seealso>
        /// </summary>
        public async Task<(HttpResponseMessage Response, T Data)> GetAPI<T>(string Address) 
        {
            var Req = await MakeRequest(Address, CancellationToken.None);
            T Data = Helpers.BetterDeserialize<T>(await Req!.Content.ReadAsStringAsync());
            return (Req, Data);
        }

        /// <summary>
        /// Makes a request to the speicifed URI - note that requests may be delayed by
        /// up to 30 seconds depending on if the rate-limit ceiling has been hit
        /// <param name="Address" type="URI">The URI to request from</param>
        /// <returns>The HttpResponseMessage from the host</returns>
        /// </summary>
        public async Task<HttpResponseMessage?> MakeRequest(string Address) =>
            await MakeRequest(Address, CancellationToken.None);

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
            
            if(!Can_Request)
                await Wait_For_Reset();
            // Make the request, and update the status variable
            LastRequest = DateTime.Now;
            
            await Lock.WaitAsync();
            HttpResponseMessage Req;
            try{
                Req = await client.GetAsync(Address, Cancellation);
            }
            catch(Exception e)
            {
                Lock.Release();
                throw e;
            }
            Lock.Release();

            Status = new APIStatus(Req);

            // If for whatever reason the request was subject to an API lockout, wait for that to go away
            await Wait_For_Unlock();
            
            return Req;
        }

        /// <summary>
        /// Fetches nation data from the API
        /// </summary>
        /// <remarks>
        /// This method deos not return the HttpResponseMessage from the request to the API
        /// For this reason, it's recommended that some level of validation is done to ensure
        /// that the nation being request axtually exists.
        /// </remarks>
        /// <param name="NationName">Which nation to fetch data for</param>
        /// <returns>The nation's data from the API</returns>
        public async Task<NationAPI> GetNation(string NationName) =>
            (await GetAPI<NationAPI>($"https://www.nationstates.net/cgi-bin/api.cgi?nation={NationName}")).Data;

        /// <summary>
        /// Fetches region data from the API
        /// </summary>
        /// <remarks>
        /// This method deos not return the HttpResponseMessage from the request to the API
        /// For this reason, it's recommended that some level of validation is done to ensure
        /// that the region being request axtually exists.
        /// </remarks>
        /// <param name="RegionName">Which region to fetch data for</param>
        /// <returns>The region's data from the API</returns>
        public async Task<RegionAPI> GetRegion(string RegionName) =>
            (await GetAPI<RegionAPI>($"https://www.nationstates.net/cgi-bin/api.cgi?region={RegionName}")).Data;

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