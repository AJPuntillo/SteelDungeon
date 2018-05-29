using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {

    [Header("Item Name ('None' if not applicable)")]
    public string nameTag = "None";
    [Header("Item Bonus Stat")]
    public bool hasBonus = false;
    [Header("Item Max Value (0 if not applicable)")]
    public int max = 0;
    [Header("Item Uses (0 if not applicable)")]
    public int uses = 0;
    [Header("Item Max Health (0 if not applicable)")]
    public int maxHealth = 0;
    [Header("Item Damage (0 if not applicable)")]
    public int damage = 0;
    [Header("Item Defense (0 if not applicable)")]
    public int defense = 0;
    [Header("Item Move Speed (0 if not applicable)")]
    public float moveSpeed = 0;
    [Header("Item Attack Speed (0 if not applicable)")]
    public float attackSpeed = 0;
    [Header("Item Stun Duration (0 if not applicable)")]
    public float stunDuration = 0;
    [Header("Rarity")]
    public Color color = Color.white;
    [Header("Text Pop Up")]
    public GameObject text;

    private int maxChance = 60;
    private int bonusChance = 80;
    private GameObject playerUI;
    private Text statsText;

    void Start()
    {
        playerUI = GameObject.FindWithTag("PlayerUI");
        statsText = playerUI.transform.GetChild(2).GetChild(0).GetComponent<Text>();

        if (text != null)
        {
            text.SetActive(false);
        }

        CreateItem();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            text.SetActive(true);
        }
        if (other.tag == "PlayerWeapon" && text.activeSelf)
        {
            Transform sprite = playerUI.transform.GetChild(0);
            if (gameObject.tag == "Weapon")
            {
                sprite.GetChild(2).GetChild(0).GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                other.transform.parent.GetComponent<PlayerItem>().RemoveBuffs();
                other.transform.parent.GetComponent<PlayerItem>().weapon = this;
                other.transform.parent.GetComponent<PlayerItem>().weaponStats = text.GetComponent<TextMesh>().text;
                other.transform.parent.GetComponent<PlayerItem>().ApplyBuffs();
                Destroy(gameObject);
            }
            if (gameObject.tag == "Armor")
            {
                sprite.GetChild(3).GetChild(0).GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                other.transform.parent.GetComponent<PlayerItem>().RemoveBuffs();
                other.transform.parent.GetComponent<PlayerItem>().armor = this;
                other.transform.parent.GetComponent<PlayerItem>().armorStats = text.GetComponent<TextMesh>().text;
                other.transform.parent.GetComponent<PlayerItem>().ApplyBuffs();
                Destroy(gameObject);
            }
            if (gameObject.tag == "Consumable")
            {
                sprite.GetChild(4).GetChild(0).GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                sprite.GetChild(4).GetChild(0).GetChild(0).GetComponent<Text>().enabled = true;
                sprite.GetChild(4).GetChild(0).GetChild(0).GetComponent<Text>().text = uses.ToString();
                other.transform.parent.GetComponent<PlayerItem>().RemoveBuffs();
                other.transform.parent.GetComponent<PlayerItem>().consumable = this;
                other.transform.parent.GetComponent<PlayerItem>().consumableStats = text.GetComponent<TextMesh>().text;
                other.transform.parent.GetComponent<PlayerItem>().ApplyBuffs();
                Destroy(gameObject);
            }

            statsText = other.transform.parent.GetComponent<PlayerItem>().WriteStats(statsText);            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            text.SetActive(false);
        }
    }

    // SPECIAL WEAPON
    public void UseSpecialWeapon(Player player)
    {

    }

    // CONSUMABLE
    public void UseConsumable(Player player)
    {
        if (nameTag == "Health Potion")
        {
            if (uses > 0)
            {
                if (player.health != player.maxHealth && !player.dead)
                {
                    player.health += 1;
                    uses--;
                    playerUI.transform.GetChild(0).GetChild(4).GetChild(0).GetChild(0).GetComponent<Text>().text = uses.ToString();
                }
            }
        }
    }

    float CreateRandomStat(float min_, float max_)
    {
        if (min_ != 0)
        {
            float min = min_;
            float max = max_;

            if (max == 0)
            {
                max = min;
            }

            float rand = Random.Range(min, max);
            int chance = Random.Range(0, 100);

            if (chance >= maxChance)
            {
                if(Mathf.Round(rand) < min_)
                {
                    return min_;
                }
                return Mathf.Round(rand);
            }
            else
            {
                return min_;
            }
        }
        else
        {
            return min_;
        }
    }

    int CreateRandomStat(int min_, int max_)
    {
        if (min_ != 0)
        {
            int min = min_;
            int max = max_;

            if (max == 0)
            {
                max = min;
            }

            int rand = Random.Range(min, max + 1);
            int chance = Random.Range(0, 100);

            if (chance > maxChance)
            {
                return rand;
            }
            else
            {
                return min_;
            }
        }
        else
        {
            return min_;
        }
    }

    void GenerateText()
    {
        text.GetComponent<TextMesh>().text = "";
        if (uses > 0)
        {
            text.GetComponent<TextMesh>().text += "+" + uses + " Use\n" + nameTag + "\n";
        }
        if (maxHealth > 0)
        {
            text.GetComponent<TextMesh>().text += "+" + maxHealth + "\nMax Health\n";
        }
        if (damage > 0)
        {
            text.GetComponent<TextMesh>().text += "+" + damage + "\nDamage\n";
        }
        if (defense > 0)
        {
            text.GetComponent<TextMesh>().text += "+" + defense + "\nDefense\n";
        }
        if (moveSpeed > 0)
        {
            text.GetComponent<TextMesh>().text += "+" + moveSpeed + "\nMove Speed\n";
        }
        if (attackSpeed != 0)
        {
            text.GetComponent<TextMesh>().text += "+" + attackSpeed + "\nAttack Speed\n";
        }
        if (stunDuration > 0)
        {
            text.GetComponent<TextMesh>().text += "+" + stunDuration + "\nStun Duration\n";
        }
    }

    public void CreateItem()
    {
        uses = CreateRandomStat(uses, max);
        maxHealth = CreateRandomStat(maxHealth, max);
        damage = CreateRandomStat(damage, max);
        defense = CreateRandomStat(defense, max);
        moveSpeed = CreateRandomStat(moveSpeed, max);
        //attackSpeed = CreateRandomStat(attackSpeed, max);
        //stunDuration = CreateRandomStat(stunDuration, max);

        if (hasBonus)
        {
            int chance = Random.Range(0, 100);

            if(chance > bonusChance)
            {
                chance = Random.Range(0, 6);

                if(chance == 0)
                {
                    maxHealth += 1;
                }
                if (chance == 1)
                {
                    damage += 1;
                }
                if (chance == 2)
                {
                    defense += 1;
                }
                if (chance == 3)
                {
                    moveSpeed += 10;
                }
                if (chance == 4)
                {
                    attackSpeed -= 0.2f;
                }
                if (chance == 5)
                {
                    stunDuration += 0.2f;
                }
            }
        }

        if (text != null)
        {
            GenerateText();
        }
    }
}
