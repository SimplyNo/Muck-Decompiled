// Decompiled with JetBrains decompiler
// Type: Map
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
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
  public List<Map.MapMarker> mapMarkers;
  public static Map Instance;
  private Vector2 startHoldPos;
  private Vector2 startMapPos;
  public Transform markerParent;
  public Transform playerMarkerParent;
  public GameObject mapMarkerPrefab;
  public Texture[] markerTextures;

  public bool active { get; set; } = true;

  private void Awake()
  {
    Map.Instance = this;
    this.active = false;
    this.mapMarkers = new List<Map.MapMarker>();
  }

  public void GenerateMap()
  {
    this.mapSize = this.map.sizeDelta.x;
    this.mapRatio = this.mapSize / ((float) MapGenerator.mapChunkSize * (float) MapGenerator.worldScale);
    Texture2D texture2D = TextureGenerator.ColorTextureFromHeightMap(MapGenerator.Instance.heightMap, MapGenerator.Instance.textureData);
    texture2D.minimumMipmapLevel = 0;
    this.mapTextureMaterial.mainTexture = (Texture) texture2D;
    this.mapRender.material = this.mapTextureMaterial;
    this.maxPos = new Vector3(this.mapSize / 2f, this.mapSize / 2f);
  }

  private void Update()
  {
    if (!this.active)
      return;
    this.ShowPlayers();
    this.PlayerInput();
    this.UpdateMap();
  }

  private void PlayerInput()
  {
    float y = Input.mouseScrollDelta.y;
    Vector2 mousePosition = (Vector2) Input.mousePosition;
    if (Input.GetMouseButtonDown(0))
    {
      this.startHoldPos = mousePosition;
      this.startMapPos = (Vector2) this.map.transform.position;
    }
    if (Input.GetMouseButton(0))
    {
      this.map.transform.position = (Vector3) (this.startMapPos - (this.startHoldPos - mousePosition));
      this.map.transform.localPosition = (Vector3) this.ClampVector((Vector2) this.map.transform.localPosition, (Vector2) (this.maxPos * this.map.transform.localScale.x));
    }
    if ((double) y > 0.0)
    {
      float num = this.map.transform.localScale.x + 0.3f;
      if ((double) num > 6.0)
        num = 6f;
      this.map.transform.localScale = new Vector3(num, num, num);
    }
    else
    {
      if ((double) y >= 0.0)
        return;
      float num = this.map.transform.localScale.x - 0.3f;
      if ((double) num < 0.2)
        num = 0.2f;
      this.map.transform.localScale = new Vector3(num, num, num);
    }
  }

  private Vector2 ClampVector(Vector2 v, Vector2 max)
  {
    if ((double) v.x > (double) max.x)
      v.x = max.x;
    else if ((double) v.x < -(double) max.x)
      v.x = -max.x;
    if ((double) v.y > (double) max.y)
      v.y = max.y;
    else if ((double) v.y < -(double) max.y)
      v.y = -max.y;
    return v;
  }

  public void ToggleMap()
  {
    this.active = !this.active;
    this.mapParent.gameObject.SetActive(this.active);
    if (this.active)
    {
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
    }
    else
    {
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
    }
  }

  private void UpdateMap()
  {
    foreach (Map.MapMarker mapMarker in this.mapMarkers)
    {
      if (mapMarker != null)
      {
        if ((Object) mapMarker.worldObject == (Object) null || !mapMarker.worldObject.gameObject.activeInHierarchy)
        {
          mapMarker.marker.gameObject.SetActive(false);
        }
        else
        {
          mapMarker.marker.gameObject.SetActive(true);
          mapMarker.marker.localPosition = this.WorldPositionToMap(mapMarker.worldObject.position);
        }
      }
    }
  }

  private void ShowPlayers()
  {
    if (!(bool) (Object) PlayerMovement.Instance)
      return;
    Vector3 position = PlayerMovement.Instance.transform.position;
    this.playerIcon.transform.localPosition = new Vector3(position.x, position.z, 0.0f) * this.mapRatio;
    this.playerIcon.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -PlayerMovement.Instance.orientation.eulerAngles.y);
  }

  private Vector3 WorldPositionToMap(Vector3 worldPos) => new Vector3(worldPos.x, worldPos.z, 0.0f) * this.mapRatio;

  public Map.MapMarker AddMarker(
    Transform t,
    Map.MarkerType markerType,
    Texture texture,
    Color col,
    string name = "",
    float scale = 1f)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(this.mapMarkerPrefab, this.markerParent);
    RawImage component = gameObject.GetComponent<RawImage>();
    component.texture = this.markerTextures[(int) markerType];
    component.color = col;
    gameObject.transform.localPosition = this.WorldPositionToMap(t.position);
    gameObject.transform.localScale *= scale;
    if ((Object) texture != (Object) null)
      gameObject.GetComponent<RawImage>().texture = texture;
    Map.MapMarker mapMarker = new Map.MapMarker(markerType, gameObject.transform, t);
    this.mapMarkers.Add(mapMarker);
    gameObject.GetComponentInChildren<TextMeshProUGUI>().text = name;
    if (markerType == Map.MarkerType.Player)
      gameObject.transform.SetParent(this.playerMarkerParent);
    return mapMarker;
  }

  public void RemoveMarker(Map.MapMarker marker)
  {
    if ((bool) (Object) marker.marker)
      Object.Destroy((Object) marker.marker.gameObject);
    this.mapMarkers.Remove(marker);
  }

  public enum MarkerType
  {
    Player,
    Ping,
    Gem,
    Other,
  }

  public class MapMarker
  {
    public Map.MarkerType type;
    public Transform marker;
    public Transform worldObject;

    public MapMarker(Map.MarkerType type, Transform marker, Transform worldObject)
    {
      this.type = type;
      this.marker = marker;
      this.worldObject = worldObject;
    }
  }
}
