using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Vector3 lastCamPos;
    private Transform cam;
    private float textureSize;
    [SerializeField] private Vector2 parallaxEffect;

    private void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture2D = sprite.texture;
        textureSize = texture2D.width / sprite.pixelsPerUnit;
        GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
        Vector2 g = GetComponent<SpriteRenderer>().size;
        g.x *= 3;
        GetComponent<SpriteRenderer>().size = g;

    }
    private void Update()
    {
        Vector3 deltaMovement = cam.position - lastCamPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffect.x, deltaMovement.y * parallaxEffect.y);
        lastCamPos = cam.position;

        if(Mathf.Abs(cam.position.x - transform.position.x) >= textureSize)
        {
            float offset = (cam.position.x - transform.position.x) % textureSize;
            //transform.position = new Vector3(cam.position.x + offset, transform.position.y);
        }
    }
}
