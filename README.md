# OwlCore.Nomad [![Version](https://img.shields.io/nuget/v/OwlCore.Nomad.svg)](https://www.nuget.org/packages/OwlCore.Nomad)

A lightweight event stream handler framework.

Nomad enables distributing a modifiable application domain across a set of nodes with eventual consistency.

### Implementations

Several prototype implementations were created to test several scenarios we expected to serve.

The publicly available implementations are:

- A generic [base](https://github.com/Arlodotexe/OwlCore.Nomad) toolset for interacting with event stream handlers and entries.
- A toolset for using [Nomad with Kubo](https://github.com/Arlodotexe/OwlCore.Nomad.Kubo).
- A [base](https://github.com/Arlodotexe/OwlCore.Nomad.Storage) and [Kubo](https://github.com/Arlodotexe/OwlCore.Nomad.Storage.Kubo) implementation of [OwlCore.Storage](https://github.com/Arlodotexe/OwlCore.Storage).
- A [PeerSwarm](https://github.com/Arlodotexe/OwlCore.Nomad.Kubo.PeerSwarm/) manager for Kubo, useful for piggybacking off of public peer routing for private content routing.
- The [WindowsAppCommunity.Sdk](https://github.com/WindowsAppCommunity/WindowsAppCommunity.Sdk), roaming data for projects, publishers and users.

## Install

Published releases are available on [NuGet](https://www.nuget.org/packages/OwlCore.Nomad). To install, run the following command in the [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console).

    PM> Install-Package OwlCore.Nomad
    
Or using [dotnet](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet)

    > dotnet add package OwlCore.Nomad

## Usage

```cs
var test = new Thing();
```

## Financing

We accept donations [here](https://github.com/sponsors/Arlodotexe) and [here](https://www.patreon.com/arlodotexe), and we do not have any active bug bounties.

## Versioning

Version numbering follows the Semantic versioning approach. However, if the major version is `0`, the code is considered alpha and breaking changes may occur as a minor update.

## License

All OwlCore code is licensed under the MIT License. OwlCore is licensed under the MIT License. See the [LICENSE](./src/LICENSE.txt) file for more details.
