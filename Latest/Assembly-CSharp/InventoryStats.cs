// Decompiled with JetBrains decompiler
// Type: InventoryStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class InventoryStats : MonoBehaviour
{
  public TextMeshProUGUI text;
  public TextMeshProUGUI numbersText;

  private void OnEnable() => this.UpdateStats();

  private void UpdateStats()
  {
    int maxHp = PlayerStatus.Instance.maxHp;
    int maxShield = PlayerStatus.Instance.maxShield;
    int num1 = (int) (100.0 * (double) PlayerStatus.Instance.GetArmorRatio());
    int num2 = (int) (100.0 * (double) PowerupInventory.Instance.GetStrengthMultiplier((int[]) null)) - 100;
    float num3 = (float) (int) (100.0 * (double) PowerupInventory.Instance.GetCritChance());
    int num4 = (int) (100.0 * (double) PowerupInventory.Instance.GetAttackSpeedMultiplier((int[]) null)) - 100;
    int num5 = (int) (100.0 * (double) PowerupInventory.Instance.GetSpeedMultiplier((int[]) null)) - 100;
    int maxHit = this.FindMaxHit();
    ((TMP_Text) this.text).set_text("HP\nShield\nArmor\nStrength\nCritical%\nAttack Speed\nSpeed\nMax Hit");
    ((TMP_Text) this.numbersText).set_text(string.Format("{0}\n{1}\n{2}\n{3}\n{4}%\n{5}\n{6}\n{7}", (object) maxHp, (object) maxShield, (object) num1, (object) num2, (object) num3, (object) num4, (object) num5, (object) maxHit));
  }

  private int FindMaxHit()
  {
    InventoryItem currentItem = Hotbar.Instance.currentItem;
    return (int) ((!Object.op_Equality((Object) currentItem, (Object) null) ? (double) currentItem.attackDamage : 1.0) * (double) PowerupCalculations.Instance.GetMaxMultiplier().damageMultiplier);
  }

  public InventoryStats() => base.\u002Ector();
}
