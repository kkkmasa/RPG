using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] float crystalExitTimer;
    GameObject currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] bool cloneInsteadOfCrystal;

    [Header("Explosive crystal")]
    [SerializeField] bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] bool canMoveToEnemy;
    [SerializeField] float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] bool canUseMultiStacks;
    [SerializeField] int amountOfStacks;
    [SerializeField] float mulitiStackCooldown;
    [SerializeField] float useTimeWindow;
    [SerializeField] List<GameObject> crystalLeft = new List<GameObject>();


    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystals())
            return;

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
                return;

            Vector2 playerPos = player.transform.position;

            player.transform.position = currentCrystal.transform.position;

            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
        }
    }

    public void CurrentCrystalChoosenRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller crystal_Skill_Controller = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        crystal_Skill_Controller.SetupCrystal(crystalExitTimer, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform), player);
    }

    bool CanUseMultiCrystals()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {

                if (crystalLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);
                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().
                SetupCrystal(crystalExitTimer, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), player);

                if (crystalLeft.Count <= 0)
                {
                    cooldown = mulitiStackCooldown;
                    RefulCrystal();
                }
                return true;
            }
        }
        return false;
    }

    void RefulCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }
    void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = mulitiStackCooldown;
        ResetAbility();
    }
}
