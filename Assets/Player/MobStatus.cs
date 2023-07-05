using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStatus : MonoBehaviour
{
    //��Ԃ̒�`
    protected enum StateEnum
    {
        Normal, //�ʏ�
        Attack, //�U����
        Die //���S    
    }

    [SerializeField] private float lifeMax = 10; //���C�t�ő�l
    protected Animator animator;
    protected StateEnum state = StateEnum.Normal; //Mob�̏��
    private float life; //���݂�HP

    //�ړ��\���ǂ���
    public bool IsMoveable => StateEnum.Normal == state;

    //�U���\���ǂ���
    public bool IsAttackable => StateEnum.Normal == state;

    //���C�t�ő�l��Ԃ�
    public float LifeMax => lifeMax;

    //���C�t�̒l��Ԃ�
    public float Life => life;

    protected virtual void Start()
    {
        //������Ԃ�HP�͖��^���ɂ���
        life = lifeMax;
        animator = GetComponentInChildren<Animator>();
    }

    //�L�����N�^�[���|�ꂽ���̏���
    protected virtual void OnDie()
    {

    }

    //�w��l�̃_���[�W���󂯂�
    public void Damage(int damage)
    {
        if (state == StateEnum.Die) return;

        life -= damage;

        if (life > 0) return;

        //�|�ꂽ����Die�̃A�j���[�V�������Đ�����
        state = StateEnum.Die;
        animator.SetTrigger("Die");
        OnDie();
    }

    //�񕜃A�C�e�����E������
    public void recovery(int recovery)
    {
        if (state == StateEnum.Die) return;

        life += recovery;
    }

    //�\�ł���΍U�����̏�ԂɑJ�ڂ���
    public void GotoAttackStateIfPossible()
    {
        if (!IsAttackable) return;

        //�U�����鎞��Attack�̃A�j���[�V�������Đ�����
        state = StateEnum.Attack;
        animator.SetTrigger("Attack");
    }

    //�\�ł����Normal�̏�ԂɑJ�ڂ���
    public void GoToNormalStateIfPossible()
    {
        if (state == StateEnum.Die) return;
        state = StateEnum.Normal;
    }
}
