dotnet publish
pwsh -Command {
    Import-Module -Force .\bin\Debug\netcoreapp2.1\publish\GameInfo.psd1
    Update-MarkdownHelp .\docs
    New-ExternalHelp .\docs -OutputPath . -Force
}