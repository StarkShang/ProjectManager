cd "../ProjectFinder"

$out = "output" # output directory for dotnet build
$target = "~/self/goto" #install directory

if (Test-Path "$out")
{
	rm -rf "$out"
}

# build from source code
if ($1 -eq "--native")
{
    echo "Native installing ..."
    dotnet publish "ProjectFinder.csproj" --output "$out" --runtime osx-x64 --configuration Release --self-contained
    cp -vf "../scripts/goto.ps" "#out/goto.sh"
}
else
{
    echo "Installing"
    dotnet publish "../ProjectFinder/ProjectFinder.csproj" --output "$out" --configuration Release
    cp -vf "../scripts/goto_native.ps1" "$out/goto.ps1"
}

# install to /usr/local/self/goto directory
if (Test-Path $"target")
{
	rm -rf "$target"
}

if (-not Test-Path "$target")
{
	mv -vf "$out" "$target"
}

# clean up
if (Test-Path "$out")
{
	rm -rf "$out"
}

cd "../scripts"