using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenCvSharp;
using OpenCvSharp.Extensions;

using OpenCvSharp.Aruco;

namespace GameServer
{
  public partial class Form1 : Form
  {

    VideoCapture capture;
    Mat frameOrig;
    Mat frame;
    Mat frameBong;
    Bitmap image;
    Thread cameraThread;
    bool isCameraRunning = false;

    Dictionary dictionary = CvAruco.GetPredefinedDictionary(PredefinedDictionaryName.Dict6X6_250);
    DetectorParameters parameters = DetectorParameters.Create();

    // calibration
    Mat cameraMatrix = new Mat(new OpenCvSharp.Size(3, 3), MatType.CV_32F, 1);
    Mat distCoeffs = new Mat(1, 8, MatType.CV_32F, 1);

    private void CaptureCamera()
    {
        cameraThread = new Thread(new ThreadStart(CaptureCameraCallback));
        cameraThread.Start();
    }

    private void CaptureCameraCallback()
    {

        capture = new VideoCapture(0);
        capture.Open(0);
        capture.FrameWidth = 1920;
        capture.FrameHeight = 1080;

        frameOrig = new Mat();
        frame = new Mat();
        frameBong = new Mat();  

        if (capture.IsOpened())
        {
            while (isCameraRunning)
            {

                // read frame from camera
                capture.Read(frameOrig);
                // scale down frame
                Cv2.Resize(frameOrig, frame, new OpenCvSharp.Size(640, 360));
                
                frameBong = frameOrig.EmptyClone();


                // detect markers
                //std::vector<int> ids;
                //std::vector<std::vector<cv::Point2f> > corners;
                //cv::aruco::detectMarkers(image, dictionary, corners, ids);
                //using (var undistorted = new Mat())
                //Cv2.Undistort(image, undistorted, camera, distortion, newCamera);
                var undistorted = new Mat();
                CvAruco.DetectMarkers(frame, dictionary, out Point2f[][] corners, out int[] ids, parameters, out Point2f[][] rejected);
                if (ids.Any())
                {
                  // detect markers
                  CvAruco.DrawDetectedMarkers(frame, corners, ids);

                  // detect markers pose
                  using (var rvecs = new Mat())
                  using (var tvecs = new Mat())
                  {
                      CvAruco.EstimatePoseSingleMarkers(corners, 0.065f, cameraMatrix, distCoeffs, rvecs, tvecs);
                      for (var i = 0; i < ids.Length; i++)
                      {
                          var rvec = rvecs.Get<Vec3d>(i);
                          var tvec = tvecs.Get<Vec3d>(i);

                          // draw markers pose
                          CvAruco.DrawAxis(frame, cameraMatrix, distCoeffs, rvec, tvec, 0.05f);
                          CvAruco.DrawAxis(frameBong, cameraMatrix, distCoeffs, rvec, tvec, 0.05f);
                      }
                  }


                }


                // draw frame on picture
                image = BitmapConverter.ToBitmap(frame);
                if (pictureBoxCam.Image != null)
                {
                    pictureBoxCam.Image.Dispose();
                }
                pictureBoxCam.Image = image;


                // draw frame on picture
                image = BitmapConverter.ToBitmap(frameBong);
                if (pictureBoxGame.Image != null)
                {
                    pictureBoxGame.Image.Dispose();
                }
                pictureBoxGame.Image = image;

            }
        }
    }


    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }


    // When the user clicks on the start/stop button, start or release the camera and setup flags
    private void button1_Click(object sender, EventArgs e)
    {
      if (button1.Text.Equals("Start"))
      {
        CaptureCamera();
        button1.Text = "Stop";
        isCameraRunning = true;
      }
      else
      {
        capture.Release();
        button1.Text = "Start";
        isCameraRunning = false;
      }

      //DetectMarkers(outputImage);
    }

    private void pictureBoxCam_Click(object sender, EventArgs e)
    {

    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      //close threads
      //cameraThread.Abort();
      //capture.Release();

      //exit application
       Environment.Exit(0);
    }

    private void button2_Click(object sender, EventArgs e)
    {
      //form2 = new Form2()
    }

