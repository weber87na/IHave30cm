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

    //讓 CM 計算好看點
    //Millimeter = mm
    //1 cm = 10mm
    public static class RulerUnitConverter
    {
        public static int CM(this int mm){
            return mm * 10;
        }
    }
}
