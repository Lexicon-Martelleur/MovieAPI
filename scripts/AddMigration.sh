#!/bin/bash

export ASPNETCORE_ENVIRONMENT=Development
MIGRATION=$1

main () {
	set -ex
	check_input
	add_migration $MIGRATION
}

check_input () {
	if [ -z "$MIGRATION" ]; then
  		echo "No argument for migration name provided"
		echo "Usage $0 <MIGRATION_NAME>"
  		exit 1
	else
		echo "Building bigration: $1..."
	fi
}

add_migration () {
	dotnet.exe ef migrations add $1 \
		--project MovieCardAPI.DB/ \
		--startup-project MovieCardAPI/ \
	    --configuration Development \
		--context MovieContext
}

main
