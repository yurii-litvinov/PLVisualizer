FROM node:19.2-alpine as build

WORKDIR /src
COPY package-lock.json .
COPY package.json .
RUN npm install --silent
COPY . .
CMD ["npm", "start"]
