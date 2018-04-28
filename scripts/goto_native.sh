#!/bin/bash

exe="/usr/local/self/goto/ProjectFinder"

DEST=$("$exe" $@)

[ $? == 0 ] && cd "$DEST"
[ $? == 1 ] && echo "$DEST"
