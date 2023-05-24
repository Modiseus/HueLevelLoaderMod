using InControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityModManagerNet;

namespace LevelLoaderMod
{
#if DEBUG
    [EnableReloading]
#endif
    public class Main
    {

        private static Vector2 scrollPosition;
        private static String wheelOffsetString = "";
        private static int levelSorting = 0;


        public static void Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnFixedGUI = FixedGUI;
            modEntry.OnToggle = OnToggle;

            modEntry.OnUnload = Unload;

        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool toggle)
        {
            return true;
        }

        private static bool Unload(UnityModManager.ModEntry modEntry)
        {
            return true;
        }

        private static void FixedGUI(UnityModManager.ModEntry modEntry)
        {
            if (!modEntry.Enabled)
            {
                return;
            }

            GUIStyle style = new GUIStyle();
            Texture2D texture = new Texture2D(1, 1);
            Color background = Color.black;
            background.a = 0.7f;
            texture.SetPixel(0, 0, background);
            texture.Apply();

            style.normal.background = texture;
            style.normal.textColor = Color.white;

            GUILayout.BeginVertical(style);

            GUILayout.Label("Level Loader Mod Enabled");

#if DEBUG
            System.Version version = Assembly.GetExecutingAssembly().GetName().Version;
            GUILayout.Label("Version " + version.ToString());
#endif

            GameManager gm = GameManager.instance;
            if (gm == null)
            {
                GUILayout.EndVertical();
                return;
            }
            PlayerNew player = gm.Player;
            if (player == null)
            {
                GUILayout.EndVertical();
                return;
            }

            GUILayout.BeginHorizontal();

            GUILevels(250, 500);


            GUILayout.BeginVertical();

            GUILayout.Label("Unlocks");

            if (GUILayout.Button("Unlock All Colors"))
            {
                UnlockColors();
            }

            GUIColors();

            if (GUILayout.Button("Unlock Map Regions"))
            {
                UnlockMap();
            }

            if (GUILayout.Button("Unlock Events"))
            {
                UnlockEvents();
            }

            if (GUILayout.Button("Complete Tutorials"))
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

            GUILayout.Label("Move Color Wheel Vertically:");

            GUILayout.BeginHorizontal();

            wheelOffsetString = GUILayout.TextField(wheelOffsetString);
            if (GUILayout.Button("Move"))
            {
                SetWheelOffset();
                wheelOffsetString = "";
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();


        }

        private static Boolean SetWheelOffset()
        {
            float offset;
            if (!float.TryParse(wheelOffsetString, out offset))
            {
                return false;
            }

            ColourWheelTrigger instance = ColourWheelTrigger.instance;
            if (!instance)
            {
                return false;
            }

            Vector3 newPos = instance.transform.position + Vector3.up * offset;
            instance.transform.position = newPos;

            return true;
        }

        private static void GUIColors()
        {
            ColorToggle(HueColour.HueColorNames.Aqua);
            ColorToggle(HueColour.HueColorNames.Blue);
            ColorToggle(HueColour.HueColorNames.Purple);
            ColorToggle(HueColour.HueColorNames.Pink);
            ColorToggle(HueColour.HueColorNames.Red);
            ColorToggle(HueColour.HueColorNames.Orange);
            ColorToggle(HueColour.HueColorNames.Yellow);
            ColorToggle(HueColour.HueColorNames.Lime);

        }

        private static void ColorToggle(HueColour.HueColorNames color)
        {

            SaveLoadManager slm = Singleton<SaveLoadManager>.Instance;

            Color textColor = HueColour.HueColourValue(color);

            GUIStyle style = new GUIStyle(GUI.skin.toggle);
            style.normal.textColor = textColor;
            style.onNormal.textColor = textColor;

            bool unlockedBefore = slm.IsColourUnlocked(color);
            bool unlockedAfter = GUILayout.Toggle(unlockedBefore, color.ToString(), style);
            if (unlockedBefore != unlockedAfter)
            {
                slm.SetColourUnlocked(color, unlockedAfter);
            }
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
            GUILayout.BeginVertical();

            GUILayout.Label("Levels");

            string[] sortingOptions = new string[] { "Regions", "Alphabetical" };
            int xCount = sortingOptions.Length;

            levelSorting = GUILayout.SelectionGrid(levelSorting, sortingOptions, xCount);

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width), GUILayout.Height(height));

