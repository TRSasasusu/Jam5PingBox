﻿using System.Reflection;
using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;

namespace Jam5PingBox
{
    public class Jam5PingBox : ModBehaviour
    {
        public static Jam5PingBox Instance;
        public INewHorizons NewHorizons;

        public static void Log(string text, MessageType messageType = MessageType.Message) {
            Instance.ModHelper.Console.WriteLine(text, messageType);
        }

        public void Awake()
        {
            Instance = this;
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }

        public void Start()
        {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"Mod {nameof(Jam5PingBox)} is loaded!", MessageType.Success);

            // Get the New Horizons API and load configs
            NewHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            NewHorizons.LoadConfigs(this);

            new Harmony("orclecle.Jam5PingBox").PatchAll(Assembly.GetExecutingAssembly());

            // Example of accessing game code.
            //OnCompleteSceneLoad(OWScene.TitleScreen, OWScene.TitleScreen); // We start on title screen
            //LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;
            NewHorizons.GetStarSystemLoadedEvent().AddListener(loadScene => {
                ModHelper.Console.WriteLine($"Loaded into {loadScene}!", MessageType.Success);
                var objectModifier = new ObjectModifier();
            });
        }
    }

}
