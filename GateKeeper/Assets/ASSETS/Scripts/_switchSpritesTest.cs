using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System;

public class _switchSpritesTest : MonoBehaviour
{
    public SpriteAtlas _sprite;
    public Texture2D spriteSheet;

    public Sprite playerS;

    [Tooltip("This is the standard SpriteSheet where the script gets its indexes from.")]
    public string defaultSpriteSheetName;
    public string spriteSheetName;
    public bool switchSprite;
    Sprite[] subSprites;
    Sprite[] defaultSubSprites;
    Sprite[] newSpriteArray;

    private void Start()
    {
        //subSprites = Resources.LoadAll<Sprite>(spriteSheet.name);
        //defaultSubSprites = Resources.LoadAll<Sprite>("ASSETS/Sprites/" + defaultSpriteSheetName);
        //print(subSprites.Length);

        
    }




    void LateUpdate()
    {
        

        //if (switchSprite) 
        //{                 
        //    foreach (var renderer in GetComponents<SpriteRenderer>())
        //    {
        //        int spriteIndex = Array.IndexOf(defaultSubSprites, renderer.sprite);
        //        var newSprite = subSprites[spriteIndex];
        //        if (newSprite) { renderer.sprite = newSprite; }
        //    }
        //    switchSprite = false;
        //}
        

    }
}