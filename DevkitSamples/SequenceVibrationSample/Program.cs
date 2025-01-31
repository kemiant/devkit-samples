using Datafeel;
using Datafeel.NET.Serial;
using Datafeel.NET.BLE;
using System.Reflection;

var manager = new DotManagerConfiguration()
    .AddDot(1)
    .AddDot(2)
    .AddDot(3)
    .AddDot(4)
    .CreateDotManager();

var client = new DatafeelModbusClientConfiguration()
    .UseNetBleTransceiver()
    .CreateClient();

var trackPlayer = new TrackPlayer(manager);

await manager.Start(client);

string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Tracks\my-track.json");
await trackPlayer.PlayTrack(path);



