namespace USACE_Wizard_GUI
{
    using RGiesecke.DllExport;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="UsaceDcw" />
    /// </summary>
    public class UsaceDcw
    {
        //[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPTStr, SizeParamIndex = 0)]
        /// <summary>
        /// The MultiGUI
        /// </summary>
        /// <param name="count">The count<see cref="int"/></param>
        /// <param name="projectID">The projectID<see cref="int"/></param>
        /// <param name="files">The files<see cref="string[]"/></param>
        /// <returns>The <see cref="int"/></returns>
        [DllExport]
        public static int MultiGUI(int count, int projectID, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] string[] files)
        {
            return ShowMultiGUI(projectID, files);
        }

        /// <summary>
        /// The ShowMultiGUI
        /// </summary>
        /// <param name="projectID">The projectID<see cref="int"/></param>
        /// <param name="files">The files<see cref="string[]"/></param>
        /// <returns>The <see cref="int"/></returns>
        private static int ShowMultiGUI(int projectID, string[] files)
        {
            try
            {
                List<string> fileList = new List<string>();

                if (!string.IsNullOrEmpty(files[0]))
                {
                    fileList = new List<string>(files);
                }

                if (fileList.Count == 0)
                {
                    //FileDialog fd = new OpenFileDialog();
                    OpenFileDialog fd = new OpenFileDialog();
                    fd.Multiselect = true;

                    if (fd.ShowDialog() == DialogResult.OK)
                    {
                        fileList = new List<string>(fd.FileNames);
                    }
                    else
                    {
                        return 0;
                    }
                }

                var gui = new DocumentWizard2(projectID, fileList) { StartPosition = FormStartPosition.CenterScreen };

                //if (gui.Data.Files.Count > 0)
                //{
                gui.ShowDialog();
                //}
                //else
                //{
                //    MessageBox.Show("You must select a file to associate with the ProjectWise Document", "USACE Document Creation Wizard", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    return 0;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //GTH  11/1/2024 - Finally get rid of the spinning castle when things to south
                Application.Exit();
                
            }

            return 0;
        }

        /// <summary>
        /// The ShowGUI
        /// </summary>
        /// <param name="iDestProjNo">The iDestProjNo<see cref="int"/></param>
        /// <param name="sTemplateFileName">The sTemplateFileName<see cref="string"/></param>
        /// <returns>The <see cref="int"/></returns>
        [DllExport]
        public static int ShowGUI(int iDestProjNo, [MarshalAs(UnmanagedType.LPTStr)]string sTemplateFileName)
        {
            //return ShowDcwGUI2(iDestProjNo, sTemplateFileName);
            return ShowMultiGUI(iDestProjNo, new string[1] { sTemplateFileName });
        }

        //private static int ShowDcwGUI2(int iDestProjNo, string sTemplateFileName)
        //{
        //    var gui = new DocumentWizard(iDestProjNo, sTemplateFileName) { StartPosition = FormStartPosition.CenterScreen };

        //    if (!string.IsNullOrEmpty(gui._file.FilePath))
        //    {
        //        gui.ShowDialog();
        //    }
        //    else
        //    {
        //        MessageBox.Show("You must select a file to associate with the ProjectWise Document", "USACE Document Creation Wizard", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return 0;
        //    }

        ////    return gui._file.DocumentID;
        ////}
        ///// <summary>
        ///// The InitDRW
        ///// </summary>
        ///// <param name="projectid">The projectid<see cref="int"/></param>
        ///// <param name="documentid">The documentid<see cref="int"/></param>
        ///// <returns>The <see cref="int"/></returns>
        //[DllExport]
        //public static int InitDRW(int projectid, int documentid)
        //{
        //    return ShowDRWGUI(projectid, documentid);
        //}

        ///// <summary>
        ///// The ShowDRWGUI
        ///// </summary>
        ///// <param name="projectid">The projectid<see cref="int"/></param>
        ///// <param name="documentid">The documentid<see cref="int"/></param>
        ///// <returns>The <see cref="int"/></returns>
        //private static int ShowDRWGUI(int projectid, int documentid)
        //{
        //    try
        //    {
        //        var gui = new DocumentWizard(projectid, documentid) { StartPosition = FormStartPosition.CenterScreen };

        //        gui.ShowDialog();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //    return 0;
        //}

        /// <summary>
        /// The InitDRWTest
        /// </summary>
        /// <param name="projectID">The projectID<see cref="int"/></param>
        /// <param name="files">The files<see cref="int[]"/></param>
        /// <returns>The <see cref="int"/></returns>
        [DllExport]
        public static int InitDRWTest(int projectID, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_INT)] int[] files)
        {
            return ShowDRWMultiFile(projectID, files);
        }

        /// <summary>
        /// The ShowDRWMultiFile
        /// </summary>
        /// <param name="projectID">The projectID<see cref="int"/></param>
        /// <param name="files">The files<see cref="int[]"/></param>
        /// <returns>The <see cref="int"/></returns>
        public static int ShowDRWMultiFile(int projectID, int[] files)
        {
            try
            {
                var fileids = new List<int>(files);
                var gui = new DocumentWizard2(projectID, fileids);
                gui.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return 0;
        }
    }
}
