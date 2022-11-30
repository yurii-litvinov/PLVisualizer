FROM node:19.2-alpine as build

WORKDIR /frontend
COPY package-lock.json ./
COPY package.json ./
RUN npm ci --silent
RUN npm install --silent
COPY . .
EXPOSE 3000
CMD ["npm", "start"]
