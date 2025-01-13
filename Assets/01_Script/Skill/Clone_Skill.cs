using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] GameObject clonePrefab;
    [SerializeField] float cloneDuration;
    [SerializeField] float colorLossingSpeed;
    [Space]
    [SerializeField] bool canAttack;


    public void CreateClone(Transform _clonePosition)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<Clone_Skill_Controller>().Setup(_clonePosition, cloneDuration, colorLossingSpeed, canAttack);
    }


}
