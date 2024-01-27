using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Bootstrap script.
/// Inits the game status for the scene,
/// and has loading/saving procedues (for Story Mode and New/Load game options menu progress).
/// Mission-critical script!
/// </summary>
public class Bootstrap : MonoBehaviour
{
    //Variables and stuff
    public GameManager GameManager;         //GameManager script

    //State init code
    public bool difficulty_override = false;
    public GameManager.GameState _gameState; //New GameState
    public GameManager.GameType _gameType;   //New GameType
    //public GameManager.FMVType _fmvType;     //New FMVType
    //public LevelInfo.Levels _level_enum;   //New level

    //File vars
    private const string GameFile = "GameSlot";         //Base filename for saving story mode data
    private const string GameFile_Ext = ".sav";         //Extension for above

    /// <summary>
    /// Use this for initialization
    /// Sensitive code!! Do NOT edit unless you know WTF you are doing!
    /// </summary>
    private IEnumerator Start()
    {
        //Yield while GM inits
        yield return new WaitForSecondsRealtime(.1f);
        /*
        gm = GameObject.Find("GameManager");
        if (gm == null)
        {
            Debug.Log("Could not find the GameData object! Creating it...");
            // If not; create it from prefab
            gm = Instantiate(Resources.Load(GameManager.Prefab_Ctrl + "GlobalPrefab") as GameObject) as GameObject;
            gm.name = "  GameData"; //Name it
            // Cache the gamedata script
            GameManager = gm.GetComponent<GameManager>() as GameManager;
            //Load our Game and Options data
            LoadGame();
            LoadOptions();
            Debug.Log("GameData object created succesfully!");
        }
        else
        {
        */

        if (!GameManager.instance)
        {
            Debug.LogError("Fatal error! GameManager not found!");
        }
        else
        {
            // Cache the gamedata script
            GameManager = GameManager.instance;
            Debug.Log("Found the GameData object!");
        }

        //Set game states

        //If GameState is not IGNORE or override flag, set it
        if (difficulty_override)
        {
            _gameState = (GameManager.GameState)((byte)(GameManager.instance.difficulty) - (byte)(GameManager.GameState.GameRails));
            GameManager.gameState = _gameState;
        }
        else
        {
            if (_gameState != GameManager.GameState.IGNORE)
            {
                GameManager.gameState = _gameState;
            }
        }

        //Ditto for GameType
        if (_gameType != GameManager.GameType.IGNORE)
        {
            GameManager.gameType = _gameType;
        }

        //FMVType
        /*
        if (_fmvType != GameManager.FMVType.IGNORE)
        {
            GameManager.fmvType = _fmvType;
        }
        */

        //Level
        /*
        if (_level_enum != LevelInfo.Levels.IGNORE)
        {
            GameManager.level_enum = _level_enum;
        }
        */

        /*Use the bootstrap to generate
         *a HUD to start Fight system,
         * if a fighting level!
        */
        /*
        if (Fighting == true)
        {
            //Create HUD Prefab
            Instantiate(HUD, Vector3.zero, Quaternion.identity);
        }        

        GameManager.Fighting = Fighting;   //Set fighting status
        GameManager.AvoidOOB = AvoidOOB;   //Set OOB avoidance

        //If hobo mode is on, always make HoboLevel true; else set it as appropriately
        if (GameManager.HoboMode == true)
        {
            GameManager.HoboLevel = true;
        }
        else
        {
            GameManager.HoboLevel = HoboLevel; //Set Hobo setting
        }
        GameManager.PlayMusic();           //!@Leave this here; it's a bug fix for delayed music loading
        */
    }

