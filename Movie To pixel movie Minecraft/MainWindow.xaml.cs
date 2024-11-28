using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using OpenCvSharp;
using Color = System.Windows.Media.Color;

namespace Movie_To_pixel_movie_Minecraft
{
    public partial class MainWindow : System.Windows.Window
    {
        private Dictionary<Color, string> MinecraftBlockColors;
        private int xBlocks = 50;
        private int zBlocks = 50;
        private double zoomLevel = 1.0;
        private Color[,] blockColors;
        private string commandsDirectoryPath = null;

        private string baseCoordinateY = "0";
        private string baseCoordinateX = "0";
        private string baseCoordinateZ = "0";

        private int globalX = 0;
        private int globalY = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeMinecraftBlocks();
        }

        private void InitializeMinecraftBlocks()
        {
            // Define Minecraft blocks with approximate RGB values
            MinecraftBlockColors = new Dictionary<Color, string>
{
    { Color.FromRgb(0, 0, 0), "minecraft:black_concrete" },
    { Color.FromRgb(255, 255, 255), "minecraft:white_concrete" },
    { Color.FromRgb(128, 128, 128), "minecraft:gray_concrete" },
    { Color.FromRgb(200, 200, 200), "minecraft:light_gray_concrete" }, 
    { Color.FromRgb(255, 0, 0), "minecraft:red_concrete" },
    { Color.FromRgb(128, 0, 0), "minecraft:red_terracotta" },
    { Color.FromRgb(0, 128, 0), "minecraft:green_concrete" },
    { Color.FromRgb(0, 255, 0), "minecraft:lime_concrete" },
    { Color.FromRgb(0, 0, 255), "minecraft:blue_concrete" },
    { Color.FromRgb(0, 0, 128), "minecraft:blue_terracotta" },
    { Color.FromRgb(255, 165, 0), "minecraft:orange_terracotta" },
    { Color.FromRgb(255, 192, 203), "minecraft:pink_concrete" },
    { Color.FromRgb(128, 0, 128), "minecraft:purple_concrete" },
    { Color.FromRgb(255, 255, 0), "minecraft:yellow_concrete" },
    { Color.FromRgb(139, 69, 19), "minecraft:brown_terracotta" },
    { Color.FromRgb(245, 222, 179), "minecraft:sand" },
    { Color.FromRgb(210, 180, 140), "minecraft:oak_planks" },
    { Color.FromRgb(34, 139, 34), "minecraft:green_concrete" },
    { Color.FromRgb(107, 142, 35), "minecraft:green_wool" },
    { Color.FromRgb(255, 215, 0), "minecraft:green_wool" },
    { Color.FromRgb(105, 105, 105), "minecraft:stone" },
    { Color.FromRgb(120, 128, 144), "minecraft:iron_block" }, 
    { Color.FromRgb(160, 82, 45), "minecraft:brown_wool" },
    { Color.FromRgb(255, 250, 205), "minecraft:yellow_wool" },
    { Color.FromRgb(0, 191, 255), "minecraft:cyan_concrete" },
    { Color.FromRgb(220, 20, 60), "minecraft:red_wool" },
    { Color.FromRgb(176, 224, 230), "minecraft:light_blue_wool" },
    { Color.FromRgb(255, 228, 225), "minecraft:pink_wool" },
    { Color.FromRgb(75, 0, 130), "minecraft:purple_wool" },
    { Color.FromRgb(255, 140, 0), "minecraft:orange_wool" },
    { Color.FromRgb(70, 130, 180), "minecraft:blue_wool" },
    { Color.FromRgb(46, 139, 87), "minecraft:green_terracotta" },
    { Color.FromRgb(244, 164, 96), "minecraft:terracotta" },
    { Color.FromRgb(240, 230, 140), "minecraft:yellow_terracotta" },
    { Color.FromRgb(255, 69, 0), "minecraft:red_terracotta" },
    { Color.FromRgb(110, 105, 105), "minecraft:polished_andesite" },
    { Color.FromRgb(192, 190, 192), "minecraft:polished_diorite" }, 
    { Color.FromRgb(169, 169, 169), "minecraft:smooth_stone" },
    { Color.FromRgb(222, 184, 135), "minecraft:birch_planks" },
    { Color.FromRgb(85, 107, 47), "minecraft:dark_oak_planks" },
    { Color.FromRgb(176, 196, 222), "minecraft:polished_basalt" },
    { Color.FromRgb(255, 248, 220), "minecraft:bone_block" },
    { Color.FromRgb(160, 160, 160), "minecraft:cobbled_deepslate" },
    { Color.FromRgb(47, 79, 79), "minecraft:deepslate_tiles" }
};
        }

        
        private void LoadVideo_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Push("scoreboard objectives add Timer dummy");
            // Prompt the user to select a directory to save all Minecraft commands
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Select Folder to Save Minecraft Commands"
            };

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                commandsDirectoryPath = folderDialog.SelectedPath;
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Video Files (*.mp4;*.avi)|*.mp4;*.avi"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    ProcessVideo(openFileDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("No folder selected. Cannot proceed without saving location.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ProcessVideo(string filePath)
        {
            var capture = new VideoCapture(filePath);
            int frameCount = (int)capture.Get(VideoCaptureProperties.FrameCount);
            double frameRate = capture.Get(VideoCaptureProperties.Fps);
            int framesPerSegment = (int)(frameRate); // Frames in 1 seconds
            int frameWidth = (int)capture.Get(VideoCaptureProperties.FrameWidth);
            int frameHeight = (int)capture.Get(VideoCaptureProperties.FrameHeight);

            // Calculate dimensions
            xBlocks = frameWidth / 10;
            zBlocks = frameHeight / 10;

            // Loop through video in 5-second chunks
            int currentFrame = 0;
            int segmentNumber = 1;
            CanvasOutput.Children.Clear();

            while (currentFrame < frameCount)
            {
                int segmentStartFrame = currentFrame;
                int segmentEndFrame = Math.Min(currentFrame + framesPerSegment, frameCount);
                ProcessSegment(capture, segmentStartFrame, segmentEndFrame, segmentNumber);
                currentFrame += framesPerSegment;
                segmentNumber++;
            }

            MessageBox.Show("All frames processed into segments.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ProcessSegment(VideoCapture capture, int startFrame, int endFrame, int segmentNumber)
        {
            commands.Clear(); // Reset commands for the segment

            for (int frameIndex = startFrame; frameIndex < endFrame; frameIndex++)
            {
                var frame = new Mat();
                if (!capture.Set(VideoCaptureProperties.PosFrames, frameIndex) || !capture.Read(frame))
                {
                    break; // End of video or frame error
                }

                // Process the frame and update coordinates
                ProcessFrame(frame, globalY, globalX);

                globalY++;
                if (globalY >= zBlocks) // When exceeding vertical bounds
                {
                    globalY = 0;
                    globalX += xBlocks + 1; // Move to the next "column" of blocks
                }
            }

            SaveSegmentCommands(segmentNumber);
        }

        private void SaveSegmentCommands(int segmentNumber)
        {
            if (string.IsNullOrEmpty(commandsDirectoryPath))
            {
                MessageBox.Show("Save directory not set. Please reload the video and select a folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Save setblock commands
            string segmentFilePath = Path.Combine(commandsDirectoryPath, $"segment{segmentNumber}.mcfunction");

            using (var outputFile = new StreamWriter(segmentFilePath, true)) // Open file for appending
            {
                while (commands.Count > 0)
                {
                    string command = (string)commands.Pop();
                    outputFile.WriteLine(command);
                }
            }

            // Save playback commands for the video player
            string videoPlayerFilePath = Path.Combine(commandsDirectoryPath, $"videoplayer_segment{segmentNumber}.mcfunction");

            using (var outputFile = new StreamWriter(videoPlayerFilePath, true)) // Open file for appending
            {
                // Create a temporary stack to reverse the original stack
                Stack<string> reversedStack = new Stack<string>();
                while (VideoPlayer.Count > 0)
                {
                    reversedStack.Push((string)VideoPlayer.Pop());
                }

                // Write reversed stack to the file
                while (reversedStack.Count > 0)
                {
                    string command = reversedStack.Pop();
                    outputFile.WriteLine(command);
                }
            }

            MessageBox.Show($"Segment {segmentNumber} commands saved.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }



        private unsafe void ProcessFrame(Mat frame, int yCoordinate,int x)
        {
            try
            {
                // Check for empty frame
                if (frame.Empty())
                {
                    MessageBox.Show("Frame data is empty. Skipping this frame.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int frameWidth = frame.Width;
                int frameHeight = frame.Height;
                int channels = frame.Channels();

                // Ensure frame has the expected number of channels
                if (channels != 3 && channels != 4) // BGR or BGRA
                {
                    MessageBox.Show("Unexpected frame channel count. Expected 3 or 4 channels.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Determine the pixel format and stride based on channels
                PixelFormat pixelFormat = channels == 4 ? PixelFormats.Bgra32 : PixelFormats.Bgr24;
                int stride = (int)frame.Step(); // Convert to int

                // Use Mat pointer directly as IntPtr for BitmapSource.Create
                BitmapSource bitmap;
                try
                {
                    bitmap = BitmapSource.Create(
                        frameWidth, frameHeight, 96, 96, pixelFormat, null, new IntPtr(frame.DataPointer), frameHeight * stride, stride);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show($"Error creating BitmapSource: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Scale the bitmap down to the desired block size
                int targetWidth = xBlocks;
                int targetHeight = zBlocks;
                var resizedBitmap = new TransformedBitmap(bitmap, new ScaleTransform((double)targetWidth / frameWidth, (double)targetHeight / frameHeight));

                // Calculate resized stride and create buffer for resized bitmap
                int resizedStride = (resizedBitmap.PixelWidth * pixelFormat.BitsPerPixel + 7) / 8;
                byte[] resizedPixels = new byte[resizedStride * resizedBitmap.PixelHeight];
                try
                {
                    resizedBitmap.CopyPixels(resizedPixels, resizedStride, 0);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show($"Error copying pixels from resized bitmap: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Process the pixels to display as blocks
                blockColors = new Color[xBlocks, zBlocks];
                DisplayBlocks(resizedPixels, resizedBitmap.PixelWidth, resizedBitmap.PixelHeight, resizedStride, x);
                GenerateMinecraftCommands(yCoordinate, x); // Generate commands for this frame

            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show($"ArgumentException encountered: {argEx.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void DisplayBlocks(byte[] pixels, int width, int height, int stride,int f)
        {
            CanvasOutput.Children.Clear();
            double blockSize = Math.Min(CanvasOutput.Width / xBlocks, CanvasOutput.Height / zBlocks);

            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    try
                    {
                        int index = z * stride + x * 4;

                        // Ensure that index does not exceed pixel array length
                        if (index + 2 >= pixels.Length) continue;

                        byte b = pixels[index];
                        byte g = pixels[index + 1];
                        byte r = pixels[index + 2];
                        Color color = Color.FromRgb(r, g, b);

                        var nearestBlock = GetNearestBlockColor(color);
                        blockColors[x, z] = nearestBlock.Key;
                        DrawBlock(x * blockSize, z * blockSize, blockSize, nearestBlock.Key);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        continue;
                    }
                }
            }
        }


        private KeyValuePair<Color, string> GetNearestBlockColor(Color color)
        {
            double minDistance = double.MaxValue;
            KeyValuePair<Color, string> nearestBlock = default;

            foreach (var block in MinecraftBlockColors)
            {
                Color blockColor = block.Key;
                double distance = Math.Sqrt(
                    Math.Pow(color.R - blockColor.R, 2) +
                    Math.Pow(color.G - blockColor.G, 2) +
                    Math.Pow(color.B - blockColor.B, 2));

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestBlock = block;
                }
            }

            return nearestBlock;
        }

        private void DrawBlock(double x, double z, double blockSize, Color color)
        {
            var rectangle = new System.Windows.Shapes.Rectangle
            {
                Width = blockSize,
                Height = blockSize,
                Fill = new SolidColorBrush(color)
            };

            Canvas.SetLeft(rectangle, x);
            Canvas.SetTop(rectangle, z);
            CanvasOutput.Children.Add(rectangle);
        }
        private Stack commands = new Stack();
        private Stack VideoPlayer = new Stack();

        int TimerScore = 0;
        private void GenerateMinecraftCommands(int yCoordinate, int xOffset)
        {
            if (string.IsNullOrEmpty(commandsDirectoryPath))
            {
                MessageBox.Show("Save directory not set. Please reload the video and select a folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            VideoPlayer.Push($"execute at @e[type=minecraft:armor_stand, tag=VideoPlayerArmorStand, scores={{Timer={TimerScore}}}] run clone {xOffset} {yCoordinate} 0 {xOffset + xBlocks} {yCoordinate} {zBlocks} ~ ~ ~");
            TimerScore++;
            for (int z = 0; z < zBlocks; z++)
            {
                for (int x = xOffset; x < (xBlocks + xOffset); x++)
                {
                    Color color = blockColors[x - xOffset, z];
                    string blockName = MinecraftBlockColors.TryGetValue(color, out var name) ? name : "air";
                    if (blockName != "air")
                    {
                        commands.Push($"setblock {x} {yCoordinate} {z} {blockName}");
                    }
                }
            }
        }



        private void ApplyDimensions_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(XBlocksTextBox.Text, out var xValue))
            {
                xBlocks = xValue;
            }
            if (int.TryParse(ZBlocksTextBox.Text, out var zValue))
            {
                zBlocks = zValue;
            }
        }



        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            zoomLevel *= 1.1;
            ZoomTransform.ScaleX = zoomLevel;
            ZoomTransform.ScaleY = zoomLevel;
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            zoomLevel /= 1.1;
            ZoomTransform.ScaleX = zoomLevel;
            ZoomTransform.ScaleY = zoomLevel;
        }

        private void GenerateCommands_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(commandsDirectoryPath))
            {
                MessageBox.Show("Save directory not set. Please reload the video and select a folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                string filePath = Path.Combine(commandsDirectoryPath, "allframesinminecraft.mcfunction");

                using (var outputFile = new StreamWriter(filePath, true)) // Open file for appending
                {
                    while (commands.Count > 0)
                    {
                        string command = (string)commands.Pop();
                        outputFile.WriteLine(command);
                    }
                }
                string VideoPlayerstring = Path.Combine(commandsDirectoryPath, "videoplayerinminecraft.mcfunction");

                using (var outputFile = new StreamWriter(VideoPlayerstring, true)) // Open file for appending
                {
                    // Create a temporary stack to reverse the original stack
                    Stack<string> reversedStack = new Stack<string>();
                    while (VideoPlayer.Count > 0)
                    {
                        reversedStack.Push((string)VideoPlayer.Pop());
                    }

                    // Write reversed stack to the file
                    while (reversedStack.Count > 0)
                    {
                        string command = reversedStack.Pop();
                        outputFile.WriteLine(command);
                    }
                }


                MessageBox.Show("Minecraft commands successfully generated and saved to the file.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving commands: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

