# NS.NET
NationSates DotNet Core Library

## About

NS.NET is a core library that provides Enums and data Models, as well as an API Interface utilizing Flush-Bucket rate limiting. The goal of NS.NET is twofold
- Making interacting with NationStates' XML API simpler, by providing data models and built-in methods for mapping XML data from both the API and Data Dumps to classes.
- Making abiding by the API rules simple, by providing built-in methods for interacting with the API that handle rate-limiting for the client.

## Quick Start
Prerequisites
- [Net SDK](https://dotnet.microsoft.com/en-us/download)6.0 or higher

As of now, NS.NET is not on NuGet, so to add NS.NET to your project

Clone the [latest rewviewed version](https://github.com/Vleerian/NSDotNet/tree/09936b5c9a4c897e5704433249c7e44735567aeb) into the same directory as your project folder.

In your project directory, add NS.NET as a reference
```
dotnet add reference ../NSDotNet/src/NSDotNet.csproj
```

## Building

You can build NSDotNet DLLs by running
`dotnet build src/NSDotNet.csproj -C Release`

## Building Docs
NSDotNet uses [docfx](https://github.com/dotnet/docfx) to generate documentation.
Documentation can be built with the following commands
`docfx init -o ./ -q`
`docfx docfx.json --serve`

## Example
Please consult the documentation for the rest of the methods,
```csharp
using NSDotnet;
using NSDotnet.Models;

string YourNation = "";
string YourEmail = "";
string UserNation = "";
string UserRegion = "";

var API = NSAPI.Instance;
API.UserAgent = $"TestApplication/1.0 (By {YourNation}, {YourEmail} - In Use by {UserNation})";

var Request = await API.MakeRequest($"https://www.nationstates.net/cgi-bin/api.cgi?nation={UserNation}");
RegionAPI Nation = Helpers.BetterDeserialize<RegionAPI>(await req.Content.ReadAsStringAsync());
var Request = await API.MakeRequest($"https://www.nationstates.net/cgi-bin/api.cgi?region={UserRegion}");
NationAPI Region = Helpers.BetterDeserialize<NationAPI>(await req.Content.ReadAsStringAsync());

Console.WriteLine(Nation.fullname);
Console.WriteLine(Ration.name);
```