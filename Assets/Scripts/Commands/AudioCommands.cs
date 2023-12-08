using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using poetools.Console;
using poetools.Console.Commands;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;
namespace Commands
{

    [CreateAssetMenu]
    public class AudioCommands : Command
    {
        public override string Name => "audio";

        public override void Execute(string[] args, RuntimeConsole console)
        {
            if (args.Length == 0)
                return;

            Ltg8AudioSystem audio = Ltg8.Audio;
            
            switch (args[0])
            {
                case "background":
                    if (args.Length == 1)
                    {
                        foreach (KeyValuePair<string, EventInstance> kvp in audio.PersistentAudio)
                            console.Log("audio", kvp.Key);
                    }
                    else if (args.Length == 5 && args[1] == "volume")
                    {
                        audio.AnimateVolume(audio.PersistentAudio[args[2]], float.Parse(args[3]), float.Parse(args[4]));
                    }
                    else if (args.Length == 4 && args[1] == "play")
                    {
                        EventInstance instance = RuntimeManager.CreateInstance(args[2]);
                        instance.start();
                        audio.PersistentAudio.Add(args[3], instance);
                    }
                    else if (args.Length == 3 && args[1] == "stop")
                    {
                        audio.PersistentAudio[args[2]].stop(STOP_MODE.ALLOWFADEOUT);
                        audio.PersistentAudio[args[2]].release();
                        audio.PersistentAudio.Remove(args[2]);
                    }
                    break;
            }
        }
    }
}
