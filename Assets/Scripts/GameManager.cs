using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager mInstance;

    public static GameManager GetInstance()
    {
        return mInstance;
    }

    public HeroController hero;
    public EnemyController enemy;
    public BossController boss;


    private void Awake()
    {
        mInstance = this;
    }

    private void Start()
    {
        enemy.AddWarpDelegate(WarpDelegate);
        boss.AddWarpDelegate(WarpDelegate);
    }

    public void WarpDelegate(object sender, EventArgs e)
    {
        Debug.Log("hizo warp");
        hero.transform.position = new Vector3(
                hero.transform.position.x,
                hero.transform.position.y+5,
                0f
            );
        /*if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("se apreto x");
            
        }*/
    }
}
