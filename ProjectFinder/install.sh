out="output" # output directory for dotnet build
bin="/usr/local/self/goto" #install directory

[ -d "$out" ] && rm -rf "$out"
dotnet publish --output "$out/bin" --runtime osx-x64 --configuration Release --self-contained
# mv -v "$out/Goto" "$out/GetProjectPath"
cp -vf "./SolutionList.json" "$out/SolutionList.json"
cp -vf "./goto.sh" "$out/bin/goto.sh"
ln -s  "$out/bin/goto.sh" "$out/goto"

# publish
[ -d "$bin" ] && sudo rm -rf "$bin"
sudo cp -af "$out" "$bin"