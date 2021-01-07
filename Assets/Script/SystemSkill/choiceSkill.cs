using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class choiceSkill : MonoBehaviour
{

    [SerializeField] private List<GameObject> skillItem;
    public List<GameObject> buttonList;
    [SerializeField] private GameObject Canvas;

    public int nVague;
    [SerializeField] private int vagueChoice;
    [SerializeField] private skillSystem Skill;

    // Start is called before the first frame update
    void Start()
    {
        nVague = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(nVague >= vagueChoice)
        {
            int random = Random.Range(1, 4);
            switch (random)
            {
                

                case 1:
                    InstantiateButton(new Vector2(0, 0));
                    break;
                case 2:
                    InstantiateButton(new Vector2(80, 0));
                    InstantiateButton(new Vector2(-80, 0));
                    break;                                                       
                                                                                 
                case 3:
                    InstantiateButton(new Vector2(0, 0));
                    InstantiateButton(new Vector2(-175, 0));
                    InstantiateButton(new Vector2(175, 0));
                    break;
            }

            nVague = 1;
        }
    }

    void InstantiateButton(Vector2 poz)
    {
        GameObject newChoice = Instantiate(skillItem[Random.Range(0, skillItem.Count + 1)]);
        newChoice.transform.SetParent(Canvas.transform);
        newChoice.GetComponent<RectTransform>().localPosition = poz;
        newChoice.GetComponent<buttonChoice>().Skill = Skill;
        buttonList.Add(newChoice);
    }
}
