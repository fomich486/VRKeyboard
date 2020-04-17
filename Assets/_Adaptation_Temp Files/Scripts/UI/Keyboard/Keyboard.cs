using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TevaVR.UI
{
    public class Keyboard : MonoBehaviour
    {
        private TMP_InputField inputFieldText;

        [Header("Keyboard references")]
        [SerializeField]
        private KeyboardInfo KeyboardInfo;
        [Space]
        [Header("Keyboard spawn")]
        public RectTransform Background;
        public RectTransform pref_changableButtonsPanel;
        public KeyboardButton pref_KeyboardButton;
        public ButtonLineContainer pref_buttonLineContainer;
        public RectTransform BottomLine;
        public ImageKeyboardButton ShiftButton;
        public KeyboardButton DeleteButton;
        public KeyboardButton CloseButton;
        [Space]
        [Header("Other keyboard links")]
        public Button ChangeLanguageButton;
        public KeyboardButton LangSymSwichButton;
        public Button EnterButton;

        //Local
        private RectTransform spawnedMainPanel;
        private Language currentLanguage;
        private Language currentSymbols;

        private bool isUpper = false;

        public void Init(TMP_InputField _inputField)
        {
            inputFieldText = _inputField;

            currentLanguage = KeyboardInfo.GetReqiredLanguage("English");
            InitializeLanguageKeyboard();

            //TODO:Add logic to delete button
            DeleteButton.button.onClick.AddListener(Delete);

            CloseButton.button.onClick.AddListener(() =>
            {
                //_inputField.DeactivateInputField();
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                Destroy(gameObject);
            });
        }

        #region Keyboard spawn
        private void SpawnKeyboard(Language language)
        {
            DeleteSpawnedMainKeys();

            float _padding = 4f;
            RectTransform _mainPanel = Instantiate(pref_changableButtonsPanel, gameObject.transform);
            _mainPanel.anchoredPosition = Vector2.zero;

            spawnedMainPanel = _mainPanel;

            foreach (var lineSymbols in language.LineSymbols)
            {
                ButtonLineContainer _buttonsLineContainer = Instantiate(pref_buttonLineContainer, _mainPanel.transform);

                foreach (var symbol in lineSymbols.chars)
                {
                    KeyboardButton _button = Instantiate(pref_KeyboardButton, _buttonsLineContainer.transform);
                    _button.Init(symbol, AddText);
                }
            }
            Debug.Log("Main panel size = " + _mainPanel.sizeDelta);
            //TODO: Add common bottom panel
            float _mainPanelSizeY = (language.LineSymbols.Count * pref_KeyboardButton.Rect.sizeDelta.y) + (language.LineSymbols.Count - 1) * _padding;
            float _bottomLinePosition = -_mainPanelSizeY / 2 - _padding - pref_KeyboardButton.Rect.sizeDelta.y / 2;
            Debug.Log("Bottom line position = " + _bottomLinePosition);
            BottomLine.anchoredPosition = new Vector2(0, _bottomLinePosition);
            //TODO: Setup shift button and delete buttons positions

            float _totalWidth;
            //Can't use RectTransform.sizeDelta due to ContentSizeFilter on BottomLine, require 3 frames to calculate  correct size delta of RectTransform
            float _bottomContainerSize = 355f;
            float _topContainerSizeX = language.LineSymbols[0].chars.Count * pref_KeyboardButton.Rect.sizeDelta.x + (language.LineSymbols[0].chars.Count - 1) * _padding;

            //TODO: Refactor if statement; If statement below contains code duplications
            if (_bottomContainerSize > _topContainerSizeX)
            {
                float _shiftPositionX = -_bottomContainerSize / 2 + pref_KeyboardButton.Rect.sizeDelta.x / 2;
                float _PositionY = -_mainPanelSizeY / 2 + pref_KeyboardButton.Rect.sizeDelta.y / 2;
                _totalWidth = _bottomContainerSize;

                ShiftButton.Rect.anchoredPosition = new Vector2(_shiftPositionX, _PositionY);
                DeleteButton.Rect.anchoredPosition = new Vector2(-_shiftPositionX, _PositionY);
            }
            else
            {
                float _shiftPositionX = -_topContainerSizeX / 2 + pref_KeyboardButton.Rect.sizeDelta.x / 2;
                float _PositionY = -_mainPanelSizeY / 2 + pref_KeyboardButton.Rect.sizeDelta.y / 2;
                _totalWidth = _topContainerSizeX;

                ShiftButton.Rect.anchoredPosition = new Vector2(_shiftPositionX, _PositionY);
                DeleteButton.Rect.anchoredPosition = new Vector2(-_shiftPositionX, _PositionY);
            }

            float _paddingLR = 16;
            float _paddingT = 24;
            float _paddingB = 12;

            float _totalHeight = _mainPanelSizeY + _padding + pref_KeyboardButton.Rect.sizeDelta.y;
            _totalWidth += 2 * _paddingLR;
            _totalHeight += _paddingT + _paddingB;
            Background.sizeDelta = new Vector2(_totalWidth, _totalHeight);
            Background.anchoredPosition = new Vector2(0f, -_paddingT);

            CloseButton.Rect.anchoredPosition = Background.anchoredPosition + Background.sizeDelta / 2;
        }

        private void InitializeLanguageKeyboard()
        {
            RemoveCommonButtonsListeners();
            isUpper = false;
            SpawnKeyboard(currentLanguage);

            ChangeLanguageButton.onClick.AddListener(ChangeLanguage);

            isUpper = false;
            ShiftButton.Rect.localScale = Vector3.one;
            ShiftButton.button.onClick.AddListener(ChangeRegister);
            ShiftButton.image.gameObject.SetActive(true);
            ShiftButton.buttonSymbol.text = "";

            LangSymSwichButton.button.onClick.AddListener(InitializeSymbolsKeyboard);
            LangSymSwichButton.buttonSymbol.text = "123";

        }

        public void InitializeSymbolsKeyboard()
        {
            RemoveCommonButtonsListeners();
            Language initSymbols = KeyboardInfo.GetReqiredSymbols("standart_First");
            currentSymbols = initSymbols;
            SpawnKeyboard(currentSymbols);

            ChangeLanguageButton.onClick.AddListener(InitializeLanguageKeyboard);

            //TODO: Shift button should change it's image on text
            ShiftButton.button.onClick.AddListener(ChangeSymbols);
            ShiftButton.image.gameObject.SetActive(false);
            ShiftButton.buttonSymbol.text = "(+}";

            LangSymSwichButton.button.onClick.AddListener(InitializeLanguageKeyboard);
            LangSymSwichButton.buttonSymbol.text = "ABC";
        }

        public void DeleteSpawnedMainKeys()
        {
            if (spawnedMainPanel)
                Destroy(spawnedMainPanel.gameObject);
        }

        private void RemoveCommonButtonsListeners()
        {
            ShiftButton.button.onClick.RemoveAllListeners();
            ChangeLanguageButton.onClick.RemoveAllListeners();
            LangSymSwichButton.button.onClick.RemoveAllListeners();
        }
        #endregion

        #region Keyboard input methods
        public void AddText(string charToAdd)
        {
            if (inputFieldText.text.Length >= 60)
            {
                Debug.LogWarning("Riched 60 symbols limit");
                return;
            }
            if (isUpper)
                charToAdd = charToAdd.ToUpper();

            int caretPosition = inputFieldText.text.Length;
            inputFieldText.text = inputFieldText.text.Insert(caretPosition, charToAdd);
            //inputFieldText.caretPosition++;
        }

        public void Delete()
        {
            inputFieldText.Backspace();
        }
        #endregion

        public void ChangeLanguage()
        {
            ShiftButton.Rect.localScale = Vector3.one;
            isUpper = false;

            currentLanguage = KeyboardInfo.GetNextLanguage(currentLanguage.LanguageName);
            SpawnKeyboard(currentLanguage);
        }

        public void ChangeSymbols()
        {
            currentSymbols = KeyboardInfo.GetNextSymbols(currentSymbols.LanguageName);
            SpawnKeyboard(currentSymbols);

            //TODO: Refactor temp coede below [Code to change preview symbols trigger]
            if (currentSymbols.LanguageName == "standart_First")
                ShiftButton.buttonSymbol.text = "(+}";
            else
                ShiftButton.buttonSymbol.text = "123";
        }

        public void Enter()
        {

        }

        public void ChangeRegister()
        {
            ShiftButton.Rect.localScale = new Vector3(ShiftButton.Rect.localScale.x, -ShiftButton.Rect.localScale.y, ShiftButton.Rect.localScale.z);
            List<KeyboardButton> buttons = new List<KeyboardButton>();
            ButtonLineContainer[] buttonLineContainers = spawnedMainPanel.GetComponentsInChildren<ButtonLineContainer>();
            foreach (var container in buttonLineContainers)
            {
                KeyboardButton[] keyboardButtons = container.GetComponentsInChildren<KeyboardButton>();
                buttons.AddRange(keyboardButtons);
            }

            if (isUpper)
            {
                Debug.Log("to lower");
                foreach (var btn in buttons)
                {
                    btn.buttonSymbol.text = btn.buttonSymbol.text.ToLower();
                }
            }
            else
            {
                Debug.Log("to upper");
                foreach (var btn in buttons)
                {
                    btn.buttonSymbol.text = btn.buttonSymbol.text.ToUpper();
                }
            }

            isUpper = !isUpper;
        }
    }
}