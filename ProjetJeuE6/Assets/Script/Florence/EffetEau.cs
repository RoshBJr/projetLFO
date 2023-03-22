using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffetEau : MonoBehaviour
{
    // Script de l'effet du mouvement de l'eau

    //Programmeur Florence

    public float fps = 30.0f;         //Fps du mouvement de l'eau
    public Texture2D[] frames;      //Images du mouvement

    private int frameIndex;
    private Projector projector;    //Projector GameObject

    void Start()
    {
        projector = GetComponent<Projector>();
        NextFrame();
        InvokeRepeating("NextFrame", 1 / fps, 1 / fps);
    }

    void NextFrame()
    {
        projector.material.SetTexture("_ShadowTex", frames[frameIndex]);
        frameIndex = (frameIndex + 1) % frames.Length;
    }

}

