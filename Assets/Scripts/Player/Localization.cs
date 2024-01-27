using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script helps game in localizing strings
/// </summary>
public class Localization : MonoBehaviour
{
    #region MainMenuDefs
    /// <summary>
    /// Enum with Main Menu items
    /// </summary>
    public enum MainMenuItem
    {        
        MMI_NULL,

        //Main Menu items
        MMI_MM_NGame,
        MMI_MM_LGame,
        MMI_MM_Training,
        MMI_MM_Options,
        MMI_MM_Credits,
        MMI_MM_Exit,

        //Option items
        MMI_Opt_Options,    //2ndary label, chars split across gap
        MMI_Opt_MVol,
        MMI_Opt_SVol,
        MMI_Opt_Diff,
        MMI_Opt_Lang,
        MMI_Opt_Pitch,
        MMI_Opt_Ctrls,

        //VectorVision
        MMI_VV_VV,
        MMI_VV_MonType,
        MMI_VV_VVMode,
        MMI_VV_EdgeFilter,
        MMI_VV_Scale,
        MMI_VV_FPS,
        MMI_VV_t12,
        MMI_VV_Test,
        MMI_VV_Apply,        

        //Kbd items
        MMI_KBD_Hdr,
        MMI_KBD_Dir,
        MMI_KBD_G19Flash,
        MMI_KBD_G19Video,

        //Gamepad items
        MMI_GPD_Hdr1,
        MMI_GPD_Type,           //!@Dead
        MMI_GPD_Dir,

        MMI_Fls_Hdr1,
        MMI_Fls_Dir,
        //!@ Dead actions names
        MMI_MAP_LeftYawM,
        MMI_MAP_RightYawP,
        MMI_MAP_Left2RollM,     //!@
        MMI_MAP_Right2RollP,    //!@
        MMI_MAP_Brake,
        MMI_MAP_Shoot,
        MMI_MAP_Special,
        MMI_MAP_UTurn,
        MMI_MAP_Accel,          //!@
        MMI_MAP_HardBarrelM,    //!@
        MMI_MAP_HardBarrelP,    //!@
        MMI_MAP_Start,
        MMI_MAP_Menu,
        MMI_MAP_LStick,         //!@ LStick x/y axises combined, for gamepad menu
        MMI_MAP_RStick,         //!@ RStick x/y axises combined, for gamepad menu

        //New/Load game stuff
        MMI_Gam_NewGame,
        MMI_Gam_LoadGame,
        MMI_Gam_DelGameHdr,
        MMI_Gam_DelDir,
        MMI_Gam_NewGameName_Hdr,
        MMI_Gam_NewGameName_Dir,
        MMI_Gam_NewGameName_Filename,
        MMI_Gam_NewGameName_DFilename,

        //MMI_Cheater1,           //Cheater messages        
        MMI_Back,               //Back button
        MMI_Yes,                //Yes
        MMI_No,                 //No
        MMI_VV_Dir,              //VectorVision ToggleSendConstantly directions

        //Video settings
        MMI_Video_VS,               //Video settings options item/Menu header item
        MMI_Video_Sample,           //Message about pressing BRoll- btn/key to sample video settings
        MMI_Video_PalType,          //Palette type header
        MMI_Video_Dither,           //Dither? msg
        MMI_Video_DithTex,          //Dither texture header
        MMI_Video_DithStrSize,       //Dither strength/threshold
        MMI_Video_PSX_Fog,          //Fog?
        MMI_Video_PSX_FogColor,     //Fog color
        MMI_Video_PSX_FogDist,      //Fog distance
        MMI_Video_PSX_FogTexDist,   //Fog texture distortion

        MMI_MAX,MMI_Opt_YawRoll=MMI_MAX,            //Options yaw/roll flighstick swap
        
    }
    public MainMenuItem MMI_type = MainMenuItem.MMI_NULL;       //Type of MMI for this Localz script in Main Menus
    public MainMenuItem old_MMI_type = MainMenuItem.MMI_NULL;                //MMI type on previous frame. Used for updates
    #endregion

    #region InGameDefs
    //!@ Needs modified for StarEagle usage
    /// <summary>
    /// In-game localization (Gameover screen, level/final level/minigame results)
    /// </summary>
    public enum InGameItem
    {
        //Nothing
        ING_NULL,

