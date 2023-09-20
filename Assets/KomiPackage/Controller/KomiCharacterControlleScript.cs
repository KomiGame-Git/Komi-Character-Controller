using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class KomiCharacterControlleScript : MonoBehaviour
{
    [HeaderAttribute("キャラクター設定\n・WASDキーで移動\n・Spaceキーでジャンプ")]
    [TooltipAttribute("キャラクターのGameObject")]
    public GameObject Character;
    [SerializeField,TooltipAttribute("キャラクターの動くスピード")]
    private float Speed;

    [SerializeField, TooltipAttribute("キャラクターのジャンプ力")]
    private float JumpPower;

    [SerializeField, TooltipAttribute("キャラクターのジャンプ回数")]
    private int MaxJumpCount;
    private int JumpCount;

    [SerializeField,TooltipAttribute("キャラクターの前方向を表すVector3")]
    private Vector3 CharacterForwardVector3;

    
    [HideInInspector]
    public Vector3 UnitCharacterForward;
    [HideInInspector]
    public Vector3 UnitCharacterRight;

    private Quaternion CharacterForwardQuaternion;
    private Rigidbody CharacterRigidbody;

    [HeaderAttribute("カメラ設定\n・F5キーでFPSとTPS視点の切り替え")]
    [TooltipAttribute("カメラのGameObject")]
    public GameObject Camera;

    [TooltipAttribute("キャラクター中心からカメラ(目)の相対位置(X, Y, Z)[FPS]")]
    public Vector3 relativeCameraPosition_FPS;
    [TooltipAttribute("キャラクター中心からカメラ(目)の相対位置(X, Y, Z)[TPS]")]
    public Vector3 relativeCameraPosition_TPS;

    [HideInInspector]
    public Vector3 UnitCameraForward_FPS;

    private Quaternion CameraForwardQuaternion;
    private bool FPT_TPS_Switch;


    [HeaderAttribute("地面設定\n・TagNameまたはGroundObjectListのどちらかで設定を行ってください。\n※正しく設定ができていない場合無制限にジャンプができてしまいます。")]
    [SerializeField, TooltipAttribute("地面Objectに付与したTag名")]
    private String GroundTagName;
    [SerializeField, TooltipAttribute("地面となるGameObjectリスト")]
    private List<GameObject> GroundObjectList;

    private KomiGroundCheckScript GroundCheckScript;

    // Start is called before the first frame update
    void Start()
    {
        this.CharacterSet();

        this.CameraSet();

        this.UnitVectorCheck();

        this.FPT_TPS_Switch = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        this.UnitVectorCheck();
        this.CharacterRotate();
        this.CameraRotate_FPS_X();
        this.CharacterMove();
        this.CharacterJump();

        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (this.FPT_TPS_Switch)
            {
                this.FPT_TPS_Switch = false;
                this.TPS_Mode();
            }
            else
            {
                this.FPT_TPS_Switch = true;
                this.FPS_Mode();
            }
        }

        this.test();

    }


    private void CharacterSet()
    {
        if (this.Character != null)
        {
            Vector3 HorizonatalCharacterForwad = new Vector3(this.Character.transform.forward.x, 0f, this.Character.transform.forward.z);
            Vector3 SelectCharacterForward = new Vector3(this.CharacterForwardVector3.x, 0f, this.CharacterForwardVector3.z);

            this.CharacterForwardQuaternion = Quaternion.AngleAxis(Vector3.SignedAngle(HorizonatalCharacterForwad, SelectCharacterForward, Vector3.up), Vector3.up);

            if (this.Character.GetComponent<Rigidbody>() != null)
            {
                this.CharacterRigidbody = this.Character.GetComponent<Rigidbody>();
                Debug.Log("CharacterにRigidbodyコンポーネントが付与されていたので、ユーザー設定の状態で使用します。");
            }
            else
            {
                this.CharacterRigidbody = this.Character.AddComponent<Rigidbody>() as Rigidbody;
                this.CharacterRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                Debug.Log("CharacterにRigidbodyコンポーネントが付与されていなかったので\nCharacterインスタンスに動的にRigidbodyを追加しました。");
            }

            this.GroundCheckScript = this.Character.AddComponent<KomiGroundCheckScript>();
            this.GroundCheckScript.OnCollisionEvent = this.GroundCheck;

        }
        else
        {
            Debug.LogError("CharacterをInspectorで設定してください!");
        }

        if (this.Speed == 0f)
        {
            Debug.LogWarning("Speedが0になっています");
        }

    }

    private void CameraSet()
    {
        if (this.Character != null && this.Camera != null)
        {
            Vector3 HorizonatalCharacterForwad = new Vector3(this.Character.transform.forward.x, 0f, this.Character.transform.forward.z);
            Vector3 SelectCameraForward = new Vector3(this.relativeCameraPosition_FPS.x, 0f, this.relativeCameraPosition_FPS.z);

            this.CameraForwardQuaternion = Quaternion.AngleAxis(Vector3.SignedAngle(HorizonatalCharacterForwad, SelectCameraForward, Vector3.up), Vector3.up);
            this.UnitCameraForward_FPS = this.CameraForwardQuaternion * HorizonatalCharacterForwad;

            this.Camera.transform.parent = this.Character.transform;

            this.FPS_Mode();

        }
    }

    private void UnitVectorCheck()
    {
        if (this.Character != null)
        {
            Vector3 HorizonatalCharacterForwad = new Vector3(this.Character.transform.forward.x, 0f, this.Character.transform.forward.z);

            this.UnitCharacterForward = this.CharacterForwardQuaternion * HorizonatalCharacterForwad;
            this.UnitCharacterRight = Quaternion.AngleAxis(90, Vector3.up) * this.UnitCharacterForward;
            this.UnitCameraForward_FPS = this.CameraForwardQuaternion * HorizonatalCharacterForwad;
        }
    }

    private void CharacterRotate()
    {
        if(this.Character != null)
        {
            float Y_Rotation = Input.GetAxis("Mouse X");

            this.Character.transform.Rotate(0f, Y_Rotation, 0f);
        }
    }

    private void CameraRotate_FPS_X()
    {
        if (this.Camera != null && this.FPT_TPS_Switch)
        {
            float X_Rotation = -1 * Input.GetAxis("Mouse Y");
            float currentXAngle = Vector3.SignedAngle(this.Camera.transform.forward, this.UnitCameraForward_FPS, this.Camera.transform.right);
            
            //Debug.Log($"currentXAngle:{currentXAngle} X_Rotation:{X_Rotation}");
            if (X_Rotation < 0 && currentXAngle <= 90 || X_Rotation > 0 && currentXAngle >= -90)
            {
                this.Camera.transform.Rotate(X_Rotation, 0f, 0f);
            }
        }
    }

    private void CharacterMove()
    {
        if (this.Character != null)
        {

            float X_Unit_Scalar = Input.GetAxis("Horizontal");
            float Z_Unit_Scalar = Input.GetAxis("Vertical");

            Vector3 MoveVector = this.UnitCharacterForward * Z_Unit_Scalar + this.UnitCharacterRight * X_Unit_Scalar;
            
            Vector3 UnitVector = MoveVector.normalized;

            Vector3 ForceVector = this.Speed * UnitVector;

            if (this.CharacterRigidbody.velocity.magnitude < ForceVector.magnitude)
            {
                this.CharacterRigidbody.AddForce(ForceVector);
            }

        }
    }

    private void CharacterJump()
    {
        if (this.Character != null && Input.GetKeyDown(KeyCode.Space) && this.JumpCount < this.MaxJumpCount)
        {
            this.CharacterRigidbody.AddForce(new Vector3(0f, this.JumpPower, 0f), ForceMode.VelocityChange);
            this.JumpCount++;
        }

    }


    private void FPS_Mode()
    {
        if(this.Camera != null)
        {
            this.Camera.transform.localPosition = this.relativeCameraPosition_FPS;

            this.Camera.transform.forward = this.UnitCameraForward_FPS;
        }
    }

    private void TPS_Mode()
    {
        if(this.Camera != null)
        {
            this.Camera.transform.localPosition = this.relativeCameraPosition_TPS;

            this.Camera.transform.LookAt(this.Character.transform);
        }
    }

    void GroundCheck(Collision collision)
    {
        if (collision.gameObject.tag.Equals(this.GroundTagName) || this.GroundObjectList.Contains(collision.gameObject))
        {
            this.JumpCount = 0;
        }
    }


    private void test()
    {
        //Debug.Log($"Camera_X:{this.Camera.transform.localEulerAngles.x} Y:{this.Camera.transform.localEulerAngles.y}");
        //Debug.Log($"Character_X:{this.Character.transform.eulerAngles.x} Y:{this.Camera.transform.eulerAngles.y}");
        //Debug.Log($"{this.JumpCount}");
    }

}
