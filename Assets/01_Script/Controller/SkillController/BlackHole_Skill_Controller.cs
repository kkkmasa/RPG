using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] GameObject hotkyePrefab;
    [SerializeField] List<KeyCode> keyCodeList;
    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;
    bool canGrow = true;
    bool canShrink;
    bool playerDisapper = true;

    public bool playerCanExitState { get; private set; }

    float blackholeTimer;

    int amountOfAttack = 4;
    float cloneAttackCooldown = 4;
    float cloneAttackTimer;
    bool canAttack;
    bool canCreateHotkeys = true;


    List<Transform> targets = new List<Transform>();
    List<GameObject> createdHotkey = new List<GameObject>();


    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuration)
    {
        this.maxSize = _maxSize;
        this.growSpeed = _growSpeed;
        this.shrinkSpeed = _shrinkSpeed;
        this.amountOfAttack = _amountOfAttacks;
        this.cloneAttackCooldown = _cloneAttackCooldown;
        this.blackholeTimer = _blackholeDuration;

        if (SkillManager.instance.clone.crystalInsteadOfClone)
            playerDisapper = false;
    }

    void Update()
    {

        this.blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;
            if (targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackholeAblity();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
            return;

        DestoryHotkeys();
        canAttack = true;
        canCreateHotkeys = false;

        if (playerDisapper)
        {
            PlayerManager.instance.player.fx.MakeTransprent(true);
            playerDisapper = false;
        }

    }

    private void CloneAttackLogic()
    {
        cloneAttackTimer -= Time.deltaTime;
        if (cloneAttackTimer < 0 && canAttack && amountOfAttack > 0)
        {
            if (this.targets.Count <= 0)
            {
                canAttack = false;
                canShrink = true;
                return;
            }
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;
            if (Random.Range(0, 100) > 50)
                xOffset = 1.8f;
            else
                xOffset = -1.8f;

            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChoosenRandomTarget();
            }
            else
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            }


            amountOfAttack--;
            if (amountOfAttack <= 0)
            {
                Invoke("FinishBlackholeAblity", 1f);
            }
        }
    }

    private void FinishBlackholeAblity()
    {
        DestoryHotkeys();
        playerCanExitState = true;
        canAttack = false;
        canShrink = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {

            other.GetComponent<Enemy>().FreezeTime(true);

            CreateHotkey(other);

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            other.GetComponent<Enemy>().FreezeTime(false);
        }
    }
    void DestoryHotkeys()
    {
        foreach (GameObject g in this.createdHotkey)
        {
            Destroy(g);
        }
    }

    public void AddEnemyToList(Transform _enemy) => this.targets.Add(_enemy);

    private void CreateHotkey(Collider2D other)
    {
        if (keyCodeList.Count <= 0)
            return;

        if (!canCreateHotkeys)
            return;

        GameObject newHotkey = Instantiate(this.hotkyePrefab, other.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotkey.Add(newHotkey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        BlackHole_HotKey_Controller blackHole_HotKey_Controller = newHotkey.GetComponent<BlackHole_HotKey_Controller>();
        blackHole_HotKey_Controller.SetupHotKey(choosenKey, other.transform, this);
    }
}
