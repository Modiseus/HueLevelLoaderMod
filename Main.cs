using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityModManagerNet;

namespace LevelLoaderMod
{
    public class Main
    {

        private static Vector2 scrollPosition;


        public static void Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnFixedGUI = FixedGUI;
        }


        private static void FixedGUI(UnityModManager.ModEntry modEntry)
        {
            

            GUILayout.BeginVertical();

            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.black);
            texture.Apply();
            GUIStyle style = new GUIStyle();

            style.normal.background = texture;
            style.normal.textColor = Color.white;

            GUILayout.Label("Level Loader Mod Enabled", style);

            GameManager gm = GameManager.instance;
            if (gm == null)
            {
                GUILayout.EndVertical();
                return;
            }
            PlayerNew player = gm.Player;
            if(player == null)
            {
                GUILayout.EndVertical();
                return;
            }


            GUILayout.BeginHorizontal();

            GUILevels(250, 500);

            GUILayout.BeginVertical();

            if (GUILayout.Button("Unlock Colors"))
            {
                UnlockColors();
            }

            if (GUILayout.Button("Unlock Map"))
            {
                UnlockMap();
            }

            if (GUILayout.Button("Unlock Events"))
            {
                UnlockEvents();
            }

            if(GUILayout.Button("Complete Tutorials"))
            {
                CompleteTutorials();
            }

            if (GUILayout.Button("Prepare For NG+"))
            {
                Singleton<SaveLoadManager>.Instance.NewGame();
                UnlockColors();
                UnlockMap();
                UnlockEvents();
                CompleteTutorials();
                LoadLevel("OldLadyHouse");
            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();


        }

        private static void UnlockColors()
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

        private static void UnlockMap()
        {
            foreach (Regions region in Enum.GetValues(typeof(Regions)))
            {
                Singleton<SaveLoadManager>.Instance.UnlockRegion(region);
            }
        }

        private static void UnlockEvents()
        {
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.EnteredVillage);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.FreedMiner);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.DrGreySpeak);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.DrGreyDisappear);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.SpokenToMiner);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.CanUseColourWheel);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.SpokenToLighthouseKeeper);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.Voiceover1);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.TurnOnLighthouse);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.VOIntro);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.ColourwheelTutorial);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.DrGreyVillageLetter);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.DrGreyOrange);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.DrGreyPurple);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.DrGreyPink);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.DrGreyRed);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.DrGreyBlue);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.DrGreyYellow);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.DrGreyLime);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.DrGreyMountains);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.DrGreyRoofTalk);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.DrGreyUniStart);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.DrGreyLastLevel);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.DrGreyRoofFall);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.GameComplete);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.GameCompleteGotOnBoat);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.GameCompleteReturnedToHouse);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.GameCompleteDream);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.CreditsRolled);
            Singleton<SaveLoadManager>.Instance.TriggerEvent(EventTrigger.PostCreditsEnteredVillage);
        }

        private static void CompleteTutorials()
        {
            Singleton<SaveLoadManager>.Instance.CompleteTutorial(TutorialType.ColourWheel);
            Singleton<SaveLoadManager>.Instance.CompleteTutorial(TutorialType.EnterDoor);
            Singleton<SaveLoadManager>.Instance.CompleteTutorial(TutorialType.JumpDown);
            Singleton<SaveLoadManager>.Instance.CompleteTutorial(TutorialType.Pull);
            Singleton<SaveLoadManager>.Instance.CompleteTutorial(TutorialType.Talk);                       
        }

        private static void LoadLevel(String sceneName)
        {
            GameManager.instance.HueSceneManager.SaveSceneInfo(Vector3.zero, Vector3.zero, -1, Vector2.zero);
            UnityEngine.Object.Destroy(GameManager.instance.Player.gameObject);
            Singleton<SaveLoadManager>.Instance.Checkpoint(sceneName, -1, Vector3.zero);
            LoadingComponent.Instance.LoadNextLevel();
        }

        private static void GUILevels(int width, int height)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width), GUILayout.Height(height));

            var levelList = GameManager.instance.GetLevelNames();
            foreach (string name in levelList)
            {
                //This level crashes the game because it does not exist.
                if(name == "LocalisationTestScene")
                {
                    continue;
                }


                if (GUILayout.Button(name))
                {
                    LoadLevel(name);
                }

            }

            GUILayout.EndScrollView();

        }



    }
}
