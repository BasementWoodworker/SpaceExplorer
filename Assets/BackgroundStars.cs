using UnityEngine;

public class BackgroundStars : MonoBehaviour
{
    public Sprite starImg;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        Debug.Log(renderer.bounds.size);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
