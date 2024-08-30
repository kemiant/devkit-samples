using Datafeel;
using FluentModbus;
using System;

/**
 * Sample Project showcasing the use of the DotProps when wanting to istruct DataFeel dots using SendWriteCommand()
 * In this sample the Dot LEDs are in GlobalManual mode, and their RGBs are all set to the same value that changes every 100 MS.
 * This is then packaged in to a DotPropsJson, and sent to the Dot using the SendWriteCommand() call.
 */


var manager = new DotManager();
manager.Connect(1);

var props = new DotPropsWritable(1) // Vessel for our WriteCommand
{
    //Address = 1,
    LedMode = LedMode.GlobalManual,
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
    props.GlobalLed.Red = (byte)random.Next(0, 50);
    props.GlobalLed.Green = (byte)random.Next(0, 50);
    props.GlobalLed.Blue = (byte)random.Next(0, 50);
    // Send the WriteCommand to the Dot
    await manager.SendWriteCommand(props);
    await Task.Delay(100);
}

manager.Dispose();