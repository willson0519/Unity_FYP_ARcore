using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{

    public RectTransform prefabMenu, plus, top1, body1,top3,body3,top4,body4,
        close, price, priceClose, priceList, handles, body5, top5, handles3, handles4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void plusSelection()
    {
        prefabMenu.DOAnchorPos(new Vector2(0, -180), 0.25f);
       // close.DOAnchorPos(new Vector2(-11, 191), 0.25f);
        plus.DOAnchorPos(new Vector2(490, -650), 0.25f);
        // priceList.DOAnchorPos(new Vector2(550, 185),0.25f);
        priceClose.DOAnchorPos(new Vector2(349, 185), 0.25f);

    }

    public void obj1()
    {
        body1.DOAnchorPos(new Vector2(-445, -22),0.25f);
        top1.DOAnchorPos(new Vector2(444, -36), 0.25f);
        body3.DOAnchorPos(new Vector2(-717, -22), 0.25f);
        top3.DOAnchorPos(new Vector2(687, -36), 0.25f);
        handles3.DOAnchorPos(new Vector2(1027, -36), 0.25f);
        body4.DOAnchorPos(new Vector2(-839, -22), 0.25f);
        top4.DOAnchorPos(new Vector2(813, -36), 0.25f);
        handles.DOAnchorPos(new Vector2(833, -36), 0.25f);
        body5.DOAnchorPos(new Vector2(-833, -22), 0.25f);
        top5.DOAnchorPos(new Vector2(929, -36), 0.25f);
        handles3.DOAnchorPos(new Vector2(833, -36), 0.25f);
        handles4.DOAnchorPos(new Vector2(1125, -36), 0.25f);

    }

    public void obj2()
    {
        body1.DOAnchorPos(new Vector2(-591, -22), 0.25f);
        top1.DOAnchorPos(new Vector2(565, -36), 0.25f);
        body3.DOAnchorPos(new Vector2(-717, -22), 0.25f);
        top3.DOAnchorPos(new Vector2(687, -36), 0.25f);
        handles3.DOAnchorPos(new Vector2(1027, -36), 0.25f);
        body4.DOAnchorPos(new Vector2(-839, -22), 0.25f);
        top4.DOAnchorPos(new Vector2(813, -36), 0.25f);
        handles.DOAnchorPos(new Vector2(833, -36), 0.25f);
        body5.DOAnchorPos(new Vector2(-833, -22), 0.25f);
        top5.DOAnchorPos(new Vector2(929, -36), 0.25f);
        handles3.DOAnchorPos(new Vector2(833, -36), 0.25f);
        handles4.DOAnchorPos(new Vector2(1125, -36), 0.25f);
    }

    public void obj3()
    {
        body3.DOAnchorPos(new Vector2(-445, -22), 0.25f);
        top3.DOAnchorPos(new Vector2(346, -36), 0.25f);
        handles3.DOAnchorPos(new Vector2(442, -36), 0.25f);
        body1.DOAnchorPos(new Vector2(-591, -22), 0.25f);
        top1.DOAnchorPos(new Vector2(565, -36), 0.25f);
        body4.DOAnchorPos(new Vector2(-839, -22), 0.25f);
        top4.DOAnchorPos(new Vector2(813, -36), 0.25f);
        handles4.DOAnchorPos(new Vector2(1125, -36), 0.25f);
        handles.DOAnchorPos(new Vector2(833, -36), 0.25f);
        body5.DOAnchorPos(new Vector2(-833, -22), 0.25f);
        top5.DOAnchorPos(new Vector2(929, -36), 0.25f);
       

    }

    public void obj4()
    {
        body4.DOAnchorPos(new Vector2(-445, -22), 0.25f);
        top4.DOAnchorPos(new Vector2(346, -36), 0.25f);
        handles4.DOAnchorPos(new Vector2(442, -36), 0.25f);
        body1.DOAnchorPos(new Vector2(-591, -22), 0.25f);
        top1.DOAnchorPos(new Vector2(565, -36), 0.25f);
        body3.DOAnchorPos(new Vector2(-717, -22), 0.25f);
        top3.DOAnchorPos(new Vector2(687, -36), 0.25f);
        handles3.DOAnchorPos(new Vector2(1027, -36), 0.25f);
        handles.DOAnchorPos(new Vector2(833, -36), 0.25f);
        body5.DOAnchorPos(new Vector2(-833, -22), 0.25f);
        top5.DOAnchorPos(new Vector2(929, -36), 0.25f);
       
        
    }
    public void obj5()
    {
        body1.DOAnchorPos(new Vector2(-591, -22), 0.25f);
        top1.DOAnchorPos(new Vector2(565, -36), 0.25f);
        body3.DOAnchorPos(new Vector2(-717, -22), 0.25f);
        top3.DOAnchorPos(new Vector2(687, -36), 0.25f);
        handles3.DOAnchorPos(new Vector2(1027, -36), 0.25f);
        body4.DOAnchorPos(new Vector2(-839, -22), 0.25f);
        top4.DOAnchorPos(new Vector2(813, -36), 0.25f);
        handles4.DOAnchorPos(new Vector2(1125, -36), 0.25f);
        handles.DOAnchorPos(new Vector2(442, -36), 0.25f);
        body5.DOAnchorPos(new Vector2(-445, -22), 0.25f);
        top5.DOAnchorPos(new Vector2(346, -36), 0.25f);
       
    }

    //public void closeMenu()
   // {
     //   body1.DOAnchorPos(new Vector2(-591, -22), 0.25f);
     //   top1.DOAnchorPos(new Vector2(565, -36), 0.25f);
     //   body3.DOAnchorPos(new Vector2(-717, -22), 0.25f);
     //  top3.DOAnchorPos(new Vector2(687, -36), 0.25f);
     //   body4.DOAnchorPos(new Vector2(-839, -22), 0.25f);
     //   top4.DOAnchorPos(new Vector2(813, -36), 0.25f);
     //   prefabMenu.DOAnchorPos(new Vector2(0, -310), 0.25f);
     //   close.DOAnchorPos(new Vector2(-11,280), 0.25f);
     //   plus.DOAnchorPos(new Vector2(490, -433), 0.25f);
        
       
   // }

    public void priceAmount()
    {
        price.DOAnchorPos(new Vector2(-115, 185), 0.25f);
        priceClose.DOAnchorPos(new Vector2(349, 185), 0.25f);
    }
     public void closePrice()
    {
        body1.DOAnchorPos(new Vector2(-591, -22), 0.25f);
        top1.DOAnchorPos(new Vector2(565, -36), 0.25f);
        body3.DOAnchorPos(new Vector2(-717, -22), 0.25f);
        top3.DOAnchorPos(new Vector2(687, -36), 0.25f);
        handles3.DOAnchorPos(new Vector2(1027, -36), 0.25f);
        body4.DOAnchorPos(new Vector2(-839, -22), 0.25f);
        top4.DOAnchorPos(new Vector2(813, -36), 0.25f);
        handles4.DOAnchorPos(new Vector2(1125, -36), 0.25f);
        prefabMenu.DOAnchorPos(new Vector2(0, -310), 0.25f);
        close.DOAnchorPos(new Vector2(-11, 280), 0.25f);
        plus.DOAnchorPos(new Vector2(490, -433), 0.25f);
        price.DOAnchorPos(new Vector2(-115, 282), 0.25f);
        priceClose.DOAnchorPos(new Vector2(350, 282), 0.25f);
        handles.DOAnchorPos(new Vector2(833, -36), 0.25f);
        body5.DOAnchorPos(new Vector2(-833, -22), 0.25f);
        top5.DOAnchorPos(new Vector2(929, -36), 0.25f);
       
    }


}
