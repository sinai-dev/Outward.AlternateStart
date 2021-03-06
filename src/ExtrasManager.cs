using System;
using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SideLoader;

namespace AlternateStart
{
    public class ExtrasManager : MonoBehaviour
    {
        public static ExtrasManager Instance;

        Vector3 spawn = new Vector3(-12.2f, 0.2f, -0.6f);
        Vector3 spawnRot = new Vector3(0, 75.2f, 0);

        public List<Character> allPlayers;

        internal void Awake()
        {
            Instance = this;
            ExtrasManager.Init();
            allPlayers = new List<Character>();
        }

        internal static void Init()
        {
            SL.OnGameplayResumedAfterLoading += SL_OnGameplayResumedAfterLoading;
        }

        private static void SL_OnGameplayResumedAfterLoading()
        {
            Character host = CharacterManager.Instance.GetWorldHostCharacter();
            if (SceneManagerHelper.ActiveSceneName == "Berg")
            {
                //Debug.Log("In Berg");
                if (!host.Inventory.SkillKnowledge.IsItemLearned((int)ScenarioPassives.Survivor))
                {
                    //Debug.Log("Not Survivor");
                    Instance.StartCoroutine(Instance.DeactivateTrainer(1f));
                }
            }
        }


        public IEnumerator DeactivateTrainer(float timer)
        {
            yield return new WaitForSeconds(timer);

            var characters = CharacterManager.Instance.Characters.Values;
            foreach (Character character in characters)
            {
                //Debug.Log(character.Name);
                if (!character.IsLocalPlayer && character.Name == "Eto Akiyuki")
                {
                    Debug.Log("Found Eto");
                    character.gameObject.SetActive(false);
                }
            }
        }

        //might optimize-----------------------------
        void Update()
        {
            if (SceneManagerHelper.ActiveSceneName == "DreamWorld")
            {
                foreach (PlayerSystem _character in Global.Lobby.PlayersInLobby)
                {
                    if(!_character.IsLocalPlayer) { return; }
                    float distance = Vector3.Distance(_character.ControlledCharacter.transform.position, spawn);
                    if (distance > 3f)
                    {
                        _character.ControlledCharacter.Teleport(spawn, spawnRot);
                    }
                }
            }
        }

        //////////////////////////////////////
        // DO NOT DELETE
        //////////////////////////////////////
        //to get UIDs from Debug window
        /*[HarmonyPatch(typeof(DT_QuestEventCheats), "OnClickNewQEEvent")]
        public class DT_QuestEventCheats_OnClickNewQEEvent
        {
            [HarmonyPostfix]
            public static void Postfix(string _eventUID, Button _item, BaseEventData _eventData)
            {
                Debug.Log("UID: " + _eventUID);
                System.Windows.Forms.Clipboard.SetText(_eventUID);
            }
        }*/
    }
}