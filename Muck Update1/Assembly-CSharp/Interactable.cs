// Decompiled with JetBrains decompiler
// Type: Interactable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

public interface Interactable
{
  void Interact();

  void LocalExecute();

  void AllExecute();

  void ServerExecute();

  void RemoveObject();

  string GetName();
}
