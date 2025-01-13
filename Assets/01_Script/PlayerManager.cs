using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;
    void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;
    }
}
