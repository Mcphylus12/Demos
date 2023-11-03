FROM node:17-alpine3.12 AS build
WORKDIR /src
COPY ["FE-CustomerStore/package.json", "."]
RUN echo 
RUN npm install
COPY ["FE-CustomerStore", "."]
RUN npm run build

FROM nginx:1.21.4
COPY --from=build /src/dist /usr/share/nginx/html