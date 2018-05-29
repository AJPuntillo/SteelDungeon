using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerItem : MonoBehaviour {

    // The player's three item slots
    [HideInInspector]
    public Item weapon;
    [HideInInspector]
    public Item armor;
    [HideInInspector]
    public Item consumable;

    [HideInInspector]
    public string weaponStats;
    [HideInInspector]
    public string armorStats;
    [HideInInspector]
    public string consumableStats;

    // Use this for initialization
    void Start () {
        weapon = gameObject.AddComponent<Item>();
        armor = gameObject.AddComponent<Item>();
        consumable = gameObject.AddComponent<Item>();
        weaponStats = "";
        armorStats = "";
        consumableStats = "";
    }
	
	// Update is called once per frame
	void Update () {
        // Update speical weapon (ex. projectiles)
        weapon.UseSpecialWeapon(gameObject.GetComponent<Player>());
        // Update item on button down
        /*
		if(CrossPlatformInputManager.GetAxis("Item") > 0)
        {
            consumable.UseConsumable(gameObject.GetComponent<Player>());
        }
        */
        if (Input.GetButtonDown("Item"))
        {
            consumable.UseConsumable(gameObject.GetComponent<Player>());
        }
        
    }

    public void ApplyBuffs()
    {
        gameObject.GetComponent<Player>().maxHealth += weapon.maxHealth + armor.maxHealth + consumable.maxHealth;
        gameObject.GetComponent<Player>().damage += weapon.damage + armor.damage + consumable.damage;
        gameObject.GetComponent<Player>().defense += weapon.defense + armor.defense + consumable.defense;
        gameObject.GetComponent<Player>().attackSpeed += -weapon.attackSpeed + armor.attackSpeed + consumable.attackSpeed;
        gameObject.GetComponent<Player>().stunDuration += weapon.stunDuration + armor.stunDuration + consumable.stunDuration;
        gameObject.GetComponent<Player>().moveSpeed += weapon.moveSpeed + armor.moveSpeed + consumable.moveSpeed;
    }

    public void RemoveBuffs()
    {
        gameObject.GetComponent<Player>().maxHealth -= weapon.maxHealth + armor.maxHealth + consumable.maxHealth;
        gameObject.GetComponent<Player>().damage -= weapon.damage + armor.damage + consumable.damage;
        gameObject.GetComponent<Player>().defense -= weapon.defense + armor.defense + consumable.defense;
        gameObject.GetComponent<Player>().attackSpeed -= -weapon.attackSpeed + armor.attackSpeed + consumable.attackSpeed;
        gameObject.GetComponent<Player>().stunDuration -= weapon.stunDuration + armor.stunDuration + consumable.stunDuration;
        gameObject.GetComponent<Player>().moveSpeed -= weapon.moveSpeed + armor.moveSpeed + consumable.moveSpeed;
    }

    public Text WriteStats(Text statsText)
    {
        statsText.text = "";
        if (weaponStats != "")
        {
            statsText.text += "--WEAPON--\n";
            statsText.text += weaponStats + "\n";
        }
        if (armorStats != "")
        {
            statsText.text += "--ARMOR--\n";
            statsText.text += armorStats + "\n";
        }
        if (consumableStats != "")
        {
            statsText.text += "--USE--\n";
            statsText.text += consumableStats;
        }

        return statsText;
    }
}
