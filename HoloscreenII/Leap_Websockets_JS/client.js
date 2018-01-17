const WebSocket = require('ws');

const ws = new WebSocket('ws://10.0.0.54:9999');

ws.on('open', function open() {
  ws.send("haha!");
});