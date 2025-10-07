# --- Build stage ---
FROM node:20-alpine AS build
WORKDIR /app

# Copy dependency files first
COPY package*.json ./

# Install dependencies (including chart libs)
RUN npm ci --legacy-peer-deps && \
    npm install --legacy-peer-deps \
      chart.js chartjs-plugin-zoom chartjs-adapter-date-fns date-fns


# Copy only what’s needed
COPY angular.json tsconfig*.json ./
COPY src ./src

# Build Angular app in production mode
RUN npm run build -- --configuration production

# --- Runtime stage ---
FROM nginx:alpine

# Install gettext for runtime env substitution
RUN apk add --no-cache gettext

# Copy built Angular app
COPY --from=build /app/dist/edelmetall-frontend/browser /usr/share/nginx/html

# Copy config template and entrypoint
COPY src/assets/config.json.template /usr/share/nginx/html/assets/config.json.template
COPY docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh

# Expose HTTP port
EXPOSE 80

# Start Nginx
ENTRYPOINT ["/docker-entrypoint.sh"]
CMD ["nginx", "-g", "daemon off;"]