        //Character names
        ING_CHAR_MIN, ING_CHAR_EAGLE = ING_CHAR_MIN,
        ING_CHAR_BUNNY,
        ING_CHAR_OWL,
        ING_CHAR_MAX, ING_CHAR_DOG = ING_CHAR_MAX,

        //LevelInfo.LevelData components
        ING_LEVEL_NAME,
        ING_LEVEL_ABBREV,
        ING_LEVEL_LOCNAME,
        ING_LEVEL_BLURB,

        //Pause menu elements
        ING_PAUSE_RETRY,
        ING_PAUSE_CONTINUE,
        ING_PAUSE_BACK,
        ING_PAUSE_RUSURE,
        ING_PAUSE_YES,
        ING_PAUSE_NO,

        //Mission stuff
        ING_MISS_NO,        //Mission No. X
        ING_MISS_C,         //Mission Acomp/Complete placeholder
        ING_MISS_COMP,      //Mission Complete (bad route)
        ING_MISS_ACOMP,     //Mission Accomplished (good route)
    
        //Results screen stuff
        ING_RESULTS_ENS,
        ING_RESULTS_ENS_TTL,
        ING_RESULTS_STATUS,

        //Gameover
        ING_GAMEOVER,
        ING_MAX = ING_GAMEOVER
    }
    public InGameItem ING_type = InGameItem.ING_NULL;
    #endregion

    #region Map
    /// <summary>
    /// Localization for Map items
    /// </summary>
    public enum MapItem
    {
        Map_Null,               //Nothing
        Map_Options,            //"Map Options" header
        Map_Opt_Dir,            //Map Options directions
        Map_Opt_GotoMiss,       //Goto Next Mission
        Map_Opt_ChangeMiss,     //Change mission
        Map_Opt_Retry,          //Retry current mission
        Map_Opt_Back,           //Back
        Map_MAX,Map_Opt_Exit=Map_MAX,//Exit to MM
        
    }
    public MapItem Map_Type = MapItem.Map_Null; //MapItem localization
    #endregion

    #region LangPack_StructDef
    /// <summary>
    /// Struct holding localization strings for each language
    /// </summary>
    public struct LangPack
    {
        private readonly string English;
        private readonly string Spanish;
        private readonly string French;
        private readonly string Chinese;
        private readonly string Russian;
        public LangPack(string English, string Spanish, string French, string Chinese, string Russian)
        {
            this.English = English;
            this.Spanish = Spanish;
            this.French = French;
            this.Chinese = Chinese;
            this.Russian = Russian;
        }

        /// <summary>
        /// Overload of LangPack, that sets all localized strings to one version. Used for setting all langs to one string
        /// </summary>
        /// <param name="msg">string for all langs</param>
        public LangPack(string msg)
        {
            this.English = msg;
            this.Spanish = msg;
            this.French = msg;
            this.Chinese = msg;
            this.Russian = msg;
        }

        /// <summary>
        /// Overload of LangPack, that sets english string, others to whatever. Used for placeholders mostly
        /// </summary>
        /// <param name="English">English string</param>
        /// <param name="Foreign">Foreign string</param>
        public LangPack(string English, string Foreign)
        {
            this.English = English;
            this.Spanish = Foreign;
            this.French = Foreign;
            this.Chinese = Foreign;
            this.Russian = Foreign;
        }
        public string english { get { return English; } }
        public string spanish { get { return Spanish; } }
        public string french { get { return French; } }
        public string chinese { get { return Chinese; } }
        public string russian { get { return Russian; } }

        /// <summary>
        /// Psuedo-GET function for fetching appropriate string for language set in GameManager
        /// </summary>
        public string language
        {
            get
            {
                string result = "";                                     //String to return
                GameManager.Language lang = GameManager.instance.lang;  //Current Language
                //Fetch the appropriate string for this language
                switch (lang)
                {
                    case GameManager.Language.LANG_ENGLISH:
                        result = english;
                        break;
                    case GameManager.Language.LANG_SPANISH:
                        result = spanish;
                        break;
                    case GameManager.Language.LANG_FRENCH:
                        result = french;
                        break;
                    case GameManager.Language.LANG_CHINESE:
                        result = chinese;
                        break;
                    case GameManager.Language.LANG_RUSSIAN:
                        result = russian;
                        break;
                }
                return result;
            }
        }
    }

