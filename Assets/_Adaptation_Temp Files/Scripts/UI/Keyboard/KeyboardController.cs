using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TevaVR.UI
{
    public class KeyboardController : Singleton<KeyboardController>
    {
        private const string KeyBoardPath = "Keyboard";
        //TODO: Add scripts to spawn object in different positions
        public void ShowKeyboard(TMP_InputField _inputField)
        {
            DestroyPrewKeyboard();
            Debug.Log("Show keyboard methode executing");
            Keyboard keyboard = Instantiate(Resources.Load<Keyboard>(KeyBoardPath), gameObject.transform);
            keyboard.Init(_inputField);
        }

        public void DestroyPrewKeyboard()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}