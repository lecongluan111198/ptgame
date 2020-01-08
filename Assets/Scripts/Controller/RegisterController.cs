using Assets.Scripts.Logic.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterController : MonoBehaviour
{
    // Reference to input field
    public InputField userName;
    public InputField passWord;
    public InputField ComfirmPassWord;

    public GameObject RegisterWindow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void _Resigter()
    {
        string name = userName.text;
        string password = passWord.text;
        string comfirm = ComfirmPassWord.text;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(comfirm))
        {
            Debug.Log("ffff");
        }
        else
        {
            if (password.Equals(comfirm))
            {
                StartCoroutine(APIManager.Instance._Register(name, password, (rp) =>
                {
                    if (rp)
                    {
                        _CloseRegister();
                    }
                    else
                    {
                        //Do nothing
                    }
                })); 
            }
            else
            {

            }
        }
    }


    public void _OpenRegister()
    {
        RegisterWindow.SetActive(true);
    }

    public void _CloseRegister()
    {
        RegisterWindow.SetActive(false);
    }
}
