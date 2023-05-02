using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

using ImGuiNET;
using System.Diagnostics;

namespace testOne {
    public class Game : GameWindow {

        public static float WindowWidth;
        public static float WindowHeight;
        public static float CameraWidth;
        public static float CameraHeight;

        ImGuiController UIController;

        List<string[]> logData = new List<string[]>();


        Stopwatch timer;
        Shader? shader;

        public static float[] vertices = {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
            0.5f, -0.5f, 0.0f, //Bottom-right vertex
            0.0f,  0.5f, 0.0f  //Top vertex
        };

        int VertexBufferObject;
        int VertexArrayObject;


        public Game(int width, int height, string title, string fontPath, float fontSize)
            : base(GameWindowSettings.Default, new NativeWindowSettings()
            {
                Title = title,
                Size = new Vector2i(width, height),
                WindowBorder = WindowBorder.Resizable,
                StartVisible = false,
                StartFocused = true,
                WindowState = WindowState.Normal,
                API = ContextAPI.OpenGL,
                Profile = ContextProfile.Core,
                APIVersion = new Version(3, 3)
            })
        {
            // Center the window
            this.CenterWindow();
            WindowHeight = Size.Y;
            WindowWidth = Size.X;
            CameraHeight = Size.Y;
            CameraWidth = Size.X;

            UIController = new ImGuiController((int)WindowWidth, (int)WindowHeight, fontPath, fontSize);

            log("DEBUG", "message1");
            log("DEBUG", "message2");
            log("DEBUG", "message3");
            log("DEBUG", "message4");

            // OCCTProxy myOCCTProxy = new OCCTProxy();
            // if (!myOCCTProxy.InitGLViewer())
            // {
            //     Console.WriteLine("error during init");
            // } else {
            //     Console.WriteLine("OCCT GL Initialized");
            // }

            this.timer = new Stopwatch();
            this.timer.Start();
        }

        public void log(string level, string message)
        {
            this.logData.Add(new string[] {level, DateTime.Now.ToString("HH:mm:ss"), message});
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                this.log("DEBUG", "Application close");
                //Close();
            }
        }

        protected override void OnLoad()
        {
            //GUI.LoadTheme();

            this.VSync = VSyncMode.On;
            this.IsVisible = true;

            this.GenFBO(WindowWidth, WindowHeight);
            

            base.OnLoad();

            
        }

        protected override void OnUnload()
        {
            if (shader != null)
            {
                shader.Dispose();
            }

            base.OnUnload();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            WindowWidth = e.Width;
            WindowHeight = e.Height;

            GL.DeleteFramebuffer(FBO);
            GenFBO(WindowWidth, WindowHeight);

            UIController.WindowResized((int)WindowWidth, (int)WindowHeight);

            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            if (shader != null)
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
                //GL.Disable(EnableCap.DepthTest);
                shader.Use();

                GL.DeleteVertexArray(VertexArrayObject);

                VertexArrayObject = GL.GenVertexArray();
                GL.BindVertexArray(VertexArrayObject);

                GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
                // 3. then set our vertex attributes pointers
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                double timeValue = timer.Elapsed.TotalMilliseconds / 30;
                float mod = (float)Math.Sin(timeValue) / 2.0f + 0.5f;

                int vertexColorLocation = GL.GetUniformLocation(shader.Handle, "vertexColor");
                GL.Uniform4(vertexColorLocation, 
                    colorPicked.X * mod,
                    colorPicked.Y * mod,
                    colorPicked.Z * mod,
                    colorPicked.W * mod);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, framebufferTexture);
                GL.BindVertexArray(VertexArrayObject);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            }


            
                


            UIController.Update(this, (float)args.Time);
            ImGui.DockSpaceOverViewport();
            GUI.WindowOnOffs();
            GUI.LoadOCCTWindow(ref camWidth, ref camHeight, ref framebufferTexture);
            GUI.LogWindow(logData);

            UIController.Render();
            ImGuiController.CheckGLError("End of frame");

            Context.SwapBuffers();  

            base.OnRenderFrame(args);

            //GL.Clear(ClearBufferMask.ColorBufferBit);

            

            

                      
            
        }


        public int FBO; //RBO;
        public int framebufferTexture;

        public void GenFBO(float CamWidth, float CamHeight)
        {
            FBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

            // Color Texture
            framebufferTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, framebufferTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f, (int)CamWidth, (int)CamHeight, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);



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


            
            // Attach color to FBO
             GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, framebufferTexture, 0);

            



            var fboStatus = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (fboStatus != FramebufferErrorCode.FramebufferComplete)
            {
                Console.WriteLine("Framebuffer error: " + fboStatus);
            }
        }

        public float camWidth = 800f;
        public float camHeight = 600f;

        public static float angle = 0.0f;
        public static System.Numerics.Vector4 colorPicked = new System.Numerics.Vector4(0.0f);
    }
}
