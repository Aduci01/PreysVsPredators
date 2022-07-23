using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator {
    public static Sprite CreateSprite(int width, int height) {
        Texture2D tex = CreateTexture(width, height);
        return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    public static Texture2D CreateTexture(int width, int height) {
        var texture = new Texture2D(width, height, TextureFormat.ARGB32, false) {
            filterMode = FilterMode.Point
        };

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                texture.SetPixel(height, width, Color.black);
            }
        }

        texture.Apply();
        return texture;
    }
}