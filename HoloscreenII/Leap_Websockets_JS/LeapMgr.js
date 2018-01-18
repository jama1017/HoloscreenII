// Import packages
var Leap = require('leapjs');
Leap.Controller.optimizeHMD = true;
var WebSocket = require('ws');
var fs = require('fs')
var gWebsocketConnect;
/*  ijf it is playback mode or realtime mode */
var isPlayBack = false ;
/*  Last Frame from leap */
var latestFrame = null;
/*  Last Frame from leap */
var handstring = "";
/*  file is ready */
var fileLoadReady = false;
// Start server loop
var wss = new WebSocket.Server({ port: 9999 });

/* replace all prototype function */
String.prototype.replaceAll = function(search, replacement) {
    var target = this;
    return target.replace(new RegExp(search, 'g'), replacement);
};

wss.on('connection', function connection(ws) {
    //ws.send(frame.fingers[0].joint_position[3]);
    console.log("websocket on at 9999");
    gWebsocketConnect  = ws;
    //ws.send("no hand!")
    ws.on('message', function incoming(message) {
         console.log('received: %s', message);
    });

});

/*  Playback mode from txt file */
if(isPlayBack){

    var datalist;
    var playIndex = 0;

    fs.readFile('/Users/zhuzhu/Desktop/HoloscreenTestServer/leaptest.txt', 'utf8', function(err, data) {
      if (err) throw err;
      console.log('OK:');
      datalist = data.split("hand_type:");
      console.log("File load Complete " + datalist.length);
      fileLoadReady = true;

    });

    /*  check if websocket is connect */
    var sync = setInterval(function(){
        if(gWebsocketConnect && fileLoadReady){
            gWebsocketConnect.send("hand_type:"+ datalist[playIndex].replaceAll("\r","").replaceAll("\n",""));
            console.log("hand_type:"+ datalist[playIndex]);
            playIndex ++;
        }
    }, 30);
}

var nd;
/*  Realtime mode from Leap */
if(!isPlayBack){
    console.log("Server setup");
    // Start leap motion loop


    Leap.loop({optimizeHMD: true},function(frame) {
        latestFrame = frame;
        handstring = "";
        if (frame.hands.length>0){
            for (var hand_i=0; hand_i<2; hand_i++){
                if (hand_i+1 > frame.hands.length)
                    break;

                if (hand_i>0)
                    handstring += "#OneMore#";
                var hand = frame.hands[hand_i];
                handstring += "hand_type: " + hand.type + "; ";
                handstring += "palm_pos: " + swapYZ(hand.palmPosition).toString() + "; ";
                handstring += "palm_vel: " + hand.palmVelocity.toString() + "; ";
                handstring += "palm_norm: " + swapYZ(hand.palmNormal).toString() + "; ";
                handstring += "palm_dir: " + swapYZ(hand.direction).toString() + "; ";
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
                        handstring += "finger_" + bone_i.toString() + "_pos: " + swapYZ(finger.bones[bone_i].center()).toString() + "; ";
                        handstring += "finger_" + bone_i.toString() + "_dir: " + swapXY(finger.bones[bone_i].direction()).toString() + "; ";
                    }
                }
            }
            if(gWebsocketConnect){
               // console.log(handstring);
                gWebsocketConnect.send(handstring);
            }
        }
        //  else{
        //     if(gWebsocketConnect){
        //         //console.log(handstring);
        //         gWebsocketConnect.send("");
        //     }
        // }
    });
}

function swapYZ(t){
    t[0] = -t[0];
    var tmp = t[2];
    t[2] = -t[1];
    t[1] = -tmp;
    return t;
}

function swapXY(t){
    t[0] = t[0];
    var tmp = t[2];
    t[2] = -t[1];
    t[1] = tmp;
    return t;
}
/*
hand_type: right; palm_pos: -282.88,275.819,65.8471; palm_vel: 273.779,-39.5252,-190.13; palm_norm: 0.128246,-0.173755,0.976403; finger_type: 0; finger_1_pos: -234.32200622558594,278.5409851074219,78.2833023071289; finger_1_dir: -0.473246,0.611059,0.634543; finger_2_pos: -256.15899658203125,301.8965148925781,105.95950317382812; finger_2_dir: -0.573665,0.461518,0.67669; finger_3_pos: -274.42950439453125,311.90899658203125,125.95899963378906; finger_3_dir: -0.699365,0.168903,0.694522; finger_type: 1; finger_1_pos: -303.676513671875,315.7349853515625,87.6762466430664; finger_1_dir: -0.582984,0.252181,0.772356; finger_2_pos: -316.10400390625,319.4519958496094,116.7074966430664; finger_2_dir: 0.0252027,-0.145861,0.988984; finger_3_pos: -312.1440124511719,314.4884948730469,136.08349609375; finger_3_dir: 0.420097,-0.364872,0.830895; finger_type: 2; finger_1_pos: -317.13250732421875,300.21600341796875,85.17320251464844; finger_1_dir: -0.574919,0.338002,0.745133; finger_2_pos: -321.4620056152344,303.9114990234375,113.06849670410156; finger_2_dir: 0.675126,-0.317377,0.66594; finger_3_pos: -303.41351318359375,294.9119873046875,120.9000015258789; finger_3_dir: 0.866983,-0.461426,-0.18822; finger_type: 3; finger_1_pos: -322.98748779296875,275.71649169921875,81.00589752197266; finger_1_dir: -0.605429,0.248995,0.755948; finger_2_pos: -326.4154968261719,278.6474914550781,107.36720275878906; finger_2_dir: 0.733135,-0.193543,0.651961;
finger_3_pos: -307.3584899902344,272.9259948730469,114.28800201416016; finger_3_dir: 0.918628,-0.315202,-0.238264; finger_type: 4;
finger_1_pos: -321.8345031738281,251.62249755859375,79.68094635009766; finger_1_dir: -0.542687,0.161514,0.82426; finger_2_pos: -323.260986328125,254.92250061035156,99.92085266113281; finger_2_dir: 0.837686,0.0397519,0.544703; finger_3_pos: -306.8605041503906,254.343505859375,102.05425262451172; finger_3_dir: 0.920458,-0.111148,-0.374704;
 */