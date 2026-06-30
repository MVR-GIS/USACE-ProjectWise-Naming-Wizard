using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace USACE_Wizard_GUI.UserSettings
{
    public class Setting
    {
        private Utility.ModifyRegistry _settings;

        public string CoP
        {
            get
            {
                return _settings.Read("CoP");
            }
            set
            {
                _settings.Write("CoP", value);
            }
        }

        public string Filter
        {
            get
            {
                return _settings.Read("Filter");
            }
            set
            {
                _settings.Write("Filter", value);
            }
        }

        public int DepartmentNumber
        {
            get
            {
                return Convert.ToInt32(_settings.Read("Department"));
            }
            set
            {
                _settings.Write("Department", value.ToString());
            }
        }

        public Setting()
        {
            _settings = new Utility.ModifyRegistry();
            _settings.ShowError = true;
            _settings.BaseRegistryKey = Registry.CurrentUser;
            _settings.SubKey = @"SOFTWARE\Bentley\USACEDCW";

            if (string.IsNullOrEmpty(_settings.Read("DCW")))
            {
                _settings.Write("DCW", "1");
            }
        }
    }
}
