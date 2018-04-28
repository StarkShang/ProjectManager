cd "..\ProjectFinder"

$out = "output" # output directory for dotnet build
$target = "$Env:userprofile\self\goto" #install directory

if (Test-Path "$out") {
	Remove-Item -Path "$out" -Force -Recurse
}

# build from source code
if ($1 -eq "--native") {
    echo "Native installing ..."
    dotnet publish "ProjectFinder.csproj" --output "$out" --runtime osx-x64 --configuration Release --self-contained
    Copy-Item -Path "..\scripts\goto_native.ps1" -Destination "#out\goto.ps1" -Force -Recurse
}
else {
    echo "Installing"
    dotnet publish "..\ProjectFinder\ProjectFinder.csproj" --output "$out" --configuration Release
    Copy-Item -Path "..\scripts\goto.ps1" -Destination "$out\goto.ps1" -Force -Recurse
}

# install to ~\self\goto directory
if (Test-Path "$target") {
    Remove-Item -Path "$target" -Force -Recurse
}

if (-not (Test-Path "$Env:userprofile\self")) {
    mkdir "$Env:userprofile\self"
}

if (-not (Test-Path "$target")) {
	Move-Item -Path "$out" -Destination "$target" -Force
}

# clean up
if (Test-Path "$out") {
	Remove-Item -Path "$out" -Force -Recurse
}

###############################################################################
## Make alias
###############################################################################
if (-not (Test-Path "$Env:userprofile\Documents\WindowsPowerShell")) {
    mkdir "$Env:userprofile\Documents\WindowsPowerShell"
}
if (-not (Test-Path "$PROFILE")) {
    New-Item -Path "$PROFILE"
}

$isExist = $false
$content = (Get-Content -Path "$PROFILE" | ForEach-Object {
    $isExist = $isExist -or ($_ -Match 'New-Alias goto.*')
    if ($isExist) {
        $_ -replace "New-Alias goto.*", "New-Alias goto `"$Env:userprofile\self\goto\goto.ps1`""
    }
})
if (-not $isExist) {
    Add-Content -Path "$PROFILE" -Value "New-Alias goto `"$Env:userprofile\self\goto\goto.ps1`""
}
else {
    Set-Content -Path ("$PROFILE"+".temp") -Value $content
    Remove-Item "$PROFILE"
    Rename-Item ("$PROFILE"+".temp") "$PROFILE"
}

cd "../scripts"