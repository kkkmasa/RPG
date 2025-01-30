using UnityEngine;

public class EnemyStats : CharacterStats
{
    Enemy enemy;
    [Header("Level details")]
    [SerializeField] int level = 1;

    [Range(0f, 1f)]
    [SerializeField] float percentageModifier = 0.4f;

    protected override void Start()
    {
        ApplyLevelModifiers();
        base.Start();

        enemy = GetComponent<Enemy>();
    }

    private void ApplyLevelModifiers()
    {
        this.Modify(strength);
        this.Modify(agility);
        this.Modify(intelgence);
        this.Modify(vitality);

        this.Modify(damage);
        this.Modify(critChance);
        this.Modify(critPower);

        this.Modify(maxHealth);
        this.Modify(armor);
        this.Modify(evasion);
        this.Modify(magicResistance);

        this.Modify(fireDamage);
        this.Modify(iceDamage);
        this.Modify(lightingDamage);
    }

    void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;
            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }
    public override void Die()
    {
        base.Die();
        enemy.Die();
    }

}
