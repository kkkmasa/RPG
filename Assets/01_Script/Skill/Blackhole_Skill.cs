using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] GameObject blackholePrefab;
    [SerializeField] float blackholeDuration;
    [SerializeField] float maxSize;
    [SerializeField] float growSpeed;
    [SerializeField] float shrinkSpeed;
    [SerializeField] int amountOfAttacks;
    [SerializeField] float attackCooldown;

    BlackHole_Skill_Controller currentBlackhole;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(this.blackholePrefab, player.transform.position, Quaternion.identity);
        currentBlackhole = newBlackhole.GetComponent<BlackHole_Skill_Controller>();
        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, attackCooldown, blackholeDuration);


    }

    public bool SkillCompleted()
    {

        if (!currentBlackhole) return false;

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }
        return false;
    }

    public float GetBlackholeRadius() {
        return maxSize / 2;
    }

}
