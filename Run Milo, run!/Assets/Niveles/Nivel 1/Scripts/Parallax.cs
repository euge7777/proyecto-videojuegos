using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Material material;
    public float speed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}
