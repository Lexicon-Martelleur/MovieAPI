#!/bin/bash

export ASPNETCORE_ENVIRONMENT=Development

main () {
	set -ex
	update_database
}

update_database () {
	echo "Update database to latest migration..."
	dotnet.exe ef database update \
		--project MovieCardAPI.Infrastructure/ \
		--startup-project MovieCardAPI/ \
	    --configuration Development \
		--context MovieContext
}

main
