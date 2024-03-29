﻿// Decompiled with JetBrains decompiler
// Type: hahahayes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class hahahayes : MonoBehaviour
{
  public GameObject interact;
  public Renderer woodmanRend;
  public MeshRenderer rend;
  public MeshFilter filter;
  public Mesh axe;
  public Mesh pickaxe;
  public Mesh sword;
  public Mesh bread;
  public Mesh bow;
  public Mesh chest;
  public Material[] axeMats;
  public Material[] pickaxeMats;
  public Material[] swordMats;
  public Material[] breadMat;
  public Material[] bowMats;
  public Material[] chestMats;
  public MeshFilter hatFilter;
  public MeshRenderer hatRender;
  public GameObject helmet;
  public GameObject pants;
  public GameObject shoes;
  public Mesh archerHat;
  public Mesh bean;
  public Mesh meat;
  public Mesh tree;
  public Material archerHatMat;
  public Material beanMat;
  public Material meatMat;
  public Material treeMat;
  private WoodmanBehaviour.WoodmanType type;

  public void Randomize(ConsistentRandom rand)
  {
    switch (this.type)
    {
      case WoodmanBehaviour.WoodmanType.Archer:
        this.filter.mesh = this.bow;
        this.rend.material = this.GetMaterial(this.bowMats, rand);
        this.hatFilter.mesh = this.archerHat;
        this.hatRender.material = this.archerHatMat;
        break;
      case WoodmanBehaviour.WoodmanType.Smith:
        this.filter.mesh = this.pickaxe;
        this.rend.material = this.GetMaterial(this.pickaxeMats, rand);
        this.helmet.SetActive(true);
        break;
      case WoodmanBehaviour.WoodmanType.Woodcutter:
        this.filter.mesh = this.axe;
        this.rend.material = this.GetMaterial(this.axeMats, rand);
        this.shoes.SetActive(true);
        this.hatFilter.mesh = this.tree;
        this.hatRender.material = this.treeMat;
        this.hatRender.transform.localPosition = new Vector3(-0.03f, 1.27f, 0.11f);
        this.hatRender.transform.localScale = Vector3.one * 0.455f;
        this.hatRender.transform.rotation = Quaternion.Euler(0.0f, -132f, 0.0f);
        break;
      case WoodmanBehaviour.WoodmanType.Chef:
        this.filter.mesh = this.bread;
        this.rend.material = this.breadMat[0];
        this.rend.transform.localScale *= 0.5f;
        this.hatFilter.mesh = this.meat;
        this.hatRender.material = this.meatMat;
        this.hatRender.transform.localPosition = new Vector3(0.0f, 3.2f, 0.0f);
        this.hatRender.transform.localScale = Vector3.one * 1.9f;
        this.hatRender.transform.rotation = Quaternion.Euler(0.0f, -90f, -90f);
        break;
      case WoodmanBehaviour.WoodmanType.Wildcard:
        this.filter.mesh = this.bean;
        this.rend.material = this.beanMat;
        this.pants.SetActive(true);
        break;
      default:
        this.filter.mesh = this.bean;
        this.rend.material = this.beanMat;
        break;
    }
  }

  public void SetType(WoodmanBehaviour.WoodmanType type) => this.type = type;

  public void SkinColor(ConsistentRandom rand)
  {
    Material material = this.woodmanRend.materials[0];
    float num = (float) rand.NextDouble();
    material.color = (material.color + new Color(num, num, num)) / 2f;
    this.woodmanRend.materials[0] = material;
  }

  private Material GetMaterial(Material[] mats, ConsistentRandom rand) => mats[rand.Next(0, mats.Length)];
}
