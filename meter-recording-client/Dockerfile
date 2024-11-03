#Build the React app
FROM node:16-alpine as build
WORKDIR /app
ARG REACT_APP_API_URL
ENV REACT_APP_API_URL=$REACT_APP_API_URL
COPY . .
RUN npm install
RUN npm run build

#Serve the app using a web server
FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
