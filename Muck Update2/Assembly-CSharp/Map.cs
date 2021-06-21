// Decompiled with JetBrains decompiler
// Type: Map
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
  public Transform playerIcon;
  public Transform mapParent;
  public RectTransform map;
  public RawImage mapRender;
  private float mapSize;
  private float mapRatio;
  private Vector3 maxPos;
  public Material mapTextureMaterial;
  public static Map Instance;
  private Vector2 startHoldPos;
  private Vector2 startMapPos;

  public bool active { get; set; }

  private void Awake()
  {
    Map.Instance = this;
    this.active = false;
  }

  public void GenerateMap()
  {
    this.mapSize = (float) this.map.get_sizeDelta().x;
    this.mapRatio = this.mapSize / ((float) MapGenerator.mapChunkSize * (float) MapGenerator.worldScale);
    Texture2D texture2D = TextureGenerator.ColorTextureFromHeightMap(MapGenerator.Instance.heightMap, MapGenerator.Instance.textureData);
    texture2D.set_minimumMipmapLevel(0);
    this.mapTextureMaterial.set_mainTexture((Texture) texture2D);
    ((Graphic) this.mapRender).set_material(this.mapTextureMaterial);
    this.maxPos = new Vector3(this.mapSize / 2f, this.mapSize / 2f);
  }

  private void Update()
  {
    if (!this.active)
      return;
    this.ShowPlayers();
    this.PlayerInput();
  }

  private void PlayerInput()
  {
    float y = (float) Input.get_mouseScrollDelta().y;
    Vector2 vector2_1 = Vector2.op_Implicit(Input.get_mousePosition());
    if (Input.GetMouseButtonDown(0))
    {
      this.startHoldPos = vector2_1;
      this.startMapPos = Vector2.op_Implicit(((Component) this.map).get_transform().get_position());
    }
    if (Input.GetMouseButton(0))
    {
      Vector2 vector2_2 = Vector2.op_Subtraction(this.startMapPos, Vector2.op_Subtraction(this.startHoldPos, vector2_1));
      ((Component) this.map).get_transform().set_position(Vector2.op_Implicit(vector2_2));
      Vector3 localPosition = ((Component) this.map).get_transform().get_localPosition();
      Vector3 vector3_1 = Vector3.op_Multiply(this.maxPos, (float) ((Component) this.map).get_transform().get_localScale().x);
      Vector3 vector3_2 = Vector2.op_Implicit(this.ClampVector(Vector2.op_Implicit(localPosition), Vector2.op_Implicit(vector3_1)));
      ((Component) this.map).get_transform().set_localPosition(vector3_2);
    }
    if ((double) y > 0.0)
    {
      float num = (float) (((Component) this.map).get_transform().get_localScale().x + 0.300000011920929);
      if ((double) num > 6.0)
        num = 6f;
      ((Component) this.map).get_transform().set_localScale(new Vector3(num, num, num));
    }
    else
    {
      if ((double) y >= 0.0)
        return;
      float num = (float) (((Component) this.map).get_transform().get_localScale().x - 0.300000011920929);
      if ((double) num < 0.2)
        num = 0.2f;
      ((Component) this.map).get_transform().set_localScale(new Vector3(num, num, num));
    }
  }

  private Vector2 ClampVector(Vector2 v, Vector2 max)
  {
    if (v.x > max.x)
      v.x = max.x;
    else if (v.x < -max.x)
      v.x = -max.x;
    if (v.y > max.y)
      v.y = max.y;
    else if (v.y < -max.y)
      v.y = -max.y;
    return v;
  }

  public void ToggleMap()
  {
    this.active = !this.active;
    ((Component) this.mapParent).get_gameObject().SetActive(this.active);
    if (this.active)
    {
      Cursor.set_visible(true);
      Cursor.set_lockState((CursorLockMode) 0);
    }
    else
    {
      Cursor.set_visible(false);
      Cursor.set_lockState((CursorLockMode) 1);
    }
  }

  private void ShowPlayers()
  {
    if (!Object.op_Implicit((Object) PlayerMovement.Instance))
      return;
    Vector3 position = ((Component) PlayerMovement.Instance).get_transform().get_position();
    Vector3 vector3;
    ((Vector3) ref vector3).\u002Ector((float) position.x, (float) position.z, 0.0f);
    ((Component) this.playerIcon).get_transform().set_localPosition(Vector3.op_Multiply(vector3, this.mapRatio));
    float y = (float) PlayerMovement.Instance.orientation.get_eulerAngles().y;
    ((Component) this.playerIcon).get_transform().set_localRotation(Quaternion.Euler(0.0f, 0.0f, -y));
  }

  public Map() => base.\u002Ector();
}
