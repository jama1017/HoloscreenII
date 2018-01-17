const WebSocket = require('ws');

const wss = new WebSocket.Server({ port: 9999 });

wss.on('connection', function connection(ws) {
  ws.on('message', function incoming(message) {
    console.log('received: %s', message);
  });

  ws.on('close', function() {
        console.log('Client '+ ws +'disconnected.');
  });
});

