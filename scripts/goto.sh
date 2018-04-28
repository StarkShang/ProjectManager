#!/bin/bash

exe="/usr/local/self/goto/ProjectFinder.dll"

DEST=$(dotnet "$exe" $@)

[ $? == 0 ] && cd "$DEST"
[ $? == 1 ] && echo "$DEST"