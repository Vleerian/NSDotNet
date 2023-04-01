# NSDotNet
NationSates DotNet Core Library

## About

NSDotNet is a core library that provides Enums and data Models, as well as an API Interface utilizing Flush-Bucket rate limiting.

## Building

You can build NSDotNet DLLs by running
`dotnet build src/NSDotNet.csproj -C Release`

## Building Docs
NSDotNet uses [docfx](https://github.com/dotnet/docfx) to generate documentation.
Documentation can be built with the following commands
`docfx init -o ./ -q`
`docfx docfx.json --serve`
