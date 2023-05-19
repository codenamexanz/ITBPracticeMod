using System.IO;
using MelonLoader;
using UnityEngine;
using static UnityEngine.Object;
using Il2Cpp;
using HarmonyLib;
using System.Reflection;
using static Il2Cpp.FossilCase;

namespace TourneyMod
{
    /*
     * TourneyMod - Ensures
     */
    public class TourneyMod : MelonMod
    {
        private string listOfMods = "Active Mods \n";

        public override void OnInitializeMelon()
        {
            foreach (MelonMod mod in RegisteredMelons)
            {
                listOfMods = listOfMods + mod.Info.Name + " by " + mod.Info.Author + "\n";
            }
        }
        private void DrawRegisteredMods()
        {
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.UpperRight;
            style.normal.textColor = Color.white;

            GUI.Label(new Rect(Screen.width - 500 - 10, 100, 500, 100), listOfMods, style);
        }

        private static void vhsHandsRuleDetected()
        {
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.red;
            style.fontSize = 120;

            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "BAD VHS/HANDS DETECTED", style);
        }

        public static async Task DetectClockCassette()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            GameObject[] cassettes = GameObject.FindGameObjectsWithTag("Item")
                .Where(obj => obj.name.StartsWith("Cassete") && obj.transform.parent.name == "CASSETS" && obj.transform.rotation == Quaternion.Euler(0, 0, 0))
                .ToArray();

            if (cassettes.Length > 0) // Check if the array has any elements
            {
                MelonEvents.OnGUI.Subscribe(vhsHandsRuleDetected, 100);
            }
        }

        public static async Task DetectPliersHands()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            GameObject[] clockHands = GameObject.FindGameObjectsWithTag("Item")
                .Where(obj => obj.name.StartsWith("ClockHandles") && obj.transform.localPosition == new Vector3(-64.24694f, 0.4681168f, -6.436426f))
                .ToArray();

            if (clockHands.Length > 0) // Check if the array has any elements
            {
                MelonEvents.OnGUI.Subscribe(vhsHandsRuleDetected, 100);
            }
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName == "MainMenu")
            {
                MelonEvents.OnGUI.Subscribe(DrawRegisteredMods, 100);
                MelonEvents.OnGUI.Unsubscribe(vhsHandsRuleDetected);
            }

            if (sceneName == "MainLevel")
            {
                MelonEvents.OnGUI.Unsubscribe(DrawRegisteredMods);
                DetectClockCassette();
                DetectPliersHands();
            }
        }
    }
}