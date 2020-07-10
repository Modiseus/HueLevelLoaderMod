using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityModManagerNet;

namespace LevelLoader
{
    public class Main
    {

        private static bool visible;
        private static Vector2 scrollPosition;


        public static void Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnFixedGUI = FixedGUI;
        }


        private static void FixedGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginVertical();
            
            GUILayout.Label("Level Loader by Modiseus");

            GUILayout.BeginHorizontal();
            
            GUILevels(250, 500);

            if(GUILayout.Button("Unlock All Colors"))
            {
                Singleton<SaveLoadManager>.Instance.SetColourUnlocked(HueColour.HueColorNames.Aqua, true);
                Singleton<SaveLoadManager>.Instance.SetColourUnlocked(HueColour.HueColorNames.Blue, true);
                Singleton<SaveLoadManager>.Instance.SetColourUnlocked(HueColour.HueColorNames.Lime, true);
                Singleton<SaveLoadManager>.Instance.SetColourUnlocked(HueColour.HueColorNames.Orange, true);
                Singleton<SaveLoadManager>.Instance.SetColourUnlocked(HueColour.HueColorNames.Pink, true);
                Singleton<SaveLoadManager>.Instance.SetColourUnlocked(HueColour.HueColorNames.Purple, true);
                Singleton<SaveLoadManager>.Instance.SetColourUnlocked(HueColour.HueColorNames.Red, true);
                Singleton<SaveLoadManager>.Instance.SetColourUnlocked(HueColour.HueColorNames.Yellow, true);
            }

            if(GUILayout.Button("Unlock Map"))
            {
                foreach (Regions region in Enum.GetValues(typeof(Regions)))
                {
                    Singleton<SaveLoadManager>.Instance.UnlockRegion(region);
                }
            }

            if(GUILayout.Button("Unlock Events"))
            {
                foreach (EventTrigger eventTrigger in Enum.GetValues(typeof(EventTrigger)))
                {
                    
                    Singleton<SaveLoadManager>.Instance.TriggerEvent(eventTrigger);
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private static void GUILevels(int width, int height)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width), GUILayout.Height(height));

            var levelList = GameManager.instance.GetLevelNames();
            foreach (string name in levelList)
            {
                if (GUILayout.Button(name))
                {
                    String sceneName = name;


                    GameManager.instance.HueSceneManager.SaveSceneInfo(Vector3.zero, Vector3.zero, -1, Vector2.zero);
                    UnityEngine.Object.Destroy(GameManager.instance.Player.gameObject);
                    Singleton<SaveLoadManager>.Instance.Checkpoint(sceneName, -1, Vector3.zero);
                    LoadingComponent.Instance.LoadNextLevel();
                }

            }

            GUILayout.EndScrollView();

        }
    }
}
