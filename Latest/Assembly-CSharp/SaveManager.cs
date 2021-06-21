// Decompiled with JetBrains decompiler
// Type: SaveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
  public PlayerSave state;

  public static SaveManager Instance { get; set; }

  private void Awake()
  {
    if (Object.op_Inequality((Object) SaveManager.Instance, (Object) null) && Object.op_Inequality((Object) SaveManager.Instance, (Object) this))
      Object.Destroy((Object) ((Component) this).get_gameObject());
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

  public SaveManager() => base.\u002Ector();
}
