#!/bin/bash

main()
{
	CWD=$(pwd)

	if [[ "$1" == "" ]]; then
		DEST=$(/usr/local/Self/Goto/Goto "$CWD")
	else
		DEST=$(/usr/local/Self/Goto/Goto "$CWD" "$1")
	fi

	if [[ $? == 0 ]]; then # 正常退出
		cd "$DEST"
	else
		echo "$DEST"
	fi
}

###############################################################################
main "$1"