---
external help file: PSModule.dll-Help.xml
Module Name: GameInfo
online version:
schema: 2.0.0
---

# Find-HowLongToBeat

## SYNOPSIS
Find information in howlongtobeat.com

## SYNTAX

```
Find-HowLongToBeat [-Title <String[]>] [-Throttle <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Find information in howlongtobeat.com

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-HowLongToBeat Bayonetta
```

## PARAMETERS

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
Game title

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
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

### System.String
## NOTES

## RELATED LINKS
