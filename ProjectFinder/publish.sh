out="output"
bin="/usr/local/Self/Goto"
[ -d "$out" ] && rm -rf "$out"
dotnet publish --output $out --runtime osx-x64 --configuration Release --self-contained
# mv -v "$out/Goto" "$out/GetProjectPath"
cp -vf ./goto.sh "$out/goto.sh"

# publish
[ -d "$bin" ] && sudo rm -rf "$bin"
sudo cp -af "$out" "$bin"