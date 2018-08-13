using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveRenderTexture : MonoBehaviour {

    public RenderTexture renderTexture;
    public string outputDirectory;
    public float interval = 0.1f;

    private int frameIndex = 0;
    private IEnumerator saveFrameCoroutine;

    private List<Texture2D> textures = new List<Texture2D>();

	// Use this for initialization
	void Start () {
        saveFrameCoroutine = SaveFrame(interval);
        StartCoroutine(saveFrameCoroutine);
	}
	
	// Update is called once per frame
	void Update () {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     SaveToFile(renderTexture, outputDirectory + "motion" + frameIndex + ".png");
        //     ++frameIndex;
        // }
	}

    IEnumerator SaveFrame(float interval)
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                SaveToFile(renderTexture, outputDirectory + "motion" + frameIndex + ".png");
                ++frameIndex;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                textures.Clear();
            }
            yield return new WaitForSecondsRealtime(interval);
        }
    }

    public void SaveToFile(RenderTexture renderTexture, string name)
    {
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = renderTexture;
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBAFloat, false);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        textures.Add(tex);

        // var bytes = tex.EncodeToPNG();
        // System.IO.File.WriteAllBytes(name, bytes);
        // UnityEngine.Object.Destroy(tex);
        RenderTexture.active = currentActiveRT;
    }

    private void OnApplicationQuit()
    {
        PixelTableWriter.write(textures, outputDirectory + "table.txt", 16);
    }
}
