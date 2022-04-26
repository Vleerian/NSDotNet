# FattKATT
![GitHub Workflow Status (branch)](https://img.shields.io/github/workflow/status/vleerian/fattkatt/multiplatPublish/main?style=for-the-badge) ![GitHub](https://img.shields.io/github/license/vleerian/fattkatt?style=for-the-badge) ![GitHub issues](https://img.shields.io/github/issues/vleerian/fattkatt?style=for-the-badge)

<p align="center">
	<img src="/assets/logo.png"><br>
  A fancy C# port of Khron and Atagait's Trigger Tool
</p>

## About

FattKATT is a simple script that takes a list of NationStates regions, sorts them by update order, and informs the user when
the game API reports they have updated.

## Usage

**DO NOT RUN TWO INSTANCES OF FATTKATT AT THE SAME TIME.**

FattKATT requires a list of regions to trigger on in `trigger_list.txt` - if this file does exist, the program will create it and prompt you to fill it out. Each trigger should be on it's own line.

FattKATT first will prompt you for your main nation - this is used exclusively to identify the current user of the script to NS' admin.

It will then ask how often it should request data from the NationStates API - it will not allow values beneath 600ms as that is the maximum speed permitted by the rate limit (One request every 0.6 seconds)

**750ms is the default value (shown in green) and is recommended by the developer. While FattKATT will permit 600ms, it is not recommended, as that can run over the NS API rate limit and result in a 15 minute timeout.**

## Running FattKATT

I suggest running [The latest release](https://github.com/Vleerian/FattKATT/releases/latest)

If you want to run directly from source, you will need the Dotnet 6.0 SDK in order to build the script.

You can build it by running
`dotnet build -C Release`

## Acknowledgments

The following people provided key contributions during the initial development process:

* The original version of KATT was programmed by [Khronion](https://github.com/Khronion)

The following people also helped review and test KATT:

* Koth and Aav tested the multiplatform builds on Linux and Mac (respectively)
* Khron provided bug reports, as well as reviewing the code for NS legality issues

## Disclaimer

The software is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

Any individual using a script-based NationStates API tool is responsible for ensuring it complies with the latest
version of the [API Terms of Use](https://www.nationstates.net/pages/api.html#terms). KATT is designed to comply with
these rules, including the rate limit, as of April 21, 2019, under reasonable use conditions, but the authors are not
responsible for any unintended or erroneous program behavior that breaks these rules.

Never run more than one program that uses the NationStates API at once. Doing so will likely cause your IP address to
exceed the API rate limit, which will in turn cause both programs to fail.
