using Datafeel;
using FluentModbus;
/**
 * Sample Project showcasing the use of the DotProps when wanting to istruct DataFeel dots using Write()
 * In this sample the Dot LEDs are in GlobalManual mode, and their RGBs are all set to the same value that changes every 100 MS.
 * This is then packaged in to a DotPropsJson, and sent to the Dot using the Write() call.
 */


var manager = new DotManager();
manager.Connect(1);

var props = new DotPropsWritable(1) // Vessel for our WriteCommand
{
    //Address = 1,
    LedMode = LedModes.GlobalManual,
    GlobalLed = new RgbLed()
    {
        Red = 0,
        Green = 0,
        Blue = 0
    },
};

Console.WriteLine("Press any key to stop");
var random = new Random();
while (!Console.KeyAvailable)
{
    // Set Global RGB values
    props.GlobalLed = new RgbLed()
    {
        Red = (byte)random.Next(0, 50),
        Green = (byte)random.Next(0, 50),
        Blue = (byte)random.Next(0, 50)
    };
    // Send the WriteCommand to the Dot
    await manager.Write(props);
    await Task.Delay(100);
}

await manager.Disconnect();