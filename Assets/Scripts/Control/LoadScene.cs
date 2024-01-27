using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles loads a Scene, especially for event driven systems
/// </summary>
public class LoadScene : MonoBehaviour
{
    public static LoadScene instance;   //This instance
    public string sceneName;            //SceneName to load

    private bool isLoading = false;     //Is this LS loading?
    private float timer = 0;            //Timer
    private float delay;                //Delay before loading scene

    /// <summary>
    /// Sets up the singleton instance onStart
    /// </summary>
    void Start()
    {
        instance = this;
    }

    /// <summary>
    /// Handles the timer
    /// </summary>
    void Update()
    {
        //If loading a scene
        if (isLoading)
        {
            //Increment timer by unscaledDeltaTime
            timer += Time.unscaledDeltaTime;

            //If timer >= delay, Load the stage, set isLoading to false
            if (timer >= delay)
            {
                isLoading = false;
                Load();
            }
        }
    }

    /// <summary>
    /// Sets the delay for loading the scene
    /// </summary>
    /// <param name="delay">Timelimit</param>
    public void LoadSceneDelayed(float delay)
    {
        timer = 0f;         //Reset timer
        this.delay = delay; //Set delay
        isLoading = true;   //Begin timer
    }

    /// <summary>
    /// Loads the main menu with delay
    /// </summary>
    /// <param name="delay">Timelimit</param>
    public void LoadMainMenuDelayed(float delay)
    {
        timer = 0f;                 //Reset timer  
        sceneName = "Main Menu";    //Set scene to Main Menu
        this.delay = delay;         //Set time limit
        isLoading = true;           //Begin timer
    }

    /// <summary>
    /// Loads the Map with delay
    /// </summary>
    /// <param name="delay">Timelimit</param>
    public void LoadMapDelayed(float delay)
    {
        timer = 0f;                 //Reset timer  
        sceneName = "Map";         //Set scene to Map
        this.delay = delay;         //Set time limit
        isLoading = true;           //Begin timer
    }

    //!@ STUB
    /// <summary>
    /// Reloads the current level scene, with checkpoint stuff restored
    /// </summary>
    /// <param name="delay">Timelimit</param>
    /// <param name="removeLife">Remove a life?</param>
    public void ReloadScene(float delay, bool removeLife)
    {
        if (removeLife)
        {
            //!@ LivesCount.instance.RemoveLife();
        }

        //!@ DO STUFF HERE (make func call in another singleton script specifically designed for loading/saving checkpoint level progress
        timer = 0f;                                 //Reset timer          
        sceneName = Application.loadedLevelName;    //Set scene to current
        this.delay = delay;                         //Set time limit
        isLoading = true;                           //Begin timer
    }

    /// <summary>
    /// Sets the scene name to load
    /// </summary>
    /// <param name="scene">Scene name</param>
    public void SetScene(string scene)
    {
        sceneName = scene;
    }

    /// <summary>
    /// Loads the scene
    /// </summary>
    private void Load()
    {
        //Restore time from pause menu!
        UnityEngine.Time.timeScale = 1f;    
        SceneManager.LoadScene(sceneName);
    }
}
