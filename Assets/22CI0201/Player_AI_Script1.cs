﻿using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using static MapManager;

public class Player_AI_Script1 : MonoBehaviour
{
    [Header("一歩のサイズ")]
    [SerializeField] private float one_steps_size;

    [SerializeField] private ParticleSystem particle;

    [Header("現在の向き")]
    [SerializeField] private Vector3 current_direction;

    [Header("マップの有無を確認する変数")]
    [SerializeField] private bool exists_map;

    //AIの状態
    public enum AI_state { non = 0, normal = 1, dead = 2, poison = 3, }

    //AIの向いている方向 (前後)
    public enum AI_direction_FB { non = 0, forward = 1, back = -1, }

    //AIの向いている方向 (左右)
    public enum AI_direction_LR { non = 0, left = 1, right = -1, }

    //テスト用の変数
    int hoge;
    //AIの状態
    AI_state state;

    //AIの前後左右の向き
    AI_direction_LR direction_LR;
    AI_direction_FB direction_FB;

    //現在のマップチップ上の位置
    private int current_X;
    private int current_Y;

    //MapManagerへの参照
    private MapManager mapManager;

    //進行予定のマップチップが入る変数
    private int map_X;
    private int map_Y;

    //現在のステートの状態からベクトルを作成する
    private Vector3 direction
    {
        get { return new Vector3((int)direction_LR, 0, (int)direction_FB); }
    }

    //進行予定方向の保存用変数
    Vector3 Destination;

    void Start()
    {
        //MapManagerコンポーネントを持つオブジェクトを見つけて参照を取得
        mapManager = FindObjectOfType<MapManager>();

        //1マスの移動距離を設定
        One_steps_size = 1f;

        Initialized(one_steps_size, 1, 0, 0, 0, 0);

        current_direction = direction;
    }

    void Update()
    {
        hoge++;
        if (hoge == 1000 || hoge == 2000 || hoge == 3000 || hoge == 4000 || hoge == 5000 || hoge == 6000)
        {
            go_1steps();
        }
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------
    //以下、メソッドなど
    //---------------------------------------------------------------------------------------------------------------------------------------------

    //メソッド
    #region 
    //１歩のサイズを初期化する
    private void one_steps_size_set(float set_one_steps_size)
    {
        one_steps_size = set_one_steps_size;
    }

    //生成時に値を設定する
    public void Initialized(float one_steps_size, int directionFB, int directionLR, int AIstate, int currentX, int currentY)
    {
        One_steps_size = one_steps_size;
        Direction_FB = (AI_direction_FB)directionFB;
        Direction_LR = (AI_direction_LR)directionLR;
        State = (AI_state)AIstate;
        Current_x = (int)transform.position.x;
        Current_y = (int)transform.position.z;
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
        Destination = transform.TransformDirection(current_direction) * one_steps_size;
        if (map_check())
        {
            transform.position += transform.TransformDirection(current_direction) * one_steps_size;
            play_particle_effect();
            if (Destination.z == 1) { current_Y++; }
            if (Destination.z == -1) { current_Y--; }
            if (Destination.x == 1) { current_X++; }
            if (Destination.x == -1) { current_X--; }
        }

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
        switch (direction_LR)
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
        switch (state)
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

    //マップの有無を確認する
    public bool map_check()
    {
        UnityEngine.Debug.Log("map_checkが実行");
        UnityEngine.Debug.Log(mapManager.mapStates[current_Y + (int)Destination.x, current_X + (int)Destination.z]);
        if ((int)Destination.z < 0 || (int)Destination.x < 0)
        {
            return false;
        }
        if (mapManager.mapStates[current_Y + (int)Destination.x, current_X + (int)Destination.z] >= 1)
        {
            return true;
        }
        //UnityEngine.Debug.Log(mapManager.mapStates[current_Y, current_X]);
        return false;
    }
    #endregion

    //各変数のget/set
    #region
    //1歩のサイズのget/set
    public float One_steps_size
    {
        get { return one_steps_size; }
        protected set { one_steps_size = value; }
    }

    //現在の向きのget/set
    public Vector3 Current_direction
    {
        get { return current_direction; }
        protected set { current_direction = value; }
    }

    //マップの有無のget/set
    public bool Exists_map
    {
        get { return exists_map; }
        protected set { exists_map = value; }
    }

    //マップ上のX位置のget/set
    public int Current_x
    {
        get { return current_X; }
        protected set { current_X = value; }
    }

    //マップ上のY位置のget/set
    public int Current_y
    {
        get { return current_Y; }
        protected set { current_Y = value; }
    }

    //AIの状態のget/set
    public AI_state State
    {
        get { return state; }
        protected set { state = value; }
    }

    //AIの前後の向きのget/set
    public AI_direction_FB Direction_FB
    {
        get { return direction_FB; }
        protected set { direction_FB = value; }
    }

    //AIの左右の向きのget/set
    public AI_direction_LR Direction_LR
    {
        get { return direction_LR; }
        protected set { direction_LR = value; }
    }
    #endregion
}
