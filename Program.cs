using System.Device.Gpio;
using System.Device.Gpio.Drivers;
using GHIElectronics.Endpoint.Pins;
using GHIElectronics.Endpoint.Devices.Display;
using SkiaSharp;
using EndpointDisplayTest.Properties;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

var gpioDriver = new LibGpiodDriver((int)STM32MP1.Port.D);
var gpioController = new GpioController(PinNumberingScheme.Logical, gpioDriver);
gpioController.OpenPin(14, PinMode.Output);
gpioController.Write(14, PinValue.High); // low is on

var screenWidth = 480;
var screenHeight = 272;

SKBitmap bitmap = new SKBitmap(screenWidth, screenHeight, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
bitmap.Erase(SKColors.Transparent);

var configuration = new FBDisplay.ParallelConfiguration()
{
    Clock = 10000,
    Width = 480,
    Hsync_start = 480 + 2,
    Hsync_end = 480 + 2 + 41,
    Htotal = 480 + 2 + 41 + 2,
    Height = 272,
    Vsync_start = 272 + 2,
    Vsync_end = 272 + 2 + 10,
    Vtotal = 272 + 2 + 10 + 2,

};
var fbDisplay = new FBDisplay(configuration);
var displayController = new DisplayController(fbDisplay);


////Bouncing Logo

//var dirx = 1;
//var diry = 1;

//var x = 0;
//var y = 100;

//while (true) {
//    x += 10 * dirx;
//    y += 5 * diry;

//    if (x >= fbDisplay.Width-100) { 
//        dirx = -1; }
//    else if (x <= 0)
//        dirx = 1;

//    if (y >= fbDisplay.Height-67){
//        diry = -1;
//    }
//    else if (y <= 0)
//        diry = 1;

//    using (var canvas = new SKCanvas(bitmap)){
//        canvas.DrawColor(SKColors.Black);
//        canvas.Clear(SKColors.Black); //same thing but also erases anything else on the canvas first
//        var img = Resources.logo;
//        var info = new SKImageInfo(imageWidth, imageHeight); // width and height of rect
//        var sk_img = SKBitmap.Decode(img, info);
//        canvas.DrawBitmap(sk_img, x, y);
//    }

//    var data = bitmap.Copy(SKColorType.Rgb565).Bytes;

//    displayController.Flush(data);

//    Thread.Sleep(1);
//}


/////Rotary controlled logo

//var adcController = new AdcController(STM32MP1.Adc.Pin.ANA1);

//var x = 0;
//var y = 100;

//var imageWidth = 100;
//var imageHeight = 67;

//while (true)
//{
//    // Read Rotary Module to move Logo
//    var v = adcController.Read();
//    var v1 = (v * 4 / 6553);
//    x = (int)v1 * 10;
//    Console.WriteLine(x.ToString());
//    Thread.Sleep(10);


///Drawing various things on the screen

//var imageWidth = 100;
//var imageHeight = 67;

//while (true)
//{

//    Console.WriteLine("Running");
//    //Initialize Screen
//    using (var screen = new SKCanvas(bitmap))
//    {
//        //Create Black Screen 
//        screen.DrawColor(SKColors.Black);
//        screen.Clear(SKColors.Black); //same thing but also erases anything else on the canvas first

//        // Draw Logo from Resources
//        var logo = Resources.logo;
//        var info = new SKImageInfo(imageWidth, imageHeight);
//        var sk_img = SKBitmap.Decode(logo, info);
//        screen.DrawBitmap(sk_img, 0, 200);

//        // Draw circle
//        using (SKPaint circle = new SKPaint())
//        {
//            circle.Color = SKColors.Blue;
//            circle.IsAntialias = true;
//            circle.StrokeWidth = 15;
//            circle.Style = SKPaintStyle.Stroke;
//            screen.DrawCircle(410, 220, 30, circle); //arguments are x position, y position, radius, and paint
//        }

//        // Draw Oval
//        using (SKPaint oval = new SKPaint())
//        {
//            oval.Style = SKPaintStyle.Stroke;
//            oval.Color = SKColors.Blue;
//            oval.StrokeWidth = 10;
//            screen.DrawOval(300, 20, 60, 10, oval);

//            oval.Style = SKPaintStyle.Fill;
//            oval.Color = SKColors.SkyBlue;
//            screen.DrawOval(300, 20, 60, 10, oval);

//        }

//        // Draw Line
//        float[] intervals = [10, 20, 10, 20, 5, 40,];//sets the dash intervals
//        using (SKPaint line = new SKPaint())
//        {
//            line.Color = SKColors.Red;
//            line.IsAntialias = true;
//            line.StrokeWidth = 20;
//            line.Style = SKPaintStyle.Stroke;

//            //Rounds the ends of the line
//            line.StrokeCap = SKStrokeCap.Round;

//            //Creates dashes in line based on intervals array
//            line.PathEffect = SKPathEffect.CreateDash(intervals, 25);

//            // Create linear gradient from upper-left to lower-right
//            line.Shader = SKShader.CreateLinearGradient(
//                new SKPoint(0, 0),
//                new SKPoint(screenWidth, screenHeight),
//                new SKColor[] { SKColors.Red, SKColors.Blue },
//                new float[] { 0, 1 },
//                SKShaderTileMode.Repeat);

//            screen.DrawLine(0, 0, 400, 200, line);
//        }

//        //Using SkiaTypeface

//        byte[] fontfile = Resources.OldeEnglish;
//        Stream stream = new MemoryStream(fontfile);

//        using (SKPaint textPaint = new SKPaint())
//        using (SKTypeface tf = SKTypeface.FromStream(stream))
//        {
//            textPaint.Color = SKColors.White;
//            textPaint.IsAntialias = true;
//            textPaint.StrokeWidth = 2;
//            textPaint.Style = SKPaintStyle.Stroke;

//            //SKFont Text - 
//            SKFont font = new SKFont();
//            font.Size = 40;
//            font.ScaleX = 2;
//            font.Typeface = tf;
//            SKTextBlob textBlob = SKTextBlob.Create("D", font);
//            screen.DrawText(textBlob, 10, 150, textPaint);

//        }

//        //Basic Text
//        // Draw text
//        using (SKPaint text = new SKPaint())
//        {
//            text.Color = SKColors.Yellow;
//            text.IsAntialias = true;
//            text.StrokeWidth = 2;
//            text.Style = SKPaintStyle.Stroke;
//            screen.DrawText("Hello World", 20, 20, text);
//        }

//        // Draw text
//        using (SKPaint text = new SKPaint())
//        {
//            text.Color = SKColors.Yellow;
//            text.IsAntialias = true;
//            text.StrokeWidth = 2;
//            text.Style = SKPaintStyle.Stroke;

//            //SKFont Text - 
//            SKFont font = new SKFont();
//            font.Size = 22;
//            font.ScaleX = 2;
//            SKTextBlob textBlob = SKTextBlob.Create("I am endpoint yellow", font);
//            screen.DrawText(textBlob, 50, 100, text);
//        }

//        // Character Outlines
//        using (SKPaint textPaint = new SKPaint())
//        {
//            // Set Style for the character outlines
//            textPaint.Style = SKPaintStyle.Stroke;

//            // Set TextSize 100x100
//            textPaint.TextSize = Math.Min(100, 100);

//            // Measure the text
//            SKRect textBounds = new SKRect();
//            textPaint.MeasureText("@", ref textBounds);

//            // Coordinates to center text on screen
//            float xText = screenWidth / 2 - textBounds.MidX;
//            float yText = screenHeight / 2 - textBounds.MidY;

//            // Get the path for the character outlines
//            using (SKPath textPath = textPaint.GetTextPath("@", xText, yText))
//            {
//                // Create a new path for the outlines of the path
//                using (SKPath outlinePath = new SKPath())
//                {
//                    // Convert the path to the outlines of the stroked path
//                    textPaint.StrokeWidth = 1;
//                    textPaint.GetFillPath(textPath, outlinePath);

//                    // Stroke that new path
//                    using (SKPaint outlinePaint = new SKPaint())
//                    {
//                        outlinePaint.Style = SKPaintStyle.Stroke;
//                        outlinePaint.StrokeWidth = 1;
//                        outlinePaint.Color = SKColors.Red;

//                        screen.DrawPath(outlinePath, outlinePaint);
//                    }
//                }
//            }

//            //Text along a path
//            const string text = "SKIASHARP library ENDPOINT uses ";

//            using (SKPath circularPath = new SKPath())
//            {
//                float radius = 0.35f * Math.Min(screenWidth, screenHeight);
//                circularPath.AddCircle(screenWidth / 2, screenHeight / 2, radius);

//                using (SKPaint textPaint2 = new SKPaint())
//                {
//                    textPaint2.TextSize = 100;
//                    float textWidth = textPaint2.MeasureText(text);
//                    textPaint.TextSize *= 2 * 3.14f * radius / textWidth;
//                    textPaint.Color = SKColors.Green;

//                    screen.DrawTextOnPath(text, circularPath, 0, 0, textPaint);
//                }
//            }

//            // Draw Italic text
//            using (SKPaint italicText = new SKPaint())
//            {
//                SKFontStyle fontStyle = new SKFontStyle();

//                italicText.Color = SKColors.Yellow;
//                italicText.IsAntialias = true;
//                italicText.StrokeWidth = 2;
//                italicText.Style = SKPaintStyle.Stroke;

//                SKFont font2 = new SKFont(SKTypeface.Default, 12, 1, 0);

//                font2.Size = 22;
//                SKTextBlob textBlob = SKTextBlob.Create("ItalicFonts", font2);

//                screen.DrawText(textBlob, 200, 200, italicText);
//            }

//            // Flush to screen
//            var data = bitmap.Copy(SKColorType.Rgb565).Bytes;
//            displayController.Flush(data);
//            Thread.Sleep(1);
//        }
//    }
//}


//Convert Animated gif to bmp sequence

var img = Resources.endpointFireworks;

SKBitmap[] sk_imgs = null;
var framecnt = 0;

while (true)
{
    using (var stream = new MemoryStream(img))
    {
        using (var canvas = new SKCanvas(bitmap))
        {
            // Draw Logo from Resources

            using (SKManagedStream skStream = new SKManagedStream(stream))
            {
                using (SKCodec codec = SKCodec.Create(skStream))
                {
                    //Get frame count and allocate bitmaps
                    int frameCount = codec.FrameCount;
                    var bitmaps = new SKBitmap[frameCount];
                    var durations = new int[frameCount];
                    var accumulatedDurations = new int[frameCount];

                    if (sk_imgs == null)
                    {
                        sk_imgs = new SKBitmap[frameCount];
                    }

                    //Note: There's also a RepetitionCount property of SKCodec not used here

                    //Loop through the frames
                    for (int frame = 0; frame < frameCount; frame++)
                    {
                        //From the FrameInfo collection, get the duration of each frame
                        durations[frame] = codec.FrameInfo[frame].Duration;

                        // Create a full - color bitmap for each frame
                        if (sk_imgs[frame] == null)
                        {
                            var imageInfo = new SKImageInfo(codec.Info.Width, codec.Info.Height, SKImageInfo.PlatformColorType, SKAlphaType.Unpremul);
                            bitmaps[frame] = new SKBitmap(imageInfo);

                            //Get the address of the pixels in that bitmap
                            IntPtr pointer = bitmaps[frame].GetPixels();

                            //Create an SKCodecOptions value to specify the frame
                            SKCodecOptions codecOptions = new SKCodecOptions(frame);

                            //Copy pixels from the frame into the bitmap
                            codec.GetPixels(imageInfo, pointer, codecOptions);

                            var pixelArray = bitmaps[frame].Bytes;

                            //pin the managed array so that the GC doesn't move it
                            var gcHandle = GCHandle.Alloc(pixelArray, GCHandleType.Pinned);

                            // special image
                            var info = new SKImageInfo(codec.Info.Width, codec.Info.Height); // width and height of rect


                            var sk_img = SKBitmap.Decode(img, info);

                            //install the pixels with the color type of the pixel data
                            sk_img.InstallPixels(imageInfo, pixels: gcHandle.AddrOfPinnedObject(), rowBytes: imageInfo.RowBytes, ctable: null, releaseProc: delegate { gcHandle.Free(); }, context: null);

                            sk_imgs[frame] = sk_img;
                        }

                        framecnt++;

                        if (framecnt > frameCount)
                        {
                            //Create Black Screen 
                            canvas.DrawColor(SKColors.White);
                            canvas.Clear(SKColors.White);
                            canvas.DrawBitmap(sk_imgs[frame], (screenWidth - codec.Info.Width) / 2, (screenHeight - codec.Info.Height) / 2); // where it is on the screen

                            var data = bitmap.Copy(SKColorType.Rgb565).Bytes;

                            displayController.Flush(data);

                            Thread.Sleep(durations[frame]);
                        }
                        else
                        {
                            /// Show something else until image is done decoding
                        }
                    }
                }
            }

        }
    }
}

