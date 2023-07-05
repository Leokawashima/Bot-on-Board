using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStatus : MonoBehaviour
{
    //状態の定義
    protected enum StateEnum
    {
        Normal, //通常
        Attack, //攻撃中
        Die //死亡    
    }

    [SerializeField] private float lifeMax = 10; //ライフ最大値
    protected Animator animator;
    protected StateEnum state = StateEnum.Normal; //Mobの状態
    private float life; //現在のHP

    //移動可能かどうか
    public bool IsMoveable => StateEnum.Normal == state;

    //攻撃可能かどうか
    public bool IsAttackable => StateEnum.Normal == state;

    //ライフ最大値を返す
    public float LifeMax => lifeMax;

    //ライフの値を返す
    public float Life => life;

    protected virtual void Start()
    {
        //初期状態はHPは満タンにする
        life = lifeMax;
        animator = GetComponentInChildren<Animator>();
    }

    //キャラクターが倒れた時の処理
    protected virtual void OnDie()
    {

    }

    //指定値のダメージを受ける
    public void Damage(int damage)
    {
        if (state == StateEnum.Die) return;

        life -= damage;

        if (life > 0) return;

        //倒れた時にDieのアニメーションを再生する
        state = StateEnum.Die;
        animator.SetTrigger("Die");
        OnDie();
    }

    //回復アイテムを拾った時
    public void recovery(int recovery)
    {
        if (state == StateEnum.Die) return;

        life += recovery;
    }

    //可能であれば攻撃中の状態に遷移する
    public void GotoAttackStateIfPossible()
    {
        if (!IsAttackable) return;

        //攻撃する時にAttackのアニメーションを再生する
        state = StateEnum.Attack;
        animator.SetTrigger("Attack");
    }

    //可能であればNormalの状態に遷移する
    public void GoToNormalStateIfPossible()
    {
        if (state == StateEnum.Die) return;
        state = StateEnum.Normal;
    }
}
