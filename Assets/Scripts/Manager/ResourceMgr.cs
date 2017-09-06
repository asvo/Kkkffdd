using UnityEngine;
using System.Collections;
using System.IO;

public class ResourceMgr : Single<ResourceMgr> {

	
    public Sprite LoadSpriteByFile(string fileName)
    {
        FileStream fs = new FileStream(Application.dataPath + "/Arts/scene/" + fileName + ".png", FileMode.Open,
            FileAccess.Read);
        fs.Seek(0, SeekOrigin.Begin);

        byte[] bytes = new byte[fs.Length];
        fs.Read(bytes, 0, (int)fs.Length);
        fs.Close();
        fs.Dispose();
        fs = null;

        int wid = 4800, hei = 1080;
        Texture2D t2d = new Texture2D(4800, 1080);
        t2d.LoadImage(bytes);

        Sprite sp = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));

        return sp;
    }

}
