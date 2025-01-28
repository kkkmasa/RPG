using Unity.Mathematics;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    EntityFX fx;

    [Header("メジャー")]
    public Stat strength;
    public Stat agility;
    public Stat intelgence;
    public Stat vitality;

    [Header("オフェンス")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;

    [Header("ディフェンス")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    [SerializeField] float alimentDuration = 4;
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isIgnited; //ダメージの時間が伸びる
    public bool isChilled; //鎧を20%下げる
    public bool isShocked; //命中率を20%下げる

    float ignitedTimer;
    float chillTimer;
    float shockTimer;

    float igniteDamageCooldown = .3f;
    float igniteDamageTimer;
    int igniteDamage;
    [SerializeField] GameObject showckStrikPrifab;
    int shockDamage;

    public int currentHealth;
    protected bool isDead;

    public System.Action onHealthChanged;

    protected virtual void Start()
    {
        currentHealth = GetMaxHealthValue();
        critPower.SetDefaultValue(150);

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chillTimer -= Time.deltaTime;
        shockTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chillTimer < 0)
            isChilled = false;

        if (shockTimer < 0)
            isShocked = false;

        if (isIgnited)
        {
            ApplyIgniteDamage();
        }

    }

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0 && isIgnited)
        {
            DecreaseHealthBy(igniteDamage);

            if (currentHealth <= 0)
                Die();

            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalcCritDamage(totalDamage);
        }
        totalDamage = CheckArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        // 燃える剣などで追加ダメージがある場合
        //DoMagicalDamage(_targetStats);
    }

    #region Magical Damage

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + this.intelgence.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        AttemptApply(_targetStats, _fireDamage, _iceDamage, _lightingDamage);
    }

    private static void AttemptApply(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (UnityEngine.Random.value > .5f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (UnityEngine.Random.value > .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (UnityEngine.Random.value > .5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        if (canApplyShock)
            _targetStats.SetupShockDamage(Mathf.RoundToInt(_lightingDamage * .1f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockDamage(int _damage) => shockDamage = _damage;

    #endregion
    private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= (_targetStats.magicResistance.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShocked = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            ignitedTimer = alimentDuration;
            this.isIgnited = _ignite;
            fx.IgniteForSeconds(alimentDuration);
        }
        if (_chill && canApplyChill)
        {
            this.chillTimer = alimentDuration;
            this.isChilled = _chill;
            fx.ChillForSeconds(alimentDuration);
            float _slowPercentage = 0.35f;
            GetComponent<Entry>().SlowEntityBy(_slowPercentage, alimentDuration);
        }
        if (_shock && canApplyShocked)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;

                HitNeaestEnemyTarget();
            }
        }
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        this.shockTimer = alimentDuration;
        this.isShocked = _shock;
        fx.ShockForSeconds(alimentDuration);
    }

    private void HitNeaestEnemyTarget()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(this.showckStrikPrifab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ThunderStrike_Controller>().SetupThunderStrike(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);
        GetComponent<Entry>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDead)
            Die();
    }
    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        if (onHealthChanged != null)
            onHealthChanged();
    }
    public virtual void Die()
    {
        isDead = true;
    }

    #region Stats Calc
    private int CheckArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    bool TargetCanAvoidAttack(CharacterStats _target)
    {
        int totalEvasion = _target.evasion.GetValue() + _target.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }

    bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (UnityEngine.Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }
    int CalcCritDamage(int _damage)
    {
        float totalCalcPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float calcDamage = _damage + totalCalcPower;
        return Mathf.RoundToInt(calcDamage);
    }
    public int GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue() * 5;
    #endregion
}
