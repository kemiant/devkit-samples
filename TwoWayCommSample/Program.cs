using Datafeel;

/**
 * Sample Project showcasing receiving sensor data from the DataFeel dot and Sending in commands based on the read value.
 * In this sample the Dot LEDs are in GlobalManual mode, and their RGBs are set based on the Skin temprature reading from the dot.
 * the set RGB value is set on a gradient between Blue (MinTemp) to Red (maxTemp).
 * The Thermal unit of the Dot starts in the coldest intensity until the Minimum temprature is reached when it switches to Hottest intensity until maxTemp is reached.
 * It continously goes back and forth between hot and cold until the program is ended where it sets the Dot to be off.
 */


var manager = new DotManager();
manager.Connect(1);

var props = new DotPropsWritable(1)
{
    LedMode = LedMode.GlobalManual,
    GlobalLed = new RgbLed()
    {
        Red = 0,
        Green = 0,
        Blue = 0
    },
    ThermalMode = ThermalMode.OpenLoop,
    ThermalIntensity = 1                    //Start on cold 
};

float CurrentTemp = 25.0f;
float maxTemp = 40.0f;
float minTemp = 20.0f;

Console.WriteLine("Press any key to stop");
while (!Console.KeyAvailable)
{
    // read all sensor data
    var resultsProps = await manager.SendReadCommand(props);
    CurrentTemp = (float)resultsProps.SkinTemperature;

    Console.WriteLine(resultsProps.SkinTemperature);
    if (CurrentTemp > maxTemp)
    {
        props.ThermalIntensity = 1;         // turn it cold
    }
    else if (CurrentTemp < minTemp)
    {
        props.ThermalIntensity = -1;        // turn it hot
    }

    // set the color of all LEDs (global) based on the temperature reported by the dot
    (byte red, byte green, byte blue) = TemperatureToRGB(CurrentTemp, minTemp, maxTemp);
    props.GlobalLed.Red = red;
    props.GlobalLed.Green = green;
    props.GlobalLed.Blue = blue;

    await manager.SendWriteCommand(props);
    await Task.Delay(50);
}

// Turn off the Dot settings
await manager.SendWriteCommand(new DotPropsWritableOff(props.Address));
manager.Dispose();



// --- helper function --- 
static (byte, byte, byte) TemperatureToRGB(float temp, float minTemp, float maxTemp)
{
    // Clamp the temperature to be within the min and max range
    temp = Math.Max(minTemp, Math.Min(maxTemp, temp));

    // Calculate the interpolation factor (0 to 1)
    float factor = (temp - minTemp) / (maxTemp - minTemp);

    // Define RGB values for blue and red
    (byte r, byte g, byte b) blue = (0, 0, 255);
    (byte r, byte g, byte b) red = (255, 0, 0);

    // Calculate the interpolated RGB values
    byte r = (byte)(blue.r + factor * (red.r - blue.r));
    byte g = (byte)(blue.g + factor * (red.g - blue.g));
    byte b = (byte)(blue.b + factor * (red.b - blue.b));

    return (r, g, b);
}
