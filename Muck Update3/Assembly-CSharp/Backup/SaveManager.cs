// Decompiled with JetBrains decompiler
// Type: SaveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
  public PlayerSave state;

  public static SaveManager Instance { get; set; }

  private void Awake()
  {
    if ((Object) SaveManager.Instance != (Object) null && (Object) SaveManager.Instance != (Object) this)
      Object.Destroy((Object) this.gameObject);
    else
      SaveManager.Instance = this;
    this.Load();
  }

  public void Save() => PlayerPrefs.SetString("save", this.Serialize<PlayerSave>(this.state));

  public void Load()
  {
    if (PlayerPrefs.HasKey("save"))
      this.state = this.Deserialize<PlayerSave>(PlayerPrefs.GetString("save"));
    else
      this.NewSave();
  }

  public void NewSave()
  {
    this.state = new PlayerSave();
    this.Save();
    MonoBehaviour.print((object) "Creating new save file");
  }

  public string Serialize<T>(T toSerialize)
  {
    XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
    StringWriter stringWriter1 = new StringWriter();
    StringWriter stringWriter2 = stringWriter1;
    // ISSUE: variable of a boxed type
    __Boxed<T> local = (object) toSerialize;
    xmlSerializer.Serialize((TextWriter) stringWriter2, (object) local);
    return stringWriter1.ToString();
  }

  public T Deserialize<T>(string toDeserialize) => (T) new XmlSerializer(typeof (T)).Deserialize((TextReader) new StringReader(toDeserialize));
}