    /// <summary>
    /// Overload of LangPack, with members as public and mutable. Used for serialization with TextSwitch components etc
    /// </summary>
    [System.Serializable]
    public struct LangPack2
    {
        public string English;
        public string Spanish;
        public string French;
        public string Chinese;
        public string Russian;
        public LangPack2(string English, string Spanish, string French, string Chinese, string Russian)
        {
            this.English = English;
            this.Spanish = Spanish;
            this.French = French;
            this.Chinese = Chinese;
            this.Russian = Russian;
        }

        /// <summary>
        /// Overload of LangPack, that sets all localized strings to one version. Used by XBone or things I'm too lazy for
        /// </summary>
        /// <param name="English"></param>
        public LangPack2(string English)
        {
            this.English = English;
            this.Spanish = English;
            this.French = English;
            this.Chinese = English;
            this.Russian = English;
        }
        public string english { get { return English; } }
        public string spanish { get { return Spanish; } }
        public string french { get { return French; } }
        public string chinese { get { return Chinese; } }
        public string russian { get { return Russian; } }

        /// <summary>
        /// Psuedo-GET function for fetching appropriate string for language set in GameManager
        /// </summary>
        public string language
        {
            get
            {
                string result = "";                                     //String to return
                GameManager.Language lang = GameManager.instance.lang;  //Current Language
                //Fetch the appropriate string for this language
                switch (lang)
                {
                    case GameManager.Language.LANG_ENGLISH:
                        result = english;
                        break;
                    case GameManager.Language.LANG_SPANISH:
                        result = spanish;
                        break;
                    case GameManager.Language.LANG_FRENCH:
                        result = french;
                        break;
                    case GameManager.Language.LANG_CHINESE:
                        result = chinese;
                        break;
                    case GameManager.Language.LANG_RUSSIAN:
                        result = russian;
                        break;
                }
                return result;
            }
        }
    }
    #endregion

    private GameManager.Language oldLang = GameManager.Language.LANG_ENGLISH;   //Language on previous frame

    public void _Start()
    {
        StartCoroutine(Start());
    }

    public IEnumerator Start()
    {
        //Yield until GameManager is found
        yield return new WaitForSeconds(.1f);

        //If Ing_type is not null, init the localization
        if (ING_type != InGameItem.ING_NULL)
        {
            InGame();
        }

        //Handle localization of map items if not null and in Map scene
        string scene=Application.loadedLevelName;
        if ((Map_Type != MapItem.Map_Null) && (scene == "Map"))
        {
            Map();
        }
    }

    /// <summary>
    /// Update MMI items when language changes
    /// </summary>
    void Update()
    {
        //If GM exists
        if (GameManager.instance)
        {
            GameManager.Language Lang = GameManager.instance.lang;  //Get current language

            //If a delta occurred on MMI type change
            if (old_MMI_type != MMI_type)
            {
                //Set old to current, update menu text
                old_MMI_type = MMI_type;
                MainMenu();
            }

            //If a delta occured with the language
            if (oldLang != Lang)
            {
                oldLang = GameManager.instance.lang;                //Update old language (deadlock out of spamming this block)
                //If MMI type is not null                                                                    
                if (MMI_type != MainMenuItem.MMI_NULL)
                {
                    MainMenu();                                     //Localize the text for current language
                }
            }
            oldLang = GameManager.instance.lang;                    //Update old language
        }
    }

