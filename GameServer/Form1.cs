using OpenCvSharp;
using OpenCvSharp.Aruco;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace GameServer
{
  public partial class Form1 : Form
  {

    VideoCapture capture;
    Mat frameOrig;
    Mat frame;
    Bitmap image;
    Thread cameraThread;

    Dictionary dictionary = CvAruco.GetPredefinedDictionary(PredefinedDictionaryName.Dict4X4_250);
    DetectorParameters parameters = DetectorParameters.Create();

    // camera calibration
    Mat cameraMatrix = new Mat(new OpenCvSharp.Size(3, 3), MatType.CV_32F, 1);
    Mat distCoeffs = new Mat(1, 8, MatType.CV_32F, 1);

    private void CaptureCamera()
    {
      cameraThread = new Thread(new ThreadStart(CaptureCameraCallback));
      cameraThread.Start();
    }

    private void ReleaseCamera()
    {
      if (cameraThread != null) cameraThread.Abort();
      if (capture != null) capture.Release();
    }

    // camera calibration
    private void CalibrateCamera()
    {
      cameraMatrix = Mat.Eye(3, 3, MatType.CV_64FC1);
      distCoeffs = Mat.Zeros(4, 1, MatType.CV_64FC1);
    }

    private void CaptureCameraCallback()
    {

      capture = new VideoCapture(0);
      capture.Open(0);
      capture.FrameWidth = 1920;
      capture.FrameHeight = 1080;

      frameOrig = new Mat();
      frame = new Mat();

      if (capture.IsOpened())
      {
        while (chkLive.Checked)
        {

          // read frame from camera
          capture.Read(frameOrig);

          // scale down frame
          Cv2.Resize(frameOrig, frame, new OpenCvSharp.Size(pictureBoxGame.Width, pictureBoxGame.Height)); // 640, 360

          // calibrate camera - de-skew frame
          frame = OpenWarpPerspective(frame);

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

                double[,] rvecDouble = new double[3,3];
                rvecDouble[0,0]=rvec.Item0;
                rvecDouble[0,1]=rvec.Item1;
                rvecDouble[0,2]=rvec.Item2;
                RotationMatrixToEulerZXY(rvecDouble);
                //Mat R;
                //OpenCvSharp.Cv2.Rodrigues(rvec, R); // calculate your object pose R matrix
                //double[,] mtxR;
                //double[,] mtxQ;
                //double[,] qx ; double[,] qy ; double[,] qz;
                //OpenCvSharp.Cv2.RQDecomp3x3(R, out mtxR, out mtxQ, out qx , out qy , out qz);

                //double[,] cameraMatrix1;
                //double[,] rotMatrix;
                //double[] transVect;
                //double[,] rotMatrixX;
                //double[,] rotMatrixY;
                //double[,] rotMatrixZ;
                //double[] eulerAngles;
                //OpenCvSharp.Cv2.DecomposeProjectionMatrix(rvec,
                //      cameraMatrix1,
                //      rotMatrix,
                //      transVect,
                //      rotMatrixX,
                //      rotMatrixY,
                //      rotMatrixZ,
                //      eulerAngles);
                //float theta = Math.Sqrt(a[0]**2 + a[1]**2 + a[2]**2); 
                //float b = a/theta ;
                //float qx = b[0] * Math.Sin(theta/2);
                //float qy = -b[1] * Math.Sin(theta/2);
                //float qz = b[2] * Math.Sin(theta/2);
                //float qw = Math.Cos(theta/2);

                // if this is the roomba position
                if (ids[i] == 0)
                {
                  //Ball.Left = (int)corners[0][0].X;
                  //Ball.Top = (int)corners[0][0].Y;
                }
                // left guard
                else if (ids[i] == 1)
                {
                }
                // right guard
                else if (ids[i] == 2)
                {
                }

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

        }
      }
    }

    public void RotationMatrixToEulerZXY(double[,] R)
    {
        var i = 2;
        var j = 0; // EULER_NEXT[2]
        var k = 1; // EULER_NEXT[3]

        var cos_beta = Math.Sqrt(Math.Pow(R[i, i], 2) + Math.Pow(R[j, i], 2));

        double alpha, beta, gamma;
        double EULER_EPSILON = 2.71828;
        if (cos_beta > EULER_EPSILON)
        {
            alpha = Math.Atan2(R[k, j], R[k, k]);
            beta = Math.Atan2(-R[k, i], cos_beta);
            gamma = Math.Atan2(R[j, i], R[i, i]);
        }
        else
        {
            alpha = Math.Atan2(-R[j, k], R[j, j]);
            beta = Math.Atan2(-R[k, i], cos_beta);
            gamma = 0.0;
        }

        //alpha = wrap_angles(alpha, 0.0, 2.0 * Math.PI); // Z
        //beta = wrap_angles(beta, 0.0, 2.0 * Math.PI); // X
        //gamma = wrap_angles(gamma, 0.0, 2.0 * Math.PI); // Y

        //RadianToDegree
        alpha = 180.0 - alpha * (180/Math.PI);
        beta = beta * (180/Math.PI);
        gamma = gamma * (180/Math.PI);

        Console.WriteLine(alpha + "," + beta + "," + gamma);
        // errr... 180 - Z result seems right. Why?
        //return new Rotation(RadianToDegree(beta), RadianToDegree(gamma), 180.0 - RadianToDegree(alpha));
    }

    public Form1()
    {
      InitializeComponent();
      this.KeyPreview = true;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      labelHelp.Text = "left up Q down A";
      labelHelp.Text += "\nright up P down L";
    }


    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      try
      {
        ReleaseCamera();

        //exit application
        Environment.Exit(0);

      }
      catch (Exception ex)
      {
        Console.WriteLine("Exception: {0}", ex);
        //lblError.Text = e.Message;
      }

    }


    const int limit_Pad = 170;
    const int limit_Ball = 245;
    const int x = 227, y = 120;

    int player_right_won = 0;
    int player_won = 0;

    int stepTop;
    int stepLeft;

    bool leftUp = false;
    bool leftDown = false;
    bool rightUp = false;
    bool rightDown = false;

    Random r = new Random();

    private int getLimitTop()
    {
      return pictureBoxGame.Top;
    }
    private int getLimitBottom()
    {
      return pictureBoxGame.Height - pictureBoxGame.Top;
    }
    private int getLimitRight()
    {
      return pictureBoxGame.Left + pictureBoxGame.Width;
    }

    // key pressed event
    private void Pressed(object sender, KeyEventArgs e)
    {

      // left paddle
      if (e.KeyCode == Keys.Q)
      {
        leftUp = true;
      }
      else if (e.KeyCode == Keys.A)
      {
        leftDown = true;
      }

      // right paddle
      if (e.KeyCode == Keys.Up || e.KeyCode == Keys.P)
      {
        rightUp = true;
      }
      else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.L)
      {
        rightDown = true;
      }

      timerLeftPaddle.Start();
      timerRightPaddle.Start();
    }

    // key released event
    private void Released(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Q)
      {
        leftUp = false;
      }
      else if (e.KeyCode == Keys.A)
      {
        leftDown = false;
      }

      if (e.KeyCode == Keys.Up || e.KeyCode == Keys.P)
      {
        rightUp = false;
      }
      else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.L)
      {
        rightDown = false;
      }

      timerLeftPaddle.Stop();
      timerRightPaddle.Stop();
    }

    private void timerLeftPaddle_Tick(object sender, EventArgs e)
    {
      if (leftUp && PlayerLeft.Location.Y > getLimitTop())
      {
        PlayerLeft.Top -= 3;
      }
      else if (leftDown && PlayerLeft.Location.Y < getLimitBottom() - PlayerLeft.Height)
      {
        PlayerLeft.Top += 3;
      }
    }

    private void timerRightPaddle_Tick(object sender, EventArgs e)
    {
      if (rightUp && PlayerRight.Location.Y > getLimitTop())
      {
        PlayerRight.Top -= 3;
      }
      else if (rightDown && PlayerRight.Location.Y < getLimitBottom() - PlayerRight.Height)
      {
        PlayerRight.Top += 3;
      }
    }



    private void MoveBall(object sender, EventArgs e)
    {
      if (Ball.Bounds.IntersectsWith(PlayerLeft.Bounds))
      {
        Collision(PlayerLeft);
      }
      else if (Ball.Bounds.IntersectsWith(PlayerRight.Bounds))
      {
        Collision(PlayerRight);
      }
      HitBorder();
      BallOutOfField();
      BallMoves();
    }

    private void Collision(PictureBox Paddle)
    {
      switch (true)
      {
        case true when Upper(Paddle):
          stepTop = Negative(4, 6);
          stepLeft = AdjustCoordinates(5, 6);
          break;
        case true when High(Paddle):
          stepTop = Negative(2, 3);
          stepLeft = AdjustCoordinates(6, 7);
          break;
        case true when Middle(Paddle):
          stepTop = 0;
          stepLeft = AdjustCoordinates(5, 5);
          break;
        case true when Low(Paddle):
          stepTop = r.Next(2, 3);
          stepLeft = AdjustCoordinates(6, 7);
          break;
        case true when Bot(Paddle):
          stepTop = r.Next(4, 6);
          stepLeft = AdjustCoordinates(5, 6);
          break;
      }
      Edge();
    }
    private int AdjustCoordinates(int i, int n)
    {
      int res = 0;

      if (Ball.Location.X < getLimitRight() / 2)
      {
        res = r.Next(i, n);
      }
      else if (Ball.Location.X > getLimitRight() / 2)
      {
        res = Negative(i, n);
      }
      return res;
    }
    private int Negative(int i, int n)
    {
      int myval = r.Next(i, n) * -1;
      return myval;
    }
    private bool Upper(PictureBox Pad)
    {
      return Ball.Location.Y >= Pad.Top - Ball.Height && Ball.Location.Y <= Pad.Top + Ball.Height;
    }
    private bool High(PictureBox Pad)
    {
      return Ball.Location.Y > Pad.Top + Ball.Height && Ball.Location.Y <= Pad.Top + 2 * Ball.Height;
    }
    private bool Middle(PictureBox Pad)
    {
      return Ball.Location.Y > Pad.Top + 2 * Ball.Height && Ball.Location.Y <= Pad.Top + 3 * Ball.Height;
    }
    private bool Low(PictureBox Pad)
    {
      return Ball.Location.Y > Pad.Top + 3 * Ball.Height && Ball.Location.Y <= Pad.Top + 4 * Ball.Height;
    }
    private bool Bot(PictureBox Pad)
    {
      return Ball.Location.Y > Pad.Top + 4 * Ball.Height && Ball.Location.Y <= Pad.Bottom + Ball.Height;
    }
    private void HitBorder()
    {
      if (Ball.Location.Y <= 0 || Ball.Location.Y >= limit_Ball)
      {
        stepTop *= -1;
      }
    }
    private void BallOutOfField()
    {
      if (player_won == 10 || player_right_won == 10)
      {
        EndGame();
      }

      if (Ball.Location.X < 0 - PlayerLeft.Width && Ball.Location.X < getLimitRight() / 2)
      {
        NewPoint(5);
        PlayerRightWon();
      }
      else if (Ball.Location.X > PlayerRight.Location.X + PlayerRight.Width && Ball.Location.X > getLimitRight() / 2)
      {
        NewPoint(-5);
        PlayerLeftWon();
      }
    }
    private void Edge()
    {
      if (Ball.Location.X < getLimitRight() / 2)
      {
        if (Ball.Location.X < 0 + Ball.Height / 3)
        {
          stepLeft *= -1;
        }
      }
      else if (Ball.Location.X > getLimitRight() / 2)
      {
        if (Ball.Location.X > PlayerRight.Location.X + (Ball.Width / 3))
        {
          stepLeft *= -1;
        }
      }
    }
    private void NewPoint(int n)
    {
      Ball.Location = new System.Drawing.Point(x, y);
      stepTop = 0;
      stepLeft = n;
    }
    private void StartValues()
    {
      stepTop = 0;
      stepLeft = -5;
    }

    private void BallMoves()
    {
      //
      // tell the ball wehere it should be
      //

      if (chkLive.Checked)
      {
        // get ball location from camera

        //tell the ball wehere it should go
        //SendToRoomba("*************** " + Ball.Top.ToString());
        //SendToRoomba("*************** " + Ball.Left.ToString());
      }
      else
      {
        // update ball picture
        Ball.Top += stepTop;
        Ball.Left += stepLeft;
      }

    }

    private void Computer(object sender, EventArgs e)
    {
      if (chkComputer.Checked)
      {
        if (PlayerRight.Location.Y <= 0)
        {
          PlayerRight.Location = new System.Drawing.Point(PlayerRight.Location.X, 0);
        }
        else if (PlayerRight.Location.Y >= limit_Pad)
        {
          PlayerRight.Location = new System.Drawing.Point(PlayerRight.Location.X, limit_Pad);
        }
        if (Ball.Location.Y < PlayerRight.Top + (PlayerRight.Height / 2))
        {
          PlayerRight.Top -= 3;
        }
        else if (Ball.Location.Y > PlayerRight.Top + (PlayerRight.Height / 2))
        {
          PlayerRight.Top += 3;
        }
      }

    }
    private void PlayerLeftWon()
    {
      player_won++;
      labelLeftScore.Text = player_won.ToString();
    }
    private void PlayerRightWon()
    {
      player_right_won++;
      labelRightScore.Text = player_right_won.ToString();
    }
    private void button2_Click(object sender, EventArgs e)
    {
      StartValues();
      timerLeftPaddle.Start();
      timerRightPaddle.Start();
      timerBall.Start();
      timerComputer.Start();
    }

    private void pictureBoxGame_Paint(object sender, PaintEventArgs e)
    {
      int penWidth = 4;
      // draw middle dashed line
      using (Pen pen = new Pen(Color.White, penWidth))
      {
        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        e.Graphics.DrawLine(pen, new PointF(pictureBoxGame.Width / 2, 0), new PointF(pictureBoxGame.Width / 2, pictureBoxGame.Height));
      }

      // draw rectangle
      Rectangle rect = new Rectangle(0, 0, pictureBoxGame.Width - penWidth, pictureBoxGame.Height - penWidth);
      using (Pen pen = new Pen(Color.White, penWidth))
      {
        e.Graphics.DrawRectangle(pen, rect);
      }

    }

    private void EndGame()
    {
      PlayerLeft.Location = new System.Drawing.Point(0, 75);
      PlayerRight.Location = new System.Drawing.Point(454, 75);
      player_won = 0;
      player_right_won = 0;
      labelLeftScore.Text = player_won.ToString();
      labelRightScore.Text = player_right_won.ToString();
      timerLeftPaddle.Stop();
      timerRightPaddle.Stop();
      timerBall.Stop();
      timerComputer.Stop();
    }

    private void chkLive_CheckedChanged(object sender, EventArgs e)
    {
      if (chkLive.Checked)
      {
        CalibrateCamera();
        CaptureCamera();
      }
      else
      {
        ReleaseCamera();
      }
    }

    // de-skew camera trapez into rectangle
    Mat OpenWarpPerspective(Mat src)
    {

      Point2f[] a =
      {
          new Point2f(0 + 160, 0),
          new Point2f(0, pictureBoxGame.Height),
          new Point2f(pictureBoxGame.Width, pictureBoxGame.Height),
          new Point2f(pictureBoxGame.Width - 160, 0)
      };

      Point2f[] b =
      {
          new Point2f(0, 0),
          new Point2f(0, pictureBoxGame.Height),
          new Point2f(pictureBoxGame.Width, pictureBoxGame.Height),
          new Point2f(pictureBoxGame.Width, 0)
      };

      Mat dest = new Mat(new OpenCvSharp.Size(pictureBoxGame.Width, pictureBoxGame.Height), MatType.CV_8UC3);
      Mat map_matrix = Cv2.GetPerspectiveTransform(a, b);
      Cv2.WarpPerspective(src, dest, map_matrix, dest.Size(), InterpolationFlags.Linear | InterpolationFlags.WarpFillOutliers, BorderTypes.Default, Scalar.All(255)); //AccessViolation

      map_matrix.Dispose();
      return dest;  
    }



    private void SendToRoomba(String message)
    {
      try
      {

        // Create a TcpClient
        TcpClient client = new TcpClient("192.168.4.1", 80);
        //PERF: optimiz with client.Connected

        // Translate the passed message into ASCII and store it as a Byte array.
        Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

        // Get a client stream for reading and writing.
        NetworkStream stream = client.GetStream();

        // Send the message to the connected TcpServer.
        stream.Write(data, 0, data.Length);
        Console.WriteLine("Sent: {0}", message);

        // Receive the TcpServer.response.

        // Buffer to store the response bytes.
        //data = new Byte[256];
        // String to store the response ASCII representation.
        //String responseData = String.Empty;

        // Read the first batch of the TcpServer response bytes.
        ////StreamReader sr = new StreamReader(stream);
        ////responseData = sr.ReadLine();
        //Int32 bytes = stream.Read(data, 0, data.Length);
        //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
        //Console.WriteLine("Received: {0}", responseData);

        stream.Close();
        client.Close();
      }
      catch (Exception e)
      {
        Console.WriteLine("Exception: {0}", e);
        lblError.Text = e.Message;
      }

    }


  }
}
