# Movie to Pixel in Minecraft

## Project Description
Convert videos into pixel-art movies within Minecraft using pure commands, without mods or plugins.

## Features
- Convert video to Minecraft block pixel art
- Generate Minecraft function commands
- Supports MP4 and AVI formats
- Customizable resolution and segment duration

## Prerequisites
- Windows with .NET Framework
- Visual Studio (recommended)
- Minecraft Java Edition

## Dependencies
- OpenCvSharp
- WPF Framework

## How to Use
1. Run the application
2. Select output directory
3. Choose video file
4. Process video
5. Import generated `.mcfunction` files in datapack one by one and run them. (/function {NameOfdatapack}segment{i}) 
Note: Loading All of them together will make your minecraft crash
6. Setup armorstand with Tag VideoPlayerArmorStand (/Tag add @e[type=armorstand] VideoPlayerArmorStand)
7. Put command block to add 1 to "Timer" scorreboard in minecraft (scoreboard players add @e Timer 1)
8. Add All the videoplayer_segment{i} To the datapack you used and then run multiple chain command blocks in to eachother that uses the functions 

## Customization
### Adjust Video Resolution
Modify divisor in `ProcessVideo` method:
```csharp
xBlocks = frameWidth / 10;  // Change 10 to desired divisor
zBlocks = frameHeight / 10; // Change 10 to desired divisor
```

### Change Segment Duration
Adjust frames per segment in `ProcessVideo` method:
```csharp
int framesPerSegment = (int)(frameRate * 5); // Change 5 to desired duration
```

## Limitations
- Performance depends on video complexity
- Block color matching is approximate
- Requires manual Minecraft command import
- If the blocks go out of range of /clone it will not work
