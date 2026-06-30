namespace USACE_Wizard_GUI
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines the <see cref="FileImport" />
    /// </summary>
    public class FileImport
    {
        public const string SequencePattern = @"_\d{3}.";

        public const string SequencePatternOriginal = @"\(\d{3}\).";

        /// <summary>
        /// Defines the IsDocCodeValidChanged
        /// </summary>
        public event EventHandler<bool> IsDocCodeValidChanged;

        /// <summary>
        /// Defines the IsDocDateValidChanged
        /// </summary>
        public event EventHandler<bool> IsDocDateValidChanged;

        /// <summary>
        /// Defines the IsProjCodeValidChanged
        /// </summary>
        public event EventHandler<bool> IsProjCodeValidChanged;

        /// <summary>
        /// Defines the UseLastModifiedDateChanged
        /// </summary>
        public event EventHandler<FileInfoEventArgs> UseLastModifiedDateChanged;

        /// <summary>
        /// Defines the ProjCodeUpdated
        /// </summary>
        public event EventHandler<FileInfoEventArgs> ProjCodeUpdated;

        /// <summary>
        /// Defines the DocCodeUpdated
        /// </summary>
        public event EventHandler<FileInfoEventArgs> DocCodeUpdated;

        /// <summary>
        /// Defines the DocDateUpdated
        /// </summary>
        public event EventHandler<FileInfoEventArgs> DocDateUpdated;

        /// <summary>
        /// Defines the DocUserAddedUpdated
        /// </summary>
        public event EventHandler<FileInfoEventArgs> DocUserAddedUpdated;

        /// <summary>
        /// Defines the NumberingUpdated
        /// </summary>
        public event EventHandler<FileInfoEventArgs> NumberingUpdated;

        /// <summary>
        /// Defines the Updated
        /// </summary>
        public event EventHandler<FileInfoEventArgs> Updated;

        /// <summary>
        /// Defines the DescriptionUpdated
        /// </summary>
        public event EventHandler<FileInfoEventArgs> DescriptionUpdated;

        /// <summary>
        /// Defines the KeywordsUpdated
        /// </summary>
        public event EventHandler<FileInfoEventArgs> KeywordsUpdated;

        /// <summary>
        /// Defines the DepartmentUpdated
        /// </summary>
        public event EventHandler<FileInfoEventArgs> DepartmentUpdated;

        /// <summary>
        /// Defines the OriginalFilenameUpdated
        /// </summary>
        public event EventHandler<FileInfoEventArgs> OriginalFilenameUpdated;

        /// <summary>
        /// Defines the MatchNameAndFileNameChanged
        /// </summary>
        public event EventHandler<FileInfoEventArgs> MatchNameAndFileNameChanged;

        /// <summary>
        /// Defines the _matchNameAndFileName
        /// </summary>
        private bool _matchNameAndFileName;

        /// <summary>
        /// Gets or sets a value indicating whether MatchNameAndFileName
        /// </summary>
        public bool MatchNameAndFileName
        {
            get
            {
                return _matchNameAndFileName;
            }
            set
            {
                if (_matchNameAndFileName != value)
                {
                    _matchNameAndFileName = value;
                    MatchNameAndFileNameChanged?.Invoke(this, new FileInfoEventArgs(this));
                }
            }
        }

        private Guid _tempID;

        public Guid TempID { get; }

        /// <summary>
        /// Gets or sets the FilePath
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets the FileName
        /// </summary>
        public string FileName
        {
            get { return Path.GetFileName(FilePath); }
        }

        /// <summary>
        /// Gets or sets the PWOriginalName
        /// </summary>
        public string PWOriginalName { get; set; }

        //GTH 6/9/2022 - Add boolean attribute record_copy
        public Boolean Record_Copy { get; set; }
        /// <summary>
        /// Gets the PWNewName
        /// </summary>
        /// 

        private string _Discipline;
        //GTH 12/5/2022 - Discipline AKA CoP
        public string Discipline
        {
            get
            {
                return _Discipline;
            }

            set
            {
                _Discipline = value;
                Filter.FilterCoP = _Discipline;
            }
        }

        public string ProductName { get; set; }

        public string Doc_Type
        {
            get
            {
                return Filter.FilterDocumentType;
            }
            set
            {
                Filter.FilterDocumentType = value;
            }
        }

        // AKA Doc Code
        //public string Doc_Sub_Type { get; set; }
        //END GTH

        public string PWNewName
        {
            get { return GetNewName(); }
        }

        public string SequenceName
        {
            get
            {
                Match m = Regex.Match(this.PWNewName, SequencePattern, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    return m.Value.Replace(".", "");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public int OriginalSequenceNumber
        {
            get
            {
                int existingTopSequenceNumber = 0;

                var t = PWOriginalName;
                if (string.IsNullOrEmpty(t)) t = FileName;

                Match m = Regex.Match(t, SequencePattern, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    var num = Convert.ToInt32(m.Value.Replace("_", "").Replace(".", ""));
                    if (num > existingTopSequenceNumber)
                    {
                        existingTopSequenceNumber = num;
                    }
                }
                else
                {
                    Match m2 = Regex.Match(t, SequencePatternOriginal, RegexOptions.IgnoreCase);
                    if (m2.Success)
                    {
                        var num = Convert.ToInt32(m2.Value.Replace("(", "").Replace(")", "").Replace(".", ""));
                        if (num > existingTopSequenceNumber)
                        {
                            existingTopSequenceNumber = num;
                        }
                    }
                }

                return existingTopSequenceNumber;
            }
        }

        /// <summary>
        /// Defines the _originalFilename
        /// </summary>
        private string _originalFilename;

        /// <summary>
        /// Gets or sets the PWOriginalFilename
        /// </summary>
        public string PWOriginalFilename
        {
            get
            {
                return _originalFilename;
            }
            set
            {
                if (_originalFilename != value)
                {
                    _originalFilename = value;
                    OriginalFilenameUpdated?.Invoke(this, new FileInfoEventArgs(this));
                    Updated?.Invoke(this, new FileInfoEventArgs(this));
                }
            }
        }

        /// <summary>
        /// Gets the PWNewFilename
        /// </summary>
        public string PWNewFilename
        {
            get
            {
                if (string.IsNullOrEmpty(PWOriginalFilename))
                {
                    PWOriginalFilename = FileName;
                }

                if (MatchNameAndFileName)
                {
                    return PWNewName;
                }
                else
                {
                    return PWOriginalFilename;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether UseOriginalDescription
        /// </summary>
        public bool UseOriginalDescription { get; set; } = false;

        /// <summary>
        /// Defines the _description
        /// </summary>
        private string _description;

        /// <summary>
        /// Defines the _descriptionOriginal
        /// </summary>
        private string _descriptionOriginal;

        /// <summary>
        /// Gets the _descriptionDefault
        /// </summary>
        private string _descriptionDefault
        {
            get { return Filter.GetDescriptionDefault(DocumentTypeCode); }
        }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description
        {
            get
            {
                if (UseOriginalDescription)
                {
                    return _descriptionOriginal;
                }
                else
                {
                    var retVal = (string.IsNullOrEmpty(_description)) ? _descriptionDefault : _description;
                    if (string.IsNullOrEmpty(retVal))
                    {
                        retVal = _descriptionOriginal;
                    }
                    return retVal;
                }
            }
            set
            {
                if (_descriptionOriginal == null)
                {
                    _descriptionOriginal = value;
                }

                if (UseOriginalDescription)
                {
                    if (_descriptionOriginal != value)
                    {
                        _descriptionOriginal = value;
                        DescriptionUpdated?.Invoke(this, new FileInfoEventArgs(this));
                        Updated?.Invoke(this, new FileInfoEventArgs(this));
                    }
                }
                else
                {
                    if (_description != value)
                    {
                        _description = value;
                        DescriptionUpdated?.Invoke(this, new FileInfoEventArgs(this));
                        Updated?.Invoke(this, new FileInfoEventArgs(this));
                    }
                }
            }
        }

        /// <summary>
        /// Defines the _keywords
        /// </summary>
        private string _keywords;

        /// <summary>
        /// Gets or sets the Keywords
        /// </summary>
        public string Keywords
        {
            get
            {
                //if (string.IsNullOrEmpty(_keywords) && DocumentID != 0)
                //{
                //    return PWWrapper.GetAttributeColumnValue(DestinationProjectID, DocumentID, "Keywords");
                //}
                if (string.IsNullOrEmpty(_keywords))
                {
                    return string.Join("; ", _keywordsDefaultSelected.ToArray());
                }
                else
                {
                    return _keywords;
                }
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && _keywords != value)
                {
                    _keywords = value;
                    KeywordsUpdated?.Invoke(this, new FileInfoEventArgs(this));
                    Updated?.Invoke(this, new FileInfoEventArgs(this));
                }
            }
        }

        /// <summary>
        /// Defines the _keywordsAvailable
        /// </summary>
        private List<string> _keywordsAvailable;

        /// <summary>
        /// Gets the _keywordsDefault
        /// </summary>
        private List<string> _keywordsDefault
        {
            get
            {
                return Filter.GetKeywordsDefault(DocumentTypeCode).Split(';').ToList()
                    .Select(m => m.StartsWith("!") ? m.Replace("!", "").Trim() : m.Trim()).ToList()
                    .Where(m => !string.IsNullOrEmpty(m)).ToList();
            }
        }

        /// <summary>
        /// Gets the _keywordsDefaultSelected
        /// </summary>
        private List<string> _keywordsDefaultSelected
        {
            get
            {
                return Filter.GetKeywordsDefault(DocumentTypeCode).Split(';').ToList()
                    .Where(k => k.StartsWith("!")).ToList()
                    .Select(m => m.StartsWith("!") ? m.Replace("!", "").Trim() : m.Trim()).ToList()
                    .Select(m => m.Trim()).ToList().Where(m => !string.IsNullOrEmpty(m)).ToList();
            }
        }

        /// <summary>
        /// Gets the KeywordsSelected
        /// </summary>
        public List<string> KeywordsSelected
        {
            get
            {
                return Keywords.Split(';').ToList().Select(k => k.Trim()).ToList().Where(k => !string.IsNullOrEmpty(k)).ToList();
            }
        }

        /// <summary>
        /// Gets the KeywordsAvailable
        /// </summary>
        public List<string> KeywordsAvailable
        {
            get
            {
                _keywordsAvailable = new List<string>(_keywordsDefault.ToArray());

                foreach (var item in KeywordsSelected)
                {
                    if (!_keywordsAvailable.Contains(item))
                    {
                        _keywordsAvailable.Add(item);
                    }
                }

                return _keywordsAvailable;
            }
        }

        /// <summary>
        /// Gets or sets the DocumentID
        /// </summary>
        public int DocumentID { get; set; }

        /// <summary>
        /// Gets or sets the Filter
        /// </summary>
        public Filter Filter { get; set; }

        /// <summary>
        /// Gets the FilterType
        /// Gets or sets the FilterType
        /// </summary>
        public string FilterType
        {
            get { return Filter.FilterType; }
        }

        /// <summary>
        /// Gets the FilterCoP
        /// Gets or sets the FilterCoP
        /// </summary>
        public string FilterCoP
        {
            get { return Filter.FilterCoP; }
        }

        /// <summary>
        /// Gets the FilterDocumentType
        /// Gets or sets the FilterDoc
        /// </summary>
        public string FilterDocumentType
        {
            get { return Filter.FilterDocumentType; }
        }

        /// <summary>
        /// Gets the ProjCode
        /// Defines the _projCode
        /// </summary>
        //private string _projCode;

        /// <summary>
        /// Gets or sets the ProjCode
        /// </summary>
        public string DestinationProjectCode
        {
            get
            {
                //IsProjCodeValidChanged?.Invoke(this, IsProjCodeValid);
                return _namingData.DestinationProjectCode;
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsProjCodeValid
        /// </summary>
        public bool IsProjCodeValid
        {
            get
            {
                var retVal = (!string.IsNullOrEmpty(DestinationProjectCode));
                IsProjCodeValidChanged?.Invoke(this, retVal);
                return retVal;
            }
        }

        /// <summary>
        /// Defines the _docCode
        /// </summary>
        private string _docCode;

        /// <summary>
        /// Gets or sets the DocumentTypeCode
        /// </summary>
        public string DocumentTypeCode
        {
            get
            {
                return _docCode;
            }
            set
            {
                if (_docCode != value)
                {
                    _docCode = value;
                    DocCodeUpdated?.Invoke(this, new FileInfoEventArgs(this));
                    Updated?.Invoke(this, new FileInfoEventArgs(this));
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsDocCodeValid
        /// </summary>
        public bool IsDocCodeValid
        {
            get
            {
                var retVal = (!DocumentTypeCode.StartsWith("{") && !DocumentTypeCode.EndsWith("}"));
                IsDocCodeValidChanged?.Invoke(this, retVal);
                return retVal;
            }
        }

        /// <summary>
        /// Gets a value indicating whether DocNameExist
        /// </summary>
        public bool DocNameExist
        {
            get
            {
                return _namingData.ExistingFilesDataTable.AsEnumerable().Any(row => row["o_itemname"].ToString() == PWNewName);
            }
        }

        /// <summary>
        /// Gets a value indicating whether FileNameExist
        /// </summary>
        public bool FileNameExist
        {
            get
            {
                return _namingData.ExistingFilesDataTable.AsEnumerable().Any(row => row["o_filename"].ToString() == PWNewFilename);
            }
        }

        /// <summary>
        /// Defines the _docFileDate
        /// </summary>
        private string _docFileDate;

        /// <summary>
        /// Defines the _docDate
        /// </summary>
        private string _docDate;

        /// <summary>
        /// Gets or sets the DocumentDate
        /// </summary>
        public string DocumentDate
        {
            get
            {
                LoadFileDate();

                var retVal = string.Empty;

                if (UseFileLastModifiedDate && string.IsNullOrEmpty(_docFileDate))
                {
                    retVal = _docFileDate;
                }
                else if (UseFileLastModifiedDate && !string.IsNullOrEmpty(_docFileDate))
                {
                    retVal = _docFileDate;
                }
                else if (!string.IsNullOrEmpty(_docDate))
                {
                    retVal = _docDate;
                }
                else
                {
                    _docDate = DateTime.Now.ToString("yyyy-MM-dd");
                    retVal = _docDate;
                }

                return retVal;
            }
            set
            {
                if (_docDate != value)
                {
                    _docDate = value;
                    DocDateUpdated?.Invoke(this, new FileInfoEventArgs(this));
                    Updated?.Invoke(this, new FileInfoEventArgs(this));
                }
            }
        }

        /// <summary>
        /// The LoadFileDate
        /// </summary>
        private void LoadFileDate()
        {
            if (string.IsNullOrEmpty(_docFileDate))
            {
                var dtDoc = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT o_fupdatetime FROM dms_doc WHERE o_projectno = {0} AND o_itemno = {1}", DestinationProjectID, DocumentID), "CurrentDocument");

                if (dtDoc.Rows.Count > 0)
                {
                    foreach (DataRow row in dtDoc.Rows)
                    {
                        _docFileDate = Convert.ToDateTime(row["o_fupdatetime"]).ToString("yyyy-MM-dd");
                    }
                }
                else if (File.Exists(FilePath))
                {
                    _docFileDate = File.GetLastWriteTime(FilePath).ToString("yyyy-MM-dd");
                }
                else
                {
                    _docFileDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }

            if (string.IsNullOrEmpty(_docDate))
            {
                _docDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsDocDateValid
        /// </summary>
        public bool IsDocDateValid
        {
            get
            {
                var retVal = (!string.IsNullOrEmpty(DocumentDate));
                IsDocDateValidChanged?.Invoke(this, retVal);
                return retVal;
            }
        }

        /// <summary>
        /// Defines the _docUserAdded
        /// </summary>
        private string _docUserAdded;

        /// <summary>
        /// Gets or sets the DocumentUserAdded
        /// </summary>
        public string DocumentUserAdded
        {
            get
            {
                return _docUserAdded;
            }
            set
            {
                if (_docUserAdded != value)
                {
                    _docUserAdded = value;
                    DocUserAddedUpdated?.Invoke(this, new FileInfoEventArgs(this));
                    Updated?.Invoke(this, new FileInfoEventArgs(this));
                }
            }
        }

        /// <summary>
        /// Defines the _numbering
        /// </summary>
        public int _numbering = 0;

        /// <summary>
        /// Gets or sets the Numbering
        /// </summary>
        public int Numbering
        {
            get
            {
                return _numbering;
            }
            set
            {
                if (_numbering != value)
                {
                    _numbering = value;
                    NumberingUpdated?.Invoke(this, new FileInfoEventArgs(this));
                    Updated?.Invoke(this, new FileInfoEventArgs(this));
                }
            }
        }

        /// <summary>
        /// Defines the _useFileLastModifiedDate
        /// </summary>
        private bool _useFileLastModifiedDate;

        /// <summary>
        /// Gets or sets a value indicating whether UseFileLastModifiedDate
        /// </summary>
        public bool UseFileLastModifiedDate
        {
            get
            {
                return _useFileLastModifiedDate;
            }
            set
            {
                if (_useFileLastModifiedDate != value)
                {
                    _useFileLastModifiedDate = value;
                    UseLastModifiedDateChanged?.Invoke(this, new FileInfoEventArgs(this));
                }
            }
        }

        /// <summary>
        /// Defines the _department
        /// </summary>
        private int _department = 0;

        /// <summary>
        /// Defines the _namingData
        /// </summary>
        private StandardNamingData _namingData;

        /// <summary>
        /// Gets or sets the Department
        /// </summary>
        public int Department
        {
            get
            {
                return (_department > 0) ? _department : _namingData.UserSettings.DepartmentNumber;
            }
            set
            {
                if (_department != value)
                {
                    _department = value;
                    _namingData.UserSettings.DepartmentNumber = value;
                    DepartmentUpdated?.Invoke(this, new FileInfoEventArgs(this));
                    Updated?.Invoke(this, new FileInfoEventArgs(this));
                }
            }
        }

        /// <summary>
        /// Gets the Extension
        /// </summary>
        public string Extension
        {
            get
            {
                string ext = Path.GetExtension(FilePath);
                if (string.IsNullOrEmpty(ext))
                {
                    ext = Path.GetExtension(FileName);
                }
                //if (string.IsNullOrEmpty(ext))
                //{
                //    ext = Path.GetExtension(PWNewName);
                //}
                if (string.IsNullOrEmpty(ext))
                {
                    ext = Path.GetExtension(PWOriginalFilename);
                }
                if (string.IsNullOrEmpty(ext))
                {
                    ext = Path.GetExtension(PWOriginalName);
                }
                return ext;
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsAllValid
        /// </summary>
        public bool IsAllValid
        {
            get
            {
                return (IsProjCodeValid && IsDocCodeValid && IsDocDateValid && !DocNameExist && !FileNameExist);
            }
        }

        /// <summary>
        /// Gets the ValidationMessage
        /// </summary>
        public string ValidationMessage
        {
            get
            {
                if (IsAllValid)
                {
                    return "File is valid";
                }
                else
                {
                    var retMsg = string.Empty;
                    if (!IsProjCodeValid) retMsg = BuildValidationMessage(retMsg, "No project code found");
                    if (!IsDocCodeValid) retMsg = BuildValidationMessage(retMsg, "No document type selected");
                    if (!IsDocDateValid) retMsg = BuildValidationMessage(retMsg, "No document date selected");
                    if (DocNameExist) retMsg = BuildValidationMessage(retMsg, PWNewName + " already exist");
                    if (FileNameExist) retMsg = BuildValidationMessage(retMsg, PWNewFilename + " already exist");
                    return retMsg;
                }
            }
        }

        /// <summary>
        /// The BuildValidationMessage
        /// </summary>
        /// <param name="msg">The msg<see cref="string"/></param>
        /// <param name="msgToAppend">The msgToAppend<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        private string BuildValidationMessage(string msg, string msgToAppend)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return msgToAppend;
            }
            else
            {
                return msg + ", " + msgToAppend;
            }
        }

        /// <summary>
        /// Gets the CurrentFolderDescription
        /// </summary>
        public string CurrentFolderDescription
        {
            get
            {
                return PWWrapper.aaApi_GetProjectStringProperty(PWWrapper.ProjectProperty.Desc, 0);
            }
        }

        /// <summary>
        /// Gets the CurrentFolderPath
        /// </summary>
        public string CurrentFolderPath
        {
            get
            {
                return PWWrapper.GetProjectNamePath(DestinationProjectID);
            }
        }

        /// <summary>
        /// Gets the DestinationProjectID
        /// </summary>
        public int DestinationProjectID
        {
            get
            {
                var retVal = 0;

                if (_namingData != null) retVal = _namingData.DestinationProjectID;

                return retVal;
            }
        }

        /// <summary>
        /// Gets the CurrentFolderName
        /// </summary>
        public string CurrentFolderName
        {
            get
            {
                var currentFolderSplit = CurrentFolderPath.Split('\\');
                return currentFolderSplit[currentFolderSplit.Length - 1];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileImport"/> class.
        /// </summary>
        /// <param name="file">The file<see cref="string"/></param>
        /// <param name="namingData">The existingFiles<see cref="DataTable"/></param>
        public FileImport(string file, StandardNamingData namingData)
        {
            _tempID = Guid.NewGuid();
            _namingData = namingData;
            FilePath = file;

            DataTable dtDoc = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT o_itemdesc FROM dms_doc WHERE o_projectno = {0} AND o_itemno = {1}", DestinationProjectID, DocumentID), "CurrentDocument");

            foreach (DataRow row in dtDoc.Rows)
            {
                _descriptionOriginal = row["o_itemdesc"].ToString();
            }

            LoadFilterBasedOnOriginalFileName();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileImport"/> class.
        /// </summary>
        /// <param name="file">The file<see cref="int"/></param>
        /// <param name="namingData">The namingData<see cref="StandardNamingData"/></param>
        public FileImport(int file, StandardNamingData namingData)
        {
            _tempID = Guid.NewGuid();
            _namingData = namingData;
            DocumentID = file;
            FilePath = PWWrapper.GetDocumentNamePath(DestinationProjectID, DocumentID);

            DataTable dtDoc = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT o_itemname, o_itemdesc, o_filename FROM dms_doc WHERE o_projectno = {0} AND o_itemno = {1}", DestinationProjectID, DocumentID), "CurrentDocument");

            foreach (DataRow row in dtDoc.Rows)
            {
                _descriptionOriginal = row["o_itemdesc"].ToString();
                _originalFilename = row["o_filename"].ToString();
                PWOriginalName = row["o_itemname"].ToString();
            }

            LoadFilterBasedOnOriginalFileName();
        }

        /// <summary>
        /// The LoadFilterBasedOnOriginalFileName
        /// </summary>
        public void LoadFilterBasedOnOriginalFileName()
        {
            var fileNameList = new List<string>();
            var seq = string.Empty;

            Match m = Regex.Match(FileName, SequencePattern, RegexOptions.IgnoreCase);
            //var number = 0;
            if (m.Success)
            {
                //_numbering = Convert.ToInt32(m.Value.Replace("_", "").Replace(".", ""));
                seq = m.Value.Replace(".", "");
            }
            else
            {
                Match m2 = Regex.Match(FileName, SequencePatternOriginal, RegexOptions.IgnoreCase);
                //var number = 0;
                if (m2.Success)
                {
                    //_numbering = Convert.ToInt32(m2.Value.Replace("(", "").Replace(")", "").Replace(".", ""));
                    seq = m2.Value.Replace(".", "");
                }
            }

            _numbering = 0;

            var fn = GetFormatedNumber();
            var n = FileName.Replace(fn, "");

            if (!string.IsNullOrEmpty(Extension))
            {
                fileNameList = n.Replace(Extension, "").Split('-').ToList();
            }
            else
            {
                fileNameList = n.Split('-').ToList();
            }

            var standardNameMatch = false;

            if (fileNameList.Count >= 5)
            {
                var projcode = fileNameList[0];
                var doccode = fileNameList[1];
                var docdate = fileNameList[2] + "-" + fileNameList[3] + "-" + fileNameList[4];
                if (!string.IsNullOrEmpty(seq)) docdate = docdate.Replace(seq, "");
                var useradded = n.Replace(projcode + "-", "").Replace(doccode + "-", "").Replace(docdate + "-", "").Replace(docdate, "");
                if (!string.IsNullOrEmpty(seq)) useradded = useradded.Replace(seq, "");

                if (useradded == Extension)
                {
                    useradded = string.Empty;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Extension))
                    {
                        useradded = useradded.Replace(Extension, "");
                    }
                }

                DocumentTypeCode = "{Select Document Type}";
                    DocumentDate = CheckDate(docdate) ? docdate : DateTime.Now.ToString("yyyy-MM-dd");
                    DocumentUserAdded = useradded;

                    Filter = new Filter(_namingData, CurrentFolderDescription, CurrentFolderName);
                    Filter.FilterCoP = _namingData.UserSettings.CoP;
                //    }
            }
            else
            {
                DocumentTypeCode = "{Select Document Type}";
                DocumentUserAdded = "";
                Filter = new Filter(_namingData, CurrentFolderDescription, CurrentFolderName);
                Filter.FilterCoP = _namingData.UserSettings.CoP;
            }
        }

        protected bool CheckDate(string date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string BasePWName
        {
            get
            {
                var baseName = DestinationProjectCode;

                if (!string.IsNullOrEmpty(DocumentTypeCode))
                    baseName += "-" + DocumentTypeCode;

                if (!string.IsNullOrEmpty(DocumentDate))
                    baseName += "-" + DocumentDate;

                if (!string.IsNullOrEmpty(DocumentUserAdded))
                    baseName += "-" + DocumentUserAdded;

                return baseName;
            }
        }

        /// <summary>
        /// The GetNewName
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        private string GetNewName()
        {
            string newName = DestinationProjectCode;

            if (!string.IsNullOrEmpty(DocumentTypeCode))
                newName += "-" + DocumentTypeCode;

            if (!string.IsNullOrEmpty(DocumentDate))
                newName += "-" + DocumentDate;

            if (!string.IsNullOrEmpty(DocumentUserAdded))
                newName += "-" + DocumentUserAdded;

            if (Numbering > 0)
                newName += GetFormatedNumber();

            newName += Extension;

            return newName;
        }

        /// <summary>
        /// The GetFormatedNumber
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        private string GetFormatedNumber()
        {
            return string.Format("_{0}", Numbering.ToString("000"));
        }

        /// <summary>
        /// The CopyFrom
        /// </summary>
        /// <param name="source">The source<see cref="FileImport"/></param>
        /// <param name="fireUpdateEvent">The fireUpdateEvent<see cref="bool"/></param>
        public void CopyFrom(FileImport source, bool fireUpdateEvent)
        {
            DocumentTypeCode = source.DocumentTypeCode;
            DocumentDate = source.DocumentDate;
            DocumentUserAdded = source.DocumentUserAdded;
            //ProjCode = source.ProjCode;
            Filter = source.Filter;
            //FilterCoP = source.FilterCoP;
            //FilterDoc = source.FilterDoc;
            //FilterType = source.FilterType;
            Keywords = source.Keywords;
            Description = source.Description;
            Department = source.Department;
            if (fireUpdateEvent)
            {
                Updated?.Invoke(this, new FileInfoEventArgs(this));
            }
        }
    }
}
