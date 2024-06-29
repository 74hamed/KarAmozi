using System;
using System.Windows.Forms;
using SharpGL;

namespace Sensor3DViewer
{
    public partial class Form1 : Form
    {
        private float roll, pitch, yaw;
        private Timer renderTimer;
        private Timer sensorUpdateTimer;
        private OpenGLControl openGLControl;

        public Form1()
        {
            InitializeComponent();
            InitOpenGLControl();
            InitRenderLoop();
            InitSensorSimulation();
        }

        private void InitOpenGLControl()
        {
            openGLControl = new OpenGLControl();
            openGLControl.Dock = DockStyle.Fill;
            openGLControl.OpenGLInitialized += OpenGLControl_OpenGLInitialized;
            openGLControl.OpenGLDraw += OpenGLControl_OpenGLDraw;
            openGLControl.Resized += OpenGLControl_Resized;
            this.Controls.Add(openGLControl);
        }

        private void InitRenderLoop()
        {
            renderTimer = new Timer();
            renderTimer.Interval = 16; //60 FPS
            renderTimer.Tick += (sender, e) => openGLControl.Invalidate();
            renderTimer.Start();
        }

        private void InitSensorSimulation()
        {
            sensorUpdateTimer = new Timer();
            sensorUpdateTimer.Interval = 5; // update every 5ms
            sensorUpdateTimer.Tick += SensorUpdateTimer_Tick;
            sensorUpdateTimer.Start();
        }

        private void SensorUpdateTimer_Tick(object sender, EventArgs e)
        {

            roll += 1.0f; // speed of rotation
            pitch += 0.5f;
            yaw += 0.2f;

            // values should be within 0-360
            roll = roll % 360;
            pitch = pitch % 360;
            yaw = yaw % 360;
        }

        private void OpenGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            var gl = openGLControl.OpenGL;
            gl.ClearColor(0, 0, 0, 0);
        }

        private void OpenGLControl_Resized(object sender, EventArgs e)
        {
            var gl = openGLControl.OpenGL;
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Perspective(45.0f, (double)Width / (double)Height, 0.1, 100.0);
            gl.LookAt(0, 0, 5, 0, 0, 0, 0, 1, 0);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        private void OpenGLControl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            var gl = openGLControl.OpenGL;


            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();

            //first cube to the left
            gl.PushMatrix();
            gl.Translate(-2.0f, 0.0f, -8.0f); //location
            gl.Rotate(roll, 1.0f, 0.0f, 0.0f);
            gl.Rotate(pitch, 0.0f, 1.0f, 0.0f);
            gl.Rotate(yaw, 0.0f, 0.0f, 1.0f);
            DrawCube(gl);
            gl.PopMatrix();

            //second cube to the right
            gl.PushMatrix();
            gl.Translate(2.0f, 0.0f, -8.0f); //location
            gl.Rotate(roll, 1.0f, 0.0f, 0.0f);
            gl.Rotate(pitch, 0.0f, 1.0f, 0.0f);
            gl.Rotate(yaw, 0.0f, 0.0f, 1.0f);
            DrawCube(gl);
            gl.PopMatrix();

            gl.Flush();
        }

        private void DrawCube(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_QUADS);

            // Front 
            gl.Color(1.0, 0.0, 0.0);
            gl.Vertex(-1.0, -1.0, 1.0);
            gl.Vertex(1.0, -1.0, 1.0);
            gl.Vertex(1.0, 1.0, 1.0);
            gl.Vertex(-1.0, 1.0, 1.0);

            // Back 
            gl.Color(0.0, 1.0, 0.0);
            gl.Vertex(-1.0, -1.0, -1.0);
            gl.Vertex(1.0, -1.0, -1.0);
            gl.Vertex(1.0, 1.0, -1.0);
            gl.Vertex(-1.0, 1.0, -1.0);

            // Top 
            gl.Color(0.0, 0.0, 1.0);
            gl.Vertex(-1.0, 1.0, -1.0);
            gl.Vertex(1.0, 1.0, -1.0);
            gl.Vertex(1.0, 1.0, 1.0);
            gl.Vertex(-1.0, 1.0, 1.0);

            // Bottom 
            gl.Color(1.0, 1.0, 0.0);
            gl.Vertex(-1.0, -1.0, -1.0);
            gl.Vertex(1.0, -1.0, -1.0);
            gl.Vertex(1.0, -1.0, 1.0);
            gl.Vertex(-1.0, -1.0, 1.0);

            // Right 
            gl.Color(1.0, 0.0, 1.0);
            gl.Vertex(1.0, -1.0, -1.0);
            gl.Vertex(1.0, 1.0, -1.0);
            gl.Vertex(1.0, 1.0, 1.0);
            gl.Vertex(1.0, -1.0, 1.0);

            // Left 
            gl.Color(0.0, 1.0, 1.0);
            gl.Vertex(-1.0, -1.0, -1.0);
            gl.Vertex(-1.0, 1.0, -1.0);
            gl.Vertex(-1.0, 1.0, 1.0);
            gl.Vertex(-1.0, -1.0, 1.0);

            gl.End();
        }
    }
}
