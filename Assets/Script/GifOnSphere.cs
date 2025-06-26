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
        frames = Resources.LoadAll<Texture2D>("SplitGIF");

        if (frames.Length == 0)
        {
            Debug.LogError("No frames found in Resources/SplitGIF!");
            return;
        }

        // Create a material with Unlit shader
        // You can also use "Universal Render Pipeline/Unlit" if using URP
        Material unlitMat = new Material(Shader.Find("Unlit/Texture"));
        unlitMat.mainTexture = frames[0];  // Set the first frame initially
        rend.material = unlitMat;
    }

    void Update()
    {
        if (frames == null || frames.Length == 0) return;

        Debug.Log("Loading frames to update the magic sphere");

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            currentFrame = (currentFrame + 1) % frames.Length;
            rend.material.mainTexture = frames[currentFrame];
            timer = 0f;
        }
    }
}
