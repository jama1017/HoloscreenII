#include "mainwindow.h"
#include <QApplication>
#include <opencv2/opencv.hpp>
#include <stdio.h>
#include <time.h>

using namespace std;
using namespace cv;


int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    time_t startTime = time(0);
    int frames = 0;

    cout << "Opening capture" << endl;
    cv::VideoCapture cap("udpsrc port=5004 ! application/x-rtp,media=video,payload=26 ! rtph264depay ! avdec_h264  ! videoconvert ! appsink sync=false",cv::CAP_GSTREAMER);
    if (!cap.isOpened()) {
      cout << "Error opening capture" << endl;
    }

    Mat edges;
    namedWindow("edgeEffect", 1);
    while(true) {
      Mat frame;
      cap >> frame;
      frames++;
      cvtColor(frame, edges, CV_BGR2GRAY);
      GaussianBlur(edges, edges, Size(7,7), 1.5, 1.5);
      Canny(edges, edges, 0, 30, 3);
      imshow("edgeEffect", edges);
      if(waitKey(30) >= 0) break;
    }

    cout << "Closing capture" << endl;
    time_t totalTime = time(0) - startTime;
    printf("Captured %.i frames in %ld seconds\n", frames, totalTime);
    return 0;
    return a.exec();
}