            switch (levelSorting)
            {
                case 0:
                    AddLevelsByRegion();
                    break;
                case 1:
                    AddLevelsAlphabetically();
                    break;
                default:
                    AddLevelsByRegion();
                    break;
            }


            GUILayout.EndScrollView();

            GUILayout.Label("Current Level: " + Singleton<SaveLoadManager>.Instance.CurrentLevel);

            GUILayout.EndVertical();

        }


        private static void AddLevelsByRegion()
        {
            GUILayout.Label("Region - Village");
            AddLevelButton("IntroDream");
            AddLevelButton("Village");
            AddLevelButton("Lighthouse");
            AddLevelButton("OldLadyHouse");
            AddLevelButton("CycleHouse");
            AddLevelButton("ThinHouse");

            GUILayout.Label("Region - Caves");
            AddLevelButton("CaveMinerArea");

            GUILayout.Label("Region - Water");
            AddLevelButton("WaterEntrance");
            AddLevelButton("Waterfall");
            AddLevelButton("DropThroughColour");
            AddLevelButton("PullTute02");
            AddLevelButton("SpikeTute03");
            AddLevelButton("JumpColour");
            AddLevelButton("BoulderTutorialNew01");
            AddLevelButton("BoulderDropChase02");
            AddLevelButton("BoulderTrap02");
            AddLevelButton("PurpleFragmentRoom", HueColour.HueColourValue(HueColour.HueColorNames.Purple));
            AddLevelButton("PostPurpleCorridor");
            AddLevelButton("AlternatingColourSwitch");
            AddLevelButton("FallThroughColours");
            AddLevelButton("AlternatingColourJumps02");
            AddLevelButton("ClimbUpColours02");
            AddLevelButton("OrangeFragmentRoom", HueColour.HueColourValue(HueColour.HueColorNames.Orange));
            AddLevelButton("WaterExit");

            GUILayout.Label("Region - Fire");
            AddLevelButton("FireIntro");
            AddLevelButton("PuzzleSequence");
            AddLevelButton("KeyTutorial");
            AddLevelButton("JumpAlign");
            AddLevelButton("BoulderSwitchChase");
            AddLevelButton("HueDunnit");
            AddLevelButton("SkeletonRoom");
            AddLevelButton("AlternatingBoulders");
            AddLevelButton("PinkFragmentRoom", HueColour.HueColourValue(HueColour.HueColorNames.Pink));
            AddLevelButton("PostPinkCorridor");
            AddLevelButton("BoxSlideMaze");
            AddLevelButton("BrickMaze");
            AddLevelButton("NarrowCorridorCrates");
            AddLevelButton("BlackBoxDecoy");
            AddLevelButton("CrushOnStart");
            AddLevelButton("CrumblingRockJump");
            AddLevelButton("SlideAcrossTheGap");
            AddLevelButton("RedFragmentRoom", HueColour.HueColourValue(HueColour.HueColorNames.Red));
            AddLevelButton("PostRedCorridor");

            GUILayout.Label("Region - Temple");
            AddLevelButton("TempleIntro");
            AddLevelButton("CrateSequence");
            AddLevelButton("BoulderPressurepads");
            AddLevelButton("PressurePadSlide");
            AddLevelButton("ThwompTutorial");
            AddLevelButton("ThwompTrigger");
            AddLevelButton("ThwompClimb");
            AddLevelButton("CrateThwompRetrieve");
            AddLevelButton("BlueFragmentRoom", HueColour.HueColourValue(HueColour.HueColorNames.Blue));
            AddLevelButton("PostBlueCorridor");
            AddLevelButton("LongCratePressure");
            AddLevelButton("BalloonThwompJump");
            AddLevelButton("BalloonSwitchJump");
            AddLevelButton("BalloonDecoy");
            AddLevelButton("BalloonMaze");
            AddLevelButton("ThwompRunner");
            AddLevelButton("YellowFragmentRoom", HueColour.HueColourValue(HueColour.HueColorNames.Yellow));
            AddLevelButton("PostYellowCorridor");

            GUILayout.Label("Region - Tech");
            AddLevelButton("TechHub");
            AddLevelButton("TechIntro");
            AddLevelButton("LaserTutorial");
            AddLevelButton("LaserJumpSwitch");
            AddLevelButton("LaserMovingSwitch");
            AddLevelButton("LaserCrateBlock");
            AddLevelButton("LaserClimb");
            AddLevelButton("PipePush");
            AddLevelButton("LaserPlatformMadness1");
            AddLevelButton("LaserPlatformMadness2");
            AddLevelButton("LaserActivatedTutorial");
            AddLevelButton("LaserDoors");
            AddLevelButton("LaserBalloonMaze");
            AddLevelButton("ThwompLaserRunner");
            AddLevelButton("LimeFragmentRoom", HueColour.HueColourValue(HueColour.HueColorNames.Lime));
            AddLevelButton("PostLimeCorridor");
            AddLevelButton("LeverTutorial");
            AddLevelButton("LaserHeights");
            AddLevelButton("PlatformBlockLasers");
            AddLevelButton("LeverMadness");
            AddLevelButton("TechToLighthouse");

            GUILayout.Label("Region - Island");
            AddLevelButton("IslandPort");
            AddLevelButton("MountainsEntrance");
            AddLevelButton("MountainsBounceIntro");
            AddLevelButton("MountainsPostBounceIntro");
            AddLevelButton("BounceToDeath");
            AddLevelButton("MountainsBounceKeyRetrieve");
            AddLevelButton("BounceSpikePit");
            AddLevelButton("MountainsZigZag");
            AddLevelButton("BounceThwompDash");
            AddLevelButton("MountainsBounceLaserIntro");
            AddLevelButton("BounceCrateDrag");
            AddLevelButton("BouncePit");
            AddLevelButton("BounceConveyer");
            AddLevelButton("LaserBounceChange");

            GUILayout.Label("Region - University");
            AddLevelButton("UniversityOutside");
            AddLevelButton("UniversityLobby");
            AddLevelButton("BasementGoo");
            AddLevelButton("UniLetterCorridor");
            AddLevelButton("GooBalloonPressure");
            AddLevelButton("GooPressure");
            AddLevelButton("Courtyard1");
            AddLevelButton("ConveyerGoo");
            AddLevelButton("GooBalloonDip");
            AddLevelButton("TrophyRoom");
            AddLevelButton("ThwompDoubleLaser");
            AddLevelButton("UniGooStairs");
            AddLevelButton("Courtyard2");
            AddLevelButton("BounceGooIntro");
            AddLevelButton("GooBalloonCrates");
            AddLevelButton("MovingGoo");
            AddLevelButton("Courtyard3");
            AddLevelButton("ThwompGoo");
            AddLevelButton("HiddenDoorCorridor");
            AddLevelButton("SecretRoom");
            AddLevelButton("UniSlide");
            AddLevelButton("UniGooStairsDown");
            AddLevelButton("ThwompGooClimb");
            AddLevelButton("UniRooftop");
            AddLevelButton("MumRoom");
            AddLevelButton("OutroDream");
        }

        private static void AddLevelsAlphabetically()
        {
            AddLevelButton("AlternatingBoulders");
            AddLevelButton("AlternatingColourJumps02");
            AddLevelButton("AlternatingColourSwitch");
            AddLevelButton("BalloonDecoy");
            AddLevelButton("BalloonMaze");
            AddLevelButton("BalloonSwitchJump");
            AddLevelButton("BalloonThwompJump");
            AddLevelButton("BasementGoo");
            AddLevelButton("BlackBoxDecoy");
            AddLevelButton("BlueFragmentRoom");
            AddLevelButton("BoulderDropChase02");
            AddLevelButton("BoulderPressurepads");
            AddLevelButton("BoulderSwitchChase");
            AddLevelButton("BoulderTrap02");
            AddLevelButton("BoulderTutorialNew01");
            AddLevelButton("BounceConveyer");
            AddLevelButton("BounceCrateDrag");
            AddLevelButton("BounceGooIntro");
            AddLevelButton("BouncePit");
            AddLevelButton("BounceSpikePit");
            AddLevelButton("BounceThwompDash");
            AddLevelButton("BounceToDeath");
            AddLevelButton("BoxSlideMaze");
            AddLevelButton("BrickMaze");
            AddLevelButton("CaveMinerArea");
            AddLevelButton("ClimbUpColours02");
            AddLevelButton("ConveyerGoo");
            AddLevelButton("Courtyard1");
            AddLevelButton("Courtyard2");
            AddLevelButton("Courtyard3");
            AddLevelButton("CrateSequence");
            AddLevelButton("CrateThwompRetrieve");
            AddLevelButton("CrumblingRockJump");
            AddLevelButton("CrushOnStart");
            AddLevelButton("CycleHouse");
            AddLevelButton("DropThroughColour");
            AddLevelButton("FallThroughColours");
            AddLevelButton("FireIntro");
            AddLevelButton("GooBalloonCrates");
            AddLevelButton("GooBalloonDip");
            AddLevelButton("GooBalloonPressure");
            AddLevelButton("GooPressure");
            AddLevelButton("HiddenDoorCorridor");
            AddLevelButton("HueDunnit");
            AddLevelButton("IslandPort");
            AddLevelButton("JumpAlign");
            AddLevelButton("JumpColour");
            AddLevelButton("KeyTutorial");
            AddLevelButton("LaserActivatedTutorial");
            AddLevelButton("LaserBalloonMaze");
            AddLevelButton("LaserBounceChange");
            AddLevelButton("LaserClimb");
            AddLevelButton("LaserCrateBlock");
            AddLevelButton("LaserDoors");
            AddLevelButton("LaserHeights");
            AddLevelButton("LaserJumpSwitch");
            AddLevelButton("LaserMovingSwitch");
            AddLevelButton("LaserPlatformMadness1");
            AddLevelButton("LaserPlatformMadness2");
            AddLevelButton("LaserTutorial");
            AddLevelButton("LeverMadness");
            AddLevelButton("LeverTutorial");
            AddLevelButton("Lighthouse");
            AddLevelButton("LimeFragmentRoom");
            AddLevelButton("LongCratePressure");
            AddLevelButton("MountainsBounceIntro");
            AddLevelButton("MountainsBounceKeyRetrieve");
            AddLevelButton("MountainsBounceLaserIntro");
            AddLevelButton("MountainsEntrance");
            AddLevelButton("MountainsPostBounceIntro");
            AddLevelButton("MountainsZigZag");
            AddLevelButton("MovingGoo");
            AddLevelButton("MumRoom");
            AddLevelButton("NarrowCorridorCrates");
            AddLevelButton("OldLadyHouse");
            AddLevelButton("OrangeFragmentRoom");
            AddLevelButton("PinkFragmentRoom");
            AddLevelButton("PipePush");
            AddLevelButton("PlatformBlockLasers");
            AddLevelButton("PostBlueCorridor");
            AddLevelButton("PostLimeCorridor");
            AddLevelButton("PostPinkCorridor");
            AddLevelButton("PostPurpleCorridor");
            AddLevelButton("PostRedCorridor");
            AddLevelButton("PostYellowCorridor");
            AddLevelButton("PressurePadSlide");
            AddLevelButton("PullTute02");
            AddLevelButton("PurpleFragmentRoom");
            AddLevelButton("PuzzleSequence");
            AddLevelButton("RedFragmentRoom");
            AddLevelButton("SecretRoom");
            AddLevelButton("SkeletonRoom");
            AddLevelButton("SlideAcrossTheGap");
            AddLevelButton("SpikeTute03");
            AddLevelButton("TechHub");
            AddLevelButton("TechIntro");
            AddLevelButton("TechToLighthouse");
            AddLevelButton("TempleIntro");
            AddLevelButton("ThinHouse");
            AddLevelButton("ThwompClimb");
            AddLevelButton("ThwompDoubleLaser");
            AddLevelButton("ThwompGoo");
            AddLevelButton("ThwompGooClimb");
            AddLevelButton("ThwompLaserRunner");
            AddLevelButton("ThwompRunner");
            AddLevelButton("ThwompTrigger");
            AddLevelButton("ThwompTutorial");
            AddLevelButton("TrophyRoom");
            AddLevelButton("UniGooStairs");
            AddLevelButton("UniGooStairsDown");
            AddLevelButton("UniLetterCorridor");
            AddLevelButton("UniRooftop");
            AddLevelButton("UniSlide");
            AddLevelButton("UniversityLobby");
            AddLevelButton("UniversityOutside");
            AddLevelButton("Village");
            AddLevelButton("WaterEntrance");
            AddLevelButton("WaterExit");
            AddLevelButton("Waterfall");
            AddLevelButton("YellowFragmentRoom");
        }

        private static void AddLevelButton(string name)
        {
            if (GUILayout.Button(name))
            {
                LoadLevel(name);
            }
        }

        private static void AddLevelButton(string name, Color color)
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = color;
            if (GUILayout.Button(name, style))
            {
                LoadLevel(name);
            }
        }

    }
}
