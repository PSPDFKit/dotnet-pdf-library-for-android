#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 || $# -gt 2 ]]; then
  echo "Usage: $0 <build-log-path> [report-file-path]" >&2
  exit 2
fi

LOG_FILE="$1"
REPORT_FILE="${2:-${LOG_FILE%/*}/critical-binding-warnings.txt}"

if [[ ! -f "$LOG_FILE" ]]; then
  echo "[binding-warning-check] Build log not found: $LOG_FILE" >&2
  exit 2
fi

# Fatal warning codes.
FATAL_CODES_REGEX='BG8A04|BG8402'

fatal_matches=$(grep -nE "warning[[:space:]]+(${FATAL_CODES_REGEX})" "$LOG_FILE" \
  | awk '{ line=$0; sub(/^[0-9]+:/, "", line); if (!seen[line]++) print $0 }' || true)

if [[ -n "$fatal_matches" ]]; then
  mkdir -p "$(dirname "$REPORT_FILE")"
  {
    echo "Critical binding warning check failed"
    echo "Build log: $LOG_FILE"
    echo
    echo "Fatal warning matches (${FATAL_CODES_REGEX}):"
    echo "$fatal_matches"
    echo
  } > "$REPORT_FILE"

  echo "[binding-warning-check] FAILED. See report: $REPORT_FILE" >&2
  echo "[binding-warning-check] Fatal codes: ${FATAL_CODES_REGEX}" >&2
  exit 1
fi

echo "[binding-warning-check] No fatal binding warnings (${FATAL_CODES_REGEX})." >&2
