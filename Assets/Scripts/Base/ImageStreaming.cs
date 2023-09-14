using Base;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ImageStreaming: MonoBehaviour
{
    public string url;
    public bool fillAspectRatio = false;
    public bool fullSize = false;
    public bool localAsset = false;
    public bool initEmpty = false;
    public Button btnClick;

    private Image image;
    private int pixelsPerUnit = 100;
    private uint extrude = 0;
    private SpriteMeshType meshType = SpriteMeshType.FullRect;
    private Vector3 scale;
    private Texture2D texture;

    public Image Image
    {
        get
        {
            if (image == null)
                image = GetComponent<Image>();
            return image;
        }
    }


    void Start()
    {
        var rect = gameObject.GetComponent<RectTransform>();
        image = GetComponent<Image>();
        if (initEmpty && image != null) image.sprite = Resources.Load<Sprite>("empty");
        
        if (rect != null) scale = rect.localScale;

        if (!string.IsNullOrEmpty(url))
            StartCoroutine(load());
    }

    public string setUrl
    {
        set
        {
            url = value;
            if (gameObject.active)
            {
                StartCoroutine(load());
            }
        }
    }
    
    private IEnumerator load()
    {
        if (url != "")
        {
            var loadurl = url;
            
            if (localAsset) loadurl = Application.streamingAssetsPath + "/" + url;

#if !UNITY_EDITOR && UNITY_WEBGL
            var www = new WWW(loadurl, null, AssetManager.Headers);
#else
            var www = new WWW(loadurl);
#endif
            yield return www;

            if (www.error != null)
            {
                //image.enabled = false;
            }
            else
            {
                texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                www.LoadImageIntoTexture(texture);
                image.sprite = Sprite.Create(texture,
                    new Rect(0, 0, texture.width, texture.height),
                    Vector2.one / 2,
                    pixelsPerUnit,
                    extrude,
                    meshType);

                resize();
            }
        }
    }
    public void resize()
    {
        var rect = gameObject.GetComponent<RectTransform>();
        if (rect == null) return;

        if (fullSize)
        {
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        if (fillAspectRatio)// && texture != null)
        {
            /*
            float d1 = (float)texture.width / (float)texture.height;
            float d2 = rect.rect.width / rect.rect.height;
            if (d2 > d1)
                rect.transform.localScale = scale * (d2 / d1);
            else
                rect.transform.localScale = scale * (d1 / d2);*/
            image.type = Image.Type.Simple;
            image.preserveAspect = true;
        }
    }

    public void setTypeFilled(Image.Type type)
    {
        image.type = type;
    }
}
