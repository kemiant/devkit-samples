// See https://aka.ms/new-console-template for more information
using Datafeel;
using FluentModbus;

var client = ModbusClientProvider.CreateRtuSerialClient();
client.Connect();

// Change depending on your hardware version
var dot = new DotV6(1);

// LED Settings
dot.ClearLeds();
dot.LedMode = LedModes.GlobalManual;
dot.GlobalLed = new RgbLed(50, 50, 50);
dot.IndividualManualLeds[0] = new RgbLed();
dot.IndividualManualLeds[1] = new RgbLed();
dot.IndividualManualLeds[2] = new RgbLed();
dot.IndividualManualLeds[3] = new RgbLed();
dot.IndividualManualLeds[4] = new RgbLed();
dot.IndividualManualLeds[5] = new RgbLed();
dot.IndividualManualLeds[6] = new RgbLed();
dot.IndividualManualLeds[7] = new RgbLed();

// Thermal Settings
dot.ThermalMode = ThermalModes.Off;
dot.ThermalIntensity = .25f;

// Vibration Settings
dot.VibrationMode = VibrationModes.Sequence;
dot.VibrationIntensity = 127;
dot.VibrationSequence[0].Waveforms = VibrationWaveforms.StrongClickP100;
dot.VibrationSequence[1].Waveforms = VibrationWaveforms.TransitionRampUpLongSmooth1P0ToP50;
dot.VibrationSequence[2].Waveforms = VibrationWaveforms.StrongClickP100;
dot.VibrationSequence[3].Waveforms = VibrationWaveforms.TransitionRampUpLongSmooth1P0ToP50;
dot.VibrationSequence[4].Waveforms = VibrationWaveforms.EndSequence;

await Task.Run(async () =>
{
    var random = new Random();
    while (true)
    {
        dot.GlobalLed = new RgbLed()
        {
            Red = (byte)random.Next(0, 255),
            Green = (byte)random.Next(0, 255),
            Blue = (byte)random.Next(0, 255)
        };
        dot.VibrationGo = true;
        var cts = new CancellationTokenSource(100);
        await dot.WriteAllSettings(client, cts.Token);
        await Task.Delay(1000);
    }
});
