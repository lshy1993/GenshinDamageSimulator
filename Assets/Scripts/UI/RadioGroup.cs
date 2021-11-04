using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class RadioGroup : MonoBehaviour
    {
        public RadioButton[] Buttons;
        private int toggled = -1;

        public void Switch(int x)
        {
            if (toggled != -1) Buttons[toggled].SetToggle(false);
            if (x != -1) Buttons[x].SetToggle(true);
            toggled = x;
            //for(int i = 0; i < Buttons.Length; i++)
            //{
            //    Buttons[i].SetToggle(i == x);
            //}
        }

        public void Reset()
        {
            Switch(-1);
        }

        public Selectable GetFirstBtn()
        {
            return Buttons[0].GetComponent<Selectable>();
        }

        public Selectable GetLastBtn()
        {
            return Buttons[Buttons.Length - 1].GetComponent<Selectable>();
        }
    }
}
