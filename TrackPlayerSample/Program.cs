using Datafeel;
using FluentModbus;
/**
 * Sample Project showcasing the the use of the JSON trackplayer, which "plays" a track that is structured within a JSON file, adhereing to the Datafeel schema.
 * The PlayTrack has 3 different input overloads: filePath, a Track object, or a StreamReader.
 * In this sample the Filepath is passed
 */



string fileName = "my-track.json";
var manager = new DotManager();
manager.Connect(1);

var trackPlayer = new TrackPlayer(manager);
string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "\\Track\\", fileName);
await trackPlayer.PlayTrack(path);

