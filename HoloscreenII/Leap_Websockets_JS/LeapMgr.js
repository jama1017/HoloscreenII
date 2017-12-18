// Import packages
var Leap = require('leapjs');
var WebSocket = require('ws');

// Latest frame from leap motion
var latestFrame = null;

// Start server loop
var wss = new WebSocket.Server({ port: 8080 });

wss.on('connection', function connection(ws) {
    //ws.send(frame.fingers[0].joint_position[3]);
});

console.log("Server setup");

// Start leap motion loop
Leap.loop(function(frame) {
    latestFrame = frame;
    var handstring = "";
    if (frame.hands.length>0){
        for (var hand_i=0; hand_i<2; hand_i++){
            if (hand_i+1 > frame.hands.length)
                break;
            var hand = frame.hands[hand_i];
            handstring += "hand_type: " + hand.type + "; ";
            handstring += "palm_pos: " + hand.palmPosition.toString() + "; ";
            handstring += "palm_vel: " + hand.palmVelocity.toString() + "; ";
            handstring += "palm_norm: " + hand.palmNormal.toString() + "; ";
            for (var i=0; i<5; i++){
                /* 
                0 = THUMB
                1 = INDEX
                2 = MIDDLE
                3 = RING
                4 = PINKY
                */
                var finger = hand.fingers[i];
                handstring += "finger_type: " + finger.type + "; ";
                for (var bone_i=1; bone_i<4; bone_i++){
                    handstring += "finger_" + bone_i.toString() + "_pos: " + finger.bones[bone_i].center().toString() + "; ";
                    handstring += "finger_" + bone_i.toString() + "_dir: " + finger.bones[bone_i].direction().toString() + "; ";
                }
            }
            if (frame.hands.length>1)
                handstring += "#n#";
        }
        console.log(handstring);
    }
});

console.log("Leap setup");