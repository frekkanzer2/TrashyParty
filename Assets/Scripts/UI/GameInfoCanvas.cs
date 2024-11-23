using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameInfoCanvas : MonoBehaviour
{
    public GameObject KeySuggestionItemPrefab;
    public TextMeshProUGUI GameName, GameDescription;
    public GameObject KeySuggestionsContainer;

    private string key_a_animator_res = "KeyAC";
    private string key_b_animator_res = "KeyBC";
    private string key_x_animator_res = "KeyXC";
    private string key_y_animator_res = "KeyYC";
    private List<Tuple<IGamepad.Key, RuntimeAnimatorController>> controllers;
    private GameManager gameManager;

    private RuntimeAnimatorController GetControllerByKey(IGamepad.Key key) => controllers.Find(item => item.Item1 == key).Item2;

    private void Start()
    {
        controllers = new();
        controllers.Add(new(IGamepad.Key.ActionButtonDown, Resources.Load(key_a_animator_res) as RuntimeAnimatorController));
        controllers.Add(new(IGamepad.Key.ActionButtonRight, Resources.Load(key_b_animator_res) as RuntimeAnimatorController));
        controllers.Add(new(IGamepad.Key.ActionButtonLeft, Resources.Load(key_x_animator_res) as RuntimeAnimatorController));
        controllers.Add(new(IGamepad.Key.ActionButtonUp, Resources.Load(key_y_animator_res) as RuntimeAnimatorController));
        gameManager = GameManager.Instance;
        gameManager.KeySuggestions.ForEach(sugg =>
        {
            GameObject keyInstance = GameObject.Instantiate(KeySuggestionItemPrefab, KeySuggestionsContainer.transform);
            keyInstance.GetComponent<KeyContainerItem>().SetAction(sugg.Action, GetControllerByKey(sugg.Key));
        });
        GameName.text = gameManager.GameName;
        GameDescription.text = gameManager.GameDescription;
    }

}
