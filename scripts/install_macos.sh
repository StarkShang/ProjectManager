#!/bin/bash
cd "../ProjectFinder"

out="output" # output directory for dotnet build
target="/usr/local/self/goto" #install directory

[ -d "$out" ] && rm -rf "$out"

# build from source code
if [[ "$1" == "--native" ]]; then
	echo "Start native installation..."
    dotnet publish "ProjectFinder.csproj" --output "$out" --runtime osx-x64 --configuration Release --self-contained
    cp -vf "../scripts/goto_native.sh" "#out/goto.sh"
else 
	echo "Start installation..."
    dotnet publish "../ProjectFinder/ProjectFinder.csproj" --output "$out" --configuration Release
    cp -vf "../scripts/goto.sh" "$out/goto.sh"
fi

# install to /usr/local/self/goto directory
[ ! -d "/usr/local/self" ] && sudo mkdir "/usr/local/self"
[ -d "$target" ] && sudo rm -rf "$target"
[ ! -d "$target" ] && sudo mv -vf "$out" "$target"

# clean up
[ -d "$out" ] && rm -rf "$out"