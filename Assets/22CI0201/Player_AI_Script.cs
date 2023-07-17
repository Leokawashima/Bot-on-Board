using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player_AI_Script : MonoBehaviour
{
    [Header("一歩のサイズ")]
    [SerializeField] float one_steps_size;

    [SerializeField] private ParticleSystem particle;

    [Header("現在の向き")]
    [SerializeField] public Vector3 current_direction;

    //AIの状態
    enum AI_state { non = 0, normal = 1, dead = 2, poison = 3, }

    //AIの向いている方向 (前後)
    enum AI_direction_FB { non = 0, forward = 1, back = -1, }

    //AIの向いている方向 (左右)
    enum AI_direction_LR { non = 0, left = 1, right = -1, }

    int hoge;
    AI_state state;
    AI_direction_LR direction_LR;
    AI_direction_FB direction_FB;

    //現在のステートの状態からベクトルを作成する
    public Vector3 direction
    {
        get { return new Vector3((int)direction_FB, 0,(int)direction_LR); }
    }

    void Start()
    {
        one_steps_size_set(1f);
        
        state = AI_state.non;
        direction_FB = AI_direction_FB.non;
        direction_LR = AI_direction_LR.left;

        current_direction = direction;   
    }

    void Update()
    {
        hoge++;
        if (hoge == 1000)
        {
            go_1steps();
        }
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------
    //以下、メソッド一覧
    //---------------------------------------------------------------------------------------------------------------------------------------------
    #region 
    //１歩のサイズを初期化する
    void one_steps_size_set(float set_one_steps_size)
    {
        one_steps_size = set_one_steps_size;
    }
    
    //パーティクルエフェクトを再生する
    void play_particle_effect()
    {
        particle.Play();
    }

    //アニメーションを再生する
    void play_animation()
    {
        //未実装
    }

    //現在の向きに1歩進む
    void go_1steps()
    {
        current_direction = direction;
        play_particle_effect();
        transform.position += transform.TransformDirection(current_direction) * one_steps_size;
        UnityEngine.Debug.Log("現在の向きへの移動が実行されました。");
    }

    //１歩前に進む
    void go_forward_1steps()
    {
        play_particle_effect();
        transform.position += transform.TransformDirection(current_direction) * one_steps_size;
        UnityEngine.Debug.Log("実行されました。");
    }

    //１歩後ろに下がる
    void go_back_1steps()
    {
        transform.position += transform.TransformDirection(Vector3.back) * one_steps_size;
        UnityEngine.Debug.Log("実行されました。");
    }

    //１歩右に進む
    void go_right_1steps()
    {
        transform.position += transform.TransformDirection(Vector3.right) * one_steps_size;
        UnityEngine.Debug.Log("実行されました。");
    }

    //１歩左に進む
    void go_left_1steps()
    {
        transform.position += transform.TransformDirection(Vector3.left) * one_steps_size;
        UnityEngine.Debug.Log("実行されました。");
    }

    //AIの向いている、左右の方向をチェックする
    void direction_check_LR()
    {
        switch(direction_LR)
        {
            case AI_direction_LR.non:
                UnityEngine.Debug.Log("AIは右も左も向いていません。");
                break;

            case AI_direction_LR.left:
                UnityEngine.Debug.Log("AIは左を向いています。");
                break;

            case AI_direction_LR.right:
                UnityEngine.Debug.Log("AIは右を向いています。");
                break;
        }
    }

    //AIの向いている、前後の方向をチェックする
    void direction_check_FB()
    {
        switch (direction_FB)
        {
            case AI_direction_FB.non:
                UnityEngine.Debug.Log("AIは前も後ろも向いていません。");
                break;

            case AI_direction_FB.forward:
                UnityEngine.Debug.Log("AIは前を向いています。");
                break;

            case AI_direction_FB.back:
                UnityEngine.Debug.Log("AIは後ろを向いています。");
                break;
        }
    }

    //AIの状態異常をチェックする
    void state_check()
    {
        switch(state)
        {
            case AI_state.non:
                UnityEngine.Debug.Log("AIの状態が存在しません。");
                break;

            case AI_state.normal:
                UnityEngine.Debug.Log("AIに状態異常はありません。");
                break;

            case AI_state.dead:
                UnityEngine.Debug.Log("AIは死亡しています。");
                break;

            case AI_state.poison:
                UnityEngine.Debug.Log("AIは毒状態になっています。");
                break;
        }
    }
    #endregion
}
