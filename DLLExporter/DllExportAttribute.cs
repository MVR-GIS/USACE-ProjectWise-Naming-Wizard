using System;

namespace DllExporter
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class DllExportAttribute : Attribute
    {
        private string entryPoint;

        public string EntryPoint
        {
            get
            {
                return this.entryPoint;
            }
            set
            {
                this.entryPoint = value;
            }
        }
    }
}