using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KomiGameManager : MonoBehaviour
{
    [SerializeField]
    [HeaderAttribute("KomiGameManagerの説明文表示用のprivate変数です。\nゲーム自体に影響ありません。\nKomiGameManagerは\r\n主にゲームの中断、マウスカーソルの表示設定に機能します\r\nゲームの中断：public void EndGame();\r\nマウスカーソルの表示：Escapeキーで表示、非表示を切り替えます。")]
    private string Explanatory;
    private bool MouseCursorSwitch { set; get; }
    // Start is called before the first frame update
    void Start()
    {
        this.MouseCursorSwitch = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (this.MouseCursorSwitch)
            {
                this.MouseCursorSwitch = false;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                this.MouseCursorSwitch = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

    }


    public void EndGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif

    }



    

}
