using System.Collections;
using UnityEngine;

public class BossStateManager : EnemyStateManager {
    [SerializeField] private SceneManager sceneMan = null;
    
    private Collider2D shieldCollider; 
    public override void Start()
    {
        shieldCollider = BossManager.instance.shieldBoss.GetComponent<Collider2D>();
        base.Start();

        CommandConsole KILL = new CommandConsole("KillBoss", "KillBoss : kill the boss", null, (_) => { TakeDamage(EMainStatsSo.maxHealth + 100, Vector2.zero, 0, false, false, false); });
        CommandConsoleRuntime.Instance.AddCommand(KILL);
    }

    public override void OnDeath(bool byFall = false)
    {
        base.OnDeath(byFall);

        if ((NeverDestroy.Instance.minute + NeverDestroy.Instance.second) != 0) GameManager.Instance.Score += (20 * 60 / (NeverDestroy.Instance.minute * 60 + NeverDestroy.Instance.second));
        else GameManager.Instance.Score *= 2;
        
        NeverDestroy.Instance.GoHUbWin();
        GameManager.Instance.SetND();
        NeverDestroy.Instance.Score = GameManager.Instance.Score;

        UIManager.Instance.DisableUI();
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene() {
        yield return new WaitForSeconds(6.1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("HUB Win");
    }

    public override void TakeDamage(float damage, Vector2 position, float knockUpValue, bool knockup, bool isExplosion, bool isPoison = false)
   {
       if (!shieldCollider.enabled && BossManager.instance.currentBossPhase != ExtensionMethods.PhaseBoss.Begin)
       {
             base.TakeDamage(damage, position, knockUpValue, knockup, isExplosion);
                 BossManager.instance.UpdateSlider(health);
       }
    
   }
   
}
