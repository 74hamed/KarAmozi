using System;
using System.Windows.Forms;
using SharpGL;
using SharpGL.SceneGraph.Assets;

namespace Sensor3DViewer
{
    public partial class Form1 : Form
    {
        private float roll, pitch, yaw;
        private Timer sensorUpdateTimer;
        private OpenGLControl openGLControl;
        public Form1()
        {
            InitializeComponent();
            InitOpenGLControl();
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

        private void InitSensorSimulation()
        {
            sensorUpdateTimer = new Timer();
            sensorUpdateTimer.Interval = 450; // updating rate
            sensorUpdateTimer.Tick += SensorUpdateTimer_Tick;
            sensorUpdateTimer.Start();
        }

        private void SensorUpdateTimer_Tick(object sender, EventArgs e)
        {
            // rotating the cube randomly (for now)
            Random rand = new Random();
            roll = (float)(rand.NextDouble() * 360);
            pitch = (float)(rand.NextDouble() * 360);
            yaw = (float)(rand.NextDouble() * 360);

            openGLControl.Invalidate();
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
      
            gl.Translate(0.0f, 0.0f, -5.0f);

            gl.Rotate(roll, 1.0f, 0.0f, 0.0f);
            gl.Rotate(pitch, 0.0f, 1.0f, 0.0f);
            gl.Rotate(yaw, 0.0f, 0.0f, 1.0f);

            //texture mapping(https://stackoverflow.com/questions/71933324/sharpgl-low-resolution-textures)

            //var texture = new SharpGL.SceneGraph.Assets.Texture();
            //texture.Create(gl, "cratetexture.jpg");
            //gl.Enable(OpenGL.GL_TEXTURE_2D);
            //gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);

            DrawCube(gl);

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

        // Method to update the sensor data
        public void UpdateSensorData(float newRoll, float newPitch, float newYaw)
        {
            roll = newRoll;
            pitch = newPitch;
            yaw = newYaw;
            openGLControl.Invalidate(); 
        }

    }
}
