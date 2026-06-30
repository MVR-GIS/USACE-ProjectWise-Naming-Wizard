namespace USACE_Wizard_GUI
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines the <see cref="FileImportCollection" />
    /// </summary>
    public class FileImportCollection : List<FileImport>
    {
        /// <summary>
        /// Defines the UseFileModifiedDateChanged
        /// </summary>
        public event EventHandler UseFileModifiedDateChanged;

        /// <summary>
        /// Defines the FileUpdated
        /// </summary>
        public event EventHandler<FileInfoEventArgs> FileUpdated;

        /// <summary>
        /// Defines the AllFilesUpdated
        /// </summary>
        public event EventHandler AllFilesUpdated;

        /// <summary>
        /// Defines the MatchAllChanged
        /// </summary>
        public event EventHandler MatchAllChanged;

        /// <summary>
        /// Defines the MatchAllFilenameToNameChanged
        /// </summary>
        public event EventHandler MatchAllFilenameToNameChanged;

        /// <summary>
        /// Defines the SelectionChanged
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Defines the _matchAll
        /// </summary>
        private bool _matchAll = false;

        /// <summary>
        /// Gets a value indicating whether IsValid
        /// </summary>
        public bool IsValid
        {
            get
            {
                var DuplicateFilenames = FindDuplicateFileNames();
                var DuplicateFilenameCount = DuplicateFilenames.Count;
                var DuplicateNames = FindDuplicateNames();
                var DuplicateNameCount = DuplicateNames.Count;

                if (DuplicateNameCount > 0 || DuplicateFilenameCount > 0)
                {
                    return false;
                }

                foreach (var item in this)
                {
                    if (!item.IsAllValid)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Gets the ValidationMessage
        /// </summary>
        public string ValidationMessage
        {
            get
            {
                if (IsValid)
                {
                    //var ValidationResults = new List<ValidationInfo>();
                    //ValidationOccurred?.Invoke(this, ValidationResults);
                    return "All files are valid";
                }
                else
                {
                    //var ValidationResults = new List<ValidationInfo>();
                    var msg = string.Empty;
                    var nonValidFiles = this.Where(f => f.IsAllValid == false);
                    foreach (var item in nonValidFiles)
                    {
                        if (msg == string.Empty)
                        {
                            msg = item.FileName + ": " + item.ValidationMessage;
                        }
                        else
                        {
                            msg += ", " + item.FileName + ": " + item.ValidationMessage;
                        }

                        //ValidationResults.Add(new ValidationInfo() { File = item, Message = item.ValidationMessage });
                    }

                    var DuplicateFilenames = FindDuplicateFileNames();
                    var DuplicateNames = FindDuplicateNames();

                    if (DuplicateFilenames.Count > 0)
                    {
                        foreach (var item in DuplicateFilenames)
                        {
                            if (msg == string.Empty)
                            {
                                msg = item.Count + " files have the same filename " + item.Value;
                            }
                            else
                            {
                                msg += ", " + item.Count + " files have the same filename " + item.Value;
                            }

                            //var d = this.Where(i => i.PWNewFilename == item.Value);
                            //foreach (var ditem in d)
                            //{
                            //    var found = false;
                            //    foreach (var vitem in ValidationResults)
                            //    {
                            //        if (vitem.File == ditem)
                            //        {
                            //            vitem.Message += ", duplicate filenames";
                            //            found = true;
                            //        }
                            //    }
                            //    if (!found)
                            //    {
                            //        ValidationResults.Add(new ValidationInfo() { File = ditem, Message = "Duplicate filenames" });
                            //    } 
                            //}
                        }
                    }

                    if (DuplicateNames.Count > 0)
                    {
                        foreach (var item in DuplicateNames)
                        {
                            if (msg == string.Empty)
                            {
                                msg = item.Count + " files have the same name " + item.Value;
                            }
                            else
                            {
                                msg += ", " + item.Count + " files have the same name " + item.Value;
                            }

                            //var d = this.Where(i => i.PWNewName == item.Value);
                            //foreach (var ditem in d)
                            //{
                            //    var found = false;
                            //    foreach (var vitem in ValidationResults)
                            //    {
                            //        if (vitem.File == ditem)
                            //        {
                            //            vitem.Message += ", duplicate names";
                            //            found = true;
                            //        }
                            //    }
                            //    if (!found)
                            //    {
                            //        ValidationResults.Add(new ValidationInfo() { File = ditem, Message = "Duplicate names" });
                            //    } 
                            //}
                        }
                    }

                    //ValidationOccurred?.Invoke(this, ValidationResults);
                    return msg;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether MatchAll
        /// </summary>
        public bool MatchAll
        {
            get
            {
                return _matchAll;
            }
            set
            {
                if (_matchAll != value)
                {
                    _matchAll = value;
                    //SetUseSameNameForAllAppendNumber();
                    MatchAllChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        private bool _autoNumberDuplicates = true;

        public bool AutoNumberDuplicates
        {
            get
            {
                return _autoNumberDuplicates;
            }
            set
            {
                if (_autoNumberDuplicates != value)
                {
                    _autoNumberDuplicates = value;
                }
            }
        }

        /// <summary>
        /// Defines the _matchAllFilenameToName
        /// </summary>
        private bool _matchAllFilenameToName;

        /// <summary>
        /// Gets or sets a value indicating whether MatchAllFilenameToName
        /// </summary>
        public bool MatchAllFilenameToName
        {
            get
            {
                return _matchAllFilenameToName;
            }
            set
            {
                if (_matchAllFilenameToName != value)
                {
                    _matchAllFilenameToName = value;
                    SetMatchFileNameToNameAll(_matchAllFilenameToName);
                    RenameDuplicate();
                    MatchAllFilenameToNameChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Defines the _selected
        /// </summary>
        public FileImport _selected;

        /// <summary>
        /// Gets or sets the Selected
        /// </summary>
        public FileImport Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                SelectionChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Gets or sets the SelectedIndex
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                if (Selected != null)
                {
                    return IndexOf(Selected);
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                Selected = this[value];
            }
        }

        /// <summary>
        /// Defines the _useFileModifiedDate
        /// </summary>
        private bool _useFileModifiedDate;

        /// <summary>
        /// Gets or sets a value indicating whether UseFileModifiedDate
        /// </summary>
        public bool UseFileModifiedDate
        {
            get
            {
                return _useFileModifiedDate;
            }
            set
            {
                if (_useFileModifiedDate != value)
                {
                    _useFileModifiedDate = value;
                    foreach (var item in this)
                    {
                        item.UseFileLastModifiedDate = value;
                    }
                    _namingData.UserSettings.UseModifiedDate = value;
                    UseFileModifiedDateChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Defines the _useOrignalDescription
        /// </summary>
        private bool _useOrignalDescription;

        /// <summary>
        /// Gets or sets a value indicating whether UserOrignalDescription
        /// </summary>
        public bool UserOrignalDescription
        {
            get
            {
                return _useOrignalDescription;
            }
            set
            {
                if (_useOrignalDescription != value)
                {
                    _useOrignalDescription = value;
                    foreach (var item in this)
                    {
                        item.UseOriginalDescription = value;
                    }
                    UseOriginalDescriptionChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Defines the UseOriginalDescriptionChanged
        /// </summary>
        public event EventHandler UseOriginalDescriptionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileImportCollection"/> class.
        /// </summary>
        /// <param name="files">The files<see cref="List{string}"/></param>
        /// <param name="namingData">The namingData<see cref="StandardNamingData"/></param>
        public FileImportCollection(List<string> files, StandardNamingData namingData)
        {
            _namingData = namingData;

            foreach (var item in files)
            {
                var f = new FileImport(item, namingData);
                f.Description = string.Empty;
                f.Updated += F_Updated;
                Add(f);
            }

            Sort((x, y) => x.FilePath.CompareTo(y.FilePath));

            if (this.Count > 0) Selected = this[0];

            MatchAllFilenameToName = true;
            UseFileModifiedDate = _namingData.UserSettings.UseModifiedDate;
        }

        /// <summary>
        /// Defines the _namingData
        /// </summary>
        private StandardNamingData _namingData;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileImportCollection"/> class.
        /// </summary>
        /// <param name="files">The files<see cref="List{int}"/></param>
        /// <param name="namingData">The namingData<see cref="StandardNamingData"/></param>
        public FileImportCollection(List<int> files, StandardNamingData namingData)
        {
            _namingData = namingData;

            foreach (var item in files)
            {
                var f = new FileImport(item, _namingData);
                f.Updated += F_Updated;
                Add(f);
            }

            Sort((x, y) => x.FilePath.CompareTo(y.FilePath));

            if (this.Count > 0) Selected = this[0];

            MatchAllFilenameToName = true;
            UseFileModifiedDate = _namingData.UserSettings.UseModifiedDate;
        }

        /// <summary>
        /// The ChangeDestination
        /// </summary>
        /// <param name="projectID">The projectID<see cref="int"/></param>
        /// <param name="namingData">The namingData<see cref="StandardNamingData"/></param>
        public void ChangeDestination(int projectID, StandardNamingData namingData)
        {
            if (_namingData == null) _namingData = namingData;
            _namingData.DestinationProjectID = projectID;
        }

        /// <summary>
        /// The F_Updated
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="FileInfoEventArgs"/></param>
        private void F_Updated(object sender, FileInfoEventArgs e)
        {
            foreach (var item in this)
            {
                item.Updated -= F_Updated;
            }

            SetUseSameNameForAllAppendNumber();
            //CopyFromToAll(e.File);
            FileUpdated?.Invoke(this, new FileInfoEventArgs(e.File));
            if (MatchAll)
            {
                AllFilesUpdated?.Invoke(this, new EventArgs());
                //SetUseSameNameForAllAppendNumber();
                //RenameDuplicate();
            }

            //if (MatchAll)
            //{
            //    CopyFromToAll(e.File);
            //    foreach (var item in this.Where(i => i != e.File))
            //    {
            //        Updated?.Invoke(this, new FileInfoEventArgs(item));
            //    }
            //}

            foreach (var item in this)
            {
                item.Updated += F_Updated;
            }
        }

        /// <summary>
        /// The CopyFromToAll
        /// </summary>
        /// <param name="source">The source<see cref="FileImport"/></param>
        //public void CopyFromToAll(FileImport source)
        //{
        //    foreach (var item in this)
        //    {
        //        item.CopyFrom(source, MatchAll);
        //    }
        //}

        /// <summary>
        /// The SetMatchFileNameToNameAll
        /// </summary>
        /// <param name="value">The value<see cref="bool"/></param>
        public void SetMatchFileNameToNameAll(bool value)
        {
            foreach (var item in this)
            {
                item.MatchNameAndFileName = value;
            }
        }

        /// <summary>
        /// The SetUseSameNameForAllAppendNumber
        /// </summary>
        public void SetUseSameNameForAllAppendNumber()
        {
            if (Selected != null)
            {
                if (_matchAll)
                {
                    foreach (var item in this)
                    {
                        item.CopyFrom(Selected, false); //MatchAll);
                    }
                }
                else
                {
                    foreach (var item in this)
                    {
                        item.Numbering = 0;
                    }
                }
                RenameDuplicate();
            }
        }

        /// <summary>
        /// The RenameDuplicate
        /// </summary>
        public void RenameDuplicate()
        {
            //if (MatchAll || AutoNumberDuplicates)
            //{
            var doneFiles = new List<FileImport>();

            var itemsByGroupWithCount = this.GroupBy(i => new { i.DestinationProjectCode, i.DocumentTypeCode, i.DocumentDate, i.DocumentUserAdded, i.BasePWName, i.Extension })
                     .Select(item => new
                     {
                         ProjCode = item.Key.DestinationProjectCode,
                         DocCode = item.Key.DocumentTypeCode,
                         DocDate = item.Key.DocumentDate,
                         DocUserAdded = item.Key.DocumentUserAdded,
                         BaseName = item.Key.BasePWName,
                         DocExt = item.Key.Extension,
                         Total = item.Count()
                     });

            foreach (var item in itemsByGroupWithCount)
            {
                //foreach (var i in this.Where(i => i.DestinationProjectCode == item.ProjCode && i.DocumentTypeCode == item.DocCode && i.DocumentDate == item.DocDate && i.DocumentUserAdded == item.DocUserAdded && i.Extension == item.DocExt))
                //{
                //    i._numbering = 0;
                //}

                //if (item.Total > 0)
                //{
                var nextNum = GetNextSequence(item.BaseName, item.DocExt);
                var ExistingCount = GetExitingCountWithSameBasenameAndExt(item.BaseName, item.DocExt);

                foreach (var f in this.Where(i => i.DestinationProjectCode == item.ProjCode && i.DocumentTypeCode == item.DocCode && i.DocumentDate == item.DocDate && i.DocumentUserAdded == item.DocUserAdded && i.Extension == item.DocExt))
                {
                    //if (!doneFiles.Contains(f)) f._numbering = GetNextSequence(f) + 1;
                    //if (nextNum > 0 && ((ExistingCount > 1 && f.DocumentID > 0) || (ExistingCount > 0 && f.DocumentID == 0)))
                    //{
                        if (MatchAll || (AutoNumberDuplicates && ExistingCount > 1))
                        {
                            f._numbering = nextNum;
                            nextNum = nextNum + 1;
                        }
                        else
                        {
                            f._numbering = 0;
                        }
                    //}
                    //else
                    //{
                    //    f._numbering = 0;
                    //}
                }
                //}
            }
            //}
        }

        private int GetExitingCountWithSameBasenameAndExt(string baseName, string extension)
        {
            return _namingData.ExistingFilesDataTable.AsEnumerable().Where(i => i["o_itemname"].ToString().StartsWith(baseName) && i["o_itemname"].ToString().EndsWith(extension)).Count();
        }

        public int GetNextSequence(string baseName, string extension)//FileImport file)
        {
            var existingTopSequenceNumber = 0;

            var selected = this.Select(s => s.PWOriginalName).ToList();

            //var existing = _namingData.ExistingFilesDataTable.AsEnumerable().Where(i => i["o_itemname"].ToString().StartsWith(file.BasePWName) && i["o_itemname"].ToString().EndsWith(file.Extension) && !selected.Contains(i["o_itemname"].ToString()));
            var existing = _namingData.ExistingFilesDataTable.AsEnumerable().Where(i => i["o_itemname"].ToString().StartsWith(baseName) && i["o_itemname"].ToString().EndsWith(extension) && !selected.Contains(i["o_itemname"].ToString()));

            var existingFilenames = new List<string>();

            foreach (DataRow ERow in existing)
            {
                var f = ERow["o_itemname"].ToString();
                var num = GetNumber(f);
                if (num > existingTopSequenceNumber)
                {
                    existingTopSequenceNumber = num;
                }
                existingFilenames.Add(f);
            }

            //var n = this.Where(i => i.PWNewName.StartsWith(file.BasePWName) && i.Extension == file.Extension).Count();
            //if (n > existingTopSequenceNumber)
            //{
            //    existingTopSequenceNumber = n;
            //}

            //foreach (var item in this.Where(i => i.PWNewName.StartsWith(file.BasePWName) && i.Extension == file.Extension))
            //{
            //    if (!existingFilenames.Contains(item.PWOriginalFilename) && file.OriginalSequenceNumber != 0)
            //    {
            //        existingTopSequenceNumber = file.OriginalSequenceNumber - 1;
            //    }
            //    else
            //    {
            //        var num = this.Where(i => i.PWNewName.StartsWith(file.BasePWName) && i.Extension == file.Extension).Select(f => f.OriginalSequenceNumber).Max();
            //        //var f = item.PWNewName;
            //        //var num = GetNumber(f);
            //        if (num > existingTopSequenceNumber)
            //        {
            //            existingTopSequenceNumber = num;
            //        }
            //    }
            //}

            return existingTopSequenceNumber + 1;
        }

        private int GetNumber(string fileName)
        {
            string pattern = @"_\d{3}.";
            string patternOrig = @"\(\d{3}\).";

            int existingTopSequenceNumber = 0;

            Match m = Regex.Match(fileName, pattern, RegexOptions.IgnoreCase);
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
                Match m2 = Regex.Match(fileName, patternOrig, RegexOptions.IgnoreCase);
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

        /// <summary>
        /// The FindDuplicateNames
        /// </summary>
        /// <returns>The <see cref="List{FileDuplicateStats}"/></returns>
        private List<FileDuplicateStats> FindDuplicateNames()
        {
            var retVal = this.GroupBy(i => i.PWNewName)
                .Select(item => new FileDuplicateStats()
                {
                    Value = item.Key,
                    Count = item.Count()
                }).Where(i => i.Count > 1).ToList();

            return retVal;
        }

        /// <summary>
        /// The FindDuplicateFileNames
        /// </summary>
        /// <returns>The <see cref="List{FileDuplicateStats}"/></returns>
        private List<FileDuplicateStats> FindDuplicateFileNames()
        {
            var retVal = this.GroupBy(i => i.PWNewFilename)
                .Select(item => new FileDuplicateStats()
                {
                    Value = item.Key,
                    Count = item.Count()
                }).Where(i => i.Count > 1).ToList();

            return retVal;
        }

        /// <summary>
        /// Defines the <see cref="FileDuplicateStats" />
        /// </summary>
        private class FileDuplicateStats
        {
            /// <summary>
            /// Gets or sets the Value
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// Gets or sets the Count
            /// </summary>
            public int Count { get; set; }
        }

        /// <summary>
        /// The MoveSelectedToIndex
        /// </summary>
        /// <param name="index">The index<see cref="int"/></param>
        public void MoveSelectedToIndex(int index)
        {
            if (SelectedIndex > -1)
            {
                FileImport i = this[SelectedIndex];
                RemoveAt(SelectedIndex);
                Insert(index, i);
                SelectedIndex = index;

                RenameDuplicate();
            }
        }
    }
}
