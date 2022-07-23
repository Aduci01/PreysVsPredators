using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace {
    public class StatUi : MonoBehaviour {
        [SerializeField] private TMP_Text preyText, predatorText;
        [SerializeField] private int _predatorNum, _preyNum;

        public void SetStats(int predator, int prey) {
            _predatorNum = predator;
            _preyNum = prey;

            preyText.text = "Preys: " + prey;
            predatorText.text = "Predators: " + predator;
        }

        /*
        private void OnGUI() {
            GUI.Label(new Rect(5, 30, 100, 25), "Predators: " + _predatorNum, _predatorStyle);
            GUI.Label(new Rect(5, 55, 100, 25), "Preys: " + _preyNum, _preyStyle);
        }*/
    }
}