    /// <summary>
    /// Handles localization of the credits
    /// </summary>
    public string _Credits()
    {
        string msg = "";                                    //Multi-line message of credits to return
        LangPack newLine = new LangPack("\n");              //Dummy langPack with newLine

        //!@ Needs adjusted
        #region CreditsText
        //Big LangPack containing each line of Credits
        LangPack[] pack = new LangPack[]
            {
                //TRANSLATE ME some headers for Russian (and new missing stuff)
                new LangPack("Hall of Heroes:", "Salón de Héroes", "Salle des héros", "英雄榜", "Зал Героев:"),
                newLine,               
                newLine,
            };
        #endregion

        //Concatenate a huge string
        foreach (LangPack p in pack)
        {
            string m = p.language;
            msg += m;
            if (m != "\n")
            {
                msg += "\n";
            }
        }
        return msg;
    }
    /// <summary>
    /// Updates MMIs in the Main Menu
    /// </summary>
    public void MainMenu()
    {        
        #region MM_Pack
        //Big pack aligned with MainMenuItem enum for localization
        LangPack[] pack = new LangPack[(byte)(MainMenuItem.MMI_MAX)+1]
        {
            new LangPack("NULL"),
            
            new LangPack("New Game", "Juego Nuevo", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Load Game", "Cargar Nuevo", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Training", "Instrucción", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Options", "Opciones", "Options", "选项", "Опции"),
            new LangPack("Credits", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Exit", "Salir", "Sortie", "退出", "Выход"),

            new LangPack(" Opt       ions", " Opci       ones", " Opt       ions", "选       项", "Оп    ц    ии"),
            new LangPack("Music vol.", "Vol. de Música ", " Vol. de la musique ", "音量", "Громкость"),
            new LangPack("Sfx vol.", "Vol. de sonido", " Vol. sonore", "TRANSLATE ME", " TRANSLATE ME"),                                                //!@ TRANSLATE ME
            new LangPack("Game Mode", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Language", "Idioma", "La langue", "語言", "язык"),
            new LangPack("Invert Pitch", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Controls", "  Controles", "  Commandes", "控制选项", "Управление"),

            new LangPack("VectorVision™", "TRANSLATE ME™", "TRANSLATE ME™", "TRANSLATE ME™", "TRANSLATE ME™"),
            new LangPack("Monitor Type", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("VectorVision™ Mode", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Edge Filter", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Scale (w,h)", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("FPS", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("t1, t2"),
            new LangPack("Test Settings", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Apply Changes", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),                        

            new LangPack("Controls (Keyboard)\n", "Controles (Teclado)\n", "Commandes (clavier)\n", "控制（键盘）\n", "Управление (Клавиатура)\n"),
            new LangPack("Click to begin remapping.","TRANSLATE ME"),
            new LangPack("G19 Flash","TRANSLATE ME"),
            new LangPack("G19 Video","TRANSLATE ME"),

            new LangPack("Controls (Gamepad)", "Controles (Mando)", "Commandes (Gamepad)", "TRANSLATE ME", "Управление (Геймпад)"),
            new LangPack("Gamepad Type:", "Tipo de Mando:", "Type de Gamepad:", "手柄类型：", "Тип Геймпада:"),
            new LangPack("Click to begin remapping.","TRANSLATE ME"),

            new LangPack("Controls (Flighstick)", "Controles (TRANSLATE ME)", "Commandes (TRANSLATE ME)", "TRANSLATE ME", "Управление (TRANSLATE ME)"),
            new LangPack("Click to begin remapping.","TRANSLATE ME"),
            //!@ Dead types
            new LangPack("Left/Yaw-", "Izqu./TRANSLATE ME", "Gauche/TRANSLATE ME", "向左/TRANSLATE ME", "Влево/TRANSLATE ME"),
            new LangPack("Right/Yaw+", "Dere./TRANSLATE ME", "Droit./TRANSLATE ME", "向右/TRANSLATE ME", "Вправо/TRANSLATE ME"),
            new LangPack("Left2/Roll-", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),    //!@
            new LangPack("Right2/Roll+", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),   //!@
            new LangPack("Brake", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Shoot", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Special", "Espec.", "Spéc", "特技", "Спец-атака"),
            new LangPack("UTurn", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Accel.", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),         //!@
            new LangPack("Hard/BRoll-", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),    //!@
            new LangPack("Hard/BRoll+", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),    //!@
            new LangPack("Start", "Comenz.", "Commenc.", "开始", "Старт"),
            new LangPack("Menu", "Menú", "Menu", "菜单", "Меню"),
            new LangPack("^\\<>/Pitch,Yaw±", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),//!@
            new LangPack("Roll±", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),           //!@

            new LangPack(" New       Game", "Juego       Nuevo", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Load       Game", "Cargar       Juego ", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Delete       Game?", "¿Borrar       Juego? ", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Floppies are still valuable today and in use for vintage computing enthusiasts! Are you sure you wish to throw the saved game on this poor, innocent floppy into the trash? It just wants to be loved ;_;. #SaveTheFloppies","TRANSLATE ME"),
            new LangPack("Enter       Name ", "Nom       Bre ", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Type in or enter a filename.\nPress . button to enter a key.","TRANSLATE ME"),            
            new LangPack("Filename:", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),            
            new LangPack("Enter filename...","TRANSLATE ME"),

            //new LangPack("Achievements disabled.\nCheaters never win!", "Logros desactivados.\n¡Los tramposos nunca ganan!", "Récompenses désactivées.\nLes tricheurs ne gagneront jamais!", "成就已禁用。\n作弊就别想赢！", "Достижения отключены.\nЧитеры никогда не выигрывают!"),
            new LangPack("Back", "Regresar", "Retour", "返回", "Назад"),
            new LangPack("Yes","Si","TRANSLATE ME","TRANSLATE ME","TRANSLATE ME"),
            new LangPack("No","No","TRANSLATE ME","TRANSLATE ME","TRANSLATE ME"),
            new LangPack("(Press Hard/BRoll+ key/btn to toggle\naccelerating horizontal movement)","(TRANSLATE ME)","(TRANSLATE ME)","(TRANSLATE ME)","(TRANSLATE ME)"),

            //!@
            new LangPack("Video settings", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("(Press Hard/BRoll- key/btn to toggle sample display.)", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Palette Type", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Dither?", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Texture", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Strength, size (0-1)", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME","TRANSLATE ME"),
            new LangPack("Fog?", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME","TRANSLATE ME"),
            new LangPack("Color (rgba)", "TRANSLATE ME", "TRANSLATE ME","TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Dist (min, max)", "TRANSLATE ME", "TRANSLATE ME","TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Tex Distortion", "TRANSLATE ME", "TRANSLATE ME","TRANSLATE ME", "TRANSLATE ME"),

            new LangPack("Swap Yaw/Roll", "TRANSLATE ME", "TRANSLATE ME","TRANSLATE ME", "TRANSLATE ME"),
        };
        #endregion

        if (GameManager.instance)
        {
            Text T = this.gameObject.GetComponent<Text>();  //Get text component on this gameobject
            if (T)
            {
                byte ID = (byte)(MMI_type);                     //Convert type to byte
                string msg = pack[ID].language;                 //Fetch appropriate string for current language in appropriate langpack from array

                //Update the text. If an uppercase font is used, make msg upper for foreign compatiblity
                bool cap = false;
                foreach (Font f in GameManager.instance.CapFonts)
                {
                    bool capfont = (T.font == f);
                    if (capfont)
                    {
                        cap = true;
                        T.text = msg.ToUpper();
                        break;
                    }
                }
                if (cap == false)
                {
                    T.text = msg;
                }
            }
        }
    }

    //!@ Needs adjusted
    /// <summary>
    /// Handles localization of various in-game things (UI, minigame, level results, etc)
    /// </summary>
    public void InGame()
    {
        byte index = 0;
        byte id=(byte)(ING_type);                               //Get type as byte

        //Array of langpacks aligned with InGameItem for localization
        #region ingame
        LangPack[] pack = new LangPack[(byte)(InGameItem.ING_MAX)+0x01]
        {
            //Nothing
            new LangPack("NULL"),

            //Character names
            new LangPack("DUMMY"),
            new LangPack("DUMMY"),
            new LangPack("DUMMY"),
            new LangPack("DUMMY"),

            //LevelInfo.LevelData components
            new LangPack("DUMMY"),
            new LangPack("DUMMY"),
            new LangPack("DUMMY"),
            new LangPack("DUMMY"),

            //Pause menu elements
            new LangPack("Retry mission","TRANSLATE ME"),
            new LangPack("Continue","Continue","Continue","Continue","Continue"),
            new LangPack("Back to Main Menu","TRANSLATE ME"),
            new LangPack("Are you sure? Saved progress will be lost!", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME", "TRANSLATE ME"),
            new LangPack("Yes","Si","TRANSLATE ME","TRANSLATE ME","TRANSLATE ME"),
            new LangPack("No","No","TRANSLATE ME","TRANSLATE ME","TRANSLATE ME"),
    
            //Mission stuff
            new LangPack("Mission No. ","TRANSLATE ME"),
            new LangPack("DUMMY"),
            new LangPack("Mission complete!","TRANSLATE ME"),
            new LangPack("Mission accomplished!","TRANSLATE ME"),
    
            //Results screen stuff
            new LangPack("Enemies destroyed","TRANSLATE ME"),
            new LangPack("Ttl. enemies destroyed","TRANSLATE ME"),
            new LangPack("Team status:","TRANSLATE ME"),

            new LangPack("GAMEOVER!","TRANSLATE ME"),
        };
        #endregion ingame

        //Get the Text comp on this script
        Text T = this.gameObject.GetComponent<Text>();
        if (T)
        {
            string msg="";

            //Special cases, which run functions to get data            
            switch (ING_type)
            {
                #region ING_SpecialCases
                case InGameItem.ING_CHAR_EAGLE:
                case InGameItem.ING_CHAR_BUNNY:
                case InGameItem.ING_CHAR_OWL:
                case InGameItem.ING_CHAR_DOG:
                    index = (byte)(ING_type - InGameItem.ING_CHAR_MIN);
                    Characters c = (Characters)(index);
                    msg = PlayerProfile.GetCharacterName(c, T, false, true);
                    break;

                /*
                case InGameItem.ING_LEVEL_NAME:
                    index = (byte)(GameManager.instance.level_enum);
                    msg = LevelInfo.levelData[index].levelName.language;
                    break;
                case InGameItem.ING_LEVEL_ABBREV:
                    index = (byte)(GameManager.instance.level_enum);
                    msg = LevelInfo.levelData[index].levelAbbrev.language;
                    break;
                case InGameItem.ING_LEVEL_LOCNAME:
                    index = (byte)(GameManager.instance.level_enum);
                    msg = LevelInfo.levelData[index].levelLocName.language;
                    break;
                case InGameItem.ING_LEVEL_BLURB:
                    index = (byte)(GameManager.instance.level_enum);
                    msg = LevelInfo.levelData[index].levelBlurb.language;
                    break;                

                case InGameItem.ING_MISS_NO:
                    index = GameManager.instance.mission_index;
                    index++;
                    msg = pack[id].language;
                    msg += index.ToString();
                    break;

                case InGameItem.ING_MISS_C:
                    byte path = GameManager.instance.currentPath;       //Get the current path ID
                    index = (byte)(GameManager.instance.level_enum);    //Get the current level as index

                    //If the path == this level's bad path
                    bool bad = (path == LevelInfo.levelData[index].levelPath_Bad);
                    if (bad)
                    {
                        //Increment id by 1 (bad)
                        id = (byte)(ING_type + 1);
                    }
                    else
                    {
                        //Increment id by @ (good)
                        id = (byte)(ING_type + 2);
                    }          
          
                    //Fetch the new localized langPack and apply
                    msg = pack[id].language;
                    msg += index.ToString();
                    break;
                */
                #endregion

                default:
                    msg = pack[id].language;
                    break;
            }

            T.text = msg;
        }
    }

    /// <summary>
    /// Handles the text in the game map
    /// </summary>
    public void Map()
    {
        byte id = (byte)(Map_Type);                               //Get type as byte

        //Array of langpacks aligned with MapItem for localization
        #region Map
        LangPack[] pack = new LangPack[(byte)(MapItem.Map_MAX)+0x01]
        {            
            new LangPack(""),
            new LangPack("    Map          Options","TRANSLATE ME"),
            new LangPack("Map Ctrls:\nDirStick X/Y, CamStick X - Y/X/Z-Axis cam rot\nHard/BRoll + - Toggle Speed","TRANSLATE ME"),
            new LangPack("Goto next mission","TRANSLATE ME"),
            new LangPack("Change Mission Path","TRANSLATE ME"),
            new LangPack("Retry Mission (lose life)","TRANSLATE ME"),
            new LangPack("Back", "Regresar", "Retour", "返回", "Назад"),
            new LangPack("Exit to Main Menu", "Salir TRANSLATE ME", "Sortie TRANSLATE ME", "退出 TRANSLATE ME", "Выход TRANSLATE ME"),
        };
        #endregion

        //Get the Text comp on this script
        Text T = this.gameObject.GetComponent<Text>();
        if (T)
        {
            string msg = "";
            msg = pack[id].language;
            T.text = msg;
        }
    }
}