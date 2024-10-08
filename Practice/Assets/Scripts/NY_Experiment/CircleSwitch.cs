using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CircleSwitch;

public class CircleSwitch : MonoBehaviour
{
    
    private Vector2 targetCenter = new Vector2(0, 0);   // The center position of your target (in world space)
    private float[] circleRadii = new float[] { 2f, 5f, 9f, 12f };    // Array holding the radius of each circle

    public Transform[] units; // Reference to the player character
    public List<E_CircleType> circleTypes = new List<E_CircleType>();
    public SpriteRenderer[] circleSpriteRenderer;
    public float attackCircleDamage = 3f;
    public float healCircleAmount = 3f;
    public float buffCircleAmount = 3f;

    
    public enum E_CircleType
    {
        attack,
        heal,
        damageBuff,
        BullsEye
    }
    public void Start()
    {
        //GenerateRandomCircleTypes();

        List<E_CircleType> otherTypes = new List<E_CircleType>
        {
            E_CircleType.BullsEye,
            E_CircleType.heal,
            E_CircleType.attack,
            E_CircleType.damageBuff
        };

        circleTypes = otherTypes;
    }

    private void GenerateRandomCircleTypes()
    {
        circleTypes.Clear(); // Clear the list before generating new values
        circleTypes.Add(E_CircleType.BullsEye); // Add BullsEye at index 0

        // Create a list of the other circle types
        List<E_CircleType> otherTypes = new List<E_CircleType>
        {
            E_CircleType.attack,
            E_CircleType.heal,
            E_CircleType.damageBuff
        };

        // Reuseable shuffle function
        ShuffleList(otherTypes);

        // Add the shuffled other types to the circleTypes list
        circleTypes.AddRange(otherTypes);
    }
    private void ShuffleList<T>(List<T> list)
    {
        // Create a new instance of Random for shuffling
        System.Random rng = new System.Random();
        int n = list.Count;

        // Perform Fisher-Yates shuffle
        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            // Swap list[i] with list[j]
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    public void WhichUnitIsInWhichCircle()
    {
        foreach (var unit in units)
        {
            // Calculate the distance between the player and the center of the target
            float distanceFromCenter = Vector2.Distance(unit.position, targetCenter);

            int circleIndex = GetCircleIndex(distanceFromCenter);

            if (circleIndex >= 0)
            {
                FunctionToCall(circleTypes[circleIndex], unit);
            }
        }

    }
    
    public void FunctionToCall(E_CircleType circleType, Transform unit)
    {
        if(circleType == E_CircleType.attack)
        {
            AttackCircle(unit);       
        }
        else if(circleType == E_CircleType.heal)
        {
            HealCircle(unit);
        }
        else if(circleType == E_CircleType.damageBuff)
        {
            DamageBuffCircle(unit);
        }
        else if(circleType == E_CircleType.BullsEye)
        {
            BullsEyeCircle(unit);
        }
    }
    // Helper function to find which circle the player is in
    int GetCircleIndex(float distance)
    {
        // Iterate over the circle radii
        for (int i = 0; i < circleRadii.Length; i++)
        {
            if (distance <= circleRadii[i])
            {
                return i;
            }
        }
        // Return -1 if outside of all circles
        return -1;
    }
    public void changeCircleColor(E_CircleType circleType, int index)
    {
        if (circleType == E_CircleType.attack)
        {
            circleSpriteRenderer[index].color = Color.red;
        }
        else if (circleType == E_CircleType.heal)
        {
            circleSpriteRenderer[index].color = Color.green;
        }
        else if (circleType == E_CircleType.damageBuff)
        {
            circleSpriteRenderer[index].color = Color.blue;
        }
        else if (circleType == E_CircleType.BullsEye)
        {
            circleSpriteRenderer[index].color = Color.black;
        }
    }
    public void AttackCircle(Transform unit)
    {
        MonsterStat monsterStat = unit.gameObject.GetComponent<MonsterStat>();
        monsterStat.TakeDamage(attackCircleDamage);
        Debug.Log(unit.gameObject.name + "took damage");
    }

    public void HealCircle(Transform unit)
    {
        MonsterStat monsterStat = unit.gameObject.GetComponent<MonsterStat>();
        monsterStat.TakeHeal(healCircleAmount);
        Debug.Log(unit.gameObject.name + "took heal");
    }

    public void DamageBuffCircle(Transform unit)
    {
        MonsterStat monsterStat = unit.gameObject.GetComponent<MonsterStat>();
        monsterStat.IncreaseDamageMultiplier(buffCircleAmount);
        Debug.Log(unit.gameObject.name + "took damage buff");
    }

    public void BullsEyeCircle(Transform unit)
    {
        if(unit.gameObject.tag == "Enemy")
        {
            foreach (var character in units)
            {
                if (character.gameObject.tag == "Player") 
                {
                    MonsterStat monsterStat = unit.gameObject.GetComponent<MonsterStat>();
                    monsterStat.TakeDamage(attackCircleDamage);
                    Debug.Log(unit.gameObject.name + "took damage from enemy's bull's eye");
                }
            }
        }
        else if(unit.gameObject.tag == "Player")
        {
            foreach (var character in units)
            {
                if (character.gameObject.tag == "Enemy")
                {
                    MonsterStat monsterStat = unit.gameObject.GetComponent<MonsterStat>();
                    monsterStat.TakeDamage(attackCircleDamage);
                    Debug.Log(unit.gameObject.name + "took damage from player's bull's eye");
                }
            }
        }
        
    }
}
