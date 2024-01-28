using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
/// <summary>
/// Basic information for players
/// </summary>
public class PlayerProfile
{
    public static float max = 100f; //Normal maxHP
    public static float min = 25f;  //Always minHP value

    [Header("Health")]
    public float lowHP = min;                           //Current lowHP value
    public float maxHp = max;                           //Current MaxHP value
    public bool lowHealth_runonce = false;              //Runonce flag for low_health FX
    public float hpLeft;                                //HP remaining

    //public Characters character;                      //Character of this player
    [Header("UserInfo")]
    public string playerName;                           //Player name    
    public UnityEngine.Color playerColor;               //Player color
    public PlayerType playerType;                       //Type of player (Human or CPU)

    //public CharacterControllerStarEagle playerObject; //!@ CCSE of the player

    public static PlayerProfile GetDefaultSetup(PlayerType _playerType)
    {
        PlayerProfile profile = new PlayerProfile()
        {
            lowHealth_runonce = false,
            //character = Characters.EAGLE,
            playerName = "Person",
            playerColor = new UnityEngine.Color(1f, 1f, 1f, 1f),
            playerType = _playerType,
            hpLeft = max,
        };
        return profile;
    }

    /*
    public static PlayerProfile GetEagleDefaultSetup()
    {
        return new PlayerProfile()
        {
            lowHealth_runonce = false,
            //character = Characters.EAGLE,
            playerType = PlayerType.PLAYER,
            hpLeft = max,
        };
    }

    public static PlayerProfile GetBunnyDefaultSetup()
    {
        return new PlayerProfile()
        {
            lowHealth_runonce = false,
            character = Characters.BUNNY,
            playerType = PlayerType.CPU,
            hpLeft = max,
        };
    }

    public static PlayerProfile GetOwlDefaultSetup()
    {
        return new PlayerProfile()
        {
            lowHealth_runonce = false,
            character = Characters.OWL,
            playerType = PlayerType.CPU,
            hpLeft = max,
        };
    }

    public static PlayerProfile GetDogDefaultSetup()
    {
        return new PlayerProfile()
        {
            lowHealth_runonce = false,
            character = Characters.DOG,
            playerType = PlayerType.CPU,
            hpLeft = max,
        };
    }
    */

    /// <summary>
    /// Gets this profile's character name
    /// </summary>
    /// <returns>Character name</returns>
    public string GetCharacterName(bool rich)
    {
        const string colorOpen = "<color=";
        const string colorClose = "</color>";

        //string name = GetCharacterName(character,null,false,false);
        string name = playerName;
        if (rich) { name = colorOpen + playerColor + playerName + colorClose; }
        return name;
    }

    /*
    /// <summary>
    /// Gets character name of a selected character type
    /// </summary>
    /// <param name="type">Character type</param>    
    /// <param name="prefix">Get prefix instead of name?</param>
    /// <returns>Character/prefix name</returns>
    public static string GetCharacterName(Characters chr, Text T, bool prefix = false, bool rich=false)
    {
        string[] colors = 
        {
            "<color=yellow>",
            "<color=grey>",
            "<color=white>",
            "<color=brown>",
        };
        const string clrClose = "</color>";

        if (rich)
        {
            T.supportRichText = true;
        }

        byte ind = (byte)(chr);
        string name = "";
        string[] names = { "Sammy Jr.", "Slippery", "Gregorio", "Thomas" };
        string[] namesPre = { "Eagle", "Bunny", "Owl", "Dog" }; //Prefixes used for some gameObject naming
        string clrSt = colors[ind];        

        if (prefix)
        {
            if (rich)
            {
                name = clrSt + namesPre[ind] + clrClose;
            }
            else
            {
                name = namesPre[ind];
            }
        }
        else
        {
            if (rich)
            {
                name = clrSt + names[ind] + clrClose;
            }
            else
            {
                name = names[ind];
            }
        }
        return name;
    }
    */
}