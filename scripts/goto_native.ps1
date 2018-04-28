$exe = "/usr/local/self/goto/ProjectFinder.dll"

$DEST = & "$exe" $args

if ($lastExitCode -eq 0) {
    cd "$DEST"
}
else {
    echo "$DEST"
}