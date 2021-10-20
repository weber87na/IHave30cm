using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IHave30cm
{

    //因為 1 px 太小了 , 每隔 10 px 當一個單位
    public static class RulerUnitConverter
    {
        public static int Unit(this int px){
            return px * 10;
        }
    }
}