    /*
    /// <summary>
    /// Creates a new game
    /// </summary>
    /// <param name="filename">Filename for file</param>
    public void NewGame(string filename)
    {
        //Set global vars to default, and then save the new file
        GameManager.instance.SaveName = filename;   //Set Savename
        GameManager.livesLeft = 3;                  //3 lives
        //!@ Set score to 0 here                    //Score = 0

        //Create new MissionData slots, set 1st level to PlanetWing, Null for others
        byte i = 0;
        for (i = 0; i < GameManager.max_mission_slots; i++)
        {
            if (i == 0)
            {
                GameManager.instance.Missions[i] = new GameManager.MissionData();
                GameManager.instance.Missions[i].level = LevelInfo.Levels.PlanetWing;
            }
            else
            {
                GameManager.instance.Missions[i] = new GameManager.MissionData();
                GameManager.instance.Missions[i].level = LevelInfo.Levels.Null;
            }
        }        

        //Save the defaults to the appropriate SaveGame slot
        byte slot = GameManager.instance.SaveSlot;
        SaveGame(slot);                     
    }

    //Info on reading/writing data files with C#
    //https://msdn.microsoft.com/en-us/library/aa720464%28v=vs.71%29.aspx-

    /// <summary>
    /// Saves the story data
    /// </summary>
    /// <param name="slot">SaveGame slot</param>
    /// <returns>Success?</returns>
    public bool SaveGame(byte slot)
    {
        Debug.Log("Saving game slot #"+slot.ToString() + "...");
        bool Code = false;      //Success status

        try
        {
            byte i=0;               //Generic iterator
            byte _byt = 0;          //Generic byte
            string _str = "";       //Generic string
            bool _bln = false;      //Generic bool
            System.Int16 _int = 0;  //Generic int16

            // Create the writer for data.
            string filename = GameFile+slot.ToString()+GameFile_Ext;
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);

            //Write the filename as string
            _str = GameManager.instance.SaveName;
            w.Write(_str);

            //Write the lives as byte
            _byt = (byte)(GameManager.instance.livesLeft);
            w.Write(_byt);

            //Write the score as int16
            _int = 0;//!@ Score
            w.Write(_int);

            //Write all of the mission data
            for (i = 0; i < GameManager.max_mission_slots; i++)
            {
                LevelInfo.Levels lvl = GameManager.instance.Missions[i].level;                

                _byt = (byte)(lvl);w.Write(_byt);
                _int = GameManager.instance.Missions[i].score; w.Write(_int);
                _bln = GameManager.instance.Missions[i].medal; w.Write(_bln);
                _byt = GameManager.instance.Missions[i].team; w.Write(_byt);
            }

            //Close the file handles
            w.Close();
            fs.Close();
            Code = true;
            Debug.Log("Game File written!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("There was an issue saving the Game datafile! " + e);
            Code = false;
        }
        return Code;    //Return status           
    }

    /// <summary>
    /// Loads the story data
    /// </summary>
    /// <param name="slot">SaveGame slot</param>
    /// <returns>Success?</returns>
    public bool LoadGame(byte slot)
    {
        bool Code = false;
        bool DNE = false;
        Debug.Log("Loading game slot #" + slot.ToString() + "...");

        try
        {
            //Check if a file exists; if it doesn't, return error code
            string filename = GameFile + slot.ToString() + GameFile_Ext;
            if (File.Exists(filename) == false)
            {
                Debug.LogError("No file yet for slot" + slot.ToString() + "!");
                return false;
            }            

            byte i = 0;             //Generic iterator
            byte _byt = 0;          //Generic byte
            string _str = "";       //Generic string
            bool _bln = false;      //Generic bool
            System.Int16 _int = 0;  //Generic Int16

            // Create the reader for data.            
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);

            //Read a string, set as filename
            _str = r.ReadString();
            GameManager.instance.SaveName = _str;

            //Read a byte ,set as lives
            _byt = r.ReadByte();
            GameManager.instance.livesLeft = _byt;

            //Read an in16, set as score
            _int = r.ReadInt16();
            //!@GameManager.instance.Score = _int;

            //Read max_mission_slots amount of MissionData, set as mission slots
            for (i = 0; i < GameManager.max_mission_slots; i++)
            {
                GameManager.instance.Missions[i] = new GameManager.MissionData();

                _byt = r.ReadByte();                                    //Read a byte
                LevelInfo.Levels lvl = (LevelInfo.Levels)(_byt);    //Convert byte to Levels enum
                //If the level enum is not null
                if (lvl != LevelInfo.Levels.Null)
                {
                    GameManager.instance.level_enum = lvl;              //Set the game's current level_enum to this level
                    GameManager.instance.mission_index = i;             //Set the mission index to this one
                }                
                GameManager.instance.Missions[i].level = lvl;

                _int = r.ReadInt16();
                GameManager.instance.Missions[i].score = _int;

                _bln = r.ReadBoolean();
                GameManager.instance.Missions[i].medal = _bln;

                _byt = r.ReadByte();
                GameManager.instance.Missions[i].team = _byt;                
            }

            //Close the file
            r.Close();
            fs.Close();

            Code = true;
            Debug.Log("Game File succesfully created/loaded!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("There was an issue loading the Game datafile! " + e);
            Code = false;
        }
        return Code;
    }

    /// <summary>
    /// Retrieves the main adata from a saved game slot. Used for the New/Load game menus
    /// </summary>
    /// <param name="slot">Saved game slot to retrieve data from</param>
    /// <param name="filename">Slot's filename to return ByRef</param>
    /// <param name="lives">Slot's lives to return ByRef</param>
    /// <param name="score">Slot's score to return ByRef</param>
    /// <param name="missions">Slot's mission data array to return ByRef</param>
    /// <returns>Success?</returns>
    public bool LoadGame_SlotData(byte slot, ref string filename, ref byte lives, ref int score, ref GameManager.MissionData[] missions)
    {
        byte _byt = 0x00;               //Generic byte
        System.Int16 _int = 0x0000;     //Generic int16
        bool _bln = false;              //Generic bool
        string _str = "";               //Generic string

        missions = new GameManager.MissionData[GameManager.max_mission_slots];      //ReDim missiondata array to max_mission_slots
        bool Code = false;                                                          //Return code
        Debug.Log("Loading data from game slot #" + slot.ToString() + "...");

        try
        {
            //Check if a file exists; if it doesn't, skip loading a file,
            string _filename = GameFile + slot.ToString() + GameFile_Ext;
            if (File.Exists(_filename) == false)
            {
                Debug.LogError("File DNE!");
                return false;
            }

            byte i = 0;  //Generic iterator

            // Create the reader for data.            
            FileStream fs = new FileStream(_filename, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);

            _str = r.ReadString();      //Read a string, return ByRef as filename
            _byt = r.ReadByte();        //Read a byte, return ByRef as lives
            _int = r.ReadInt16();       //Read an Int16, return ByRef as score
            filename = _str;
            lives = _byt;
            score = _int;

            //!@ _int = r.ReadInt16();GameManager.instance.Score = _int;

            //Cycle an iterator between all mission slots
            for (i = 0; i < GameManager.max_mission_slots; i++)
            {
                byte level = r.ReadByte();                              //Read a mission byte
                LevelInfo.Levels lvl = (LevelInfo.Levels)(level);   //Conv to Levels enum
                missions[i].level = lvl;                                //Set level for slot

                //Read an int 16, set as mission score
                _int = r.ReadInt16();
                missions[i].score = _int;

                //Read a bool, set as mission medal
                _bln = r.ReadBoolean();
                missions[i].medal = _bln;

                //Read a byte, set as mission team status (bitarray as byte)
                _byt = r.ReadByte();
                missions[i].team = _byt;                
            }

            //Close the file
            r.Close();
            fs.Close();

            Code = true;
            Debug.Log("Game File succesfully created/loaded!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("There was an issue loading the Game datafile! " + e);
            Code = false;
        }
        return Code;
    }
    */
}