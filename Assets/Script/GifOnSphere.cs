using UnityEngine;

public class GifLoader : MonoBehaviour
{
    public float frameRate = 10f;

    private Texture2D[] frames;
    private Renderer rend;
    private int currentFrame = 0;
    private float timer = 0f;

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Carica tutti i frame dalla cartella Resources/SplitGIF
        frames = Resources.LoadAll<Texture2D>("SplitGIF");

        if (frames.Length == 0)
        {
            Debug.LogError(" Nessun frame trovato in Resources/SplitGIF!");
        }
    }

    void Update()
    {
        if (frames == null || frames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            currentFrame = (currentFrame + 1) % frames.Length;
            rend.material.mainTexture = frames[currentFrame];
            timer = 0f;
        }
    }
}
