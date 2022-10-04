const express = require('express');
const app = express();
const server = require('http').createServer(app);
require('dotenv').config();
const bodyParser = require("body-parser");
const digitalSignageRoutes = require("./routes/digitalSignageRoutes");
var io = require('socket.io')(server, {
    cors: { origin: "*" }
}); 

io.on('connection', function (client) {
    console.log('SocketIO: New client connected...' + client.id);
    io.on('disconnect', () => {
        console.log('SocketIO: Disconnected...' + client.id);
    });
});

app.use(function(req, res, next){
    req.io = io;
    next();
});

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));

app.use("/api", digitalSignageRoutes);

server.listen(process.env.SERVER_LISTEN_PORT, () => console.log(`Listening on port ${process.env.SERVER_LISTEN_PORT}`));