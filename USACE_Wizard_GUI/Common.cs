using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USACE_Wizard_GUI
{
    public static class Common
    {
        public static int SelectByValue(ComboBox comboBox, string value)
        {
            int i = 0;
            for (i = 0; i <= comboBox.Items.Count - 1; i++)
            {
                DataRowView cb;
                cb = (DataRowView)comboBox.Items[i];

                if (cb.Row.ItemArray[1].ToString() == value)// Change the 0 index if your want to Select by Text as 1 Index
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
