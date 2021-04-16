using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    public Text[] namesText;
    public string a, b, c;
    public Image thirdPlaceImg,secondPlaceImg;
  
   
    void Update()
    {
        namesText[0].text = a;
        namesText[1].text = b;
        namesText[2].text = c;
         

    }
}
