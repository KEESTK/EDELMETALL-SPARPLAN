#!/bin/sh
set -e

# Default API_URL if not set (frontend will call /api, Nginx will proxy it)
: "${API_URL:=/api}"

# Generate runtime config from template
if [ -f /usr/share/nginx/html/assets/config.json.template ]; then
  envsubst < /usr/share/nginx/html/assets/config.json.template > /usr/share/nginx/html/assets/config.json
  echo "[entrypoint] Wrote /assets/config.json with API_URL=$API_URL"
fi

# Start Nginx
exec "$@"
