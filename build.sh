#!/usr/bin/env bash
set -euox pipefail

cd "$(dirname "${BASH_SOURCE[0]}")"

export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

dotnet tool restore

release_build=false
cake_args=()
for arg in "$@"; do
  case "$arg" in
    --release)
      release_build=true
      ;;
    *)
      cake_args+=("$arg")
      ;;
  esac
done

dotnet cake "${cake_args[@]}"

if [[ "$release_build" == true ]]; then
  dotnet build ./samples/AndroidSample/AndroidSample.csproj -c Debug -f net10.0-android
  dotnet build ./samples/AndroidSample/AndroidSample.csproj -c Release -f net10.0-android
fi
