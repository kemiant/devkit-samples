using Datafeel;
using Datafeel.NET.Serial;
using Datafeel.NET.BLE;

var manager = new DotManagerConfiguration()
    .AddDot<Dot_63x_xxx>(1)
    .AddDot<Dot_63x_xxx>(2)
    .AddDot<Dot_63x_xxx>(3)
    .AddDot<Dot_63x_xxx>(4)
    .CreateDotManager();

var dots = new List<DotPropsWritable>()
{
    new DotPropsWritable() { Address = 1, LedMode = LedModes.GlobalManual, GlobalLed = new() },
    new DotPropsWritable() { Address = 2, LedMode = LedModes.GlobalManual, GlobalLed = new() },
    new DotPropsWritable() { Address = 3, LedMode = LedModes.GlobalManual, GlobalLed = new() },
    new DotPropsWritable() { Address = 4, LedMode = LedModes.GlobalManual, GlobalLed = new() },
};

try
{
    using var cts = new CancellationTokenSource(10000);
        var serialClient = new DatafeelModbusClientConfiguration()
        .UseWindowsSerialPortTransceiver()
        //.UseSerialPort("COM3") // Uncomment this line to specify the serial port by name
        .CreateClient();
    var result = await manager.Start(serialClient, cts.Token);
    if (result)
    {
        Console.WriteLine("Started");
    }
    else
    {
        Console.WriteLine("Failed to start");
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

var random = new Random();
while (true)
{
    var delay = Task.Delay(1000);
    foreach (var d in dots)
    {
        d.LedMode = LedModes.GlobalManual;
        d.GlobalLed.Red = (byte)random.Next(0, 255);
        d.GlobalLed.Green = (byte)random.Next(0, 255);
        d.GlobalLed.Blue = (byte)random.Next(0, 255);

        try
        {
            await manager.Write(d);
            var result = await manager.Read(d);
            Console.WriteLine($"Skin Temperature:     {result.SkinTemperature}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    await delay;
}
