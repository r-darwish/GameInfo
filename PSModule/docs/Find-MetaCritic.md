---
external help file: PSModule.dll-Help.xml
Module Name: GameInfo
online version:
schema: 2.0.0
---

# Find-MetaCritic

## SYNOPSIS
Find game information in MetaCritic

## SYNTAX

```
Find-MetaCritic -Title <String[]> [-Platform <Platform>] [-Throttle <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Find game information in MetaCritic

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-MetaCritic Bayonetta
```

## PARAMETERS

### -Platform
Game platform to look

```yaml
Type: Platform
Parameter Sets: (All)
Aliases:
Accepted values: All, PS3, Xbox360, PC, DS, PS2, PSP, Wii, IOS, PS, GBA, Xbox, GameCube, N64, Dreamcast, N3DS, PsVita, WiiU, PS4, XboxOne, PS5, Switch

Required: False
Position: Named
Default value: All
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

### -Title
The name of the game

```yaml
Type: String[]
Parameter Sets: (All)
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

### System.String[]

## OUTPUTS

### MetaCritic+FindResult

## NOTES

## RELATED LINKS
