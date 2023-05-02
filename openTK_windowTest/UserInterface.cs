﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

using ImGuiNET;

using static testOne.Game;

namespace testOne
{
    class GUI
    {
        public static float fontSize = 0.6f;

        public static bool showDemoWindow = false;
        public static bool showStatistics = false;
        public static bool showMaterialEditor = false;
        public static bool showObjectProperties = true;
        public static bool showLightProperties = true;
        public static bool showOutliner = true;

        public static float spacing = 2f;

        public static void WindowOnOffs()
        {
            LoadMenuBar();
            demoWindow();
            demoValueWindow();
            ImGui.ShowDemoWindow();
        }

        public static void LoadOCCTWindow(ref float CameraWidth, ref float CameraHeight, ref int framebufferTexture)
        {
            ImGui.Begin("OCCT");

            CameraWidth = ImGui.GetWindowWidth();
            CameraHeight = ImGui.GetWindowHeight() - ImGui.GetIO().FontGlobalScale * 71;

            //isMainHovered = ImGui.IsWindowHovered();

            ImGui.Image((IntPtr)framebufferTexture,
                new System.Numerics.Vector2(CameraWidth, CameraHeight),
                new System.Numerics.Vector2(0, 0.95f),
                new System.Numerics.Vector2(1, 0),
                new System.Numerics.Vector4(1.0f),
                new System.Numerics.Vector4(1, 1, 1, 0.2f));
            ImGui.End();
        }
       

        public static void LoadMenuBar()
        {
            ImGui.BeginMainMenuBar();
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("Save", "Ctrl + S")) 
                {
                    Console.WriteLine("Saved");
                }
                ImGui.Separator();
                if (ImGui.MenuItem("Load"))
                {
                    Console.WriteLine("Load");
                }

                ImGui.Separator();
                if (ImGui.MenuItem("Quit", "Alt+F4")) Console.WriteLine("Close Window");

                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Window"))
            {
                if (ImGui.BeginMenu("Debug"))
                {
                    ImGui.Checkbox("Statistics", ref showStatistics);
                    ImGui.Separator();
                    ImGui.Checkbox("ImGUI Demo ", ref showDemoWindow);
                    ImGui.EndMenu();
                }

                ImGui.Separator();

                if (ImGui.BeginMenu("Editor"))
                {
                    ImGui.Checkbox("Show Object Properties", ref showObjectProperties);
                    ImGui.Separator();
                    ImGui.Checkbox("Show Light Properties", ref showLightProperties); 
                    ImGui.Separator();
                    ImGui.Checkbox("Show Outliner", ref showOutliner);
                    ImGui.Separator();
                    // ImGui.Checkbox("Show Settings", ref showSettings);
                    ImGui.Separator();
                    ImGui.Checkbox("Show Material Editor", ref showMaterialEditor);
                    ImGui.EndMenu();
                }

                ImGui.EndMenu();
            }

            ImGui.Dummy(new System.Numerics.Vector2(ImGui.GetWindowWidth() -1000, 0));
            // ImGui.TextDisabled("Objects: " + (Objects.Count).ToString());
            ImGui.TextDisabled(" | ");
            ImGui.TextDisabled(GL.GetString(StringName.Renderer));
            ImGui.TextDisabled(" | ");
            ImGui.TextDisabled("FPS: " + ImGui.GetIO().Framerate.ToString("0"));
            ImGui.TextDisabled(" | ");
            ImGui.TextDisabled("MS: " + (1000 / ImGui.GetIO().Framerate).ToString("0.00"));

            ImGui.EndMainMenuBar();
        }


        



        public static void LogWindow(List<string[]> logData)
        {
            int col = 3;
            ImGui.Begin("Log");
            ImGui.BeginTable("Table", col);

            ImGui.TableSetupColumn("Level");

            ImGui.TableSetupColumn("Time");
            ImGui.TableSetupColumn("Message");
            ImGui.TableHeadersRow();

            List<string[]> rev = new List<string[]>(logData);
            
            rev.Reverse();

            foreach (string[] data in rev)
            {
                for (int i = 0;i < col; i++)
                {
                    ImGui.TableNextColumn();
                    ImGui.Text(data[i]);
                }
                ImGui.TableNextRow();
                
            }
            ImGui.EndTable();
            ImGui.End();
        }



        public static void demoWindow()
        {
            ImGui.Begin("Demo");
            ImGui.SliderAngle("angle slider", ref Game.angle);
            ImGui.ColorPicker4("color", ref Game.colorPicked);
            ImGui.End();
        }

        public static void demoValueWindow()
        {
            ImGui.Begin("Values");
            ImGui.Text(Game.angle.ToString());
            ImGui.Separator();
            ImGui.Text("R: " + Game.colorPicked.X.ToString());
            ImGui.Text("G: " + Game.colorPicked.Y.ToString());
            ImGui.Text("B: " + Game.colorPicked.Z.ToString());
            ImGui.Text("A: " + Game.colorPicked.W.ToString());
            ImGui.End();
        }

        // public static void LoadStatistics(float spacing)
        // {
        //     Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        //     // Stats
        //     ImGui.Begin("Statistics");
        //     ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //     ImGui.Separator();
        //     ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

