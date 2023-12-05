using System.Device.Gpio;
using System.Device.Gpio.Drivers;
using GHIElectronics.Endpoint.Pins;
using GHIElectronics.Endpoint.Devices.Display;
using SkiaSharp;
using EndpointDisplayTest.Properties;
using GHIElectronics.Endpoint.Devices.Adc;


var gpioDriver = new LibGpiodDriver((int)STM32MP1.Port.D);
var gpioController = new GpioController(PinNumberingScheme.Logical, gpioDriver);
gpioController.OpenPin(14, PinMode.Output);
gpioController.Write(14, PinValue.High); // low is on

SKBitmap bitmap = new SKBitmap(480, 272, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
bitmap.Erase(SKColors.Transparent);

 var configuration = new FBDisplay.ParallelConfiguration(){
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


///Rotary Logo

var adcController = new AdcController(STM32MP1.Adc.Pin.ANA1);

var x = 0;
var y = 100;

var imageWidth = 100;
var imageHeight = 67;


while (true)
{

    var v = adcController.Read();

    var v1 = (v * 4 / 6553);

    x = (int)v1 * 10;

    Console.WriteLine(x.ToString());

    Thread.Sleep(10);

    using (var canvas = new SKCanvas(bitmap))
    {
        canvas.DrawColor(SKColors.Black);
        canvas.Clear(SKColors.Black); //same thing but also erases anything else on the canvas first
        var img = Resources.logo;
        var info = new SKImageInfo(imageWidth, imageHeight);
        var sk_img = SKBitmap.Decode(img, info);
        canvas.DrawBitmap(sk_img, x, y);



        // Draw circle
        using (SKPaint paint = new SKPaint())
        {
            paint.Color = SKColors.Blue;
            paint.IsAntialias = true;
            paint.StrokeWidth = 15;
            paint.Style = SKPaintStyle.Stroke;
            canvas.DrawCircle(x, y, 30, paint); //arguments are x position, y position, radius, and paint
        }

        // Draw Line

        float[] intervals = [ 10, 20, 10, 20];
        using (SKPaint paint3 = new SKPaint())
        {
            paint3.Color = SKColors.Red;
            paint3.IsAntialias = true;
            paint3.StrokeWidth = 10;
            paint3.Style = SKPaintStyle.Stroke;
            paint3.StrokeCap = SKStrokeCap.Round;
            paint3.PathEffect = SKPathEffect.CreateDash(intervals, 25);

            // Create linear gradient from upper-left to lower-right
            paint3.Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                new SKPoint(300, 200),
                new SKColor[] { SKColors.Red, SKColors.Blue },
                new float[] { 0, 1 },
                SKShaderTileMode.Repeat);




            canvas.DrawLine(0, 0, 400, 200, paint3); 
            
        }

        // Draw text
        using (SKPaint paint2 = new SKPaint())
        {

            paint2.Color = SKColors.Yellow;
            paint2.IsAntialias = true;
            paint2.StrokeWidth = 2;
            paint2.Style = SKPaintStyle.Stroke;


            SKFont sKFont = new SKFont();

            sKFont.Size = 22;


            SKTextBlob textBlob = SKTextBlob.Create("I am endpoint yellow", sKFont);

            canvas.DrawText(textBlob, 50, 100, paint2);
        }

        var data = bitmap.Copy(SKColorType.Rgb565).Bytes;
        displayController.Flush(data);
        Thread.Sleep(1);
    }
}





 

