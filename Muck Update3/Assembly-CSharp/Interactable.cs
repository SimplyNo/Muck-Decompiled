// Decompiled with JetBrains decompiler
// Type: Interactable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

public interface Interactable
{
  void Interact();

  void LocalExecute();

  void AllExecute();

  void ServerExecute(int fromClient = -1);

  void RemoveObject();

  string GetName();

  bool IsStarted();
}
