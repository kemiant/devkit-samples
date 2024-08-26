using Datafeel;
/**
 * Sample Project showcasing the direct low level calls to the dot object, as opposed to using the DotManager, to send instructions to the dot.
 * In this sample the dot object and the Modbus Client are created manually and seperately from one another. 
 * The dot is set to be Global LED mode and sequence vibration mode, it then calls onto the WriteALLSetting() which writes the corresponding byte onto the modbus registers.
 * This sample demonstrates the use of the sequence mode where one queue up a selection of pre-defined vibration patterns.
 */
var client = ModbusClientProvider.CreateRtuSerialClient();
var dot = new DotV5(1);

// LED Settings
dot.ClearLeds();
dot.LedMode = LedMode.GlobalManual;
dot.GlobalLed.Green = 40;

// Thermal Settings
dot.ThermalMode = ThermalMode.Off;
dot.ThermalIntensity = 0f;

// Vibration Settings
dot.VibrationMode = VibrationMode.Sequence;
dot.VibrationIntensity = 127;
dot.VibrationSequence[0].Waveform = VibrationWaveform.StrongClickP100;
dot.VibrationSequence[1].Waveform = VibrationWaveform.TransitionRampUpLongSmooth1P0ToP50;
dot.VibrationSequence[2].Waveform = VibrationWaveform.StrongClickP100;
dot.VibrationSequence[3].Waveform = VibrationWaveform.TransitionRampUpLongSmooth1P0ToP50;
dot.VibrationSequence[4].Waveform = VibrationWaveform.EndSequence;

await Task.Run(async () =>
{
    var random = new Random();
    while (!Console.KeyAvailable)
    {
        dot.GlobalLed.Red = (byte)random.Next(0, 50);
        dot.GlobalLed.Green = (byte)random.Next(0, 50);
        dot.GlobalLed.Blue = (byte)random.Next(0, 50);
        dot.VibrationGo = true;
        await dot.WriteAllSettings(client);
        await Task.Delay(100);
    }
    // clear the LEDs
    dot.ClearLeds();
    await dot.WriteAllSettings(client);
});
