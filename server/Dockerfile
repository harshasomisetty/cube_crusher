FROM node:18

WORKDIR /usr/src/app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm install typescript -g
RUN tsc

EXPOSE 3000

CMD [ "node", "./dist/bin/www.js" ]
