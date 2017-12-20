const WebSocket = require('ws');
const wss = new WebSocket.Server({ port: 9999 });

/*
/*
    Websocket implementation
*/
var pmessage = "";
wss.on('connection', function connection(ws) {
    console.log("Android terminal Connected!");
  ws.on('message', function incoming(message) {
     console.log('received: %s', message);
    });
  ws.send("haha!");
});
