using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbabyAnimationCode
{
    public static int GetCGCode(int taytrai, int tayphai, int chantrai, int chanphai, int dau)
    {
        //Co giật toàn thân nhẹ đầu thẳng
        if(taytrai == 1 && tayphai == 1 && chanphai == 1 && chantrai == 1 && dau == 0)
        {
            return 1;
        }

        //Co giật toàn thân nặng đầu thẳng
        if (taytrai == 2 && tayphai == 2 && chanphai == 2 && chantrai == 2 && dau == 0)
        {
            return 2;
        }

        //Co giật toàn thân nhẹ đầu nghiêng
        if (taytrai == 1 && tayphai == 1 && chanphai == 1 && chantrai == 1 && dau == 1)
        {
            return 3;
        }

        //Co giật toàn thân nặng đầu nghiêng
        if (taytrai == 2 && tayphai == 2 && chanphai == 2 && chantrai == 2 && dau == 1)
        {
            return 4;
        }

        return 0;
    }
}
