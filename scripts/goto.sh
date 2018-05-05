#!/bin/bash

exe="/usr/local/self/goto/ProjectFinder.dll"

DEST=$(dotnet "$exe" $@)
echo $DEST
echo $DEST

ret="$?"

#echo "sb"
# if [[ "$ret" == 0 ]]; then 
# 	echo "right"
# 	echo "$DEST"
# else
#     echo "$DEST"
# fi
