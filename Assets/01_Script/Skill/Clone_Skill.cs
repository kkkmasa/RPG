using System.Collections;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] GameObject clonePrefab;
    [SerializeField] float cloneDuration;
    [SerializeField] float colorLossingSpeed;
    [Space]
    [SerializeField] bool canAttack;

    [SerializeField] bool createCloneDashStart;
    [SerializeField] bool createCloneOnDashOver;
    [SerializeField] bool canCreateCloneOnCounterAttack;

    [Header("Clone can duplicate")]
    [SerializeField] bool canDuplicateClone;
    [SerializeField] float chanceToDuplicate;

    [Header("Crystal Instead of Clone")]
    public bool crystalInsteadOfClone;


    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone) {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<Clone_Skill_Controller>().Setup(_clonePosition, cloneDuration, colorLossingSpeed, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate, player);
    }

    public void CreateCloneOnDashBegun() {
        if (createCloneDashStart) {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashOver() {
        if (createCloneOnDashOver) {
            CreateClone(player.transform, Vector3.zero);
        }
    }
    public void CreateCloneOnCounterAttack(Transform _enemyTransform) {
        if (canCreateCloneOnCounterAttack) {
            StartCoroutine(CloneWaitFor(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
        }
    }

    IEnumerator CloneWaitFor(Transform _enemyTransform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_enemyTransform, _offset);
    }


}
