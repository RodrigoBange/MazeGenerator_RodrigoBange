using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private float generationSpeed = 0.1f;

    public float GenerationSpeed
    {
        get { return generationSpeed; }
    }

    private void Awake()
    {
        instance = this;
    }
}
