using System;
using UnityEngine;

namespace CasualtiesNewAscent;

public class SkyBg : MonoBehaviour
{
    private static Texture2D _whiteTexture = null;
    private static Sprite _whiteSprite = null;
    private GameObject _skyGo;

    private static void EnsureStatics()
    {
        if (_whiteTexture == null)
        {
            var white = Color.white;
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, white);
            texture.SetPixel(1, 0, white);
            texture.SetPixel(1, 1, white);
            texture.SetPixel(0, 1, white);
            texture.filterMode = FilterMode.Point;
            _whiteTexture = texture;
        }

        if (_whiteSprite == null)
        {
            _whiteSprite = Sprite.Create(_whiteTexture, new Rect(0.0f, 0.0f, _whiteTexture.width, _whiteTexture.height), new Vector2(0.5f, 0.5f));
        }
    }

    private void Awake()
    {
        var world = WorldGeneration.world;
        if (!world)
        {
            Destroy(this);
            return;
        }

        EnsureStatics();

        _skyGo = Utils.Create("Special/wallholes", Vector2.zero, 0.0f);
        var renderer = _skyGo.GetComponent<SpriteRenderer>();
        renderer.sprite = _whiteSprite;
        renderer.gameObject.transform.position = new Vector3(0f, world.halfHeight / 2f, 0f);
        // I have zero clue what makes this number special
        // Tiles are 48 units (I think?) and the area we want to cover is 512 tiles
        // So where's the extra 20 coming from?
        renderer.gameObject.transform.localScale = new Vector3(532f * 48f, 532f * 48f, 1f);
    }

    private void Update()
    {
        // Laaaaaazy
        var world = WorldGeneration.world;
        if (!world || !Plugin.CheckIfNewLayer(world.biomeDepth) || !Plugin.CheckIfLastLayer(world.biomeDepth))
        {
            Destroy(this);
            return;
        }
    }

    private void OnDestroy()
    {
        Destroy(_skyGo);
    }
}