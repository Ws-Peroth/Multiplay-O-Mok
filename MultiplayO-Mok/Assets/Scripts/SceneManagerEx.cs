using UnityEngine.SceneManagement;


public enum SceneTypes
{
    Setup,
    Lobby,
    Game
}
public class SceneManagerEx : Singleton<SceneManagerEx>
{
    public static void SceneChange(SceneTypes scene)
    {
        SceneManager.LoadScene((int) scene);
    }
}
