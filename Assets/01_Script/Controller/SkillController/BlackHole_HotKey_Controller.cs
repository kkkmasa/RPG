using UnityEngine;
using TMPro;

public class BlackHole_HotKey_Controller : MonoBehaviour
{
    KeyCode myHotkey;
    TextMeshProUGUI myText;
    SpriteRenderer sr;

    Transform myEnemyTransform;
    BlackHole_Skill_Controller blackHole_Skill_Controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetupHotKey(KeyCode _myHotkey, Transform _myEnemy, BlackHole_Skill_Controller _blackHole_Skill_Controller)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        this.myHotkey = _myHotkey;
        myText.text = myHotkey.ToString();
        this.blackHole_Skill_Controller = _blackHole_Skill_Controller;
        this.myEnemyTransform = _myEnemy;

        
    }
    void Update()
    {
        if (Input.GetKeyDown(myHotkey))
        {
            blackHole_Skill_Controller.AddEnemyToList(this.myEnemyTransform);
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
