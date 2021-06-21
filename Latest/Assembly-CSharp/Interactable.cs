// Decompiled with JetBrains decompiler
// Type: Interactable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