    /*
    private async Task DetectMarkers()
    {
        string output = @"C:\projects\src\OfirNoy\Geekcon2021\GameServer\output.png";
        var rms = 0.0;
        var calib = 100;
        var size = new OpenCvSharp.Size(9, 6);
        var frameSize = OpenCvSharp.Size.Zero;
        var distortion = new Mat();
        var imgPoints = new List<MatOfPoint2f>();
        var objPoints = new List<MatOfPoint3f>();
        var criteria = new TermCriteria(CriteriaType.Eps | CriteriaType.MaxIter, 30, 0.001);
        var objp = MatOfPoint3f.FromArray(Create3DChessboardCorners(size, 0.025f));
        using (var capture = new VideoCapture(0))
        using (var paramters = DetectorParameters.Create())
        using (var camera = new MatOfDouble(Mat.Eye(3, 3, MatType.CV_64FC1)))
        using (var dictionary = CvAruco.GetPredefinedDictionary(PredefinedDictionaryName.Dict4X4_50))
        {
            while (capture.Grab() && calib > 0)
            {
                using (var image = capture.RetrieveMat())
                using (var gray = new Mat())
                {
                    frameSize = image.Size();
                    Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
                    if (Cv2.FindChessboardCorners(gray, size, out Point2f[] corners))
                    {
                        objPoints.Add(objp);
                        imgPoints.Add(MatOfPoint2f.FromArray(corners.ToArray()));
                        var corners2 = Cv2.CornerSubPix(gray, corners, new OpenCvSharp.Size(11, 11), new OpenCvSharp.Size(-1, -1), criteria);
                        Cv2.DrawChessboardCorners(image, size, corners2, true);
                        image.SaveImage(output);
                        calib--;
                        await Task.Delay(100);
                    }
                    image.SaveImage(output);
                }
            }
            rms = Cv2.CalibrateCamera(objPoints, imgPoints, frameSize, camera, distortion, out var rvectors, out var tvectors, CalibrationFlags.UseIntrinsicGuess | CalibrationFlags.FixK5);
            using (var newCamera = Cv2.GetOptimalNewCameraMatrix(camera, distortion, frameSize, 1, frameSize, out var roi))
            {
                await Task.Delay(1);
                while (capture.Grab())
                {
                    using (var undistorted = new Mat())
                    using (var image = capture.RetrieveMat())
                    {
                        Cv2.Undistort(image, undistorted, camera, distortion, newCamera);
                        CvAruco.DetectMarkers(undistorted, dictionary, out Point2f[][] corners, out int[] ids, paramters, out Point2f[][] rejected);
                        if (ids.Any())
                        {
                            CvAruco.DrawDetectedMarkers(undistorted, corners, ids);
                            using (var rvecs = new Mat())
                            using (var tvecs = new Mat())
                            {
                                CvAruco.EstimatePoseSingleMarkers(corners, 0.065f, newCamera, distortion, rvecs, tvecs);
                                for (var i = 0; i < ids.Length; i++)
                                {
                                    var rvec = rvecs.Get<Vec3d>(i);
                                    var tvec = tvecs.Get<Vec3d>(i);
                                    DrawAxis(undistorted, newCamera, distortion, rvec, tvec, 0.05f);
                                }
                            }
                        }
                        undistorted.SaveImage(output);
                    }
                }
            }
        }
    }

    private static IEnumerable<Point3f> Create3DChessboardCorners(OpenCvSharp.Size boardSize, float squareSize)
    {
        for (int y = 0; y < boardSize.Height; y++)
        {
            for (int x = 0; x < boardSize.Width; x++)
            {
                yield return new Point3f(x * squareSize, y * squareSize, 0);
            }
        }
    }
*/

    /*
    private static void DrawAxis(Mat image, InputArray camera, InputArray distortion, Vec3d rvec, Vec3d tvec, float length)
    {
        if (image.Total() == 0 || (image.Channels() != 1 && image.Channels() != 3))
        {
            throw new ArgumentException(nameof(image));
        }
        if (length <= 0)
        {
            throw new ArgumentException(nameof(length));
        }
        // project axis points
        var axisPoints = new MatOfPoint3f()
        {
            new Point3f(0, 0, 0),
            new Point3f(length, 0, 0),
            new Point3f(0, length, 0),
            new Point3f(0, 0, length),
        };
        var imagePoints = new MatOfPoint2f();
        Cv2.ProjectPoints(axisPoints, InputArray.Create(new[] { rvec }), InputArray.Create(new[] { tvec }), camera, distortion, imagePoints);
        // draw axis lines
        //Cv2.Line(image, imagePoints.Get<Point2f>(0), imagePoints.Get<Point2f>(1), new Scalar(0, 0, 255), 3);
        //Cv2.Line(image, imagePoints.Get<Point2f>(0), imagePoints.Get<Point2f>(2), new Scalar(0, 255, 0), 3);
        //Cv2.Line(image, imagePoints.Get<Point2f>(0), imagePoints.Get<Point2f>(3), new Scalar(255, 0, 0), 3);
    }
    */

  }
}
