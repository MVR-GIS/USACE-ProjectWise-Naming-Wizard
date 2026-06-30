namespace USACE_Wizard_GUI
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    //using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="Filter" />
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Defines the FilterTypeUpdated
        /// </summary>
        public event EventHandler<FilterEventArgs> FilterTypeUpdated;

        /// <summary>
        /// Defines the FilterCoPUpdated
        /// </summary>
        public event EventHandler<FilterEventArgs> FilterCoPUpdated;

        /// <summary>
        /// Defines the FilterDocumentTypeUpdated
        /// </summary>
        public event EventHandler<FilterEventArgs> FilterDocumentTypeUpdated;

        /// <summary>
        /// Defines the FilterAllUpdated
        /// </summary>
        public event EventHandler<FilterAllEventArgs> FilterAllUpdated;

        //GTH 12/7/2022
        public event EventHandler<FilterProductEventArgs> FilterProductUpdated;
        //END GTH

        /// <summary>
        /// Defines the _filterType
        /// </summary>
        private string _filterType;

        /// <summary>
        /// Gets or sets the FilterType
        /// </summary>
        /// 

        private string _ProductFilter = null;

        public string ProductFilter
        {
            get
            {
                return _ProductFilter;
            }
            set
            {
                if (value == "Selct Product")
                {
                    value = null;
                }
                _ProductFilter = value;

            }
        }
        public string FilterType
        {
            get
            {
                return _filterType;
            }
            set
            {
                if (_filterType != value)
                {
                    _filterType = value;
                    if (_OverwriteSaved) _namingData.UserSettings.Filter = value;
                    FilterTypeUpdated?.Invoke(this, new FilterEventArgs(_filterType));
                    FilterAllUpdated?.Invoke(this, new FilterAllEventArgs(this));
                }
            }
        }

        /// <summary>
        /// Defines the _filterCoP
        /// </summary>
        private string _filterCoP;

        //GTH
        private string _CoPID;

        //END GTH
        public string CoPID
        {
            get
            {
                //return _namingData.DisciplinesDataTable.AsEnumerable().Where(r => r.Field<string>("DisciplineName") == FilterCoP).FirstOrDefault()["ID"].ToString();
                return _CoPID;
            }
        }
        /// <summary>
        /// Gets or sets the FilterCoP
        /// </summary>
        public string FilterCoP
        {
            //AKA Discipline
            get
            {
                return _filterCoP;
            }
            set
            {
                if (_filterCoP != value)
                {
                    _filterCoP = value;
                    //GTH 12/5/2022
                    //Let's just do this once
                    //GTH  10/29/2024 Try/Catch
                    try
                    {
                        _CoPID = _namingData.DisciplinesDataTable.AsEnumerable().Where(r => r.Field<string>("DisciplineName") == FilterCoP).FirstOrDefault()["ID"].ToString();
                    }
                    catch (Exception ex) 
                    { 
                        int fred = 0;                    
                    }
                        //END GTH
                        if (_OverwriteSaved) _namingData.UserSettings.CoP = value;
                    FilterTypeUpdated?.Invoke(this, new FilterEventArgs(_filterCoP));
                    FilterAllUpdated?.Invoke(this, new FilterAllEventArgs(this));
                }
            }
        }

        /// <summary>
        /// Defines the _filterDocumentType
        /// </summary>
        private string _filterDocumentType;

        /// <summary>
        /// Gets or sets the FilterDocumentType
        /// </summary>
        public string FilterDocumentType
        {
            get
            {
                return _filterDocumentType;
            }
            set
            {
                if (_filterDocumentType != value)
                {
                    _filterDocumentType = value;
                    FilterTypeUpdated?.Invoke(this, new FilterEventArgs(_filterDocumentType));
                    FilterAllUpdated?.Invoke(this, new FilterAllEventArgs(this));
                }
            }
        }

        /// <summary>
        /// Gets the FilteredDocumentTypes
        /// </summary>
        public List<string> FilteredDocumentTypes
        {
            get
            {
                var dtFinalResult = FilterDocumentTypesDataTable.DefaultView.ToTable(true, new[] { "docfilter" });
                var vFinalResult = dtFinalResult.DefaultView;
                vFinalResult.Sort = "docfilter ASC";

                var retval = new List<string> { "*All*" };

                retval.AddRange(from DataRowView dataRowView in vFinalResult select dataRowView["DocFilter"].ToString());
                return retval;
            }
        }

        //GTH 12/3/2022

        /// <summary>
        /// Gets the FilteredDocumentCodes
        /// </summary>
        public List<string> FilteredDocumentCodes
        {
            get
            {

                //GTH 12/5/2022
                if (!string.IsNullOrEmpty(_ProductFilter))
                {
                    return _FilteredDocumentCodesByProduct;
                }
                else
                {
                    return _FilteredDocumentCodes;
                }
                //END GTH

            }
        }

        private List<string> _FilteredDocumentCodesByProduct
        {
            get
            {
                List<string> docTypeList = new List<string> { "{Select Document Type}" };

                    DataView dv = new DataView(_namingData.StdNamingDataTable);
                //string rf = "product = '" + _ProductFilter + "' and Discipline = '" + CoPID + "'";
                string rf = "product = '" + _ProductFilter + "' and Discipline like '%" + CoPID + "%'";

                if (_filterDocumentType != "*All*")
                    {
                        rf = rf + " and DocFilter = '" + _filterDocumentType + "'";
                    }
                    dv.RowFilter = rf;

                    //DataTable DT = dv.ToTable(); 
                    foreach (DataRowView drv in dv)
                    {
                        docTypeList.Add(drv["DocumentType"].ToString());
                    }

                    dv.Dispose();


                return docTypeList;
            }
        }
        private List<string> _FilteredDocumentCodes
        {
            get
            {
                List<string> docTypeList = new List<string> { "{Select Document Type}" };

                if (FilterDataView != null)
                {
                    docTypeList.AddRange(from DataRowView dataRowView in FilterDataView select dataRowView["DocumentType"].ToString());
                }
                else
                {
                    docTypeList.AddRange(from DataRowView dataRowView in _namingData.StdNamingDataTable.DefaultView select dataRowView["DocumentType"].ToString());
                }

                return docTypeList;
            }
        }

        //GTH 12/1/2022
        //Filtered Product List
        public List<string> FilteredProductCodes
        {
            get
            {
                List<string> retval = new List<string> { "{Selct Product}"};
                if (FilterDataView != null)
                {
                    retval.AddRange(from DataRowView dataRowView in FilterDataView select dataRowView["Product"].ToString());
                }
                else
                {
                    retval.AddRange(from DataRowView dataRowView in _namingData.StdNamingDataTable.DefaultView select dataRowView["Product"].ToString());
                }
                return retval;
            }
        }
        //END GTH


        /// <summary>
        /// Gets the FilterDataView
        /// </summary>
        private DataView FilterDataView
        {
            get
            {
                if (FilterDocumentTypesDataTable != null)
                {
                    var _FilteredDocTypes = FilterDocumentTypesDataTable.DefaultView;
                    
                    if (FilterDocumentType != "*All*")
                    {
                        _FilteredDocTypes.RowFilter = string.Format("docfilter = '{0}'", FilterDocumentType);
                }
                else
                {
                    _FilteredDocTypes = FilterDocumentTypesDataTable.DefaultView;
                }

                _FilteredDocTypes.Sort = "DocumentType ASC";
                    return _FilteredDocTypes;
                }
                else
                {
                    return FilterDocumentTypesDataTable.DefaultView;
                }
            }
        }

        /// <summary>
        /// The GetDescriptionDefault
        /// </summary>
        /// <param name="docCode">The docCode<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string GetDescriptionDefault(string docCode)
        {
            if (docCode.StartsWith("{") && docCode.EndsWith("}"))
            {
                return string.Empty;
            }
            else
            {
                return _namingData.StdNamingDataTable.AsEnumerable().Where(r => r.Field<string>("Discipline").Split(',').Contains(CoPID) && r.Field<string>("DocumentType") == docCode).FirstOrDefault()["Description"].ToString();
            }
        }

        /// <summary>
        /// The GetKeywordsDefault
        /// </summary>
        /// <param name="docCode">The docCode<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string GetKeywordsDefault(string docCode)
        {
            if (docCode.StartsWith("{") && docCode.EndsWith("}"))
            {
                return string.Empty;
            }
            else
            {
                return _namingData.StdNamingDataTable.AsEnumerable().Where(r => r.Field<string>("Discipline").Split(',').Contains(CoPID) && r.Field<string>("DocumentType") == docCode).FirstOrDefault()["Keyword"].ToString();
            }
        }

        /// <summary>
        /// Gets the FilterDocumentTypesDataTable
        /// </summary>
        public DataTable FilterDocumentTypesDataTable
        {
            get
            {
                if (_namingData != null && _namingData.StdNamingDataTable.Rows.Count > 0)
                {
                    var retVal = _namingData.StdNamingDataTable.Copy();

                    if (this != null && FilterType != null)
                    {
                        switch (FilterType)
                        {
                            case "CoP":
                                if (FilterCoP != null)
                                {

                                    for (var i = retVal.Rows.Count - 1; i >= 0; i--)
                                    {
                                        var dr = retVal.Rows[i];
                                        var disciplines = dr["Discipline"].ToString();
                                        var disciplinesSplit = disciplines.Split(',');

                                        var lsDiscipline = new List<string>(disciplinesSplit);

                                        if (!lsDiscipline.Contains(CoPID))
                                        {
                                            dr.Delete();
                                        }
                                    }
                                }
                                break;
                            case "Folder":
                                for (var i = retVal.Rows.Count - 1; i >= 0; i--)
                                {
                                    var dr = retVal.Rows[i];
                                    var folders = dr["Std_Folders"].ToString().ToUpper().Split(';').ToList().Select(f => f.ToUpper()).ToList();

                                    if (string.IsNullOrEmpty(folders[0].ToString()))
                                    {
                                        dr.Delete();
                                    }
                                    else
                                    {
                                        if (folders.Contains(_currentFolderName.ToUpper()) || folders.Contains(_currentFolderDesc.ToUpper()))
                                        {
                                            if (FilterCoP != null)
                                            {
                                                var disciplines = dr["Discipline"].ToString();
                                                var disciplinesSplit = disciplines.Split(',');

                                                var lsDiscipline = new List<string>(disciplinesSplit);

                                                if (!lsDiscipline.Contains(CoPID))
                                                {
                                                    dr.Delete();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            dr.Delete();
                                        }
                                    }

                                }
                                break;
                            case "All":
                                break;
                            default:
                                break;
                        }
                    }

                    return retVal;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// The Copy
        /// </summary>
        /// <returns>The <see cref="Filter"/></returns>
        internal Filter Copy()
        {
            var retVal = new Filter(_namingData, _currentFolderDesc, _currentFolderName);
            retVal.FilterCoP = FilterCoP;
            retVal.FilterDocumentType = FilterDocumentType;
            retVal.FilterType = FilterType;
            return retVal;
        }

        /// <summary>
        /// Defines the _namingData
        /// </summary>
        private StandardNamingData _namingData;

        private bool _OverwriteSaved;

        /// <summary>
        /// Defines the _currentFolderDesc
        /// </summary>
        private string _currentFolderDesc;

        /// <summary>
        /// Defines the _currentFolderName
        /// </summary>
        private string _currentFolderName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="NamingData">The NamingData<see cref="DataTable"/></param>
        /// <param name="CurrentFolderDesc">The CurrentFolderDesc<see cref="string"/></param>
        /// <param name="CurrentFolderName">The CurrentFolderName<see cref="string"/></param>
        public Filter(StandardNamingData NamingData, string CurrentFolderDesc, string CurrentFolderName)
        {
            _namingData = NamingData;
            _OverwriteSaved = (_namingData.Action == Action.Create);
            //_disciplinesDataTable = DisciplinesData;
            _currentFolderDesc = CurrentFolderDesc;
            _currentFolderName = CurrentFolderName;
            SetStartupFilterType();
        }

        /// <summary>
        /// The SetStartupFilterType
        /// </summary>
        private void SetStartupFilterType()
        {
            //List<string> stdFolder = new List<string>();

            //foreach (DataRow row in _namingData.StdNamingDataTable.Rows)
            //{
            //    if (!string.IsNullOrEmpty(row["Std_Folders"].ToString()))
            //    {
            //        stdFolder.AddRange(row["Std_Folders"].ToString().ToUpper().Split(';').ToList().Where(f => !stdFolder.Contains(f)).ToList());
            //    }
            //}

            //if (stdFolder.Contains(_currentFolderName.ToUpper()) || stdFolder.Contains(_currentFolderDesc.ToUpper()))
            //{
            //    FilterType = "Folder";
            //}
            //else
            //{
                FilterType = "CoP";

            //}
        }
    }
}
