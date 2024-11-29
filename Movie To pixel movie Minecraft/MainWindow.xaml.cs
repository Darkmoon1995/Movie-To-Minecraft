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

       
        private int globalX = 0;
        private int yMin = -63; // Minimum Y coordinate
        private int yMax = 319; // Maximum Y coordinate
        private int globalY = -63; // Start at the minimum Y


        public MainWindow()
        {
            InitializeComponent();
            InitializeMinecraftBlocks();
        }

        private void InitializeMinecraftBlocks()
        {

            // Minecraft blocks with approximate RGB values
            MinecraftBlockColors = new Dictionary<Color, string>
{ 
    { Color.FromRgb(150, 88, 55), "minecraft:acacia_log" },
{ Color.FromRgb(168, 90, 50), "minecraft:acacia_planks" },
{ Color.FromRgb(133, 97, 191), "minecraft:amethyst_block" },
{ Color.FromRgb(94, 66, 58), "minecraft:ancient_debris" },
{ Color.FromRgb(136, 136, 136), "minecraft:andesite" },
{ Color.FromRgb(101, 124, 47), "minecraft:azalea" },
{ Color.FromRgb(139, 141, 62), "minecraft:bamboo_block" },
{ Color.FromRgb(190, 170, 78), "minecraft:bamboo_mosaic" },
{ Color.FromRgb(193, 173, 80), "minecraft:bamboo_planks" },
{ Color.FromRgb(134, 100, 58), "minecraft:barrel" },
{ Color.FromRgb(80, 81, 86), "minecraft:basalt" },
{ Color.FromRgb(85, 85, 85), "minecraft:bedrock" },
{ Color.FromRgb(202, 160, 74), "minecraft:bee_nest" },
{ Color.FromRgb(193, 179, 135), "minecraft:birch_log" },
{ Color.FromRgb(192, 175, 121), "minecraft:birch_planks" },
{ Color.FromRgb( 8, 10, 15), "minecraft:black_concrete" },
{ Color.FromRgb(25, 26, 31), "minecraft:black_concrete_powder" },
{ Color.FromRgb(37, 22, 16), "minecraft:black_terracotta" },
{ Color.FromRgb(22, 21, 25), "minecraft:black_wool" },
{ Color.FromRgb(42, 36, 41), "minecraft:blackstone" },
{ Color.FromRgb(80, 80, 81), "minecraft:blast_furnace" },
{ Color.FromRgb(44, 46, 143), "minecraft:blue_concrete" },
{ Color.FromRgb(70, 73, 166), "minecraft:blue_concrete_powder" },
{ Color.FromRgb(116, 167, 253), "minecraft:blue_ice" },
{ Color.FromRgb(74, 59, 91), "minecraft:blue_terracotta" },
{ Color.FromRgb(53, 57, 157), "minecraft:blue_wool" },
{ Color.FromRgb(209, 206, 179), "minecraft:bone_block" },
{ Color.FromRgb(150, 97, 83), "minecraft:bricks" },
{ Color.FromRgb(96, 59, 31), "minecraft:brown_concrete" },
{ Color.FromRgb(125, 84, 53), "minecraft:brown_concrete_powder" },
{ Color.FromRgb(77, 51, 35), "minecraft:brown_terracotta" },
{ Color.FromRgb(114, 71, 40), "minecraft:brown_wool" },
{ Color.FromRgb(165, 26, 162), "minecraft:bubble_coral_block" },
{ Color.FromRgb(223, 224, 220), "minecraft:calcite" },
{ Color.FromRgb(185, 141, 137), "minecraft:cherry_log" },
{ Color.FromRgb(226, 178, 172), "minecraft:cherry_planks" },
{ Color.FromRgb(178, 114, 88), "minecraft:chiseled_bookshelf" },
{ Color.FromRgb(184, 100, 73), "minecraft:chiseled_copper" },
{ Color.FromRgb(54, 54, 54), "minecraft:chiseled_deepslate" },
{ Color.FromRgb(47, 23, 28), "minecraft:chiseled_nether_bricks" },
{ Color.FromRgb(53, 48, 56), "minecraft:chiseled_polished_blackstone" },
{ Color.FromRgb(231, 226, 217), "minecraft:chiseled_quartz_block" },
{ Color.FromRgb(119, 118, 119), "minecraft:chiseled_stone_bricks" },
{ Color.FromRgb(93, 98, 91), "minecraft:chiseled_tuff" },
{ Color.FromRgb(160, 166, 179), "minecraft:clay" },
{ Color.FromRgb(16, 15, 15), "minecraft:coal_block" },
{ Color.FromRgb(119, 85, 59), "minecraft:coarse_dirt" },
{ Color.FromRgb(77, 77, 80), "minecraft:cobbled_deepslate" },
{ Color.FromRgb(127, 127, 127), "minecraft:cobblestone" },
{ Color.FromRgb(192, 107, 79), "minecraft:copper_block" },
{ Color.FromRgb(112, 98, 99), "minecraft:crafter" },
{ Color.FromRgb(119, 73, 42), "minecraft:crafting_table" },
{ Color.FromRgb(130, 31, 31), "minecraft:crimson_nylium" },
{ Color.FromRgb(101, 48, 70), "minecraft:crimson_planks" },
{ Color.FromRgb(32, 10, 60), "minecraft:crying_obsidian" },
{ Color.FromRgb(191, 106, 80), "minecraft:cut_copper" },
{ Color.FromRgb(21, 119, 136), "minecraft:cyan_concrete" },
{ Color.FromRgb(36, 147, 157), "minecraft:cyan_concrete_powder" },
{ Color.FromRgb(86, 91, 91), "minecraft:cyan_terracotta" },
{ Color.FromRgb(21, 137, 145), "minecraft:cyan_wool" },
{ Color.FromRgb(67, 45, 22), "minecraft:dark_oak_log" },
{ Color.FromRgb(66, 43, 20), "minecraft:dark_oak_planks" },
{ Color.FromRgb(51, 91, 75), "minecraft:dark_prismarine" },
{ Color.FromRgb(124, 117, 114), "minecraft:dead_brain_coral_block" },
{ Color.FromRgb(131, 123, 119), "minecraft:dead_bubble_coral_block" },
{ Color.FromRgb(70, 70, 71), "minecraft:deepslate_bricks" },
{ Color.FromRgb(54, 54, 55), "minecraft:deepslate_tiles" },
{ Color.FromRgb(87, 87, 89), "minecraft:deepslate" },
{ Color.FromRgb(98, 237, 228), "minecraft:diamond_block" },
{ Color.FromRgb(134, 96, 67), "minecraft:dirt" },
{ Color.FromRgb(148, 121, 65), "minecraft:dirt_path" },
{ Color.FromRgb(50, 58, 38), "minecraft:dried_kelp_block" },
{ Color.FromRgb(134, 107, 92), "minecraft:dripstone_block" },
{ Color.FromRgb(42, 203, 87), "minecraft:emerald_block" },
{ Color.FromRgb(219, 222, 158), "minecraft:end_stone" },
{ Color.FromRgb(218, 224, 162), "minecraft:end_stone_bricks" },
{ Color.FromRgb(161, 125, 103), "minecraft:exposed_copper" },
{ Color.FromRgb(154, 121, 101), "minecraft:exposed_cut_copper" },
{ Color.FromRgb(163, 35, 46), "minecraft:fire_coral_block" },
{ Color.FromRgb(110, 109, 109), "minecraft:furnace" },
{ Color.FromRgb(55, 42, 38), "minecraft:gilded_blackstone" },
{ Color.FromRgb(171, 131, 84), "minecraft:glowstone" },
{ Color.FromRgb(246, 208, 61), "minecraft:gold_block" },
{ Color.FromRgb(149, 103, 85), "minecraft:granite" },
{ Color.FromRgb(131, 127, 126), "minecraft:gravel" },
{ Color.FromRgb(54, 57, 61), "minecraft:gray_concrete" },
{ Color.FromRgb(76, 81, 84), "minecraft:gray_concrete_powder" },
{ Color.FromRgb(57, 42, 35), "minecraft:gray_terracotta" },
{ Color.FromRgb(62, 68, 71), "minecraft:gray_wool" },
{ Color.FromRgb(73, 91, 36), "minecraft:green_concrete" },
{ Color.FromRgb(97, 119, 44), "minecraft:green_concrete_powder" },
{ Color.FromRgb(76, 83, 42), "minecraft:green_terracotta" },
{ Color.FromRgb(84, 109, 27), "minecraft:green_wool" },
{ Color.FromRgb(165, 139, 12), "minecraft:hay_block" },
{ Color.FromRgb(229, 148, 29), "minecraft:honeycomb_block" },
{ Color.FromRgb(220, 220, 220), "minecraft:iron_block" },
{ Color.FromRgb(93, 64, 47), "minecraft:jukebox" },
{ Color.FromRgb(149, 109, 70), "minecraft:jungle_log" },
{ Color.FromRgb(160, 115, 80), "minecraft:jungle_planks" },
{ Color.FromRgb(30, 67, 140), "minecraft:lapis_block" },
{ Color.FromRgb(35, 137, 198), "minecraft:light_blue_concrete" },
{ Color.FromRgb(113, 108, 137), "minecraft:light_blue_terracotta" },
{ Color.FromRgb(58, 175, 217), "minecraft:light_blue_wool" },
{ Color.FromRgb(125, 125, 115), "minecraft:light_gray_concrete" },
{ Color.FromRgb(154, 154, 148), "minecraft:light_gray_concrete_powder" },
{ Color.FromRgb(135, 106, 97), "minecraft:light_gray_terracotta" },
{ Color.FromRgb(142, 142, 134), "minecraft:light_gray_wool" },
{ Color.FromRgb(94, 168, 24), "minecraft:lime_concrete" },
{ Color.FromRgb(125, 189, 41), "minecraft:lime_concrete_powder" },
{ Color.FromRgb(103, 117, 52), "minecraft:lime_terracotta" },
{ Color.FromRgb(112, 185, 25), "minecraft:lime_wool" },
{ Color.FromRgb(147, 149, 152), "minecraft:lodestone" },
{ Color.FromRgb(169, 48, 159), "minecraft:magenta_concrete" },
{ Color.FromRgb(192, 83, 184), "minecraft:magenta_concrete_powder" },
{ Color.FromRgb(149, 88, 108), "minecraft:magenta_terracotta" },
{ Color.FromRgb(189, 68, 179), "minecraft:magenta_wool" },
{ Color.FromRgb(102, 48, 42), "minecraft:mangrove_log" },
{ Color.FromRgb(117, 54, 48), "minecraft:mangrove_planks" },
{ Color.FromRgb(111, 114, 30), "minecraft:melon" },
{ Color.FromRgb(89, 109, 45), "minecraft:moss_block" },
{ Color.FromRgb(110, 118, 94), "minecraft:mossy_cobblestone" },
{ Color.FromRgb(115, 121, 105), "minecraft:mossy_stone_bricks" },
{ Color.FromRgb(60, 57, 60), "minecraft:mud" },
{ Color.FromRgb(137, 103, 79), "minecraft:mud_bricks" },
{ Color.FromRgb(111, 98, 101), "minecraft:mycelium" },
{ Color.FromRgb(44, 21, 26), "minecraft:nether_bricks" },
{ Color.FromRgb(114, 2, 2), "minecraft:nether_wart_block" },
{ Color.FromRgb(66, 61, 63), "minecraft:netherite_block" },
{ Color.FromRgb(97, 38, 38), "minecraft:netherrack" },
{ Color.FromRgb(151, 121, 73), "minecraft:oak_log" },
{ Color.FromRgb(162, 130, 78), "minecraft:oak_planks" },
{ Color.FromRgb(98, 98, 98), "minecraft:observer" },
{ Color.FromRgb(15, 10, 24), "minecraft:obsidian" },
{ Color.FromRgb(250, 245, 206), "minecraft:ochre_froglight" },
{ Color.FromRgb(224, 97, 0), "minecraft:orange_concrete" },
{ Color.FromRgb(227, 131, 31), "minecraft:orange_concrete_powder" },
{ Color.FromRgb(161, 83, 37), "minecraft:orange_terracotta" },
{ Color.FromRgb(240, 118, 19), "minecraft:orange_wool" },
{ Color.FromRgb(82, 162, 132), "minecraft:oxidized_copper" },
{ Color.FromRgb(79, 153, 126), "minecraft:oxidized_cut_copper" },
{ Color.FromRgb(141, 180, 250), "minecraft:packed_ice" },
{ Color.FromRgb(142, 106, 79), "minecraft:packed_mud" },
{ Color.FromRgb(245, 240, 239), "minecraft:pearlescent_froglight" },
{ Color.FromRgb(213, 101, 142), "minecraft:pink_concrete" },
{ Color.FromRgb(228, 153, 181), "minecraft:pink_concrete_powder" },
{ Color.FromRgb(161, 78, 78), "minecraft:pink_terracotta" },
{ Color.FromRgb(237, 141, 172), "minecraft:pink_wool" },
{ Color.FromRgb(91, 63, 24), "minecraft:podzol" },
{ Color.FromRgb(132, 134, 133), "minecraft:polished_andesite" },
{ Color.FromRgb(99, 98, 100), "minecraft:polished_basalt" },
{ Color.FromRgb(192, 193, 194), "minecraft:polished_diorite" },
{ Color.FromRgb(154, 106, 89), "minecraft:polished_granite" },
{ Color.FromRgb(97, 104, 99), "minecraft:polished_tuff" },
{ Color.FromRgb(248, 253, 253), "minecraft:powder_snow" },
{ Color.FromRgb(99, 171, 158), "minecraft:prismarine_bricks" },
{ Color.FromRgb(100, 31, 156), "minecraft:purple_concrete" },
{ Color.FromRgb(131, 55, 177), "minecraft:purple_concrete_powder" },
{ Color.FromRgb(118, 70, 86), "minecraft:purple_terracotta" },
{ Color.FromRgb(121, 42, 172), "minecraft:purple_wool" },
{ Color.FromRgb(169, 125, 169), "minecraft:purpur_block" },
{ Color.FromRgb(235, 229, 222), "minecraft:quartz_block" },
{ Color.FromRgb(154, 105, 79), "minecraft:raw_copper_block" },
{ Color.FromRgb(221, 169, 46), "minecraft:raw_gold_block" },
{ Color.FromRgb(166, 135, 107), "minecraft:raw_iron_block" },
{ Color.FromRgb(142, 32, 32), "minecraft:red_concrete" },
{ Color.FromRgb(168, 54, 50), "minecraft:red_concrete_powder" },
{ Color.FromRgb(69, 7, 9), "minecraft:red_nether_bricks" },
{ Color.FromRgb(181, 97, 31), "minecraft:red_sandstone" },
{ Color.FromRgb(143, 61, 46), "minecraft:red_terracotta" },
{ Color.FromRgb(160, 39, 34), "minecraft:red_wool" },
{ Color.FromRgb(175, 24, 5), "minecraft:redstone_block" },
{ Color.FromRgb(95, 54, 30), "minecraft:redstone_lamp" },
{ Color.FromRgb(80, 82, 78), "minecraft:reinforced_deepslate" },
{ Color.FromRgb(33, 21, 52), "minecraft:respawn_anchor" },
{ Color.FromRgb(144, 103, 76), "minecraft:rooted_dirt" },
{ Color.FromRgb(219, 207, 163), "minecraft:sand" },
{ Color.FromRgb(223, 214, 170), "minecraft:sandstone" },
{ Color.FromRgb(240, 146, 70), "minecraft:shroomlight" },
{ Color.FromRgb(57, 58, 70), "minecraft:smithing_table" },
{ Color.FromRgb(72, 72, 78), "minecraft:smooth_basalt" },
{ Color.FromRgb(158, 158, 158), "minecraft:smooth_stone" },
{ Color.FromRgb(75, 57, 46), "minecraft:soul_soil" },
{ Color.FromRgb(195, 192, 74), "minecraft:sponge" },
{ Color.FromRgb(108, 80, 46), "minecraft:spruce_log" },
{ Color.FromRgb(114, 84, 48), "minecraft:spruce_planks" },
{ Color.FromRgb(125, 125, 125), "minecraft:stone" },
{ Color.FromRgb(122, 121, 122), "minecraft:stone_bricks" },
{ Color.FromRgb(166, 91, 51), "minecraft:stripped_acacia_log" },
{ Color.FromRgb(178, 158, 72), "minecraft:stripped_bamboo_block" },
{ Color.FromRgb(191, 171, 116), "minecraft:stripped_birch_log" },
{ Color.FromRgb(221, 164, 157), "minecraft:stripped_cherry_log" },
{ Color.FromRgb(121, 56, 82), "minecraft:stripped_crimson_stem" },
{ Color.FromRgb(65, 44, 22), "minecraft:stripped_dark_oak_log" },
{ Color.FromRgb(165, 122, 81), "minecraft:stripped_jungle_log" },
{ Color.FromRgb(109, 43, 43), "minecraft:stripped_mangrove_log" },
{ Color.FromRgb(160, 129, 77), "minecraft:stripped_oak_log" },
{ Color.FromRgb(105, 80, 46), "minecraft:stripped_spruce_log" },
{ Color.FromRgb(152, 94, 67), "minecraft:terracotta" },
{ Color.FromRgb(229, 244, 228), "minecraft:verdant_froglight" },
{ Color.FromRgb(43, 114, 101), "minecraft:warped_nylium" },
{ Color.FromRgb(43, 104, 99), "minecraft:warped_planks" },
{ Color.FromRgb(53, 109, 110), "minecraft:warped_stem" },
{ Color.FromRgb(22, 119, 121), "minecraft:warped_wart_block" },
{ Color.FromRgb(108, 153, 110), "minecraft:weathered_copper" },
{ Color.FromRgb(207, 213, 214), "minecraft:white_concrete" },
{ Color.FromRgb(225, 227, 227), "minecraft:white_concrete_powder" },
{ Color.FromRgb(209, 178, 161), "minecraft:white_terracotta" },
{ Color.FromRgb(233, 236, 236), "minecraft:white_wool" },
{ Color.FromRgb(240, 175, 21), "minecraft:yellow_concrete" },
{ Color.FromRgb(232, 199, 54), "minecraft:yellow_concrete_powder" },
{ Color.FromRgb(186, 133, 35), "minecraft:yellow_terracotta" },
    { Color.FromRgb(248, 197, 39), "minecraft:yellow_wool" }
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
            int framesPerSegment = (int)(frameRate * 5); // Frames in 5 seconds
            int frameWidth = (int)capture.Get(VideoCaptureProperties.FrameWidth);
            int frameHeight = (int)capture.Get(VideoCaptureProperties.FrameHeight);

            // Calculate dimensions And The Quality(1/10 of normal video size)
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
                if (globalY > yMax) // When exceeding vertical bounds
                {
                    globalY = yMin; // Reset to the minimum Y
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
                    //For some reson The x goes more than it should (exacly 4/3) 
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