        //     if (ImGui.TreeNode("Rendering"))
        //     {
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Text("Renderer: " + GL.GetString(StringName.Renderer));
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Separator();
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Text("FPS: " + ImGui.GetIO().Framerate.ToString("0.00"));
        //         ImGui.Text("MS: " + (1000 / ImGui.GetIO().Framerate).ToString("0.00"));
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Separator();
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         // ImGui.Text("Objects: " + (Objects.Count).ToString());
        //         // ImGui.Text("Point Lights: " + (numPL).ToString());
        //         // ImGui.Text("Selected Object: " + selectedObject.ToString());
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Separator();
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Text("Viewport Width: "); ImGui.SameLine(); ImGui.Text(" | "); ImGui.SameLine(); ImGui.Text("Viewport Height: ");
        //         ImGui.TreePop();
        //     }

        //     ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //     ImGui.Separator();
        //     ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

        //     if (ImGui.TreeNode("Camera"))
        //     {
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Text("Yaw: ");
        //         ImGui.Text("Pitch: ");
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Separator();
        //         ImGui.Text("X: " + position.X);
        //         ImGui.Text("Y: " + position.Y);
        //         ImGui.Text("Z: " + position.Z);
        //         ImGui.TreePop();
        //     }

        //     ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //     ImGui.Separator();
        //     ImGui.End();
        // }




        public static void LoadTheme()
        {
            // Styling
            ImGui.GetStyle().FrameRounding = 6;
            ImGui.GetStyle().FrameBorderSize = 1;
            ImGui.GetStyle().TabRounding = 2;
            ImGui.GetStyle().FramePadding = new System.Numerics.Vector2(4);
            ImGui.GetStyle().ItemSpacing = new System.Numerics.Vector2(8, 2);
            ImGui.GetStyle().ItemInnerSpacing = new System.Numerics.Vector2(1, 4);
            ImGui.GetStyle().WindowMenuButtonPosition = ImGuiDir.None;
            ImGui.GetIO().FontGlobalScale = fontSize;

            ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(new System.Numerics.Vector3(0.9f), 1));

            // Background color
            ImGui.PushStyleColor(ImGuiCol.WindowBg, new System.Numerics.Vector4(22f, 22f, 22f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(20f, 20f, 20f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, new System.Numerics.Vector4(60f, 60f, 60f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.FrameBgActive, new System.Numerics.Vector4(80f, 80f, 80f, 255f) / 255);

            // Popup BG
            ImGui.PushStyleColor(ImGuiCol.ModalWindowDimBg, new System.Numerics.Vector4(30f, 30f, 30f, 150f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TextDisabled, new System.Numerics.Vector4(150f, 150f, 150f, 255f) / 255);

            // Titles
            ImGui.PushStyleColor(ImGuiCol.TitleBgActive, new System.Numerics.Vector4(20f, 20f, 20f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TitleBg, new System.Numerics.Vector4(20f, 20f, 20f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TitleBgCollapsed, new System.Numerics.Vector4(15f, 15f, 15f, 255f) / 255);

            // Tabs
            ImGui.PushStyleColor(ImGuiCol.Tab, new System.Numerics.Vector4(20f, 20f, 20f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TabActive, new System.Numerics.Vector4(35f, 35f, 35f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TabUnfocused, new System.Numerics.Vector4(16f, 16f, 16f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TabUnfocusedActive, new System.Numerics.Vector4(35f, 35f, 35f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TabHovered, new System.Numerics.Vector4(80f, 80f, 80f, 255f) / 255);
            
            // Header
            ImGui.PushStyleColor(ImGuiCol.Header, new System.Numerics.Vector4(0f, 153f, 76f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.HeaderHovered, new System.Numerics.Vector4(0f, 153f, 76f, 180f) / 255);
            ImGui.PushStyleColor(ImGuiCol.HeaderActive, new System.Numerics.Vector4(0f, 153f, 76f, 255f) / 255);

            // Rezising bar
            ImGui.PushStyleColor(ImGuiCol.Separator, new System.Numerics.Vector4(40f, 40f, 40f, 255) / 255);
            ImGui.PushStyleColor(ImGuiCol.SeparatorHovered, new System.Numerics.Vector4(60f, 60f, 60f, 255) / 255);
            ImGui.PushStyleColor(ImGuiCol.SeparatorActive, new System.Numerics.Vector4(80f, 80f, 80f, 255) / 255);

            // Buttons
            ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(255, 41, 55, 200) / 255);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new System.Numerics.Vector4(255, 41, 55, 150) / 255);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, new System.Numerics.Vector4(255, 41, 55, 100) / 255);

            // Docking and rezise
            ImGui.PushStyleColor(ImGuiCol.DockingPreview, new System.Numerics.Vector4(0f, 153f, 76f, 220) / 255);
            ImGui.PushStyleColor(ImGuiCol.ResizeGrip, new System.Numerics.Vector4(217, 35, 35, 255) / 255);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripHovered, new System.Numerics.Vector4(217, 35, 35, 200) / 255);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripActive, new System.Numerics.Vector4(217, 35, 35, 150) / 255);

            // Sliders, buttons, etc
            ImGui.PushStyleColor(ImGuiCol.SliderGrab, new System.Numerics.Vector4(120f, 120f, 120f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.SliderGrabActive, new System.Numerics.Vector4(180f, 180f, 180f, 255f) / 255);
        }
    }
}
