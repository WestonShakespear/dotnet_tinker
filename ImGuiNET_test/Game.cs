// stopwatch
using System.Diagnostics;

// opentk getting started
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


//imgui.net github
using OpenTK.Mathematics;

using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;
using System;

using ImGuiNET;


namespace testOne {
    public class Game : GameWindow {

        Shader? shader;
        Stopwatch timer;

        float[] vertices = {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
            0.5f, -0.5f, 0.0f, //Bottom-right vertex
            0.0f,  0.5f, 0.0f  //Top vertex
        };

        int VertexBufferObject;
        int VertexArrayObject;

        public Game(int width, int height, string title) :
            base(GameWindowSettings.Default,
                new NativeWindowSettings() 
                { Size = (width, height), Title = title })
        {
            timer = new Stopwatch();
            timer.Start();
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            VertexBufferObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            shader = new Shader("shader.vert", "shader.frag");

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            // 3. then set our vertex attributes pointers
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);


        }

        protected override void OnUnload()
        {
            base.OnUnload();

            if (shader != null)
            {
                shader.Dispose();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            // clear the buffer
            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            // use our shader if initialized
            if (shader != null)
            {
                shader.Use();

                // sweep color
                double timeValue = timer.Elapsed.TotalSeconds;
                float greenValue = (float)Math.Sin(timeValue) / 2.0f + 0.5f;
                int vertexColorLocation = GL.GetUniformLocation(shader.Handle, "vertexColor");
                GL.Uniform4(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);


                // render tri
                GL.BindVertexArray(VertexArrayObject);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            }

            // display
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        


    }
}
