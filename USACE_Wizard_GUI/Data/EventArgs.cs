namespace USACE_Wizard_GUI
{
    using System;

    /// <summary>
    /// Defines the <see cref="FileInfoEventArgs" />
    /// </summary>
    public class FileInfoEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the File
        /// </summary>
        public FileImport File { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfoEventArgs"/> class.
        /// </summary>
        /// <param name="file">The file<see cref="FileImport"/></param>
        public FileInfoEventArgs(FileImport file)
        {
            File = file;
        }
    }

    /// <summary>
    /// Defines the <see cref="FilterEventArgs" />
    /// </summary>
    public class FilterEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterEventArgs"/> class.
        /// </summary>
        /// <param name="value">The value<see cref="string"/></param>
        public FilterEventArgs(string value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Defines the <see cref="FilterAllEventArgs" />
    /// </summary>
    public class FilterAllEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the Filter
        /// </summary>
        public Filter Filter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterAllEventArgs"/> class.
        /// </summary>
        /// <param name="filter">The filter<see cref="Filter"/></param>
        public FilterAllEventArgs(Filter filter)
        {
            Filter = filter;
        }
    }

    public class FilterProductEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the Filter
        /// </summary>
        public Filter Filter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterAllEventArgs"/> class.
        /// </summary>
        /// <param name="filter">The filter<see cref="Filter"/></param>
        public FilterProductEventArgs(Filter filter)
        {
            Filter = filter;
        }
    }
}
