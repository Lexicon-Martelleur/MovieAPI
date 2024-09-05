#!/bin/bash

export ASPNETCORE_ENVIRONMENT=Development

main () {
	set -ex
	remove_latest_migration
}

remove_latest_migration () {
	echo "Removing latest migration..."
	dotnet.exe ef migrations remove \
		--project MovieCardAPI.Infrastructure/ \
		--startup-project MovieCardAPI/ \
	    --configuration Development \
		--context MovieContext
}

main
