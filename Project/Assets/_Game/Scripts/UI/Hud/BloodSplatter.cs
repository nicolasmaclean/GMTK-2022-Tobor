using System.Collections;
using System.Collections.Generic;
using Game.Utility.UI;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class BloodSplatter : MonoBehaviour
{
    [SerializeField]
    SOSpriteAnimation _animation;

    [SerializeField]
    float _size = 1000f;

    [SerializeField]
    Color _color;

    [SerializeField]
    Vector4 _margin = new Vector4(100, 100, -100, -100);

    [SerializeField]
    Vector2 _centerClear = new Vector2(140, 180);
    
    public void Splat()
    {
        AnimatedImage img = AnimatedImage.Create(_animation);
        img.IsOneShot = true;
        Destroy(img.gameObject, _animation.Frames.Length / _animation.Fps);
        img.GetComponent<Image>().color = _color;
        
        RectTransform t = img.transform as RectTransform;
        t.sizeDelta = Vector2.one * _size / 2;

        Vector2 pos;
        if (Random.Range(0, 2) == 1)
        {
            pos = new Vector2(
                Random.Range(_margin.x, 1920 - _margin.z), 
                Random.Range(_margin.w, 1080 / 2 - _centerClear.y)
            );
        }
        else
        {
            pos = new Vector2(
                Random.Range(_margin.x, 1920 - _margin.z), 
                Random.Range(1080 / 2 + _centerClear.x, 1080 - _margin.y)
            );
        }
        
        t.SetParent(transform);
        t.anchorMin = t.anchorMax = Vector2.zero;
        t.anchoredPosition = pos;
    }
}
