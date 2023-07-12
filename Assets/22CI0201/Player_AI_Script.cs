using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AI_Script : MonoBehaviour
{
    [Header("一歩のサイズ")]
    [SerializeField] float one_steps_size;
    [SerializeField] private ParticleSystem particle;

    //AIの状態
    enum AI_state { non = 0, dead = 1, stun = 2, poison = 3, }

    //AIの向いている方向 (前後)
    enum AI_direction_FB { non = 0, forward = 1, back = -1, }

    //AIの向いている方向 (左右)
    enum AI_direction_LR { non = 0, left = 1, right = -1, }

    int hoge;
    AI_state state;
    AI_direction_LR direction_LR;
    AI_direction_FB direction_FB;

    void Start()
    {
        one_steps_size_set(1f);

        state = AI_state.non;
        direction_FB = AI_direction_FB.forward;
        direction_LR = AI_direction_LR.non;

        state_check();
    }

    void Update()
    {
        hoge++;
        if (hoge == 1000)
        {
            go_forward_1steps();
        }
    }

//---------------------------------------------------------------------------------------------------------------------------------------------
//以下、メソッド一覧
//---------------------------------------------------------------------------------------------------------------------------------------------

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

    //１歩前に進む
    void go_forward_1steps()
    {
        play_particle_effect();
        transform.position += transform.TransformDirection(Vector3.forward) * one_steps_size;
        Debug.Log("実行されました。");
    }

    //１歩後ろに下がる
    void go_back_1steps()
    {
        transform.position += transform.TransformDirection(Vector3.back) * one_steps_size;
        Debug.Log("実行されました。");
    }

    //１歩右に進む
    void go_right_1steps()
    {
        transform.position += transform.TransformDirection(Vector3.right) * one_steps_size;
        Debug.Log("実行されました。");
    }

    //１歩左に進む
    void go_left_1steps()
    {
        transform.position += transform.TransformDirection(Vector3.left) * one_steps_size;
        Debug.Log("実行されました。");
    }

    //AIの向いている、左右の方向をチェックする
    void direction_check_LR()
    {
        switch(direction_LR)
        {
            case AI_direction_LR.non:
                Debug.Log("AIは右も左も向いていません。");
                break;

            case AI_direction_LR.left:
                Debug.Log("AIは左を向いています。");
                break;

            case AI_direction_LR.right:
                Debug.Log("AIは右を向いています。");
                break;
        }
    }

    //AIの向いている、前後の方向をチェックする
    void direction_check_FB()
    {
        switch (direction_FB)
        {
            case AI_direction_FB.non:
                Debug.Log("AIは前も後ろも向いていません。");
                break;

            case AI_direction_FB.forward:
                Debug.Log("AIは前を向いています。");
                break;

            case AI_direction_FB.back:
                Debug.Log("AIは後ろを向いています。");
                break;
        }
    }

    //AIの状態異常をチェックする
    void state_check()
    {
        switch(state)
        {
            case AI_state.non:
                Debug.Log("AIに状態異常はありません。");
                break;

            case AI_state.dead:
                Debug.Log("AIは死にました。");
                break;

            case AI_state.poison:
                Debug.Log("AIは毒状態になっています。");
                break;

            case AI_state.stun:
                Debug.Log("AIは気絶しています。");
                break;
        }
    }
}
