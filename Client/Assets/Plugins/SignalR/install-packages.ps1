#$WORK_DIR=`mktemp -d`
#echo "Created temp dir ${WORK_DIR}"

.\nuget.exe install Microsoft.AspNetCore.SignalR.Client -OutputDirectory ./1 -Framework 'netstandard2.0'
.\nuget.exe install Microsoft.AspNetCore.SignalR.Protocols.NewtonsoftJson -OutputDirectory .\1 -Framework 'netstandard2.0'

Get-ChildItem -Path .\1 -Recurse -Filter 'netstandard2.0' -Directory | ForEach-Object { Get-ChildItem -Path $_.FullName -Filter '*.dll' -File -Recurse | Copy-Item -Destination . -Force -Verbose }
