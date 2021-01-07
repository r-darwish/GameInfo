# GameInfo

A Powershell module written in F# for fetching information about games.

### Note
This module is pretty new, so it's incomplete and probably have many bugs. I am not sure how much I would like to keep investing it, so that depends on the public interest. If you find this module useful then feel free to report bugs or suggest new features.

## Installation

```powershell
Install-Module GameInfo
```

## Usage

Find a game in MetaCritic:

```powershell
Find-MetaCritic bayonetta
```

Get information about a specific game:

```powershell
Get-MetaCritic bayonetta -Platform switch
```
