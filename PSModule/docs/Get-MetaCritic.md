---
external help file: PSModule.dll-Help.xml
Module Name: GameInfo
online version:
schema: 2.0.0
---

# Get-MetaCritic

## SYNOPSIS
Get the MetaCritic information page about games

## SYNTAX

### Url (Default)
```
Get-MetaCritic -Uri <Uri[]> [-Throttle <Int32>] [<CommonParameters>]
```

### GameTitle
```
Get-MetaCritic -Title <String[]> -Platform <Platform> [-Throttle <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Get the data from a MetaCritic page. The Uri parameter set expects a URI of a MetaCritic page. 
It is also possible to pipe the result from `Find-MetaCritic``

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-MetaCritic Bayonetta -Platform Switch
```

### Example 2
```powershell
PS C:\> Find-MetaCritic Bayonetta | Get-MetaCritic
```

s

## PARAMETERS

### -Platform
Game platform

```yaml
Type: Platform
Parameter Sets: GameTitle
Aliases:
Accepted values: All, PS3, Xbox360, PC, DS, PS2, PSP, Wii, IOS, PS, GBA, Xbox, GameCube, N64, Dreamcast, N3DS, PsVita, WiiU, PS4, XboxOne, PS5, Switch

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Throttle
Maximum number of parallel requests for MetaCritic

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Uri
A Uri of a valid MetaCritic game data page

```yaml
Type: Uri[]
Parameter Sets: Url
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Title
The name of the game

```yaml
Type: String[]
Parameter Sets: GameTitle
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Uri[]

### System.String[]

## OUTPUTS

### GameData

## NOTES

## RELATED LINKS
