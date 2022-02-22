using System;
using BepInEx;
using HarmonyLib;

namespace MergeSigils
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string NAME = "Merge Sigils";
        public const string GUID = "spapi.inscryption.mergesigils";
        public const string VERSION = "1.0.0";

        public void Awake()
        {
            Harmony harm = new(GUID);
            harm.PatchAll();
        }
    }
}
