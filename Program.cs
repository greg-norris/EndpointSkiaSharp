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

var adcController = new AdcController(STM32MP1.Adc.Pin.ANA1);

var x = 0;
var y = 100;
//var dirx = 1;
//var diry = 1;
var imageWidth = 100;
var imageHeight = 67;

////Bouncing Logo

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
        var info = new SKImageInfo(imageWidth, imageHeight); // width and height of rect
        var sk_img = SKBitmap.Decode(img, info);
        canvas.DrawBitmap(sk_img, x, y);
    }

    var data = bitmap.Copy(SKColorType.Rgb565).Bytes;

    displayController.Flush(data);

    Thread.Sleep(1);
}



