using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web;

public class PWWrapper
{
    internal static Hashtable sessionHandles = new Hashtable();
    public static Guid[] ApplicationActionTypes = new Guid[] { new Guid("AC08DF6B-F420-44e8-95E0-8142CF2288C5"), new Guid("BAB502F2-85B0-403c-BFF5-C314E0783818"), new Guid("DBB7BD29-F1DC-4a7b-8B12-F585C4299D7E"), new Guid("FAF4FB46-1871-4834-9C6D-C991428DB202"), new Guid("ED52A96D-8D2E-4c54-A54A-8BDA68A8B4A6") };
    public static Guid PSET_DOCUMENT_GENERIC = new Guid("4E43310A-4524-4417-AE6E-D73BD7796123");
    public static Guid PSET_ATTRIBUTE_GENERIC = new Guid("53DC3798-A8FC-415a-9EBF-2370DF192031");
    public static Guid PSET_DBTABLE_GENERIC = new Guid("84088B7B-F2E5-4bb1-BD80-8A1F6489B281");
    public static Guid PSET_DOCUMENT_ACCESS = new Guid("AD9E44D9-00A7-412c-98C5-75A561297B1C");
    public static Guid PSET_FREE_TEXT = new Guid("A45B753A-FE55-426c-B91E-366596E8CAED");
    public static Guid PSET_FILEPROPS = new Guid("3ACDE94E-CCFF-4f00-8440-152CEBF1E3C3");

    static PWWrapper()
    {
        try
        {
            if (ConfigurationManager.AppSettings["Path"] != null)
            {
                Environment.SetEnvironmentVariable("Path", ConfigurationManager.AppSettings["Path"], EnvironmentVariableTarget.Process);
            }
            aaApi_Initialize(0);
        }
        catch (DllNotFoundException)
        {
            AppendProjectWiseDllPathToEnvironmentPath();
        }
        finally
        {
            aaApi_Initialize(0);
        }
    }

    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetEnvTriggerDefStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr __aaApi_GetEnvTriggerDefStringProperty(EnvTriggerProperty property, int index);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GuidListGetAt", CharSet = CharSet.Unicode)]
    private static extern IntPtr __aaApi_GuidListGetAt(IntPtr guidListP, int iIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GuidListGetFirstGuid", CharSet = CharSet.Unicode)]
    private static extern IntPtr __aaApi_GuidListGetFirstGuid(IntPtr guidListP);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GuidListGetNextGuid", CharSet = CharSet.Unicode)]
    private static extern IntPtr __aaApi_GuidListGetNextGuid(IntPtr guidListP);
    [DllImport("dmscli.dll", EntryPoint = "aaOApi_GetAttributePickListItemValue", CharSet = CharSet.Unicode)]
    private static extern IntPtr __aaOApi_GetAttributePickListItemValue(IntPtr hPickList, int lPickIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetEnvValListStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr _aaApi_GetEnvValListStringProperty(ValueListProperty PropertyId, int lIndex);
    [DllImport("dmawin.dll", EntryPoint = "aaApi_ViewGetName", CharSet = CharSet.Unicode)]
    private static extern IntPtr _aaApi_ViewGetName(IntPtr hView);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_ActivateDatasourceByHandle(IntPtr dsHandle);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_AddCustomHierarchyMember(int iUserId, int iCustomHierarcyId, CustomHierarchMemberItemType iMemberItemType, int iMemberId1, int iMemberId2);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_AddDocumentFile(int vaultID, int documentID, string fileName);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_AddFlatSetMember(int lSetId, int lProjectId, int lDocumentId, bool bCheckOut);
    [DllImport("dmsgen.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_AddHook(int lHookId, HookTypes lHookType, GenericHookFunction lpfnHook);
    [DllImport("dmsgen.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_AddHook(int lHookId, int lHookType, DoumentHookFunction lpfnHook);
    [DllImport("dmsgen.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_AddHook(int lHookId, int lHookType, HookFunction lpfnHook);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_AddSetMember(int lSetId, int lSetType, int lParentProjectId, int lParentDocumentId, int lChildProjectId, int lChildDocumentId, int lRelationType, string lpctstrTransfer, ref int lplMemberId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_AddStateToWorkflow(int iWorkflowId, int iStateId, int iPrevStateId, int iNextStateId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_AddUserListMember(int lUserListId, int lMemberType, int lMemberId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_AddUserToGroup(int iGroupId, int iUserId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_AdminLogin(DataSourceType lDSType, string sDatasourceName, string lpctstrUsername, string lpctstrPassword);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_AdminLoginDlg(DataSourceType lDSType, StringBuilder lptstrDataSource, int lDSLength, string lpctstrUsername, string lpctstrPassword);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_AssignAccessList(int lObjectType, int lObjectId1, int lObjectId2, int lWorkflowId, int lStateId, int lMemberType, int lMemberId, uint lAccessMask);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_AttrFormatParse(string lpctstrFormat, ref int plDataType, uint pulFlags, ref int plWidth, ref int plPrecision, ref int plAction);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_AttributeSheetModifyDocument(int lProjectId, int lDocumentId, int lAttributeId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_BuildMonikerStringByDocGuid(IntPtr hDatasource, ref Guid pDocGuid, ref IntPtr pMonikerStr);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_BuildMonikerStringByProjectGuid(IntPtr hDatasource, ref Guid pProjectGuid, ref IntPtr pMonikerStr);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_ChangeDocumentFile(int vaultID, int documentID, string fileName);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_ChangeDocumentFile4(int vaultID, int documentID, uint opFlags, string sourcefile, string newfilename, string fileMIME);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CheckInDocument(int lProjectId, int lDocumentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CheckOutDocument(int lProjectNo, int lDocumentId, string lpctstrWorkdir, StringBuilder lptstrFileName, int lBufferSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_ChklSetGetDocGuidFromFileName(string pFileName, ref Guid pDocGuid);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CopyAccessControl(uint ulFlags, AccessObjectType lObjectTypeFrom, int lObjectId1From, int lObjectId2From, int lWorkflowIdFrom, int lStateIdFrom, AccessObjectType lObjectTypeTo, int lObjectId1To, int lObjectId2To);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CopyDocument(int lSourceProjectNo, int lSourceDocumentId, int lTargetProjectNo, ref int lplTargetDocumentId, string lpctstrWorkdir, string lpctstrFileName, string lpctstrName, string lpctstrDesc, DocumentCopyFlags ulFlags);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CopyDocument2(IntPtr hSrcDataSource, int lSourceProjectNo, int lSourceDocumentId, IntPtr hTrgtDataSource, int lTargetProjectNo, ref int lpTargetDocumentId, string lpctstrWorkdir, string lpctstrFileName, string lpctstrName, string lpctstrDesc, DocumentCopyFlags ulFlags);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CopyDocumentAttributes(int lSourceProjectId, int lSourceDocumentId, int lTargetProjectId, int lTargetDocumentId, AttributeCopyFlags ulFlags);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CopyDocuments(int documentCount, IntPtr hDSSource, [In] AaDocItem[] arrSrcDocs, IntPtr hDSTarget, [In, Out] AaDocItem[] arrTrgtDocs, string workdir, [In, Out] string[] fileNames, [In, Out] string[] newNames, [In, Out] string[] newDescriptions, DocumentCopyFlags flags, IntPtr callback, IntPtr userParam);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CopyOutDocument(int lProjectNo, int lDocumentId, string lpctstrWorkdir, StringBuilder lptstrFileName, int lBufferSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CopyProject(int lSourceProjectId, int lTargetProjectId, ProjectCopyDeleteAndExportFlags ulFlags, IntPtr fpCallBack, IntPtr aaUserParam, ref int lplCount);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CopyProjectResources(int sourceProjectId, ProjectResourceTypes resType, int resId, int targetProjectId, ref int plCount);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_CountEnvTriggerDefs();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateAuditTrailRecordByGUID(AuditTrailTypes lObjectTypeId, ref Guid lpcguidObjGUID, AuditTrailActions lActionTypeId, string lpctstrComment, int lParam1, int lParam2, string lpctstrParam, ref Guid lpcguidGUIDParam);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateCustomHierarchy(ref int lplCustHrchyId, int lUserId, int lParentId, uint ulFlags, string lpctstrName, string lpctstrDesc);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateDepartment(ref int lDepartmentId, string name, string description);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateDocument(ref int documentID, int vaultID, int storageID, int fileType, DocumentType itemType, int applicationID, int departmentID, int workspaceProfileID, string sourceFilePath, string fileName, string name, string description, string version, bool leaveCheckedOut, DocumentCreationFlag creationFlags, StringBuilder workingFile, int workingFileBufferSize, ref int attributeID);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateDocument2(ref DocumentCreateParam docCreateParam);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_CreateDocumentList(IntPtr hWndParent, int iLeft, int iTop, int iWidth, int iHeight, DocumentListDefinitions ulFlags);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateDocumentVersionsFromSource(IntPtr hDSTarget, [In] AaDocItem[] arrTrgtDocs, IntPtr hDSSource, [In] AaDocItem[] arrSrcDocs, int lArrayLen, string lpctstrFrmt, CreateVersionsFromSourceFlags ulFlags, IntPtr fnCBack, int aaCBackData);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateEnvAttrGuiDef([In] ref AADMSEATRGUIDEF lpAttrGuiDef);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateFlatSet(int lProjectId, ref int lplDocumentId, ref int lplSetId, int lDepartmentId, string lpctstrName, string lpctstrDesc, int lChildProjectId, int lChildDocumentId, bool bCheckOut);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateGroup(ref int iGroupNo, string lpctstrType, string lpctstrSecProvider, string lpctstrName, string lpctstrDesc);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateGui(ref int lGuiId, string sInterfaceName, string sInterfaceDescription);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateLink(int lProjectId, int lDocumentId, int lTableId, int lColumnId, string lpctstrValue);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateLinkData(int lTableId, ref int lplColumnId, StringBuilder lptstrValueBuffer, int llengthBuffer);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateLinkDataAndLink(int lTableId, int lLinkType, int lObjectId1, int lObjectId2, ref int lplColumnId, StringBuilder lptstrValueBuffer, int lLenghtBuffer);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateProject(ref int createdVaultID, int parentID, int storageID, int managerID, VaultType type, int workflowID, int workspaceProfileID, int copyAccessFromProject, string name, string description);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateReferenceInformation2(ulong ui64ElementId, ref Guid masterGuid, int iMasterModelId, ref Guid referenceGuid, int iReferenceModelId, int iReferenceType, int iNestDepth, int iFlags);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateRichProject(ref AaProjItem pProject, IntPtr projectInstance, bool cloneProjectInstance, bool ensureFullAccess, int copyAccessFrom);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateSet(int lProjectId, ref int lpDocumentId, ref int lpSetId, int lSetType, int lDepartmentId, string lpctstrName, string lpctstrDesc, int lParentProjectId, int lParentDocumentId, int lChildProjectId, int lChildDocumentId, int lRelationType, string lpctstrTransfer, string sGuid, ref int lpMemberId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateState(ref int iStateId, string sStateName, string sStateDesc);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateUser(ref int iUserId, string sUserType, string sSecurityProvider, string sUserName, string sNewPassword, string sNewDescription, string sEMail);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateUserList(ref int lplUserListId, int lListType, int lOwnerId, string lpctstrName, string lpctstrDesc);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_CreateWorkflow(ref int iWorkflowId, WorkflowTypes lWorkflowType, string sWorkflowName, string sWorkflowDesc);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteApplicationAction(int iApplId, int iUserId, ref Guid pActionTypeGuid, string sProgramName);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteDepartmentById(int lDepartmentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteDepartmentByName(string name);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteDocument(DocumentDeleteMasks uiFlags, int iProjectId, int iDocumentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteDocumentAttributes(int lProjectId, int lDocumentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteDocumentFile(int vaultID, int documentID);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteEnv(int lEnvironmentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteEnvAttrDefs(int lEnvironmentId, int iTableId, int iColumnId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteEnvCodeDef(int lEnvironmentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteEnvTriggerDefs(int lEnvironmentId, int iTableId, int iColumnId, int iTriggerColumn);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteGroupById(int iGroupId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteGui(int iGuiId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteProjectById(int iProjectId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteReferenceInformation2(ulong ui64ElementId, ref Guid masterGuid, int iMasterModelId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteSet(int iSetProjectId, int iSetItemId, int iSetId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteSetMember(int lSetId, int lMemberId, int lParentProjectId, int lParentDocumentId, int lChildProjectId, int lChildDocumentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteUserById(int iUserId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteUserByName(string sUserName);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteUserExt(int iUserId, int iUserIdForItems);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DeleteUserListById(int iUserListId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern void aaApi_DmsDataBufferFree(IntPtr hDataBuffer);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_DmsDataBufferGetBinaryProperty(IntPtr hDataBuffer, int lPropertyID, int lIdxRow);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_DmsDataBufferGetCount(IntPtr hDataBuffer);
    public static Guid aaApi_DmsDataBufferGetGuidProperty(IntPtr hDataBuffer, int lPropertyId, int lIdxRow)
    {
        return (Guid)Marshal.PtrToStructure(unsafe_aaApi_DmsDataBufferGetGuidProperty(hDataBuffer, lPropertyId, lIdxRow), Type.GetType("System.Guid"));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_DmsDataBufferGetNumericProperty(IntPtr hDataBuffer, int lPropertyId, int lIdxRow);
    public static string aaApi_DmsDataBufferGetStringProperty(IntPtr hDataBuffer, int lPropertyId, int lIdxRow)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_DmsDataBufferGetStringProperty(hDataBuffer, lPropertyId, lIdxRow));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_DmsDataBufferSelect(int DsmBufferType);
    public static string aaApi_DmsThreadBufferGetStringProperty(int lBufferId, int lPropertyId, int lIdxRow)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_DmsThreadBufferGetStringProperty(lBufferId, lPropertyId, lIdxRow));
    }

    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DocListSelectDocument(IntPtr hWndList, int iProjectId, int iDocumentId, int iAttrId);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DocListSetProject(IntPtr hWndList, int iProjectId);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DocListSetView(IntPtr hWndDocList, IntPtr hView);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DocListSynchronizeAttributeSheet(IntPtr hWndDocList);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DocListUpdateItemStatus(IntPtr hWndDocList, int lProjectId, int lDocumentId, DocListUpdateTypeMasks ulFlags);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DocumentGenerateName(long lProjectId, StringBuilder lptstrDocName, int iBufferSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DocumentGenFileNameWithPrefix(int lProjectId, string lpctstrPrefix, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lptstrDocName, int iBufferSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_DocumentGenNameWithPrefix(int lProjectId, string lpctstrPrefix, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lptstrDocName, int iBufferSize);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_DscTreeSelectSearchResultsItem(IntPtr hWndDscTree, IntPtr hDataSource);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_EnableMenuCommand(int iMenuType, MenuCommandIds menuCmdId, bool bEnable);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_EnableMenuCommand(int iMenuType, int iMenuCmdId, bool bEnable);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_EnableUserAccount(int iUserId, bool bEnable);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_ExecuteSqlStatement(string description);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_ExportDocument(int lProjectNo, int lDocumentId, string lpctstrWorkdir, StringBuilder lptstrFileName, int lBufferSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_FetchDocumentFromServer(FetchDocumentFlags ulFlags, int lProjectId, int lDocumentId, string lpctstrWorkdir, StringBuilder lptstrFileName, int lBufferSize);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_FindDocumentDlg();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_FindDocuments(IntPtr hCriteriaBuf, DocumentRequestColumns resultCols, IntPtr pCallback, ref bool bCancel, uint uiTimeToWaitForChunk, uint uiItemCountInChunk);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_FindDocumentsToBuffer(IntPtr hCriteriaBuf, DocumentRequestColumns resultCols, ref bool bCancel, ref _FINDDOC_RESULTS findDocResults);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_FindDocumentsToBuffer(IntPtr hCriteriaBuf, DocumentRequestColumns resultCols, ref bool bCancel, ref IntPtr findDocResults);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_FindDocumentsToDocumentList(IntPtr hWndDocList, IntPtr hCriteriaBuf);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_FindDscTreeDataSourceItem(IntPtr hWndTree, IntPtr hDataSource, FindDscItemFlags ulFlags, IntPtr lphItem);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_FindDscTreeItem(IntPtr hWndTree, IntPtr hParent, DscItemTypes lTypeId, int lItemId, string lpctstrText, FindDscItemFlags ulFlags);
    [DllImport("dmsgen.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_Free(IntPtr pointer);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_FreeDocument(int lProjectNo, int lDocumentId, int lUserId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_FreeLinkDataInsertDesc();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_FreeLinkDataUpdateDesc();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetAccessControlItemNumericProperty(AccessObjectProperty lPropertyId, int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetAccessControlItemNumericProperty(int lPropertyId, int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern uint aaApi_GetAccessMaskForUser(AccessObjectTypes lObjectType, int lObject1, int lObject2, int lWorkflowId, int lStateId, int lUserId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetAccessUserNumericProperty(AccessUserProperty lPropertyId, int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_GetActiveDatasource();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetActiveDatasourceName(StringBuilder lptstrName, int lNameSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetActiveDatasourceNativeType();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetActiveDatasourceType();
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_GetActiveDocumentList();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetActiveInterface();
    [DllImport("dmsgen.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetAPIVersion(ref int iMajorVersionHi, ref int iMajorVersionLo, ref int iMinorVersion, ref int iBuildVersion);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetApplicationId(int index);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetApplicationNumericProperty(ApplicationProperty lPropertyId, int lIndex);
    public static string aaApi_GetApplicationStringProperty(ApplicationProperty lPropertyId, int lIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetApplicationStringProperty(lPropertyId, lIndex));
    }

    public static string aaApi_GetAuditTrailActionTypeName(AuditTrailActions iActionTypeId, ref int pObjectTypeId)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetAuditTrailActionTypeName(iActionTypeId, ref pObjectTypeId));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetColumnNumericProperty(ColumnProperty PropertyId, int iIndex);
    public static string aaApi_GetColumnStringProperty(ColumnProperty PropertyId, int lIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetColumnStringProperty(PropertyId, lIndex));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetConnectedUserNumericProperty(UserProperty PropertyId, int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetConnectedUsers(bool bRefresh, ref int iUsersP, ref int iUserCountP);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetConnectionInfo(IntPtr hDataSource, ref bool lpbODBC, ref int lplNativeType, ref int lplLoginType, StringBuilder lptstrName, int lLenName, StringBuilder lptstrUser, int lLenUser, StringBuilder lptstrPassword, int lLenPassword, StringBuilder lptstrSchema, int lLenSchema);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetConnectionInfo2(IntPtr hDataSource, ref bool lpbODBC, ref int lplNativeType, ref int lplLoginType, StringBuilder lptstrName, int lLenName, StringBuilder lptstrUser, int lLenUser, StringBuilder lptstrSchema, int lLenSchema);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetCurrentSession(ref IntPtr handle);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetCurrentUserId();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetCustomHierarchyNumericProperty(CustomHierarchyProperties PropertyId, int lIndex);
    public static string aaApi_GetCustomHierarchyStringProperty(CustomHierarchyProperties PropertyId, int lIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetCustomHierarchyStringProperty(PropertyId, lIndex));
    }

    public static string aaApi_GetDatasourceFullName(int index)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetDatasourceFullName(index));
    }

    public static string aaApi_GetDatasourceInternalName(int index)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetDatasourceInternalName(index));
    }

    public static string aaApi_GetDatasourceName(int index)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetDatasourceName(index));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern string aaApi_GetDatasourceNameFromMoniker(IntPtr hMoniker);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern ulong aaApi_GetDatasourceStatisticsNumericProperty(DatasourceStatisticsProperty PropertyId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetDepartmentCount();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetDepartmentId(int index);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetDepartmentNumericProperty(int lPropertyId, int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetDepartmentPropertyLength(int lPropertyId);
    public static string aaApi_GetDepartmentStringProperty(DepartmentProperty PropertyId, int Index)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetDepartmentStringProperty(PropertyId, Index));
    }

    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_GetDocListMoniker(IntPtr docListP);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetDocumentCount(int lProjectId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetDocumentFileName(int iProjectId, int iDocumentId, StringBuilder sbLocalFilePath, int iBufferSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_GetDocumentGuidFromMoniker(IntPtr hMoniker);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetDocumentGUIDsByIds([In] int lCount, [In] ref AaDocItem pDocuments, [Out] Guid[] docGuids);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetDocumentGUIDsByIds([In] int lCount, [In] AaDocItem[] pDocuments, [Out] Guid[] docGuids);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetDocumentId(int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetDocumentIdsByGUIDs([In] int lCount, [In] Guid[] docGuids, [In, Out] ref AaDocItem pDocuments);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetDocumentIdsByGUIDsTest([In] int lCount, [In] Guid[] docGuids, [In, Out] ref AaDocItem[] pDocuments);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetDocumentNamePath(int ProjectId, int DocId, bool UseDesc, char tchSeparator, StringBuilder StringBuffer, int BufferSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetDocumentNumericProperty(DocumentProperty PropertyId, int lIndex);
    public static string aaApi_GetDocumentStringProperty(DocumentProperty PropertyId, int Index)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetDocumentStringProperty(PropertyId, Index));
    }

    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_GetDscTreeMoniker(IntPtr dscTreeP);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetEnvAttrDefNumericProperty(AttributeDefinitionProperty property, int index);
    public static string aaApi_GetEnvAttrDefStringProperty(AttributeDefinitionProperty propertyID, int index)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetEnvAttrDefStringProperty(propertyID, index));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetEnvAttrGuiDefNumericProperty(EnvAttrGuiProps PropertyId, int Index);
    public static string aaApi_GetEnvAttrGuiDefStringProperty(EnvAttrGuiProps PropertyId, int Index)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetEnvAttrGuiDefStringProperty(PropertyId, Index));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetEnvCodeDefNumericProperty(DocumentCodeDefinitionProperty propertyID, int index);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetEnvId(int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetEnvIdByProject(int lProjectId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetEnvNumericProperty(EnvironmentProperty lPropertyId, int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetEnvNumericProperty(int lPropertyId, int lIndex);
    public static string aaApi_GetEnvStringProperty(EnvironmentProperty lPropertyId, int lIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetEnvStringProperty(lPropertyId, lIndex));
    }

    public static string aaApi_GetEnvStringProperty(int lPropertyId, int lIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetEnvStringProperty(lPropertyId, lIndex));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetEnvTableIdColumnId(int lTableId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetEnvTableInfoByProject(int lProjectId, ref int lplEnvironmentId, ref int lplTableId, ref int lplIdColumnId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetEnvTriggerDefNumericProperty(EnvTriggerProperty property, int index);
    public static string aaApi_GetEnvTriggerDefStringProperty(EnvTriggerProperty propertyID, int index)
    {
        return Marshal.PtrToStringUni(__aaApi_GetEnvTriggerDefStringProperty(propertyID, index));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetEnvValListNumericProperty(ValueListProperty PropertyId, int lIndex);
    public static string aaApi_GetEnvValListStringProperty(ValueListProperty PropertyId, int lIndex)
    {
        return Marshal.PtrToStringUni(_aaApi_GetEnvValListStringProperty(PropertyId, lIndex));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetFExtensionApplication(string sExtension);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetGroupId(int iIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetGroupMemberNumericProperty(int lPropertyIndex, int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetGroupNumericProperty(GroupProperty PropertyId, int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetGroupNumericProperty(int lPropertyIndex, int lIndex);
    public static string aaApi_GetGroupStringProperty(GroupProperty PropertyId, int lIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetGroupStringProperty(PropertyId, lIndex));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Ansi)]
    private static extern int aaApi_GetGuidsFromFileName([In, Out] ref Guid[] docGuids, [In, Out] ref int iNumGuids, [In] string sFileName, [In] int iValidateWithChkl);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetGuiId(int index);
    public static string aaApi_GetGuiStringProperty(GuiProperty propertyID, int index)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetGuiStringProperty(propertyID, index));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetInternalDatasourceName(IntPtr hDataSource, StringBuilder lptstrDsName, int iBufferSize);
    public static string aaApi_GetLastErrorDetail()
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetLastErrorDetail());
    }

    [DllImport("dmsgen.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetLastErrorId();
    public static string aaApi_GetLastErrorMessage()
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetLastErrorMessage());
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetLinkDataColumnCount();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetLinkDataColumnNumericProperty(LinkDataProperty property, int index);
    public static string aaApi_GetLinkDataColumnStringProperty(LinkDataProperty propertyID, int index)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetLinkDataColumnStringProperty(propertyID, index));
    }

    public static string aaApi_GetLinkDataColumnValue(int lRowIndex, int lColumnIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetLinkDataColumnValue(lRowIndex, lColumnIndex));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetLinkDataDataBufferColumnCount(IntPtr hBuf);
    public static string aaApi_GetLinkDataDataBufferColumnValue(IntPtr hBuf, int iRowIndex, int iColumnIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetLinkDataDataBufferColumnValue(hBuf, iRowIndex, iColumnIndex));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetLinkDataDataBufferNumericColumnValue(IntPtr hDataBuffer, int rowIndex, int columnIndex);
    public static int aaApi_GetLinkDataDataBufferNumericProperty(IntPtr hBuf, LinkDataProperty propertyID, int iColumnIndex)
    {
        return unsafe_aaApi_GetLinkDataDataBufferNumericProperty(hBuf, propertyID, iColumnIndex);
    }

    public static string aaApi_GetLinkDataDataBufferStringProperty(IntPtr hBuf, LinkDataProperty propertyID, int iColumnIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetLinkDataDataBufferStringProperty(hBuf, propertyID, iColumnIndex));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetLinkNumericProperty(LinkProperty propertyID, int index);
    public static string aaApi_GetLinkStringProperty(LinkProperty propertyID, int index)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetLinkStringProperty(propertyID, index));
    }

    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_GetMainDocumentList();
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_GetMainDscTree();
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_GetMainFrameWindow();
    public static string aaApi_GetMessageByErrorId(int errorID)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetMessageByErrorId(errorID));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetNumericSetting(DatasourceGenericSettings lSettingId);
    public static string aaApi_GetProductVersionString()
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetProductVersionString());
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetProjectDefaultPreviewPaneView(int iProjectId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetProjectDefaultView(IntPtr projectBuffer, int rowIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_GetProjectGuidFromMoniker(IntPtr hMoniker);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetProjectGUIDsByIds([In] int lCount, [In] ref int pProjectIds, [Out] Guid[] docGuids);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetProjectId(int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetProjectIdByNamePath(string lpctstrPath);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetProjectNamePath(int ProjectId, bool UseDesc, char tchSeparator, StringBuilder StringBuffer, int BufferSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetProjectNumericProperty(ProjectProperty PropertyId, int lIndex);
    public static string aaApi_GetProjectStringProperty(ProjectProperty PropertyId, int lIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetProjectStringProperty(PropertyId, lIndex));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetSetNumericProperty(SetProperty lPropertyId, int lIndex);
    public static string aaApi_GetSetStringProperty(SetProperty iSetProp, int iIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetSetStringProperty(iSetProp, iIndex));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetStateId(int lIndex);
    public static string aaApi_GetStateStringProperty(StateProperty PropertyId, int lIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetStateStringProperty(PropertyId, lIndex));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetStorageId(int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetStorageNumericProperty(StorageProperty lPropertyId, int lIndex);
    public static string aaApi_GetStorageStringProperty(StorageProperty PropertyId, int lIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetStorageStringProperty(PropertyId, lIndex));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GetStringSetting(DatasourceGenericSettings lSettingId, StringBuilder lptstrValue, ref int lplBufferLen);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetTableId(int index);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetTableNumericProperty(TableProperty propertyID, int index);
    public static string aaApi_GetTableStringProperty(TableProperty propertyID, int index)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetTableStringProperty(propertyID, index));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetUserId(int index);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetUserListId(int iIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetUserListMemNumericProperty(UserListMemberProperty lPropertyId, int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetUserListNumericProperty(UserListProperty lPropertyId, int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetUserListNumericProperty(int lPropertyIndex, int lIndex);
    public static string aaApi_GetUserListStringProperty(UserListProperty iUserListProp, int iIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetUserListStringProperty(iUserListProp, iIndex));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetUserNumericProperty(UserProperty lPropertyId, int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetUserNumericSetting(int lParam);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetUserNumericSettingByUser(int lUserNo, int lParam);
    public static string aaApi_GetUserStringProperty(UserProperty lPropertyId, int lIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetUserStringProperty(lPropertyId, lIndex));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetWorkflowId(int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GetWorkflowStateLinkNumericProperty(WorkflowStateProperty iPropertyId, int iIndex);
    public static string aaApi_GetWorkflowStringProperty(WorkflowProperty PropertyId, int lIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetWorkflowStringProperty(PropertyId, lIndex));
    }

    public static string aaApi_GetWorkingDirectory()
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_GetWorkingDirectory());
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GiveOutDocument(int lProjectNo, int lDocumentId, string lpctstrWorkdir, string lptstrFileName, int lBufferSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GUIDChangeDocumentFile(ref Guid pDocGuid, string newDocFileName);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GUIDCheckInDocument(ref Guid guid);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GUIDCheckOutDocument(ref Guid guid, string sWorkingDir, StringBuilder StringBuffer, int BufferSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GUIDCopyDocumentAttributes(ref Guid pSourceDocGuid, ref Guid pTargetDocGuid, AttributeCopyFlags ulFlags);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GUIDCopyOutDocument(ref Guid guid, string sWorkingDir, StringBuilder StringBuffer, int BufferSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GUIDDeleteDocument(DocumentDeleteMasks uiFlags, ref Guid pDocGuid);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GUIDDeleteDocumentAttributes(ref Guid pDocGuid);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GUIDFetchDocumentFromServer(FetchDocumentFlags flags, ref Guid guid, string sWorkingDir, StringBuilder StringBuffer, int BufferSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GUIDGetDocumentNamePath(ref Guid guid, bool UseDesc, char tchSeparator, StringBuilder StringBuffer, int BufferSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GUIDGetProjectNamePath(ref Guid guid, bool UseDesc, char tchSeparator, StringBuilder StringBuffer, int BufferSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GuidListAddGuid(IntPtr guidListP, ref Guid guidP);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_GuidListCreate();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GuidListDestroy(IntPtr guidListP);
    public static Guid aaApi_GuidListGetAt(IntPtr guidListP, int iIndex)
    {
        return (Guid)Marshal.PtrToStructure(__aaApi_GuidListGetAt(guidListP, iIndex), Type.GetType("System.Guid"));
    }

    public static Guid aaApi_GuidListGetFirstGuid(IntPtr guidListP)
    {
        return (Guid)Marshal.PtrToStructure(__aaApi_GuidListGetFirstGuid(guidListP), Type.GetType("System.Guid"));
    }

    public static Guid aaApi_GuidListGetNextGuid(IntPtr guidListP)
    {
        return (Guid)Marshal.PtrToStructure(__aaApi_GuidListGetNextGuid(guidListP), Type.GetType("System.Guid"));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GuidListGetSize(IntPtr guidListP);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GUIDModifyDocument(ref Guid pDocGuid, int fileType, int itemType, int applicationId, int departmentId, int workspaceProfileId, string docFileName, string docName, string docDesc);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GUIDNewDocumentVersion(NewVersionCreationFlags ulFlags, ref Guid pDocGuid, string docVersion, string comment, ref Guid pVersionDocGuid);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GUIDPurgeDocumentCopy(ref Guid guid, int iUserID);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GUIDRefreshDocumentServerCopy(ref Guid guid);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GUIDSelectDocument(ref Guid guid);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_GUIDSelectDocumentDataBuffer(ref Guid guid);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GUIDSelectDocumentsByProjectId(ref Guid guid);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_GUIDSelectNestedReferencesDataBuffer(ref Guid masterGuidP, ReferenceListFlags flags, int maxDepth);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_GUIDSelectNestedReferencesList(ref Guid masterGuidP, int iflags, int imaxDepth);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GUIDSelectProject(ref Guid guid);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_GUIDSelectProjectDataBuffer(ref Guid projGuid);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_GUIDSelectProjectsFromBranch(ref Guid guid, string codeString, string projectName, string projectDescription, string projectVersion);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_GUIDSetParentProject(Guid pProjectGuid, Guid pParentProjectGuid);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_HasAdminSetup();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_Initialize(int init);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_IsConnectionLost();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_IsCurrentUserAdmin();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_IsDocumentCheckedIn(int lProjectId, int lDocumentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_IsDocumentCheckedOutToMe(int lProjectId, int lDocumentId, ref bool bIsOutToMe);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_IsDocumentExported(int lProjectId, int lDocumentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_IsUserConnected(int iUserId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_Login(int iDSType, string lptstrDataSource, string lpctstrUsername, string lpctstrPassword, string lpctstrSchema);
    public static bool aaApi_Login(DataSourceType lDSType, string lptstrDataSource, string lpctstrUsername, string lpctstrPassword, string lpctstrSchema, bool bReuseExistingConnection)
    {
        IntPtr zero = IntPtr.Zero;
        string key = GetCredentialHash(lDSType, lptstrDataSource, lpctstrUsername, lpctstrPassword, lpctstrSchema);
        if (bReuseExistingConnection && sessionHandles.Contains(key))
        {
            zero = (IntPtr)sessionHandles[key];
        }
        if (zero == IntPtr.Zero)
        {
            bool flag = aaApi_Login((int)lDSType, lptstrDataSource, lpctstrUsername, lpctstrPassword, lpctstrSchema);
            if (flag)
            {
                zero = aaApi_GetActiveDatasource();
                sessionHandles[key] = zero;
            }
            return flag;
        }
        if (!(aaApi_ActivateDatasourceByHandle(zero) == IntPtr.Zero))
        {
            return true;
        }
        bool flag2 = aaApi_Login((int)lDSType, lptstrDataSource, lpctstrUsername, lpctstrPassword, lpctstrSchema);
        if (flag2)
        {
            zero = aaApi_GetActiveDatasource();
            sessionHandles[key] = zero;
        }
        return flag2;
    }

    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_LoginDlg(DataSourceType lDSType, StringBuilder lptstrDataSource, int lDSLength, string lpctstrUsername, string lpctstrPassword, string lpctstrSchema);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_Logout(string lptstrDataSource);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_LogoutByHandle(IntPtr dsHandle);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_ModifyAccessItemMask(int lObjectType, int lObjectId1, int lObjectId2, int lWorkflowId, int lStateId, int lMemberType, int lMemberId, uint lAccessMask);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_ModifyDepartment(int lDepartmentId, string name, string description);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_ModifyDocument(int lProjectId, int lDocumentId, int lFileType, int lItemType, int lApplicationId, int lDepartmentId, int lWorkspaceProfileId, string lpctstrFileName, string lpctstrName, string lpctstrDesc);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_ModifyProject(int lProjectId, int lStorageId, int lManagerId, int lType, string lpctstrName, string lpctstrDesc);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_ModifyProject2(ref VaultDescriptor vaultDescriptor);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_ModifyReferenceInformation2(ulong ui64ElementId, ref Guid masterGuid, int iMasterModelId, ref Guid referenceGuid, int iReferenceModelId, int iReferenceType, int iNestDepth, int iFlags);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_ModifyUser(int iUserId, string sName, string sPassword, string sDesc, string sEmail);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_MonikersToStrings([In] int lCount, [In] IntPtr[] pMonikers, [Out] StringBuilder[] sArrayMonikers, short flags);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_MoveDocument(int lSourceProjectNo, int lSourceDocumentId, int lTargetProjectNo, ref int lplTargetDocumentId, string lpctstrWorkdir, string lpctstrFileName, string lpctstrName, string lpctstrDesc, DocumentCopyFlags ulFlags);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_MoveDocumentBindings(ref AaDocItem pSourceDocs, ref AaDocItem pTargetDocs, int lDocumentCount, uint ulFalgs);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_NewDocumentVersion(NewVersionCreationFlags ulFlags, int vaultID, int documentID, string version, string comment, ref int versionDocId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_OdbcDateTimeStringToDbDateTimeString(string lpctstrTimeToFmt, StringBuilder lptstrBuffer, int lSize);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_ProjectTreeSelectItem(IntPtr hDscTree, int iProjectId);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_ProjectTreeSetDocList(IntPtr hDscTree, IntPtr hWndDocList);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_PurgeDocumentCopy(int lProjectNo, int lDocumentId, int lUserId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_RefreshConnectedUsers();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_RefreshDatasourceStatistics();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_RefreshDocumentServerCopy(int lProjectId, int lDocumentId);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_RefreshDscTreeSubItemsByParams(IntPtr hWndTree, FindDscItemFlags ulFlags, DscItemTypes lTypeId, int lItemId, IntPtr lpbyItemData, int lItemData, IntPtr fpCompare);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_RemoveAccessList(AccessObjectType lObjectType, int lObjectId1, int lObjectId2, int lWorkflowId, int lStateId, int lMemberType, int lMemberId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_RemoveAccessList(int lObjectType, int lObjectId1, int lObjectId2, int lWorkflowId, int lStateId, int lMemberType, int lMemberId);
    [DllImport("dmsgen.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_RemoveAllErrors();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_RemoveUserFromGroup(int iGroupId, int iUserId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_RemoveUserListMember(int lUserListId, int lMemberType, int lMemberId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectAccessControlDataBuffer(AccessControlSelectionFlags ulFlags, AccessObjectType lObjectType, int lObjectId1, int lObjectId2, int lWorkflowId, int lStateId, AccessMaskFlags ulRequiredMask);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAccessControlItems(AccessObjectType lObjectType, int lObjectId1, int lObjectId2, int lWorkflowId, int lStateId, int lMemberType, int lMemberId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAccessControlItems(int lObjectType, int lObjectId1, int lObjectId2, int lWorkflowId, int lStateId, int lMemberType, int lMemberId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAccessUsers(uint ulFlags, int lObjectType, int lObjectId1, int lObjectId2, int lWorkflowId, int lStateId, uint ulRequiredMask);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectActionForApplication(int iApplicationId, ref Guid guidActionType);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAllApplications();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAllDepartments();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAllEnvs(bool showSystem);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAllEnvTriggerDefs();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAllGroups();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAllGuis();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAllProjects();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAllStates();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAllStorages();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAllTables();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAllUserLists();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAllUsers();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectAllWorkflows();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectApplication(int lApplId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectApplicationActions();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectChildProjects(int iParentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectColumn(int iTableId, int iColumnId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectColumnsByTable(int iTableId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectConnectedUser(int iUserId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectCustomHierarchiesByUserId(int lUserId);
    [DllImport("dmscli.dll")]
    public static extern IntPtr aaApi_SelectDataSourceDataBufferByHandle(IntPtr hDatasource);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectDatasources();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_SelectDatasourceStatistics();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectDepartment(int lDepartmentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectDepartmentsForProject(int lDepartmentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectDocument(int ProjectId, int lDocumentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectDocumentDataBuffer(int lProjectId, int lDocumentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectDocumentDataBufferByNameProp(int lProjectId, string sFileName, string sName, string sDesc, string sVersion);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectDocumentDataBufferVersions(int lProjectId, int lDocumentId);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectDocumentDlg(IntPtr hWndParent, string lpctstrTitle, int lApplicationId, ref int lplProjectId, ref int lplDocumentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectDocumentsByNameProp(int vaultID, string fileName, string name, string description, string version);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectDocumentsByProjectId(int ProjectId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectDocumentsByProp(int lProjectId, int lStorageId, int lFileType, int lItemType, int lApplicationId, int lDepartmentId, string lpctstrFileName, string lpctstrName, string lpctstrDesc, string lpctstrVersion, int lVersionNo, int lCreatorId, int lUpdaterId, int lLastUserId, string lpctstrStatus, int lWorkflowId, int lStateId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectDocumentVersions(int ProjectId, int lDocumentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectEnv(int lEnvironmentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectEnvAttrDefs(int environmentID, int tableID, int columnID);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectEnvAttrGuiDefs(int lEnvironmentId, int lTableId, int lColumnId, int lGuiId, int lPageNo);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectEnvByProjectId(int lProjectId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectEnvCodeDefs(int environmentID, int tableID, int columnID, CodeDefinitionType type);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectEnvTriggerDefs(int iEnvironmentId, int iTableId, int iColumnId, int iTrigColumnId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectEnvValListItems(int iEnvironmentId, int iTableId, int iColumnId, int iValueId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectGroup(int lGroupId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectGroupDataBufferById(int iGroupId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectGroupMembers(int lGroupId, int lUserId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectGroupsByUser(int lUserId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectGui(int iGuidId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectLinkData(int lTableId, int lColumnId, string lpctstrValue, ref int lplColumns);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectLinkDataByObject(int lTableId, ObjectTypeForLinkData lItemType, int lItemId1, int lItemId2, string lpctstrWhere, ref int lplColumnCount, int[] lplColumnIds, LinkDataSelectFlags ulFlags);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectLinkDataDataBuffer(int lTableId, ObjectTypeForLinkData lItemType, int lItemId1, int lItemId2, string sWhere, int lColumnCount, int[] lplColumnIds, LinkDataSelectFlags ulFlags);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectLinks(int lProjectId, int lDocumentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectParentProject(int projectId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectProject(int lProjectId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectProjectChain(int lProjectFrom, int lProjectTo);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectProjectChainDataBuffer(int lProjectFrom, int lProjectTo);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectProjectDataBuffer(int lProjectId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectProjectDataBufferByStruct(int lProjectId, ref AaProjItem lpCriteria);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectProjectDataBufferChilds(int lProjectId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectProjectDataBufferChilds2(int lProjectId, bool bWithRichProjectsOnly);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectProjectDataBufferFromBranch(int lProjectId, string lpctstrCode, string lpctstrName, string lpctstrDesc, string lpctstrVersion);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectProjectDlg(IntPtr hWndParent, string lpctstrTitle, int lProjectId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectProjectResourcesDataBuffer(int iprojId, int iResType);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectProjectsByEnvironment(int iEnvId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectProjectsByProp(string lpctstrCode, string lpctstrName, string lpctstrDesc, string lpctstrVersion);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectProjectsByType(int iType);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectProjectsFromBranch(int iParentId, string lpctstrCode, string lpctstrName, string lpctstrDesc, string lpctstrVersion);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectRichProjectOfFolder(int iProjectId);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern long aaApi_SelectSavedQueryDlg(IntPtr hWndParent, string lpctstrTitle, ref int plSQueryNo);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectSet(int iSetId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectSetByTypeMask(TypeMask ulTypeMask, int lSetId, int lParentProjectId, int lParentDocumentId, int lChildProjectId, int lChildDocumentID);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectSetByTypeMask(uint ulTypeMask, int lSetId, int lParentProjectId, int lParentDocumentId, int lChildProjectId, int lChildDocumentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectSetMasters(int iChildProjId, int iChildDocId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectSetReferences(int iMasterProjId, int iMasterDocId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectState(int iStateId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectStatesByWorkflow(int iWorkflowId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectStatesNotInWorkflow(int iWorkflowId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectStorage(int lStorageId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectTable(int iTableId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectTopLevelProjects();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectUser(int lUserId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectUserApplicationActions(int iApplicationId, int iUserId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectUserDataBufferByGroup(int lGroupId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectUserDataBufferById(int iUserId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectUserList(int iUserListId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectUserListDataBuffer();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectUserListDataBufferByProp(int lListId, int lListType, int lOwner);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectUserListMemberDataBufferByProp(int lUsrLstId, int lMemType, int lMemberId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectUserListMembers(int lUsrLstId, ManagerTypeProperty lMemType, int lMemberId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectUserLists(int iListId, int iListType, int iOwner);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectUsersByGroup(int lGroupId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectUsersByProp(string name, string description, string email);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectUsersByUserList(int lUserListId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectWorkflow(int iWorkflowId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SelectWorkflowStateLinks(int iWorkflowId, int iState);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SelectWorkspaceProfileByName(string sWorkspaceProfileName);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_SetCurrentSession(IntPtr handle);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_SetDefaultProjectView(int iUserId, int iProjectId, int iViewId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_SetDefaultView(int iUserId, ref Guid objectTypeGuid, int iObjectId, ref Guid viewTypeGuid, int iViewId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_SetDocumentFinalStatus(int lProjectId, int lDocumentId, bool bAdd, string lpctstrComment);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_SetDocumentState(uint ulFlags, int lProjectId, int lDocumentId, int lWorkflowId, int lStateId, string lpctstrComment);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_SetLinkDataColumnValue(int tableID, int columnID, string columnValue);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_SetParentProject(long lChildId, long lParentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_SetProjectWorkflow(int lProjectId, int lWorkflowId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_SetUserNumericSettingByUser(int lUserNo, int lParam, int lParamValue);
    [DllImport("dmactrl.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_ShowInfoMessage(string sMessage);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SqlSelect(string sqlStatement, IntPtr columnBind, ref int numColumnsSelected);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SqlSelectDataBuffer(string sqlStatement, IntPtr columnBind);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SqlSelectDataBufGetColumnCount(IntPtr hDataBuffer);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SqlSelectDataBufGetNumericProperty(IntPtr hDataBuffer, SqlSelectProperties lPropertyId, int lIdxCol);
    public static string aaApi_SqlSelectDataBufGetStringProperty(IntPtr hDataBuffer, SqlSelectProperties lPropertyId, int lIdxCol)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_SqlSelectDataBufGetStringProperty(hDataBuffer, lPropertyId, lIdxCol));
    }

    public static string aaApi_SqlSelectDataBufGetStringProperty(IntPtr hDataBuffer, int lPropertyId, int lIdxCol)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_DmsDataBufferGetStringProperty(hDataBuffer, lPropertyId, lIdxCol));
    }

    public static string aaApi_SqlSelectDataBufGetValue(IntPtr hDataBuffer, int iRowIndex, int iColumnIndex)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_SqlSelectDataBufGetValue(hDataBuffer, iRowIndex, iColumnIndex));
    }

    public static string aaApi_SqlSelectGetData(int iRow, int iColumn)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_SqlSelectGetData(iRow, iColumn));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_SqlSelectGetNumericProperty(SqlSelectProperties lPropertyId, int lIdxCol);
    public static string aaApi_SqlSelectGetStringProperty(SqlSelectProperties lPropertyId, int lIdxCol)
    {
        return Marshal.PtrToStringUni(unsafe_aaApi_SqlSelectGetStringProperty(lPropertyId, lIdxCol));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SQueryCriDataBufferAddCriterion(IntPtr hQueryCriBuffer, int iOrGroup, CriteriaFlags iFlags, ref Guid pGuidPropertySet, string sPropertyName, QueryProperty iPropertyId, RestrictionRelation iRelationId, CriterionDataType iFieldType, string sFieldValue);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SQueryCriDataBufferSelect(int iQueryId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_SQueryDataBufferSelectAll();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_StartPartnerApplication(string sApplCmd, string sApplArgs, string sFileName, int lProjectId, int lDocumentId, int lSetId, bool bAskCheckIn);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_StringsToMonikers([In] int lCount, [Out] IntPtr[] pMonikers, [In] string[] sArrayMonikers, short flags);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_StrToNumber(string lpctstrNumber);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_SystemVariableGet(string sVariableName, StringBuilder sbValue, int iSbValueSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_ThumbnailDataBufferSelectByDoc(ref Guid pDocGuid, string strThumbnailTimeStamp);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_Uninitialize();
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_UpdateDocumentWindows();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_UpdateEnvAttr(int tableID, int attrRecId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_UpdateLinkData(int tableID, int columnID, string columnValue);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_UpdateLinkDataColumnValue(int tableID, int columnID, string columnValue);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_UpdateProjectWindows();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaApi_UpgradeFolderToRichProject(ref AaProjItem projectItem, IntPtr projectInstance, bool cloneProjectInstance);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_ValidateMSFile(string sFileName);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaApi_VerifyUser(string userName, string password);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_ViewGetFirst();
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_ViewGetFirstForProject(int iProjectId);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_ViewGetHandle(string sViewName);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_ViewGetLastViewName();
    public static string aaApi_ViewGetName(IntPtr hView)
    {
        return Marshal.PtrToStringUni(_aaApi_ViewGetName(hView));
    }

    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaApi_ViewGetNext(IntPtr hView);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_AddClassAttribute(IntPtr lpAttr, int lAttrId, int lIndex, IntPtr lpClass);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_AddClassQualifier(IntPtr lpClass, int lQualId, ref int lpValBuf);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaOApi_ClassGetBusinessKeyAttrId(IntPtr lpClass);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_ClassSyncDataBase(IntPtr lpClass, int iCreateIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_CreateAttribute(IntPtr lpBase, ref IntPtr lppNew);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaOApi_CreateProgressDlg(IntPtr hWndParent, string sTitle, IntPtr lpParam);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_DeleteInstance(IntPtr instanceP);
    [DllImport("dmawin.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_DestroyProgressDlg(IntPtr hWnd);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_FindAttribute(ref IntPtr pAttr, int iAttrId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_FindAttributeByName(ref IntPtr lppAttr, string lpctstrAttrName);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaOApi_FindAttributePtr(int iAttrId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaOApi_FindAttributePtrByName(string wcAttrName);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_FindClass(ref IntPtr pClass, int iClassId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_FindClassByName(string lpctstrName, ref IntPtr lppClass);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaOApi_FindClassPtr(int iClassId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaOApi_FindClassPtrByName(string sClassName);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_FindQualifierByName(string lpctstrQualName, ref IntPtr lppQual);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaOApi_FindQualifierPtrByName(string sQualName);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_FreeClass(IntPtr lpClass);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_FreeInstance(IntPtr pInst);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_GetAttributeCmnProps(IntPtr lpAttr, ref int lpAttrId, StringBuilder lpctstrName, StringBuilder lpctstrDesc, ref int lpVisibility, ref int lpDataType, ref int lpDataLen);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_GetAttributeDbProps(IntPtr lpAttr, ref int lpControl, StringBuilder lpInstCol);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaOApi_GetAttributeId(IntPtr attrP);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaOApi_GetAttributeNumericProperty(IntPtr lpAttr, ODSAttributeProperty lAttrPropId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_GetAttributePicklistDefinition(int lAttrId, ref AAODSPickList lpPickList);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_GetAttributePicklistIds(int lAttrId, ref int lplPicklistClassId, ref int lplPicklistCodeId, ref int lplPicklistValueId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaOApi_GetAttributePickListItemCount(IntPtr hPickList);
    public static string aaOApi_GetAttributePickListItemValue(IntPtr hPickList, int lPickIndex)
    {
        return Marshal.PtrToStringUni(__aaOApi_GetAttributePickListItemValue(hPickList, lPickIndex));
    }

    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_GetAttributeQualifierProperties(IntPtr lpAttr, int lQualId, int lIndex, ref int lplQualId, ref int lplDataType, ref int lppValBuf);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_GetAttributeStringProperty(IntPtr lpAttr, ODSAttributeProperty lAttrPropId, StringBuilder lptstrReturnBuffer, int lSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaOApi_GetClassAttrCount(IntPtr classPtrP, bool bAll);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_GetClassAttributes(IntPtr lpClass, int lAttrId, int lIndex, ref int lpAttrId, ref int lpAddonId, ref int lpParentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaOApi_GetClassAttrId(IntPtr classPtrP, int iAttrId, int iIndex, ref int iAddOnId, ref int iParentId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaOApi_GetClassesByQualId(int iQualId, ref IntPtr iClassIdArrayP);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaOApi_GetClassId(IntPtr pClass);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaOApi_GetClassNumericProperty(IntPtr lpClass, ODSClassProperty lClassPropId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_GetClassProps(IntPtr lpClass, ref int lplClassId, StringBuilder lptstrClassName, ref int lplIsVersion, StringBuilder lptstrClassDesc, ref int lplSystem, ref int lplKeyId, ref int lplClAttrId, StringBuilder lptstrTblName, StringBuilder lptstrSeqName, StringBuilder lptstrCatName, ref int lplCatKeyId, StringBuilder lptstrModTime);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_GetClassStringProperty(IntPtr lpClass, ODSClassProperty lClassPropId, StringBuilder lptstrReturnBuffer, int lSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaOApi_GetHrcyId(int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern string aaOApi_GetHrcyStringProperty(int lPropertyId, int lIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_GetInstanceAttrId(IntPtr lpaaOdsInst, int lIndex, int lAttrType, bool bVisible, ref int lplAttrId, ref int lplAttrType, ref int lplAddonId, ref int lplParentId, ref int lplVisibility);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_GetInstanceAttrStrValue(IntPtr lpAAOdsInstance, int lAttrId, int lArrayIndex, StringBuilder lptstrValue, int lSize);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaOApi_GetInstanceClassId(IntPtr pInst);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaOApi_GetInstanceId(IntPtr pInst);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_GetInstanceNumAttrs(IntPtr pInst, int iAttrType, bool bVisible, ref int lNumAttrs);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaOApi_GetLinkFromInstance(IntPtr pClass);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_GetLinks(int lClassId, IntPtr lpInst, bool bFromFlag, ref IntPtr lpppLinks, ref int lpLinkCount);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaOApi_GetLinkToInstance(IntPtr pClass);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaOApi_GetLoadedClassCount();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaOApi_GetLoadedClassPtr(int iIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaOApi_GetQualifierId(IntPtr pQual);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_HrchyAddClass(IntPtr lpClass, IntPtr lpParent, int lHrchyId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_ImportOdsSchemaAsECXML(string ecSchemaXmlFile, bool bPreview, bool bNoValidate);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_Initialize();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_InitializeSession();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_InstanceCloseQuery(IntPtr pQuery);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_InstanceDeleteQuery(IntPtr pQuery);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_InstanceFetchQuery(ref IntPtr pInst, IntPtr pQuery);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaOApi_InstanceNewQuery();
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_InstanceSetQueryClass(IntPtr lpQuery, int lClassId, int lVersionStatus, string lpctstrWhere, IntPtr lpFromInst, IntPtr lpToInst);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_InstanceStartQuery(IntPtr pQuery);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_LinkInstToDoc(IntPtr lpInstance, int lProjNo, int lDocNo);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_LoadAllClasses(int iHierarchyId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaOApi_LoadAttributePickList(IntPtr lpInstance, int lAttributeId, bool bSorted);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaOApi_LoadInstanceByIds(int lClassId, int lInstId, int lVerId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_ModifyAttributeQualifier(int iQualifierId, ref int iQualifierVal, IntPtr lpAttr);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_ModifyAttributeQualifier(int iQualifierId, string sQualifierVal, IntPtr lpAttr);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_NewClass(IntPtr lpBase, ref IntPtr lppNew);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaOApi_NewClassPtr(IntPtr lpBase);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr aaOApi_NewInstance(int lClassId, int lVersionStatus, int[] lplIncAddons, int lNumIncAddons, int[] lplIncAttrs, int lNumIncAttrs, bool bDefaultFlag);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_RemoveAttributeQualifier(int iQualifierId, IntPtr lpAttr);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_RemoveLink(int lClassId, IntPtr lpFromInst, IntPtr lpToInst);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_SaveAttribute(ref IntPtr lppAttr);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_SaveClass(ref IntPtr lppClass);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_SaveInstance(IntPtr lpAAOdsInstance);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern int aaOApi_SelectHrchy(int lHrchyId, string lpctstrName, string lpctstrDecr, string lpctstrModTime, int lSelMask);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_SetAttributeCmnProps(IntPtr lpAttr, string lpctstrDesc, ref int lplVisibility, ref int lplDataType, ref int lplDataLen);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_SetAttributeDbProps(IntPtr lpAttr, ref int lplControl, string lpctstrInstCol);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_SetAttributeName(IntPtr lpAttr, string lpctstrAttrName);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_SetAttributeType(IntPtr lpAttr, ODSAttributeTypes lType);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_SetClassLabel(IntPtr lpClass, int lIntfId, string pszValue);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_SetClassName(string lpctstrClassName, IntPtr lpClass);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_SetClassProps(IntPtr lpClass, string lpctstrClassDesc, ref int lplSystem, ref int lplKeyId, ref int lplClAttrId, string lpctstrTblName, string lpctstrSeqName, string lpctstrCatName, int lplcatKeyId);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_SetInstanceAttrStrValue(IntPtr lpAAOdsInstance, string lpctstrValue, int lAttrId, int lArrayIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_SetInstanceAttrStrValueExt(IntPtr lpAAOdsInstance, string lpctstrValue, int lAttrId, int lArrayIndex, bool bValidate);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_SetInstanceAttrValue(IntPtr lpAAOdsInstance, ref int lpVoid, int lSize, int lAttrId, int lArrayIndex);
    [DllImport("dmscli.dll", CharSet = CharSet.Unicode)]
    public static extern bool aaOApi_SetLink(int lClassId, IntPtr lpFromInst, IntPtr lpToInst);
    private static void AppendProjectWiseDllPathToEnvironmentPath()
    {
        try
        {
            string projectWisePath = GetProjectWisePath();
            if (projectWisePath != null)
            {
                AppendToEnvironmentPath(projectWisePath + @"\bin");
            }
        }
        catch (Exception)
        {
        }
    }

    private static void AppendToEnvironmentPath(string path)
    {
        int capacity = 0x7fff;
        if (ConfigurationManager.AppSettings["Path"] != null)
        {
            Environment.SetEnvironmentVariable("Path", ConfigurationManager.AppSettings["Path"], EnvironmentVariableTarget.Process);
        }
        else
        {
            string str;
            StringBuilder valueBuffer = new StringBuilder(capacity);
            if (GetEnvironmentVariable("Path", valueBuffer, (uint)capacity) > 0)
            {
                if (valueBuffer.ToString().IndexOf(path) != -1)
                {
                    return;
                }
                str = valueBuffer.ToString() + ";" + path;
                if (str.Length >= capacity)
                {
                    throw new ApplicationException("Can not add to 'Path' environment variable because the resulting value would be too long.");
                }
            }
            else
            {
                str = path;
            }
            if (!SetEnvironmentVariable("Path", str))
            {
                throw new ApplicationException("Could not write to 'Path' environment variable.");
            }
        }
    }

    public static int BuildPath(string pwPath, int targetVaultID, int environmentID, int storageID)
    {
        bool flag = true;
        if (((aaApi_GetActiveDatasourceNativeType() == 5) || (aaApi_GetActiveDatasourceNativeType() == 20)) || (aaApi_GetActiveDatasourceNativeType() == 1))
        {
            flag = false;
        }
        char[] separator = new char[] { '\\', '/' };
        string[] strArray = pwPath.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        int iParentId = targetVaultID;
        int createdVaultID = 0;
        if ((strArray.Length == 0) && (targetVaultID > 0))
        {
            return targetVaultID;
        }
        for (int i = 0; i < strArray.Length; i++)
        {
            string str = strArray[i].Trim(" ".ToCharArray());
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > 0x3f)
                {
                    str = str.Substring(0, 0x3f);
                }
                int num4 = -1;
                if (-1 == iParentId)
                {
                    num4 = aaApi_SelectTopLevelProjects();
                }
                else
                {
                    num4 = aaApi_SelectChildProjects(iParentId);
                }
                if (num4 == -1)
                {
                    return 0;
                }
                bool flag2 = false;
                for (int j = 0; j < num4; j++)
                {
                    string str2 = aaApi_GetProjectStringProperty(ProjectProperty.Name, j);
                    if ((str2 == str) || (flag && (str2.ToLower() == str.ToLower())))
                    {
                        flag2 = true;
                        createdVaultID = aaApi_GetProjectNumericProperty(ProjectProperty.ID, j);
                        break;
                    }
                }
                if (!flag2)
                {
                    if (((-1 != iParentId) && (storageID <= 0)) && (1 == aaApi_SelectProject(iParentId)))
                    {
                        storageID = aaApi_GetProjectNumericProperty(ProjectProperty.StorageID, 0);
                    }
                    if ((storageID <= 0) && (aaApi_SelectAllStorages() > 0))
                    {
                        storageID = aaApi_GetStorageId(0);
                    }
                    if (!aaApi_CreateProject(ref createdVaultID, iParentId, storageID, aaApi_GetCurrentUserId(), VaultType.Normal, 0, 0, 0, str, ""))
                    {
                        return 0;
                    }
                    VaultDescriptor vaultDescriptor = new VaultDescriptor
                    {
                        Flags = 3,
                        EnvironmentID = environmentID,
                        VaultID = createdVaultID
                    };
                    bool flag3 = aaApi_ModifyProject2(ref vaultDescriptor);
                }
                iParentId = createdVaultID;
            }
        }
        return createdVaultID;
    }

    public static int BuildPathWithBackslashesOnly(string pwPath, int targetVaultID, int environmentID, int storageID)
    {
        bool flag = true;
        if (((aaApi_GetActiveDatasourceNativeType() == 5) || (aaApi_GetActiveDatasourceNativeType() == 20)) || (aaApi_GetActiveDatasourceNativeType() == 1))
        {
            flag = false;
        }
        char[] separator = new char[] { '\\' };
        string[] strArray = pwPath.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        int iParentId = targetVaultID;
        int createdVaultID = 0;
        if ((strArray.Length == 0) && (targetVaultID > 0))
        {
            return targetVaultID;
        }
        for (int i = 0; i < strArray.Length; i++)
        {
            string str = strArray[i].Trim(" ".ToCharArray());
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > 0x3f)
                {
                    str = str.Substring(0, 0x3f);
                }
                int num4 = -1;
                if (-1 == iParentId)
                {
                    num4 = aaApi_SelectTopLevelProjects();
                }
                else
                {
                    num4 = aaApi_SelectChildProjects(iParentId);
                }
                if (num4 == -1)
                {
                    return 0;
                }
                bool flag2 = false;
                for (int j = 0; j < num4; j++)
                {
                    string str2 = aaApi_GetProjectStringProperty(ProjectProperty.Name, j);
                    if ((str2 == str) || (flag && (str2.ToLower() == str.ToLower())))
                    {
                        flag2 = true;
                        createdVaultID = aaApi_GetProjectNumericProperty(ProjectProperty.ID, j);
                        break;
                    }
                }
                if (!flag2)
                {
                    if (((-1 != iParentId) && (storageID <= 0)) && (1 == aaApi_SelectProject(iParentId)))
                    {
                        storageID = aaApi_GetProjectNumericProperty(ProjectProperty.StorageID, 0);
                    }
                    if ((storageID <= 0) && (aaApi_SelectAllStorages() > 0))
                    {
                        storageID = aaApi_GetStorageId(0);
                    }
                    if (!aaApi_CreateProject(ref createdVaultID, iParentId, storageID, aaApi_GetCurrentUserId(), VaultType.Normal, 0, 0, 0, str, ""))
                    {
                        return 0;
                    }
                    VaultDescriptor vaultDescriptor = new VaultDescriptor
                    {
                        Flags = 3,
                        EnvironmentID = environmentID,
                        VaultID = createdVaultID
                    };
                    bool flag3 = aaApi_ModifyProject2(ref vaultDescriptor);
                }
                iParentId = createdVaultID;
            }
        }
        return createdVaultID;
    }

    public static int BuildPathWithBackslashesOnly(string pwPath, int targetVaultID, int environmentID, int storageID, int iWorkflowId)
    {
        bool flag = true;
        if (((aaApi_GetActiveDatasourceNativeType() == 5) || (aaApi_GetActiveDatasourceNativeType() == 20)) || (aaApi_GetActiveDatasourceNativeType() == 1))
        {
            flag = false;
        }
        char[] separator = new char[] { '\\' };
        string[] strArray = pwPath.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        int iParentId = targetVaultID;
        int createdVaultID = 0;
        if ((strArray.Length == 0) && (targetVaultID > 0))
        {
            return targetVaultID;
        }
        for (int i = 0; i < strArray.Length; i++)
        {
            string str = strArray[i].Trim(" ".ToCharArray());
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > 0x3f)
                {
                    str = str.Substring(0, 0x3f);
                }
                int num4 = -1;
                if (-1 == iParentId)
                {
                    num4 = aaApi_SelectTopLevelProjects();
                }
                else
                {
                    num4 = aaApi_SelectChildProjects(iParentId);
                }
                if (num4 == -1)
                {
                    return 0;
                }
                bool flag2 = false;
                for (int j = 0; j < num4; j++)
                {
                    string str2 = aaApi_GetProjectStringProperty(ProjectProperty.Name, j);
                    if ((str2 == str) || (flag && (str2.ToLower() == str.ToLower())))
                    {
                        flag2 = true;
                        createdVaultID = aaApi_GetProjectNumericProperty(ProjectProperty.ID, j);
                        break;
                    }
                }
                if (!flag2)
                {
                    if (((-1 != iParentId) && (storageID <= 0)) && (1 == aaApi_SelectProject(iParentId)))
                    {
                        storageID = aaApi_GetProjectNumericProperty(ProjectProperty.StorageID, 0);
                    }
                    if ((storageID <= 0) && (aaApi_SelectAllStorages() > 0))
                    {
                        storageID = aaApi_GetStorageId(0);
                    }
                    if (!aaApi_CreateProject(ref createdVaultID, iParentId, storageID, aaApi_GetCurrentUserId(), VaultType.Normal, 0, 0, 0, str, ""))
                    {
                        return 0;
                    }
                    VaultDescriptor vaultDescriptor = new VaultDescriptor
                    {
                        Flags = 0x43,
                        EnvironmentID = environmentID,
                        VaultID = createdVaultID,
                        WorkflowID = iWorkflowId
                    };
                    bool flag3 = aaApi_ModifyProject2(ref vaultDescriptor);
                    if (aaApi_SetProjectWorkflow(createdVaultID, iWorkflowId))
                    {
                    }
                }
                iParentId = createdVaultID;
            }
        }
        return createdVaultID;
    }

    public static int BuildPWPath(string pwPath, int targetVaultID, string userName, int environmentID)
    {
        bool flag = true;
        if (((aaApi_GetActiveDatasourceNativeType() == 5) || (aaApi_GetActiveDatasourceNativeType() == 20)) || (aaApi_GetActiveDatasourceNativeType() == 1))
        {
            flag = false;
        }
        aaApi_SelectUsersByProp(userName, null, null);
        int managerID = aaApi_GetUserId(0);
        int storageID = 1;
        bool flag2 = false;
        if (environmentID > 0)
        {
            flag2 = true;
        }
        int num3 = 0;
        char[] separator = new char[] { '\\', '/' };
        string[] strArray = pwPath.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        int iParentId = targetVaultID;
        int createdVaultID = 0;
        for (int i = 0; i < strArray.Length; i++)
        {
            string str = strArray[i].Trim(" ".ToCharArray());
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > 0x3f)
                {
                    str = str.Substring(0, 0x3f);
                }
                int num7 = -1;
                if (-1 == iParentId)
                {
                    num7 = aaApi_SelectTopLevelProjects();
                }
                else
                {
                    num7 = aaApi_SelectChildProjects(iParentId);
                }
                if (num7 == -1)
                {
                    return 0;
                }
                bool flag3 = false;
                for (int j = 0; j < num7; j++)
                {
                    string str2 = aaApi_GetProjectStringProperty(ProjectProperty.Name, j);
                    if ((str2 == str) || (flag && (str2.ToLower() == str.ToLower())))
                    {
                        flag3 = true;
                        createdVaultID = aaApi_GetProjectNumericProperty(ProjectProperty.ID, j);
                        storageID = aaApi_GetProjectNumericProperty(ProjectProperty.StorageID, j);
                        num3 = aaApi_GetProjectNumericProperty(ProjectProperty.EnvironmentID, j);
                        break;
                    }
                }
                if (!flag3)
                {
                    bool flag4 = aaApi_CreateProject(ref createdVaultID, iParentId, storageID, managerID, VaultType.Normal, 0, 0, 0, str, "");
                    if (flag4)
                    {
                        VaultDescriptor vaultDescriptor = new VaultDescriptor
                        {
                            Flags = 3
                        };
                        if (flag2)
                        {
                            vaultDescriptor.EnvironmentID = environmentID;
                        }
                        else
                        {
                            vaultDescriptor.EnvironmentID = num3;
                        }
                        vaultDescriptor.VaultID = createdVaultID;
                        flag4 = aaApi_ModifyProject2(ref vaultDescriptor);
                    }
                    if (!flag4)
                    {
                        return 0;
                    }
                }
                iParentId = createdVaultID;
            }
        }
        return createdVaultID;
    }

    public static DataTable CreateDataTableFromSQLSelect(string sSQL, string sTableName)
    {
        Console.WriteLine(sSQL);
        int numColumnsSelected = 0;
        int num2 = aaApi_SqlSelect(sSQL, IntPtr.Zero, ref numColumnsSelected);
        DataTable table = new DataTable();
        if (!string.IsNullOrEmpty(sTableName))
        {
            table.TableName = sTableName;
        }
        if (numColumnsSelected > 0)
        {
            for (int i = 0; i < numColumnsSelected; i++)
            {
                table.Columns.Add(GetDataColumn(aaApi_SqlSelectGetStringProperty(SqlSelectProperties.SQLSELECT_COLUMN_NAME, i), (SQLSelectDBColumnTypes)aaApi_SqlSelectGetNumericProperty(SqlSelectProperties.SQLSELECT_COLUMN_NATIVE_TYPE, i), (SQLSelectPWTypes)aaApi_SqlSelectGetNumericProperty(SqlSelectProperties.SQLSELECT_COLUMN_TYPE, i), aaApi_SqlSelectGetNumericProperty(SqlSelectProperties.SQLSELECT_COLUMN_LENGTH, i)));
            }
            for (int j = 0; j < num2; j++)
            {
                DataRow row = table.NewRow();
                for (int k = 0; k < numColumnsSelected; k++)
                {
                    string s = aaApi_SqlSelectGetData(j, k);
                    try
                    {
                        if (table.Columns[k].DataType == Type.GetType("System.String"))
                        {
                            row[k] = s;
                        }
                        else if (table.Columns[k].DataType == Type.GetType("System.DateTime"))
                        {
                            DateTime now = DateTime.Now;
                            if (DateTime.TryParse(s, out now))
                            {
                                row[k] = now;
                            }
                        }
                        else if (table.Columns[k].DataType == Type.GetType("System.Boolean"))
                        {
                            bool result = false;
                            string str2 = s.ToLower();
                            if (bool.TryParse(str2, out result))
                            {
                                row[k] = result;
                            }
                            else if (((str2 == "no") || (str2 == "false")) || (str2 == "0"))
                            {
                                row[k] = false;
                            }
                            else if (((str2 == "yes") || (str2 == "true")) || (str2 == "1"))
                            {
                                row[k] = true;
                            }
                        }
                        else if (table.Columns[k].DataType == Type.GetType("System.Guid"))
                        {
                            try
                            {
                                Guid guid = new Guid(s);
                                row[k] = guid;
                            }
                            catch
                            {
                            }
                        }
                        else if (table.Columns[k].DataType == Type.GetType("System.Int32"))
                        {
                            int result = 0;
                            if (int.TryParse(s, out result))
                            {
                                row[k] = result;
                            }
                        }
                        else if (table.Columns[k].DataType == Type.GetType("System.Double"))
                        {
                            double result = 0.0;
                            if (double.TryParse(s, out result))
                            {
                                row[k] = result;
                            }
                        }
                        else if (table.Columns[k].DataType == Type.GetType("System.Int64"))
                        {
                            long result = 0L;
                            if (long.TryParse(s, out result))
                            {
                                row[k] = result;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                try
                {
                    table.Rows.Add(row);
                }
                catch (Exception)
                {
                }
            }
        }        
        return table;
    }

    public static Hashtable GetAllAttributeColumnValues(int iProjectNo, int iDocumentNo)
    {
        int lplEnvironmentId = 0;
        int lplTableId = 0;
        int lplIdColumnId = 0;
        Hashtable hashtable = new Hashtable();
        if (aaApi_GetEnvTableInfoByProject(iProjectNo, ref lplEnvironmentId, ref lplTableId, ref lplIdColumnId))
        {
            int lplColumnCount = 0;
            int num5 = aaApi_SelectLinkDataByObject(lplTableId, ObjectTypeForLinkData.Document, iProjectNo, iDocumentNo, null, ref lplColumnCount, null, 0);
            for (int i = 0; i < num5; i++)
            {
                for (int j = 0; j < lplColumnCount; j++)
                {
                    hashtable.Add(aaApi_GetLinkDataColumnStringProperty(LinkDataProperty.ColumnName, j).ToLower(), aaApi_GetLinkDataColumnValue(i, j));
                }
            }
        }
        return hashtable;
    }

    public static SortedList<string, string> GetAllAttributeColumnValuesInList(int iProjectNo, int iDocumentNo)
    {
        int lplEnvironmentId = 0;
        int lplTableId = 0;
        int lplIdColumnId = 0;
        SortedList<string, string> list = new SortedList<string, string>(StringComparer.InvariantCultureIgnoreCase);
        if (aaApi_GetEnvTableInfoByProject(iProjectNo, ref lplEnvironmentId, ref lplTableId, ref lplIdColumnId))
        {
            int lplColumnCount = 0;
            if (aaApi_SelectLinkDataByObject(lplTableId, ObjectTypeForLinkData.Document, iProjectNo, iDocumentNo, null, ref lplColumnCount, null, 0) <= 0)
            {
                return list;
            }
            for (int i = 0; i < lplColumnCount; i++)
            {
                string key = aaApi_GetLinkDataColumnStringProperty(LinkDataProperty.ColumnName, i);
                if (!list.ContainsKey(key))
                {
                    list.Add(key, aaApi_GetLinkDataColumnValue(0, i));
                }
            }
        }
        return list;
    }

    public static int GetApplicationId(string sApplicationName)
    {
        int num = aaApi_SelectAllApplications();
        for (int i = 0; i < num; i++)
        {
            string str = aaApi_GetApplicationStringProperty(ApplicationProperty.Name, i);
            if (sApplicationName.ToLower() == str.ToLower())
            {
                return aaApi_GetApplicationId(i);
            }
        }
        return 0;
    }

    public static int GetApplicationNumberForApplicationName(string sAppName)
    {
        int num = aaApi_SelectAllApplications();
        for (int i = 0; i < num; i++)
        {
            if (sAppName.ToLower() == aaApi_GetApplicationStringProperty(ApplicationProperty.Name, i).ToLower())
            {
                return aaApi_GetApplicationId(i);
            }
        }
        return 0;
    }

    public static SortedList<string, ProjectWiseApplication> GetApplicationsByName()
    {
        SortedList<string, ProjectWiseApplication> list = new SortedList<string, ProjectWiseApplication>(StringComparer.InvariantCultureIgnoreCase);
        int num = aaApi_SelectAllApplications();
        for (int i = 0; i < num; i++)
        {
            string key = aaApi_GetApplicationStringProperty(ApplicationProperty.Name, i);
            int iID = aaApi_GetApplicationNumericProperty(ApplicationProperty.ID, i);
            if (!list.ContainsKey(key))
            {
                list.Add(key, new ProjectWiseApplication(iID, key));
            }
        }
        return list;
    }

    public static Hashtable GetAttributeColumnIdsFromEnvironment(int iEnvId)
    {
        Hashtable hashtable = new Hashtable();
        if (1 == aaApi_SelectEnv(iEnvId))
        {
            int num = aaApi_SelectEnvAttrDefs(iEnvId, -1, -1);
            for (int i = 0; i < num; i++)
            {
                int iTableId = aaApi_GetEnvAttrDefNumericProperty(AttributeDefinitionProperty.TableID, i);
                int iColumnId = aaApi_GetEnvAttrDefNumericProperty(AttributeDefinitionProperty.ColumnID, i);
                if (1 == aaApi_SelectColumn(iTableId, iColumnId))
                {
                    string str = aaApi_GetColumnStringProperty(ColumnProperty.Name, 0);
                    if (!hashtable.ContainsKey(iColumnId))
                    {
                        hashtable.Add(iColumnId, str.ToLower());
                    }
                }
            }
        }
        return hashtable;
    }

    public static Hashtable GetAttributeColumnNamesFromEnvironment(int iEnvId)
    {
        Hashtable hashtable = new Hashtable();
        if (1 == aaApi_SelectEnv(iEnvId))
        {
            int num = aaApi_SelectEnvAttrDefs(iEnvId, -1, -1);
            for (int i = 0; i < num; i++)
            {
                int iTableId = aaApi_GetEnvAttrDefNumericProperty(AttributeDefinitionProperty.TableID, i);
                int iColumnId = aaApi_GetEnvAttrDefNumericProperty(AttributeDefinitionProperty.ColumnID, i);
                if (1 == aaApi_SelectColumn(iTableId, iColumnId))
                {
                    string str = aaApi_GetColumnStringProperty(ColumnProperty.Name, 0);
                    if (!hashtable.ContainsKey(str.ToLower()))
                    {
                        hashtable.Add(str.ToLower(), iColumnId);
                    }
                }
            }
        }
        return hashtable;
    }

    public static string GetAttributeColumnValue(int iProjectNo, int iDocumentNo, string sColumnName)
    {
        int lplEnvironmentId = 0;
        int lplTableId = 0;
        int lplIdColumnId = 0;
        string str = string.Empty;
        if (!string.IsNullOrEmpty(sColumnName) && aaApi_GetEnvTableInfoByProject(iProjectNo, ref lplEnvironmentId, ref lplTableId, ref lplIdColumnId))
        {
            int lplColumnCount = 0;
            int num5 = aaApi_SelectLinkDataByObject(lplTableId, ObjectTypeForLinkData.Document, iProjectNo, iDocumentNo, null, ref lplColumnCount, null, 0);
            for (int i = 0; i < num5; i++)
            {
                for (int j = 0; j < lplColumnCount; j++)
                {
                    string str2 = aaApi_GetLinkDataColumnStringProperty(LinkDataProperty.ColumnName, j);
                    if (!string.IsNullOrEmpty(str2) && (sColumnName.ToLower() == str2.ToLower()))
                    {
                        str = aaApi_GetLinkDataColumnValue(i, j);
                        break;
                    }
                }
            }
        }
        return str;
    }

    public static void GetAttributeColumnValues(int iProjectNo, int iDocumentNo, ref Hashtable htAttrVals)
    {
        int lplEnvironmentId = 0;
        int lplTableId = 0;
        int lplIdColumnId = 0;
        if (aaApi_GetEnvTableInfoByProject(iProjectNo, ref lplEnvironmentId, ref lplTableId, ref lplIdColumnId))
        {
            int lplColumnCount = 0;
            int num5 = aaApi_SelectLinkDataByObject(lplTableId, ObjectTypeForLinkData.Document, iProjectNo, iDocumentNo, null, ref lplColumnCount, null, 0);
            for (int i = 0; i < num5; i++)
            {
                for (int j = 0; j < lplColumnCount; j++)
                {
                    string str = aaApi_GetLinkDataColumnStringProperty(LinkDataProperty.ColumnName, j);
                    if (htAttrVals.ContainsKey(str.ToLower()))
                    {
                        htAttrVals[str.ToLower()] = aaApi_GetLinkDataColumnValue(i, j);
                    }
                }
            }
        }
    }

    public static int GetAttrIDFromName(string wcAttrNameP)
    {
        IntPtr attrP = aaOApi_FindAttributePtrByName(wcAttrNameP);
        if (attrP == IntPtr.Zero)
        {
            return 0;
        }
        return aaOApi_GetAttributeId(attrP);
    }

    public static ArrayList GetBranchProjectNos(int iProjectNo, bool bGetSubProjects)
    {
        ArrayList list = new ArrayList();
        if (bGetSubProjects && (iProjectNo > 0))
        {
            int num = aaApi_SelectProjectsFromBranch(iProjectNo, null, null, null, null);
            for (int i = 0; i < num; i++)
            {
                list.Add(aaApi_GetProjectNumericProperty(ProjectProperty.ID, i));
            }
            return list;
        }
        if (iProjectNo <= 0)
        {
            int num3 = aaApi_SelectAllProjects();
            for (int i = 0; i < num3; i++)
            {
                list.Add(aaApi_GetProjectNumericProperty(ProjectProperty.ID, i));
            }
            return list;
        }
        list.Add(iProjectNo);
        return list;
    }

    public static int GetClassIdFromClassName(string sClassName)
    {
        IntPtr pClass = aaOApi_FindClassPtrByName(sClassName);
        if (pClass != IntPtr.Zero)
        {
            return aaOApi_GetClassId(pClass);
        }
        return 0;
    }

    public static string GetClassNameFromClassId(int iClassId)
    {
        IntPtr lpClass = aaOApi_FindClassPtr(iClassId);
        if (lpClass != IntPtr.Zero)
        {
            StringBuilder lptstrReturnBuffer = new StringBuilder(0x100);
            if (aaOApi_GetClassStringProperty(lpClass, ODSClassProperty.Name, lptstrReturnBuffer, lptstrReturnBuffer.Capacity))
            {
                return lptstrReturnBuffer.ToString();
            }
        }
        return string.Empty;
    }

    public static SortedList<string, int> GetClassPropertyIdsInList(int iClassId)
    {
        SortedList<string, int> list = new SortedList<string, int>(StringComparer.InvariantCultureIgnoreCase);
        IntPtr classPtrP = aaOApi_FindClassPtr(iClassId);
        if (classPtrP != IntPtr.Zero)
        {
            int num = aaOApi_GetClassAttrCount(classPtrP, true);
            for (int i = 0; i < num; i++)
            {
                int lpAttrId = 0;
                int lpAddonId = 0;
                int lpParentId = 0;
                if (aaOApi_GetClassAttributes(classPtrP, 0, i, ref lpAttrId, ref lpAddonId, ref lpParentId))
                {
                    IntPtr zero = IntPtr.Zero;
                    zero = aaOApi_FindAttributePtr(lpAttrId);
                    if (zero != IntPtr.Zero)
                    {
                        StringBuilder lpctstrName = new StringBuilder(0x100);
                        StringBuilder lpctstrDesc = new StringBuilder(0x100);
                        int lpVisibility = 0;
                        int lpDataType = 0;
                        int lpDataLen = 0;
                        if (aaOApi_GetAttributeCmnProps(zero, ref lpAttrId, lpctstrName, lpctstrDesc, ref lpVisibility, ref lpDataType, ref lpDataLen) && !list.ContainsKey(lpctstrName.ToString()))
                        {
                            list.Add(lpctstrName.ToString(), lpAttrId);
                        }
                    }
                }
            }
        }
        return list;
    }

    public static SortedList<string, int> GetClassPropertyIdsInList(string sClassName)
    {
        return GetClassPropertyIdsInList(GetClassIdFromClassName(sClassName));
    }

    public static Hashtable GetColumnIdsKeyedByNameFromTable(int iTableId)
    {
        Hashtable hashtable = new Hashtable();
        int num = aaApi_SelectColumnsByTable(iTableId);
        for (int i = 0; i < num; i++)
        {
            int num3 = aaApi_GetColumnNumericProperty(ColumnProperty.ColumnID, i);
            string str = aaApi_GetColumnStringProperty(ColumnProperty.Name, i);
            if (!hashtable.ContainsKey(str.ToLower()))
            {
                hashtable.Add(str.ToLower(), num3);
            }
        }
        return hashtable;
    }

    public static Hashtable GetColumnNamesKeyedByIdFromTable(int iTableId)
    {
        Hashtable hashtable = new Hashtable();
        int num = aaApi_SelectColumnsByTable(iTableId);
        for (int i = 0; i < num; i++)
        {
            int key = aaApi_GetColumnNumericProperty(ColumnProperty.ColumnID, i);
            string str = aaApi_GetColumnStringProperty(ColumnProperty.Name, i);
            if (!hashtable.ContainsKey(key))
            {
                hashtable.Add(key, str.ToLower());
            }
        }
        return hashtable;
    }

    public static string GetColumnTypeName(SQLSelectDBColumnTypes iNativeType, SQLSelectPWTypes iPWType)
    {
        if (iNativeType == SQLSelectDBColumnTypes.DateTime)
        {
            return "datetime";
        }
        if (iNativeType == SQLSelectDBColumnTypes.SQLBoolean)
        {
            return "boolean";
        }
        if (iNativeType == SQLSelectDBColumnTypes.SQLGuid)
        {
            return "guid";
        }
        if (iPWType != SQLSelectPWTypes.String)
        {
            if (iPWType == SQLSelectPWTypes.Integer)
            {
                return "integer";
            }
            if (iPWType == SQLSelectPWTypes.Double)
            {
                return "double";
            }
            if (iPWType == SQLSelectPWTypes.Long)
            {
                return "integer64";
            }
            if (iPWType == SQLSelectPWTypes.Short)
            {
                return "integer";
            }
            if (iPWType == SQLSelectPWTypes.SQLReal)
            {
                return "double";
            }
        }
        return "string";
    }

    private static string GetCredentialHash(DataSourceType lDSType, string lptstrDataSource, string lpctstrUsername, string lpctstrPassword, string lpctstrSchema)
    {
        string s = lptstrDataSource + "_" + lpctstrUsername + "_" + lpctstrPassword + "_" + lpctstrSchema + "_" + lDSType.ToString();
        byte[] bytes = Encoding.Unicode.GetBytes(s);
        return Convert.ToBase64String(MD5.Create().ComputeHash(bytes));
    }

    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetCurrentSession2", CharSet = CharSet.Unicode)]
    public static extern bool GetCurrentSession2(out long pSession);
    private static DataColumn GetDataColumn(string sName, SQLSelectDBColumnTypes iNativeType, SQLSelectPWTypes iPWType, int iLength)
    {
        if (iNativeType == SQLSelectDBColumnTypes.DateTime)
        {
            return new DataColumn(sName, Type.GetType("System.DateTime"));
        }
        if (iNativeType == SQLSelectDBColumnTypes.SQLBoolean)
        {
            return new DataColumn(sName, Type.GetType("System.Boolean"));
        }
        if (iNativeType == SQLSelectDBColumnTypes.SQLGuid)
        {
            return new DataColumn(sName, Type.GetType("System.Guid"));
        }
        if (iPWType != SQLSelectPWTypes.String)
        {
            if (iPWType == SQLSelectPWTypes.Integer)
            {
                return new DataColumn(sName, Type.GetType("System.Int32"));
            }
            if (iPWType == SQLSelectPWTypes.Double)
            {
                return new DataColumn(sName, Type.GetType("System.Double"));
            }
            if (iPWType == SQLSelectPWTypes.Long)
            {
                return new DataColumn(sName, Type.GetType("System.Int64"));
            }
            if (iPWType == SQLSelectPWTypes.Short)
            {
                return new DataColumn(sName, Type.GetType("System.Int32"));
            }
            if (iPWType == SQLSelectPWTypes.SQLReal)
            {
                return new DataColumn(sName, Type.GetType("System.Double"));
            }
        }
        return new DataColumn(sName, Type.GetType("System.String"));
    }

    public static string GetDatasourceNameFromMonikerString(string sMoniker)
    {
        IntPtr[] pMonikers = new IntPtr[1];
        string[] sArrayMonikers = new string[] { sMoniker };
        if (aaApi_StringsToMonikers(1, pMonikers, sArrayMonikers, 8))
        {
            return aaApi_GetDatasourceNameFromMoniker(pMonikers[0]);
        }
        return string.Empty;
    }

    public static int GetDepartmentId(string sDepartmentName)
    {
        if (!string.IsNullOrEmpty(sDepartmentName))
        {
            for (int i = 0; i < aaApi_SelectAllDepartments(); i++)
            {
                if (aaApi_GetDepartmentStringProperty(DepartmentProperty.Name, i).ToLower() == sDepartmentName.ToLower())
                {
                    return aaApi_GetDepartmentId(i);
                }
            }
        }
        return 0;
    }

    public static bool GetDocumentIdsFromMonikerString(string sMoniker, ref int iProjectId, ref int iDocumentId)
    {
        IntPtr[] pMonikers = new IntPtr[1];
        string[] sArrayMonikers = new string[] { sMoniker };
        bool flag = false;
        try
        {
            if (aaApi_StringsToMonikers(1, pMonikers, sArrayMonikers, 8))
            {
                byte[] destination = default(Guid).ToByteArray();
                Marshal.Copy(aaApi_GetDocumentGuidFromMoniker(pMonikers[0]), destination, 0, destination.Length);
                Guid guid2 = new Guid(destination);
                AaDocItem pDocuments = new AaDocItem();
                Guid[] docGuids = new Guid[] { guid2 };
                if (aaApi_GetDocumentIdsByGUIDs(1, docGuids, ref pDocuments))
                {
                    iProjectId = pDocuments.lProjectId;
                    iDocumentId = pDocuments.lDocumentId;
                    flag = true;
                }
            }
        }
        catch (Exception exception)
        {
            BPSUtilities.WriteLog("Error: {0}\n{1}", new object[] { exception.Message, exception.StackTrace });
        }
        return flag;
    }

    public static string GetDocumentNamePath(Guid docGuid)
    {
        StringBuilder stringBuffer = new StringBuilder(0x13e8);
        if (aaApi_GUIDGetDocumentNamePath(ref docGuid, false, '\\', stringBuffer, stringBuffer.Capacity))
        {
            return stringBuffer.ToString();
        }
        return string.Empty;
    }

    public static string GetDocumentNamePath(int iProjectId, int iDocId)
    {
        StringBuilder stringBuffer = new StringBuilder(0x13e8);
        if (aaApi_GetDocumentNamePath(iProjectId, iDocId, false, '\\', stringBuffer, stringBuffer.Capacity))
        {
            return stringBuffer.ToString();
        }
        return string.Empty;
    }

    public static string GetDocumentURL(Guid docGuid)
    {
        StringBuilder lptstrName = new StringBuilder(0x400);
        if (aaApi_GetActiveDatasourceName(lptstrName, 0x400))
        {
            return string.Format("pw://{0}/Documents/D{1}", lptstrName.ToString(), "{" + docGuid.ToString() + "}");
        }
        return string.Empty;
    }

    public static string GetDocumentURL(Guid docGuid, string sDSName)
    {
        return string.Format("pw://{0}/Documents/D{1}", sDSName, "{" + docGuid.ToString() + "}");
    }

    public static string GetDocumentURL(int iProjectId, int iDocId)
    {
        StringBuilder lptstrName = new StringBuilder(0x400);
        if (aaApi_GetActiveDatasourceName(lptstrName, 0x400))
        {
            return string.Format("pw://{0}/Documents/D{1}", lptstrName.ToString(), "{" + GetGuidStringFromIds(iProjectId, iDocId) + "}");
        }
        return string.Empty;
    }

    public static string GetDocumentURL(int iProjectId, int iDocId, string sDSName)
    {
        return string.Format("pw://{0}/Documents/D{1}", sDSName, "{" + GetGuidStringFromIds(iProjectId, iDocId) + "}");
    }

    public static string GetDocumentWebLink(string sWebAddressIncludingASPXName, Guid docGuid, WebLinkActions action)
    {
        StringBuilder lptstrName = new StringBuilder(0x400);
        if (aaApi_GetActiveDatasourceName(lptstrName, 0x400))
        {
            return GetFormattedWebLink(sWebAddressIncludingASPXName, lptstrName.ToString(), GetDocumentNamePath(docGuid), action);
        }
        return string.Empty;
    }

    public static string GetDocumentWebLink(string sWebAddressIncludingASPXName, int iProjectId, int iDocumentId, WebLinkActions action)
    {
        StringBuilder lptstrName = new StringBuilder(0x400);
        if (aaApi_GetActiveDatasourceName(lptstrName, 0x400))
        {
            return GetFormattedWebLink(sWebAddressIncludingASPXName, lptstrName.ToString(), GetDocumentNamePath(iProjectId, iDocumentId), action);
        }
        return string.Empty;
    }

    public static string GetDocumentWebLink(string sWebAddressIncludingASPXName, string sDatasource, Guid docGuid, WebLinkActions action)
    {
        return GetFormattedWebLink(sWebAddressIncludingASPXName, sDatasource, GetDocumentNamePath(docGuid), action);
    }

    public static string GetDocumentWebLink(string sWebAddressIncludingASPXName, string sDatasource, int iProjectId, int iDocumentId, WebLinkActions action)
    {
        return GetFormattedWebLink(sWebAddressIncludingASPXName, sDatasource, GetDocumentNamePath(iProjectId, iDocumentId), action);
    }

    public static int GetEnvironmentId(string sEnvironmentName)
    {
        if (!string.IsNullOrEmpty(sEnvironmentName))
        {
            for (int i = 0; i < aaApi_SelectAllEnvs(false); i++)
            {
                if (aaApi_GetEnvStringProperty(5, i).ToLower() == sEnvironmentName.ToLower())
                {
                    return aaApi_GetEnvId(i);
                }
            }
        }
        return 0;
    }

    [DllImport("KERNEL32.dll")]
    private static extern uint GetEnvironmentVariable(string name, StringBuilder valueBuffer, uint bufferSize);
    public static int GetFolderNoFromPath(string pwPath, int targetVaultID)
    {
        char[] separator = new char[] { '\\', '/' };
        string[] strArray = pwPath.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        int iParentId = targetVaultID;
        int num2 = 0;
        for (int i = 0; i < strArray.Length; i++)
        {
            string str = strArray[i].Trim(" ".ToCharArray());
            if (!string.IsNullOrEmpty(str))
            {
                int num4 = -1;
                if (-1 == iParentId)
                {
                    num4 = aaApi_SelectTopLevelProjects();
                }
                else
                {
                    num4 = aaApi_SelectChildProjects(iParentId);
                }
                if (num4 == -1)
                {
                    return 0;
                }
                bool flag = false;
                for (int j = 0; j < num4; j++)
                {
                    if (aaApi_GetProjectStringProperty(ProjectProperty.Name, j) == str)
                    {
                        flag = true;
                        num2 = aaApi_GetProjectNumericProperty(ProjectProperty.ID, j);
                        break;
                    }
                }
                if (!flag)
                {
                    return 0;
                }
                iParentId = num2;
            }
        }
        return num2;
    }

    public static string GetFormattedWebLink(string sWebAddressIncludingASPXName, string sDatasource, string sDocumentNamePath, WebLinkActions action)
    {
        string format = "{0}?location={1}&link={2}";
        string str2 = HttpUtility.UrlEncode(sDatasource);
        string str3 = HttpUtility.UrlEncode(string.Format("pw://{0}/Documents/{1}", sDatasource, sDocumentNamePath));
        string str4 = string.Format(format, sWebAddressIncludingASPXName, str2, str3);
        string str5 = string.Empty;
        switch (action)
        {
            case WebLinkActions.Open:
                str5 = "&action=DM_Open";
                break;

            case WebLinkActions.OpenReadOnly:
                str5 = "&action=DM_Open_ReadOnly";
                break;

            case WebLinkActions.View:
                str5 = "&action=DM_View";
                break;

            case WebLinkActions.Markup:
                str5 = "&action=DM_Redline";
                break;
        }
        return (str4 + str5);
    }

    public static SortedList<int, ProjectWiseGroup> GetGroupsById()
    {
        SortedList<int, ProjectWiseGroup> list = new SortedList<int, ProjectWiseGroup>();
        int num = aaApi_SelectAllGroups();
        for (int i = 0; i < num; i++)
        {
            string sName = aaApi_GetGroupStringProperty(GroupProperty.Name, i);
            int key = aaApi_GetGroupNumericProperty(GroupProperty.ID, i);
            if (!list.ContainsKey(key))
            {
                list.Add(key, new ProjectWiseGroup(key, sName, aaApi_GetGroupStringProperty(GroupProperty.Desc, i), aaApi_GetGroupStringProperty(GroupProperty.SecProvider, i), aaApi_GetGroupStringProperty(GroupProperty.Type, i)));
            }
        }
        return list;
    }

    public static SortedList<string, ProjectWiseGroup> GetGroupsByName()
    {
        SortedList<string, ProjectWiseGroup> list = new SortedList<string, ProjectWiseGroup>(StringComparer.InvariantCultureIgnoreCase);
        int num = aaApi_SelectAllGroups();
        for (int i = 0; i < num; i++)
        {
            string key = aaApi_GetGroupStringProperty(GroupProperty.Name, i);
            int iID = aaApi_GetGroupNumericProperty(GroupProperty.ID, i);
            if (!list.ContainsKey(key))
            {
                list.Add(key, new ProjectWiseGroup(iID, key, aaApi_GetGroupStringProperty(GroupProperty.Desc, i), aaApi_GetGroupStringProperty(GroupProperty.SecProvider, i), aaApi_GetGroupStringProperty(GroupProperty.Type, i)));
            }
        }
        return list;
    }

    public static string GetGuidStringFromIds(int iProjectId, int iDocId)
    {
        AaDocItem[] pDocuments = new AaDocItem[1];
        Guid[] docGuids = new Guid[1];
        pDocuments[0].lProjectId = iProjectId;
        pDocuments[0].lDocumentId = iDocId;
        if (aaApi_GetDocumentGUIDsByIds(1, pDocuments, docGuids))
        {
            return docGuids[0].ToString();
        }
        return docGuids[0].ToString();
    }

    public static bool GetIdsFromGuidString(string sDocGuid, ref int iProjId, ref int iDocId)
    {
        AaDocItem pDocuments = new AaDocItem();
        Guid[] docGuids = new Guid[] { new Guid(sDocGuid) };
        if (aaApi_GetDocumentIdsByGUIDs(1, docGuids, ref pDocuments))
        {
            iProjId = pDocuments.lProjectId;
            iDocId = pDocuments.lDocumentId;
            return true;
        }
        return false;
    }

    public static SortedList<string, string> GetInstancePropertyValuesInList(int iClassId, int iInstanceId)
    {
        SortedList<string, string> list = new SortedList<string, string>(StringComparer.InvariantCultureIgnoreCase);
        IntPtr pInst = aaOApi_LoadInstanceByIds(iClassId, iInstanceId, 0);
        if (pInst != IntPtr.Zero)
        {
            int lNumAttrs = 0;
            if (aaOApi_GetInstanceNumAttrs(pInst, 0, false, ref lNumAttrs))
            {
                for (int i = 0; i < lNumAttrs; i++)
                {
                    int lplAttrId = 0;
                    int lplAttrType = 0;
                    int lplAddonId = 0;
                    int lplParentId = 0;
                    int lplVisibility = 0;
                    if (aaOApi_GetInstanceAttrId(pInst, i, 0, false, ref lplAttrId, ref lplAttrType, ref lplAddonId, ref lplParentId, ref lplVisibility))
                    {
                        IntPtr lpAttr = aaOApi_FindAttributePtr(lplAttrId);
                        StringBuilder lptstrReturnBuffer = new StringBuilder(0x200);
                        if (aaOApi_GetAttributeStringProperty(lpAttr, ODSAttributeProperty.Name, lptstrReturnBuffer, lptstrReturnBuffer.Capacity))
                        {
                            int num8 = aaOApi_GetAttributeNumericProperty(lpAttr, ODSAttributeProperty.DataLength);
                            if (num8 > 0)
                            {
                                StringBuilder lptstrValue = new StringBuilder(num8 + 2);
                                if (aaOApi_GetInstanceAttrStrValue(pInst, lplAttrId, 0, lptstrValue, lptstrValue.Capacity) && !list.ContainsKey(lptstrReturnBuffer.ToString()))
                                {
                                    list.Add(lptstrReturnBuffer.ToString(), lptstrValue.ToString());
                                }
                            }
                        }
                    }
                }
            }
            aaOApi_FreeInstance(pInst);
        }
        return list;
    }

    public static SortedList<string, string> GetInstancePropertyValuesInList(int iClassId, int iInstanceId, SortedList<string, int> slProperties)
    {
        SortedList<string, string> list = new SortedList<string, string>(StringComparer.InvariantCultureIgnoreCase);
        IntPtr lpAAOdsInstance = aaOApi_LoadInstanceByIds(iClassId, iInstanceId, 0);
        if (lpAAOdsInstance != IntPtr.Zero)
        {
            foreach (KeyValuePair<string, int> pair in slProperties)
            {
                StringBuilder lptstrValue = new StringBuilder(0x200);
                if (aaOApi_GetInstanceAttrStrValue(lpAAOdsInstance, pair.Value, 0, lptstrValue, lptstrValue.Capacity) && !list.ContainsKey(pair.Key))
                {
                    list.Add(pair.Key, lptstrValue.ToString());
                }
            }
            aaOApi_FreeInstance(lpAAOdsInstance);
        }
        return list;
    }

    public static int GetInterfaceId(string sInterfaceName)
    {
        if (!string.IsNullOrEmpty(sInterfaceName))
        {
            for (int i = 0; i < aaApi_SelectAllGuis(); i++)
            {
                if (aaApi_GetGuiStringProperty(GuiProperty.Name, i).ToLower() == sInterfaceName.ToLower())
                {
                    return aaApi_GetGuiId(i);
                }
            }
        }
        return 0;
    }

    public static List<PWColumn> GetListOfColumnsByTableId(int iTableId)
    {
        int num = aaApi_SelectColumnsByTable(iTableId);
        List<PWColumn> list = new List<PWColumn>();
        for (int i = 0; i < num; i++)
        {
            PWColumn item = new PWColumn
            {
                ColumnId = aaApi_GetColumnNumericProperty(ColumnProperty.ColumnID, i),
                Name = aaApi_GetColumnStringProperty(ColumnProperty.Name, i),
                Length = aaApi_GetColumnNumericProperty(ColumnProperty.Length, i),
                TypeName = GetColumnTypeName((SQLSelectDBColumnTypes)aaApi_GetColumnNumericProperty(ColumnProperty.SQLType, i), (SQLSelectPWTypes)aaApi_GetColumnNumericProperty(ColumnProperty.Type, i)),
                TableId = iTableId
            };
            list.Add(item);
        }
        return list;
    }

    public static SortedList<int, PWColumn> GetListOfColumnsByTableIdKeyedById(int iTableId)
    {
        int num = aaApi_SelectColumnsByTable(iTableId);
        SortedList<int, PWColumn> list = new SortedList<int, PWColumn>();
        for (int i = 0; i < num; i++)
        {
            PWColumn column = new PWColumn
            {
                ColumnId = aaApi_GetColumnNumericProperty(ColumnProperty.ColumnID, i),
                Name = aaApi_GetColumnStringProperty(ColumnProperty.Name, i),
                Length = aaApi_GetColumnNumericProperty(ColumnProperty.Length, i),
                TypeName = GetColumnTypeName((SQLSelectDBColumnTypes)aaApi_GetColumnNumericProperty(ColumnProperty.SQLType, i), (SQLSelectPWTypes)aaApi_GetColumnNumericProperty(ColumnProperty.Type, i)),
                TableId = iTableId
            };
            list.Add(column.ColumnId, column);
        }
        return list;
    }

    public static SortedList<string, PWColumn> GetListOfColumnsByTableIdKeyedByName(int iTableId)
    {
        int num = aaApi_SelectColumnsByTable(iTableId);
        SortedList<string, PWColumn> list = new SortedList<string, PWColumn>(StringComparer.InvariantCultureIgnoreCase);
        for (int i = 0; i < num; i++)
        {
            PWColumn column = new PWColumn
            {
                ColumnId = aaApi_GetColumnNumericProperty(ColumnProperty.ColumnID, i),
                Name = aaApi_GetColumnStringProperty(ColumnProperty.Name, i),
                Length = aaApi_GetColumnNumericProperty(ColumnProperty.Length, i),
                TypeName = GetColumnTypeName((SQLSelectDBColumnTypes)aaApi_GetColumnNumericProperty(ColumnProperty.SQLType, i), (SQLSelectPWTypes)aaApi_GetColumnNumericProperty(ColumnProperty.Type, i)),
                TableId = iTableId
            };
            list.Add(column.Name, column);
        }
        return list;
    }

    public static string GetMonikerStringFromDocumentGuid(Guid docGuid)
    {
        StringBuilder builder = new StringBuilder();
        try
        {
            IntPtr zero = IntPtr.Zero;
            if (aaApi_BuildMonikerStringByDocGuid(aaApi_GetActiveDatasource(), ref docGuid, ref zero))
            {
                string str = Marshal.PtrToStringUni(zero);
                builder.Append(str);
                aaApi_Free(zero);
            }
        }
        catch (Exception exception)
        {
            BPSUtilities.WriteLog("Error: {0}\n{1}", new object[] { exception.Message, exception.StackTrace });
        }
        return builder.ToString();
    }

    public static string GetMonikerStringFromDocumentGuidString(string sDocGuid)
    {
        try
        {
            Guid docGuid = new Guid(sDocGuid);
            return GetMonikerStringFromDocumentGuid(docGuid);
        }
        catch (Exception exception)
        {
            BPSUtilities.WriteLog("Error: {0}\n{1}", new object[] { exception.Message, exception.StackTrace });
        }
        return string.Empty;
    }

    public static string GetMonikerStringFromDocumentIds(int iProjectId, int iDocumentId)
    {
        StringBuilder builder = new StringBuilder();
        string guidStringFromIds = GetGuidStringFromIds(iProjectId, iDocumentId);
        try
        {
            Guid pDocGuid = new Guid(guidStringFromIds);
            IntPtr zero = IntPtr.Zero;
            if (aaApi_BuildMonikerStringByDocGuid(aaApi_GetActiveDatasource(), ref pDocGuid, ref zero))
            {
                string str2 = Marshal.PtrToStringUni(zero);
                builder.Append(str2);
                aaApi_Free(zero);
            }
        }
        catch (Exception exception)
        {
            BPSUtilities.WriteLog("Error: {0}\n{1}", new object[] { exception.Message, exception.StackTrace });
        }
        return builder.ToString();
    }

    public static string GetMonikerStringFromProjectGuid(Guid projGuid)
    {
        StringBuilder builder = new StringBuilder();
        try
        {
            IntPtr zero = IntPtr.Zero;
            if (aaApi_BuildMonikerStringByProjectGuid(aaApi_GetActiveDatasource(), ref projGuid, ref zero))
            {
                string str = Marshal.PtrToStringUni(zero);
                builder.Append(str);
                aaApi_Free(zero);
            }
        }
        catch (Exception exception)
        {
            BPSUtilities.WriteLog("Error: {0}\n{1}", new object[] { exception.Message, exception.StackTrace });
        }
        return builder.ToString();
    }

    public static string GetMonikerStringFromProjectGuidString(string sProjGuid)
    {
        try
        {
            Guid projGuid = new Guid(sProjGuid);
            return GetMonikerStringFromProjectGuid(projGuid);
        }
        catch (Exception exception)
        {
            BPSUtilities.WriteLog("Error: {0}\n{1}", new object[] { exception.Message, exception.StackTrace });
        }
        return string.Empty;
    }

    public static string GetMonikerStringFromProjectId(int iProjectId)
    {
        StringBuilder builder = new StringBuilder();
        string projectGuidStringFromId = GetProjectGuidStringFromId(iProjectId);
        try
        {
            Guid pProjectGuid = new Guid(projectGuidStringFromId);
            IntPtr zero = IntPtr.Zero;
            if (aaApi_BuildMonikerStringByProjectGuid(aaApi_GetActiveDatasource(), ref pProjectGuid, ref zero))
            {
                string str2 = Marshal.PtrToStringUni(zero);
                builder.Append(str2);
                aaApi_Free(zero);
            }
        }
        catch (Exception exception)
        {
            BPSUtilities.WriteLog("Error: {0}\n{1}", new object[] { exception.Message, exception.StackTrace });
        }
        return builder.ToString();
    }

    public static int GetNonStandardUserNumericSettingByUser(int iUserId, int iParamNo)
    {
        string sqlStatement = string.Format("select o_intval from v_dms_ucfg where o_userno = {0} and o_paramno = {1}", iUserId, iParamNo);
        int numColumnsSelected = 0;
        if (0 < aaApi_SqlSelect(sqlStatement, IntPtr.Zero, ref numColumnsSelected))
        {
            return int.Parse(aaApi_SqlSelectGetData(0, 0));
        }
        return -1;
    }

    public static string GetNonStandardUserStringSettingByUser(int iUserId, int iParamNo)
    {
        StringBuilder builder = new StringBuilder(280);
        string sqlStatement = string.Format("select o_textval from v_dms_ucfg where o_userno = {0} and o_paramno = {1}", iUserId, iParamNo);
        int numColumnsSelected = 0;
        if (0 < aaApi_SqlSelect(sqlStatement, IntPtr.Zero, ref numColumnsSelected))
        {
            string str2 = aaApi_SqlSelectGetData(0, 0);
            builder.Append(str2);
            return builder.ToString();
        }
        return string.Empty;
    }

    public static string GetProjectGuidStringFromId(int iProjectId)
    {
        Guid[] docGuids = new Guid[1];
        if (aaApi_GetProjectGUIDsByIds(1, ref iProjectId, docGuids))
        {
            return docGuids[0].ToString();
        }
        return string.Empty;
    }

    public static int GetProjectIdFromMonikerString(string sMoniker)
    {
        IntPtr[] pMonikers = new IntPtr[1];
        string[] sArrayMonikers = new string[] { sMoniker };
        int num = 0;
        try
        {
            if (!aaApi_StringsToMonikers(1, pMonikers, sArrayMonikers, 8))
            {
                return num;
            }
            byte[] destination = default(Guid).ToByteArray();
            Marshal.Copy(aaApi_GetProjectGuidFromMoniker(pMonikers[0]), destination, 0, destination.Length);
            Guid projGuid = new Guid(destination);
            IntPtr hDataBuffer = aaApi_GUIDSelectProjectDataBuffer(ref projGuid);
            if (!(hDataBuffer != IntPtr.Zero))
            {
                return num;
            }
            if (1 == aaApi_DmsDataBufferGetCount(hDataBuffer))
            {
                num = aaApi_DmsDataBufferGetNumericProperty(hDataBuffer, 1, 0);
            }
            aaApi_DmsDataBufferFree(hDataBuffer);
        }
        catch (Exception exception)
        {
            BPSUtilities.WriteLog("Error: {0}\n{1}", new object[] { exception.Message, exception.StackTrace });
        }
        return num;
    }

    public static string GetProjectNamePath(int iProjectId)
    {
        StringBuilder stringBuffer = new StringBuilder(0x13e8);
        if (aaApi_GetProjectNamePath(iProjectId, false, '\\', stringBuffer, stringBuffer.Capacity))
        {
            return stringBuffer.ToString();
        }
        return string.Empty;
    }

    public static int GetProjectNoFromPath(string pwPath)
    {
        return ProjectNoFromPath(pwPath);
    }

    public static int GetProjectNoFromPathNoCase(string pwPath)
    {
        return ProjectNoFromPath(pwPath, true);
    }

    public static Hashtable GetProjectProperties(int iProjectId)
    {
        Hashtable hashtable = new Hashtable();
        IntPtr hDataBuffer = aaApi_SelectRichProjectOfFolder(iProjectId);
        if (hDataBuffer != IntPtr.Zero)
        {
            if (1 == aaApi_DmsDataBufferGetCount(hDataBuffer))
            {
                int lClassId = aaApi_DmsDataBufferGetNumericProperty(hDataBuffer, 0x1c, 0);
                int lInstId = aaApi_DmsDataBufferGetNumericProperty(hDataBuffer, 0x1f, 0);
                IntPtr pInst = aaOApi_LoadInstanceByIds(lClassId, lInstId, 0);
                if (pInst != IntPtr.Zero)
                {
                    int lNumAttrs = 0;
                    if (aaOApi_GetInstanceNumAttrs(pInst, 0, false, ref lNumAttrs))
                    {
                        for (int i = 0; i < lNumAttrs; i++)
                        {
                            int lplAttrId = 0;
                            int lplAttrType = 0;
                            int lplAddonId = 0;
                            int lplParentId = 0;
                            int lplVisibility = 0;
                            if (aaOApi_GetInstanceAttrId(pInst, i, 0, false, ref lplAttrId, ref lplAttrType, ref lplAddonId, ref lplParentId, ref lplVisibility))
                            {
                                IntPtr lpAttr = aaOApi_FindAttributePtr(lplAttrId);
                                StringBuilder lptstrReturnBuffer = new StringBuilder(0x200);
                                if (aaOApi_GetAttributeStringProperty(lpAttr, ODSAttributeProperty.Name, lptstrReturnBuffer, lptstrReturnBuffer.Capacity))
                                {
                                    StringBuilder lptstrValue = new StringBuilder(0x200);
                                    if (aaOApi_GetInstanceAttrStrValue(pInst, lplAttrId, 0, lptstrValue, lptstrValue.Capacity))
                                    {
                                        hashtable.Add(lptstrReturnBuffer.ToString(), lptstrValue.ToString());
                                    }
                                }
                            }
                        }
                    }
                    aaOApi_FreeInstance(pInst);
                }
            }
            aaApi_DmsDataBufferFree(hDataBuffer);
        }
        return hashtable;
    }

    public static SortedList<string, int> GetProjectPropertyIdsInList(int iProjectId)
    {
        SortedList<string, int> list = new SortedList<string, int>(StringComparer.InvariantCultureIgnoreCase);
        IntPtr hDataBuffer = aaApi_SelectRichProjectOfFolder(iProjectId);
        if (hDataBuffer != IntPtr.Zero)
        {
            if (1 == aaApi_DmsDataBufferGetCount(hDataBuffer))
            {
                int lClassId = aaApi_DmsDataBufferGetNumericProperty(hDataBuffer, 0x1c, 0);
                int lInstId = aaApi_DmsDataBufferGetNumericProperty(hDataBuffer, 0x1f, 0);
                IntPtr pInst = aaOApi_LoadInstanceByIds(lClassId, lInstId, 0);
                if (pInst != IntPtr.Zero)
                {
                    int lNumAttrs = 0;
                    if (aaOApi_GetInstanceNumAttrs(pInst, 0, false, ref lNumAttrs))
                    {
                        for (int i = 0; i < lNumAttrs; i++)
                        {
                            int lplAttrId = 0;
                            int lplAttrType = 0;
                            int lplAddonId = 0;
                            int lplParentId = 0;
                            int lplVisibility = 0;
                            if (aaOApi_GetInstanceAttrId(pInst, i, 0, false, ref lplAttrId, ref lplAttrType, ref lplAddonId, ref lplParentId, ref lplVisibility))
                            {
                                IntPtr lpAttr = aaOApi_FindAttributePtr(lplAttrId);
                                StringBuilder lptstrReturnBuffer = new StringBuilder(0x200);
                                if (aaOApi_GetAttributeStringProperty(lpAttr, ODSAttributeProperty.Name, lptstrReturnBuffer, lptstrReturnBuffer.Capacity) && !list.ContainsKey(lptstrReturnBuffer.ToString()))
                                {
                                    list.Add(lptstrReturnBuffer.ToString(), lplAttrId);
                                }
                            }
                        }
                    }
                    aaOApi_FreeInstance(pInst);
                }
            }
            aaApi_DmsDataBufferFree(hDataBuffer);
        }
        return list;
    }

    public static SortedList<string, string> GetProjectPropertyValuesInList(int iProjectId)
    {
        SortedList<string, string> instancePropertyValuesInList = new SortedList<string, string>(StringComparer.InvariantCultureIgnoreCase);
        IntPtr hDataBuffer = aaApi_SelectRichProjectOfFolder(iProjectId);
        if (hDataBuffer != IntPtr.Zero)
        {
            if (1 == aaApi_DmsDataBufferGetCount(hDataBuffer))
            {
                int iClassId = aaApi_DmsDataBufferGetNumericProperty(hDataBuffer, 0x1c, 0);
                int iInstanceId = aaApi_DmsDataBufferGetNumericProperty(hDataBuffer, 0x1f, 0);
                instancePropertyValuesInList = GetInstancePropertyValuesInList(iClassId, iInstanceId);
            }
            aaApi_DmsDataBufferFree(hDataBuffer);
        }
        return instancePropertyValuesInList;
    }

    public static string GetProjectURL(int iProjectId)
    {
        Guid[] docGuids = new Guid[1];
        if (aaApi_GetProjectGUIDsByIds(1, ref iProjectId, docGuids))
        {
            StringBuilder lptstrName = new StringBuilder(0x400);
            if (aaApi_GetActiveDatasourceName(lptstrName, 0x400))
            {
                return string.Format("pw://{0}/Documents/P{1}/", lptstrName.ToString(), "{" + docGuids[0].ToString() + "}");
            }
        }
        return string.Empty;
    }

    public static string GetProjectWebLink(string sWebAddressIncludingASPXName, int iProjectId)
    {
        StringBuilder lptstrName = new StringBuilder(0x400);
        if (aaApi_GetActiveDatasourceName(lptstrName, 0x400))
        {
            return GetFormattedWebLink(sWebAddressIncludingASPXName, lptstrName.ToString(), GetProjectNamePath(iProjectId) + "//", WebLinkActions.None);
        }
        return string.Empty;
    }

    public static string GetProjectWisePath()
    {
        string str = null;
        try
        {
            Version version = new Version("08.01");
            string[] strArray = new string[] { @"SOFTWARE\Wow6432Node\Bentley\ProjectWise Explorer", @"SOFTWARE\Wow6432Node\Bentley\ProjectWise Administrator", @"SOFTWARE\Bentley\ProjectWise Explorer", @"SOFTWARE\Bentley\ProjectWise Administrator" };
            foreach (string str2 in strArray)
            {
                RegistryKey key2 = Registry.LocalMachine.OpenSubKey(str2);
                if (key2 != null)
                {
                    string[] subKeyNames = key2.GetSubKeyNames();
                    if (subKeyNames != null)
                    {
                        for (int i = 0; i < subKeyNames.Length; i++)
                        {
                            RegistryKey key3 = key2.OpenSubKey(subKeyNames[i]);
                            if (key3 != null)
                            {
                                string str3 = (string)key3.GetValue("Version");
                                if (str3 != null)
                                {
                                    Version version2 = new Version(str3);
                                    if (version2 >= version)
                                    {
                                        str = (string)key3.GetValue("PathName");
                                        version = version2;
                                    }
                                }
                            }
                        }
                        if (str != null)
                        {
                            break;
                        }
                    }
                }
            }
            if (str == null)
            {
                throw new ApplicationException("Registry search could not find installation directory for a ProjectWise version matching minimum required version '" + version + "'.\nMake sure a ProjectWise version matching the above version is installed on this system.");
            }
        }
        catch (Exception)
        {
        }
        return str;
    }

    public static int GetPWNumericSetting(int iSetting)
    {
        IntPtr hDataBuffer = aaApi_SqlSelectDataBuffer(string.Format("select o_intval from dms_gcfg where o_compguid = '4493b532-0cc3-45ee-be8c-1de7b9a7bad4' and o_paramno = {0}", iSetting), IntPtr.Zero);
        string s = aaApi_SqlSelectDataBufGetValue(hDataBuffer, 0, 0);
        aaApi_DmsDataBufferFree(hDataBuffer);
        int result = 0;
        int.TryParse(s, out result);
        return result;
    }

    public static string GetPWStringSetting(int iSetting)
    {
        IntPtr hDataBuffer = aaApi_SqlSelectDataBuffer(string.Format("select o_textval from dms_gcfg where o_compguid = '4493b532-0cc3-45ee-be8c-1de7b9a7bad4' and o_paramno = {0}", iSetting), IntPtr.Zero);
        string str2 = aaApi_SqlSelectDataBufGetValue(hDataBuffer, 0, 0);
        aaApi_DmsDataBufferFree(hDataBuffer);
        return str2;
    }

    public static int GetRichProjectId(int iProjectId)
    {
        int num = 0;
        IntPtr hDataBuffer = aaApi_SelectRichProjectOfFolder(iProjectId);
        if (hDataBuffer != IntPtr.Zero)
        {
            if (1 == aaApi_DmsDataBufferGetCount(hDataBuffer))
            {
                num = aaApi_DmsDataBufferGetNumericProperty(hDataBuffer, 1, 0);
            }
            aaApi_DmsDataBufferFree(hDataBuffer);
        }
        return num;
    }

    public static int GetStateId(string sStateName)
    {
        for (int i = 0; i < aaApi_SelectAllStates(); i++)
        {
            if (aaApi_GetStateStringProperty(StateProperty.Name, i).ToLower() == sStateName.ToLower())
            {
                return aaApi_GetStateId(i);
            }
        }
        return 0;
    }

    public static string GetStateName(int iStateId)
    {
        if (1 == aaApi_SelectState(iStateId))
        {
            return aaApi_GetStateStringProperty(StateProperty.Name, 0);
        }
        return string.Empty;
    }

    public static int GetStorageAreaId(string sStorageAreaName)
    {
        int num = aaApi_SelectAllStorages();
        for (int i = 0; i < num; i++)
        {
            string str = aaApi_GetStorageStringProperty(StorageProperty.Name, i);
            if (sStorageAreaName.ToLower() == str.ToLower())
            {
                return aaApi_GetStorageId(i);
            }
        }
        return 0;
    }

    public static string GetUniqueDocumentName(int iProjectId, string sDocumentName)
    {
        StringBuilder lptstrDocName = new StringBuilder(0x7f);
        if (aaApi_DocumentGenNameWithPrefix(iProjectId, sDocumentName, lptstrDocName, lptstrDocName.Capacity))
        {
            return lptstrDocName.ToString();
        }
        return sDocumentName;
    }

    public static string GetUniqueFileName(int iProjectId, string sFileName)
    {
        StringBuilder lptstrDocName = new StringBuilder(0x7f);
        if (aaApi_DocumentGenFileNameWithPrefix(iProjectId, sFileName, lptstrDocName, lptstrDocName.Capacity))
        {
            return lptstrDocName.ToString();
        }
        return sFileName;
    }

    public static string GetURLEncodedDocumentMoniker(int iProjectId, int iDocumentId)
    {
        return HttpUtility.UrlEncode(GetMonikerStringFromDocumentIds(iProjectId, iDocumentId));
    }

    public static SortedList<string, ProjectWiseUserList> GetUserListsByName()
    {
        SortedList<string, ProjectWiseUserList> list = new SortedList<string, ProjectWiseUserList>(StringComparer.InvariantCultureIgnoreCase);
        int num = aaApi_SelectAllUserLists();
        for (int i = 0; i < num; i++)
        {
            string key = aaApi_GetUserListStringProperty(UserListProperty.Name, i);
            int iID = aaApi_GetUserListNumericProperty(UserListProperty.ID, i);
            if (!list.ContainsKey(key))
            {
                list.Add(key, new ProjectWiseUserList(iID, key, aaApi_GetUserListStringProperty(UserListProperty.Description, i), aaApi_GetUserListNumericProperty(UserListProperty.Owner, i), aaApi_GetUserListNumericProperty(UserListProperty.Type, i)));
            }
        }
        return list;
    }

    public static SortedList<string, ProjectWiseUser> GetUsersByDomainAndUsername()
    {
        SortedList<string, ProjectWiseUser> list = new SortedList<string, ProjectWiseUser>(StringComparer.InvariantCultureIgnoreCase);
        int num = aaApi_SelectAllUsers();
        for (int i = 0; i < num; i++)
        {
            int iID = aaApi_GetUserNumericProperty(UserProperty.ID, i);
            bool bDisabled = 1 == aaApi_GetUserNumericProperty(UserProperty.Flags, i);
            string str = aaApi_GetUserStringProperty(UserProperty.SecProvider, i);
            string str2 = aaApi_GetUserStringProperty(UserProperty.Name, i);
            string key = string.Empty;
            if (string.IsNullOrEmpty(str))
            {
                key = str2;
            }
            else
            {
                key = string.Format(@"{0}\{1}", str, str2);
            }
            if (!list.ContainsKey(key))
            {
                list.Add(key, new ProjectWiseUser(iID, str2, aaApi_GetUserStringProperty(UserProperty.Desc, i), aaApi_GetUserStringProperty(UserProperty.SecProvider, i), aaApi_GetUserStringProperty(UserProperty.Email, i), aaApi_GetUserStringProperty(UserProperty.Type, i), bDisabled));
            }
        }
        return list;
    }

    public static SortedList<int, ProjectWiseUser> GetUsersById()
    {
        SortedList<int, ProjectWiseUser> list = new SortedList<int, ProjectWiseUser>();
        int num = aaApi_SelectAllUsers();
        for (int i = 0; i < num; i++)
        {
            string sName = aaApi_GetUserStringProperty(UserProperty.Name, i);
            int key = aaApi_GetUserNumericProperty(UserProperty.ID, i);
            bool bDisabled = 1 == aaApi_GetUserNumericProperty(UserProperty.Flags, i);
            if (!list.ContainsKey(key))
            {
                list.Add(key, new ProjectWiseUser(key, sName, aaApi_GetUserStringProperty(UserProperty.Desc, i), aaApi_GetUserStringProperty(UserProperty.SecProvider, i), aaApi_GetUserStringProperty(UserProperty.Email, i), aaApi_GetUserStringProperty(UserProperty.Type, i), bDisabled));
            }
        }
        return list;
    }

    public static SortedList<string, ProjectWiseUser> GetUsersByName()
    {
        SortedList<string, ProjectWiseUser> list = new SortedList<string, ProjectWiseUser>(StringComparer.InvariantCultureIgnoreCase);
        int num = aaApi_SelectAllUsers();
        for (int i = 0; i < num; i++)
        {
            string key = aaApi_GetUserStringProperty(UserProperty.Name, i);
            int iID = aaApi_GetUserNumericProperty(UserProperty.ID, i);
            bool bDisabled = 1 == aaApi_GetUserNumericProperty(UserProperty.Flags, i);
            if (!list.ContainsKey(key))
            {
                list.Add(key, new ProjectWiseUser(iID, key, aaApi_GetUserStringProperty(UserProperty.Desc, i), aaApi_GetUserStringProperty(UserProperty.SecProvider, i), aaApi_GetUserStringProperty(UserProperty.Email, i), aaApi_GetUserStringProperty(UserProperty.Type, i), bDisabled));
            }
        }
        return list;
    }

    public static int GetWorkflowId(string sWorkflowName)
    {
        if (!string.IsNullOrEmpty(sWorkflowName))
        {
            for (int i = 0; i < aaApi_SelectAllWorkflows(); i++)
            {
                if (aaApi_GetWorkflowStringProperty(WorkflowProperty.Name, i).ToLower() == sWorkflowName.ToLower())
                {
                    return aaApi_GetWorkflowId(i);
                }
            }
        }
        return 0;
    }

    public static string GetWorkflowName(int iWorkflowId)
    {
        if (1 == aaApi_SelectWorkflow(iWorkflowId))
        {
            return aaApi_GetWorkflowStringProperty(WorkflowProperty.Name, 0);
        }
        return string.Empty;
    }

    public static Hashtable GetWorkflows()
    {
        Hashtable hashtable = new Hashtable();
        for (int i = 0; i < aaApi_SelectAllWorkflows(); i++)
        {
            string str = aaApi_GetWorkflowStringProperty(WorkflowProperty.Name, i);
            int num2 = aaApi_GetWorkflowId(i);
            hashtable.Add(str.ToLower(), num2);
        }
        return hashtable;
    }

    public static Hashtable GetWorkflowStates(string sWorkflowName)
    {
        Hashtable hashtable = new Hashtable();
        if (!string.IsNullOrEmpty(sWorkflowName))
        {
            int workflowId = GetWorkflowId(sWorkflowName);
            if (workflowId > 0)
            {
                int num2 = aaApi_SelectStatesByWorkflow(workflowId);
                for (int j = 0; j < num2; j++)
                {
                    string str = aaApi_GetStateStringProperty(StateProperty.Name, j);
                    int num4 = aaApi_GetStateId(j);
                    hashtable.Add(str.ToLower(), num4);
                }
            }
            return hashtable;
        }
        int num5 = aaApi_SelectAllStates();
        for (int i = 0; i < num5; i++)
        {
            string str2 = aaApi_GetStateStringProperty(StateProperty.Name, i);
            int num7 = aaApi_GetStateId(i);
            hashtable.Add(str2.ToLower(), num7);
        }
        return hashtable;
    }

    public static bool mcmMain_GetDocumentIdByFilePath(string sFileName, int iValidateWithChkl, ref int iProjectNo, ref int iDocumentNo)
    {
        bool flag = false;
        Guid pDocGuid = default(Guid);
        if (aaApi_ChklSetGetDocGuidFromFileName(sFileName, ref pDocGuid) && (1 == aaApi_GUIDSelectDocument(ref pDocGuid)))
        {
            flag = true;
            iProjectNo = aaApi_GetDocumentNumericProperty(DocumentProperty.ProjectID, 0);
            iDocumentNo = aaApi_GetDocumentNumericProperty(DocumentProperty.ID, 0);
        }
        return flag;
    }

    public static int ProjectNoFromPath(string pwPath)
    {
        bool bDoCaselessCompare = true;
        if (((aaApi_GetActiveDatasourceNativeType() == 5) || (aaApi_GetActiveDatasourceNativeType() == 20)) || (aaApi_GetActiveDatasourceNativeType() == 1))
        {
            bDoCaselessCompare = false;
        }
        return ProjectNoFromPath(pwPath, bDoCaselessCompare);
    }

    private static int ProjectNoFromPath(string pwPath, bool bDoCaselessCompare)
    {
        char[] separator = new char[] { '\\' };
        string[] strArray = pwPath.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        int iParentId = -1;
        int num2 = 0;
        if (strArray.Length == 0)
        {
            return 0;
        }
        for (int i = 0; i < strArray.Length; i++)
        {
            if (string.IsNullOrEmpty(strArray[i]))
            {
                continue;
            }
            string str = strArray[i].Trim(" ".ToCharArray());
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > 0x3f)
                {
                    str = str.Substring(0, 0x3f);
                }
                int num4 = -1;
                if (-1 == iParentId)
                {
                    num4 = aaApi_SelectTopLevelProjects();
                }
                else
                {
                    num4 = aaApi_SelectChildProjects(iParentId);
                }
                if (num4 == -1)
                {
                    return 0;
                }
                bool flag = false;
                for (int j = 0; j < num4; j++)
                {
                    string str2 = aaApi_GetProjectStringProperty(ProjectProperty.Name, j);
                    if ((bDoCaselessCompare && (str2.ToLower() == str.ToLower())) || (str2 == str))
                    {
                        num2 = aaApi_GetProjectNumericProperty(ProjectProperty.ID, j);
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    return 0;
                }
                iParentId = num2;
            }
        }
        return num2;
    }

    public static void SetAttributesToRichProjectProperties(int iProjectId)
    {
        SortedList<string, string> projectPropertyValuesInList = GetProjectPropertyValuesInList(iProjectId);
        if (projectPropertyValuesInList.Count > 0)
        {
            int lplEnvironmentId = 0;
            int lplTableId = 0;
            int lplIdColumnId = 0;
            if (aaApi_GetEnvTableInfoByProject(iProjectId, ref lplEnvironmentId, ref lplTableId, ref lplIdColumnId))
            {
                int num4 = aaApi_SelectEnvAttrDefs(lplEnvironmentId, lplTableId, -1);
                SortedList<int, string> slAttrVals = new SortedList<int, string>();
                for (int i = 0; i < num4; i++)
                {
                    int key = aaApi_GetEnvAttrDefNumericProperty(AttributeDefinitionProperty.ColumnID, i);
                    string str = aaApi_GetEnvAttrDefStringProperty(AttributeDefinitionProperty.DefaultValue, i);
                    if (str.StartsWith("$PROJECT#"))
                    {
                        string str2 = str.Replace("$PROJECT#", "").Replace("$", "");
                        if (projectPropertyValuesInList.ContainsKey(str2) && !slAttrVals.ContainsKey(key))
                        {
                            slAttrVals.Add(key, projectPropertyValuesInList[str2]);
                        }
                    }
                }
                if (slAttrVals.Count > 0)
                {
                    foreach (int num7 in GetBranchProjectNos(iProjectId, true))
                    {
                        int num8 = aaApi_SelectDocumentsByProjectId(num7);
                        for (int j = 0; j < num8; j++)
                        {
                            SetAttributesValuesFromColumnIds(num7, aaApi_GetDocumentId(j), slAttrVals);
                        }
                    }
                }
            }
        }
    }

    public static bool SetAttributesValues(int iProjectNo, int iDocumentNo, Hashtable htAttrVals)
    {
        int lplEnvironmentId = 0;
        int lplTableId = 0;
        int lplIdColumnId = 0;
        if (aaApi_GetEnvTableInfoByProject(iProjectNo, ref lplEnvironmentId, ref lplTableId, ref lplIdColumnId))
        {
            int num4 = aaApi_SelectLinks(iProjectNo, iDocumentNo);
            if (num4 == 0)
            {
                aaApi_FreeLinkDataInsertDesc();
                bool flag = false;
                int num5 = aaApi_SelectColumnsByTable(lplTableId);
                Hashtable hashtable = new Hashtable();
                for (int i = 0; i < num5; i++)
                {
                    string str = aaApi_GetColumnStringProperty(ColumnProperty.Name, i);
                    int num7 = aaApi_GetColumnNumericProperty(ColumnProperty.ColumnID, i);
                    if (num7 != lplIdColumnId)
                    {
                        hashtable.Add(str.ToLower(), num7);
                    }
                }
                foreach (DictionaryEntry entry in htAttrVals)
                {
                    if (!string.IsNullOrEmpty(entry.Value.ToString()) && hashtable.ContainsKey(entry.Key.ToString()))
                    {
                        int columnID = (int)hashtable[entry.Key.ToString()];
                        string columnValue = entry.Value.ToString();
                        if ((columnID != lplIdColumnId) && aaApi_SetLinkDataColumnValue(lplTableId, columnID, columnValue))
                        {
                            flag = true;
                        }
                    }
                }
                if (flag)
                {
                    int lplColumnId = 0;
                    StringBuilder lptstrValueBuffer = new StringBuilder(30);
                    if (aaApi_CreateLinkDataAndLink(lplTableId, 1, iProjectNo, iDocumentNo, ref lplColumnId, lptstrValueBuffer, lptstrValueBuffer.Capacity))
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < num4; i++)
                {
                    string columnValue = aaApi_GetLinkStringProperty(LinkProperty.ColumnValue, i);
                    int lplColumnCount = 0;
                    aaApi_SelectLinkDataByObject(lplTableId, ObjectTypeForLinkData.Document, iProjectNo, iDocumentNo, null, ref lplColumnCount, null, 0);
                    aaApi_FreeLinkDataUpdateDesc();
                    bool flag2 = false;
                    for (int j = 0; j < lplColumnCount; j++)
                    {
                        string str4 = aaApi_GetLinkDataColumnStringProperty(LinkDataProperty.ColumnName, j);
                        if (htAttrVals.ContainsKey(str4.ToLower()))
                        {
                            try
                            {
                                string str5 = htAttrVals[str4.ToLower()].ToString();
                                int columnID = aaApi_GetLinkDataColumnNumericProperty(LinkDataProperty.ColumnID, j);
                                if ((columnID != lplIdColumnId) && aaApi_UpdateLinkDataColumnValue(lplTableId, columnID, str5))
                                {
                                    flag2 = true;
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    if (flag2 && aaApi_UpdateLinkData(lplTableId, lplIdColumnId, columnValue))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static bool SetAttributesValuesFromColumnIds(int iProjectNo, int iDocumentNo, SortedList<int, string> slAttrVals)
    {
        int lplEnvironmentId = 0;
        int lplTableId = 0;
        int lplIdColumnId = 0;
        if (aaApi_GetEnvTableInfoByProject(iProjectNo, ref lplEnvironmentId, ref lplTableId, ref lplIdColumnId))
        {
            int num4 = aaApi_SelectLinks(iProjectNo, iDocumentNo);
            if (num4 == 0)
            {
                aaApi_FreeLinkDataInsertDesc();
                bool flag = false;
                foreach (KeyValuePair<int, string> pair in slAttrVals)
                {
                    if ((pair.Key != lplIdColumnId) && aaApi_SetLinkDataColumnValue(lplTableId, pair.Key, pair.Value))
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    int lplColumnId = 0;
                    StringBuilder lptstrValueBuffer = new StringBuilder(30);
                    if (aaApi_CreateLinkDataAndLink(lplTableId, 1, iProjectNo, iDocumentNo, ref lplColumnId, lptstrValueBuffer, lptstrValueBuffer.Capacity))
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < num4; i++)
                {
                    string columnValue = aaApi_GetLinkStringProperty(LinkProperty.ColumnValue, i);
                    aaApi_FreeLinkDataUpdateDesc();
                    bool flag2 = false;
                    foreach (KeyValuePair<int, string> pair2 in slAttrVals)
                    {
                        if ((pair2.Key != lplIdColumnId) && aaApi_UpdateLinkDataColumnValue(lplTableId, pair2.Key, pair2.Value))
                        {
                            flag2 = true;
                        }
                    }
                    if (flag2 && aaApi_UpdateLinkData(lplTableId, lplIdColumnId, columnValue))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static bool SetAttributesValuesFromColumnIds(int iProjectNo, int iDocumentNo, Hashtable htAttrVals)
    {
        int lplEnvironmentId = 0;
        int lplTableId = 0;
        int lplIdColumnId = 0;
        if (aaApi_GetEnvTableInfoByProject(iProjectNo, ref lplEnvironmentId, ref lplTableId, ref lplIdColumnId))
        {
            int num4 = aaApi_SelectLinks(iProjectNo, iDocumentNo);
            if (num4 == 0)
            {
                aaApi_FreeLinkDataInsertDesc();
                bool flag = false;
                foreach (DictionaryEntry entry in htAttrVals)
                {
                    if (!string.IsNullOrEmpty(entry.Value.ToString()))
                    {
                        int key = (int)entry.Key;
                        string columnValue = entry.Value.ToString();
                        if ((key != lplIdColumnId) && aaApi_SetLinkDataColumnValue(lplTableId, key, columnValue))
                        {
                            flag = true;
                        }
                    }
                }
                if (flag)
                {
                    int lplColumnId = 0;
                    StringBuilder lptstrValueBuffer = new StringBuilder(30);
                    if (aaApi_CreateLinkDataAndLink(lplTableId, 1, iProjectNo, iDocumentNo, ref lplColumnId, lptstrValueBuffer, lptstrValueBuffer.Capacity))
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < num4; i++)
                {
                    string columnValue = aaApi_GetLinkStringProperty(LinkProperty.ColumnValue, i);
                    aaApi_FreeLinkDataUpdateDesc();
                    bool flag2 = false;
                    foreach (DictionaryEntry entry2 in htAttrVals)
                    {
                        if (!string.IsNullOrEmpty(entry2.Value.ToString()))
                        {
                            int key = (int)entry2.Key;
                            string str3 = entry2.Value.ToString();
                            if ((key != lplIdColumnId) && aaApi_UpdateLinkDataColumnValue(lplTableId, key, str3))
                            {
                                flag2 = true;
                            }
                        }
                    }
                    if (flag2 && aaApi_UpdateLinkData(lplTableId, lplIdColumnId, columnValue))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool SetAttributeValue(int iProjectNo, int iDocumentNo, string sColumnName, string sValue)
    {
        int lplEnvironmentId = 0;
        int lplTableId = 0;
        int lplIdColumnId = 0;
        if (aaApi_GetEnvTableInfoByProject(iProjectNo, ref lplEnvironmentId, ref lplTableId, ref lplIdColumnId))
        {
            int num4 = aaApi_SelectLinks(iProjectNo, iDocumentNo);
            if (num4 != 0)
            {
                for (int i = 0; i < num4; i++)
                {
                    string columnValue = aaApi_GetLinkStringProperty(LinkProperty.ColumnValue, i);
                    int lplColumnCount = 0;
                    aaApi_SelectLinkDataByObject(lplTableId, ObjectTypeForLinkData.Document, iProjectNo, iDocumentNo, null, ref lplColumnCount, null, 0);
                    aaApi_FreeLinkDataUpdateDesc();
                    bool flag2 = false;
                    for (int j = 0; j < lplColumnCount; j++)
                    {
                        string str3 = aaApi_GetLinkDataColumnStringProperty(LinkDataProperty.ColumnName, j);
                        if (sColumnName.ToLower() == str3.ToLower())
                        {
                            int columnID = aaApi_GetLinkDataColumnNumericProperty(LinkDataProperty.ColumnID, j);
                            int num13 = aaApi_GetLinkDataColumnNumericProperty(LinkDataProperty.ColumnLength, j);
                            if (sValue.Length > num13)
                            {
                                sValue = sValue.Substring(0, num13 - 1);
                            }
                            if ((columnID != lplIdColumnId) && aaApi_UpdateLinkDataColumnValue(lplTableId, columnID, sValue))
                            {
                                flag2 = true;
                                break;
                            }
                        }
                    }
                    if (flag2 && aaApi_UpdateLinkData(lplTableId, lplIdColumnId, columnValue))
                    {
                        return true;
                    }
                }
            }
            else
            {
                aaApi_FreeLinkDataInsertDesc();
                bool flag = false;
                int num5 = aaApi_SelectColumnsByTable(lplTableId);
                int columnID = 0;
                for (int i = 0; i < num5; i++)
                {
                    string str = aaApi_GetColumnStringProperty(ColumnProperty.Name, i);
                    if (str.ToLower() == str.ToLower())
                    {
                        columnID = aaApi_GetColumnNumericProperty(ColumnProperty.ColumnID, i);
                        break;
                    }
                }
                if (((columnID > 0) && (columnID != lplIdColumnId)) && aaApi_SetLinkDataColumnValue(lplTableId, columnID, sValue))
                {
                    flag = true;
                }
                if (flag)
                {
                    int lplColumnId = 0;
                    StringBuilder lptstrValueBuffer = new StringBuilder(30);
                    if (aaApi_CreateLinkDataAndLink(lplTableId, 1, iProjectNo, iDocumentNo, ref lplColumnId, lptstrValueBuffer, lptstrValueBuffer.Capacity))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    [DllImport("dmscli.dll", EntryPoint = "aaApi_SetCurrentSession2", CharSet = CharSet.Unicode)]
    public static extern bool SetCurrentSession2(long hSession);
    public static bool SetDocumentListView(string sViewName)
    {
        IntPtr hView = aaApi_ViewGetHandle(sViewName);
        return ((hView != IntPtr.Zero) && aaApi_DocListSetView(aaApi_GetActiveDocumentList(), hView));
    }

    public static bool SetDocumentListViewByProject(int iProjectId, string sViewName)
    {
        for (IntPtr ptr = aaApi_ViewGetFirstForProject(iProjectId); ptr != IntPtr.Zero; ptr = aaApi_ViewGetNext(ptr))
        {
            if (aaApi_ViewGetName(ptr).ToLower() == sViewName.ToLower())
            {
                return aaApi_DocListSetView(aaApi_GetActiveDocumentList(), ptr);
            }
        }
        return false;
    }

    [DllImport("KERNEL32.dll")]
    private static extern bool SetEnvironmentVariable(string name, string val);
    public static bool SetNonStandardUserNumericSettingByUser(int iUserId, int iParamNo, int iUserSetting)
    {
        aaApi_ExecuteSqlStatement("create view v_dms_ucfg as select * from dms_ucfg");
        aaApi_ExecuteSqlStatement(string.Format("delete from v_dms_ucfg where o_userno = {0} and o_paramno = {1}", iUserId, iParamNo));
        return aaApi_ExecuteSqlStatement(string.Format("insert into v_dms_ucfg (o_userno, o_paramno, o_intval, o_textval, o_compguid) values ({0},{1},{2},'','00000000-0000-0000-0000-000000000000')", iUserId, iParamNo, iUserSetting));
    }

    public static bool SetNonStandardUserStringSettingByUser(int iUserId, int iParamNo, string sUserSetting)
    {
        aaApi_ExecuteSqlStatement("create view v_dms_ucfg as select * from dms_ucfg");
        aaApi_ExecuteSqlStatement(string.Format("delete from v_dms_ucfg where o_userno = {0} and o_paramno = {1}", iUserId, iParamNo));
        return aaApi_ExecuteSqlStatement(string.Format("insert into v_dms_ucfg (o_userno, o_paramno, o_intval, o_textval, o_compguid) values ({0},{1},0,'{2}','00000000-0000-0000-0000-000000000000')", iUserId, iParamNo, sUserSetting));
    }

    public static bool SetPWNumericSetting(int iSetting, int iValue)
    {
        aaApi_ExecuteSqlStatement("create view v_dms_gcfg as select * from dms_gcfg");
        aaApi_ExecuteSqlStatement(string.Format("delete from v_dms_gcfg where o_compguid = '4493b532-0cc3-45ee-be8c-1de7b9a7bad4' and o_paramno = {0}", iSetting));
        return aaApi_ExecuteSqlStatement(string.Format("insert into v_dms_gcfg(o_compguid, o_paramno, o_intval) values ('4493b532-0cc3-45ee-be8c-1de7b9a7bad4', {0}, {1})", iSetting, iValue));
    }

    public static bool SetPWStringSetting(int iSetting, string sValue)
    {
        aaApi_ExecuteSqlStatement("create view v_dms_gcfg as select * from dms_gcfg");
        aaApi_ExecuteSqlStatement(string.Format("delete from v_dms_gcfg where o_compguid = '4493b532-0cc3-45ee-be8c-1de7b9a7bad4' and o_paramno = {0}", iSetting));
        return aaApi_ExecuteSqlStatement(string.Format("insert into v_dms_gcfg(o_compguid, o_paramno, o_intval, o_textval) values ('4493b532-0cc3-45ee-be8c-1de7b9a7bad4', {0}, 0, '{1}')", iSetting, sValue));
    }

    public static bool SetRichProjectProperties(int iProjectId, SortedList<string, string> slPropertyNamesPropertyValues)
    {
        bool flag = false;
        IntPtr hDataBuffer = aaApi_SelectRichProjectOfFolder(iProjectId);
        if (hDataBuffer != IntPtr.Zero)
        {
            if (1 == aaApi_DmsDataBufferGetCount(hDataBuffer))
            {
                int lClassId = aaApi_DmsDataBufferGetNumericProperty(hDataBuffer, 0x1c, 0);
                int lInstId = aaApi_DmsDataBufferGetNumericProperty(hDataBuffer, 0x1f, 0);
                IntPtr lpAAOdsInstance = aaOApi_LoadInstanceByIds(lClassId, lInstId, 0);
                if (lpAAOdsInstance != IntPtr.Zero)
                {
                    SortedList<string, int> projectPropertyIdsInList = GetProjectPropertyIdsInList(iProjectId);
                    foreach (string str in slPropertyNamesPropertyValues.Keys)
                    {
                        string lpctstrValue = slPropertyNamesPropertyValues[str];
                        int lAttrId = 0;
                        if (!projectPropertyIdsInList.ContainsKey(str))
                        {
                            string key = str.ToLower().Replace(" ", "_");
                            if (!projectPropertyIdsInList.ContainsKey(key))
                            {
                                key = string.Format("PROJECT_{0}", str.ToLower()).Replace(" ", "_");
                                if (projectPropertyIdsInList.ContainsKey(key))
                                {
                                    lAttrId = projectPropertyIdsInList[key];
                                }
                            }
                            else
                            {
                                lAttrId = projectPropertyIdsInList[key];
                            }
                        }
                        else
                        {
                            lAttrId = projectPropertyIdsInList[str.ToLower()];
                        }
                        if (lAttrId > 0)
                        {
                            aaOApi_SetInstanceAttrStrValueExt(lpAAOdsInstance, lpctstrValue, lAttrId, 0, true);
                        }
                    }
                    if (aaOApi_SaveInstance(lpAAOdsInstance))
                    {
                        flag = true;
                    }
                }
            }
            aaApi_DmsDataBufferFree(hDataBuffer);
        }
        return flag;
    }

    public static bool SetRichProjectPropertiesByIds(int iProjectId, SortedList<int, string> slPropertyIdsPropertyValues)
    {
        bool flag = false;
        IntPtr hDataBuffer = aaApi_SelectRichProjectOfFolder(iProjectId);
        if (hDataBuffer != IntPtr.Zero)
        {
            if (1 == aaApi_DmsDataBufferGetCount(hDataBuffer))
            {
                int lClassId = aaApi_DmsDataBufferGetNumericProperty(hDataBuffer, 0x1c, 0);
                int lInstId = aaApi_DmsDataBufferGetNumericProperty(hDataBuffer, 0x1f, 0);
                IntPtr lpAAOdsInstance = aaOApi_LoadInstanceByIds(lClassId, lInstId, 0);
                if (lpAAOdsInstance != IntPtr.Zero)
                {
                    foreach (int num3 in slPropertyIdsPropertyValues.Keys)
                    {
                        if (num3 > 0)
                        {
                            string lpctstrValue = slPropertyIdsPropertyValues[num3];
                            aaOApi_SetInstanceAttrStrValueExt(lpAAOdsInstance, lpctstrValue, num3, 0, true);
                        }
                    }
                    if (aaOApi_SaveInstance(lpAAOdsInstance))
                    {
                        flag = true;
                    }
                }
            }
            aaApi_DmsDataBufferFree(hDataBuffer);
        }
        return flag;
    }

    public static bool SetRichProjectProperty(int iProjectId, string sAttributeName, string sAttributeValue)
    {
        bool flag = false;
        IntPtr hDataBuffer = aaApi_SelectRichProjectOfFolder(iProjectId);
        if (hDataBuffer != IntPtr.Zero)
        {
            if (1 == aaApi_DmsDataBufferGetCount(hDataBuffer))
            {
                int lClassId = aaApi_DmsDataBufferGetNumericProperty(hDataBuffer, 0x1c, 0);
                int lInstId = aaApi_DmsDataBufferGetNumericProperty(hDataBuffer, 0x1f, 0);
                IntPtr lpAAOdsInstance = aaOApi_LoadInstanceByIds(lClassId, lInstId, 0);
                SortedList<string, int> classPropertyIdsInList = GetClassPropertyIdsInList(lClassId);
                if (lpAAOdsInstance != IntPtr.Zero)
                {
                    int lAttrId = 0;
                    if (!classPropertyIdsInList.ContainsKey(sAttributeName))
                    {
                        string key = sAttributeName.ToLower().Replace(" ", "_");
                        if (!classPropertyIdsInList.ContainsKey(key))
                        {
                            key = string.Format("PROJECT_{0}", sAttributeName.ToLower()).Replace(" ", "_");
                            if (classPropertyIdsInList.ContainsKey(key))
                            {
                                lAttrId = classPropertyIdsInList[key];
                            }
                        }
                        else
                        {
                            lAttrId = classPropertyIdsInList[key];
                        }
                    }
                    else
                    {
                        lAttrId = classPropertyIdsInList[sAttributeName.ToLower()];
                    }
                    if (lAttrId > 0)
                    {
                        aaOApi_SetInstanceAttrStrValueExt(lpAAOdsInstance, sAttributeValue, lAttrId, 0, true);
                        if (aaOApi_SaveInstance(lpAAOdsInstance))
                        {
                            flag = true;
                        }
                    }
                }
            }
            aaApi_DmsDataBufferFree(hDataBuffer);
        }
        return flag;
    }

    [DllImport("dmscli.dll", EntryPoint = "aaApi_DmsDataBufferGetGuidProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_DmsDataBufferGetGuidProperty(IntPtr hDataBuffer, int lPropertyId, int lIdxRow);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_DmsDataBufferGetStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_DmsDataBufferGetStringProperty(IntPtr hDataBuffer, int lPropertyId, int lIdxRow);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_DmsThreadBufferGetStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_DmsThreadBufferGetStringProperty(int lBufferId, int lPropertyId, int lIdxRow);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetApplicationStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetApplicationStringProperty(ApplicationProperty lPropertyId, int lIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetAuditTrailActionTypeName", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetAuditTrailActionTypeName(AuditTrailActions iActionTypeId, ref int pObjectTypeId);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetColumnStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetColumnStringProperty(ColumnProperty PropertyId, int lIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetCustomHierarchyStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetCustomHierarchyStringProperty(CustomHierarchyProperties PropertyId, int lIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetDatasourceFullName", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetDatasourceFullName(int index);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetDatasourceInternalName", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetDatasourceInternalName(int index);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetDatasourceName", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetDatasourceName(int index);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetDepartmentStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetDepartmentStringProperty(DepartmentProperty PropertyId, int Index);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetDocumentStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetDocumentStringProperty(DocumentProperty PropertyId, int Index);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetEnvAttrDefStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetEnvAttrDefStringProperty(AttributeDefinitionProperty property, int index);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetEnvAttrGuiDefStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetEnvAttrGuiDefStringProperty(EnvAttrGuiProps PropertyId, int Index);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetEnvStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetEnvStringProperty(EnvironmentProperty lPropertyId, int lIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetEnvStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetEnvStringProperty(int lPropertyId, int lIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetGroupStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetGroupStringProperty(GroupProperty PropertyId, int lIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetGuiStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetGuiStringProperty(GuiProperty propertyID, int index);
    [DllImport("dmsgen.dll", EntryPoint = "aaApi_GetLastErrorDetail", CharSet = CharSet.Unicode)]
    public static extern IntPtr unsafe_aaApi_GetLastErrorDetail();
    [DllImport("dmsgen.dll", EntryPoint = "aaApi_GetLastErrorMessage", CharSet = CharSet.Unicode)]
    public static extern IntPtr unsafe_aaApi_GetLastErrorMessage();
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetLinkDataColumnStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetLinkDataColumnStringProperty(LinkDataProperty propertyID, int index);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetLinkDataColumnValue", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetLinkDataColumnValue(int lRowIndex, int lColumnIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetLinkDataDataBufferColumnValue", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetLinkDataDataBufferColumnValue(IntPtr hBuf, int iRowIndex, int iColumnIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetLinkDataDataBufferNumericProperty", CharSet = CharSet.Unicode)]
    private static extern int unsafe_aaApi_GetLinkDataDataBufferNumericProperty(IntPtr hBuf, LinkDataProperty propertyID, int iColumnIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetLinkDataDataBufferStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetLinkDataDataBufferStringProperty(IntPtr hBuf, LinkDataProperty propertyID, int iColumnIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetLinkStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetLinkStringProperty(LinkProperty propertyID, int index);
    [DllImport("dmsgen.dll", EntryPoint = "aaApi_GetMessageByErrorId", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetMessageByErrorId(int errorID);
    [DllImport("dmsgen.dll", EntryPoint = "aaApi_GetProductVersionString", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetProductVersionString();
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetProjectStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetProjectStringProperty(ProjectProperty PropertyId, int lIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetSetStringProperty", CharSet = CharSet.Unicode)]
    public static extern IntPtr unsafe_aaApi_GetSetStringProperty(SetProperty iSetProp, int iIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetStateStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetStateStringProperty(StateProperty PropertyId, int lIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetStorageStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetStorageStringProperty(StorageProperty PropertyId, int lIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetTableStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetTableStringProperty(TableProperty propertyID, int index);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetUserListStringProperty", CharSet = CharSet.Unicode)]
    public static extern IntPtr unsafe_aaApi_GetUserListStringProperty(UserListProperty iUserListProp, int iIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetUserStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetUserStringProperty(UserProperty lPropertyId, int lIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetWorkflowStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetWorkflowStringProperty(WorkflowProperty PropertyId, int lIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_GetWorkingDirectory", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_GetWorkingDirectory();
    [DllImport("dmscli.dll", EntryPoint = "aaApi_SqlSelectDataBufGetStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_SqlSelectDataBufGetStringProperty(IntPtr hDataBuffer, SqlSelectProperties lPropertyId, int lIdxCol);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_SqlSelectDataBufGetStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_SqlSelectDataBufGetStringProperty(IntPtr hDataBuffer, int lPropertyId, int lIdxCol);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_SqlSelectDataBufGetValue", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_SqlSelectDataBufGetValue(IntPtr hDataBuffer, int iRowIndex, int iColumnIndex);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_SqlSelectGetData", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_SqlSelectGetData(int iRow, int iColumn);
    [DllImport("dmscli.dll", EntryPoint = "aaApi_SqlSelectGetStringProperty", CharSet = CharSet.Unicode)]
    private static extern IntPtr unsafe_aaApi_SqlSelectGetStringProperty(SqlSelectProperties lPropertyId, int lIdxCol);

    [StructLayout(LayoutKind.Sequential)]
    public struct _AADOC_ITEM
    {
        public int lProjectId;
        public int lDocumentId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct _AAEALINKAGE
    {
        public int lLinkageType;
        public PWWrapper._AADOC_ITEM documentId;
        public int lEnvironmentId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct _FINDDOC_RESULT
    {
        public uint dwColumnCount;
        public PWWrapper._FINDDOC_RESULTCOL[] pCol;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct _FINDDOC_RESULTCOL
    {
        [FieldOffset(0)]
        public uint dwType;
        [FieldOffset(4)]
        public uint __padding;
        [FieldOffset(8)]
        public int lValue;
        [FieldOffset(8)]
        public uint ulValue;
        [FieldOffset(8)]
        public ulong uint64Value;
        [FieldOffset(8)]
        private ulong int64Value;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct _FINDDOC_RESULTS
    {
        public uint dwRowCount;
        public PWWrapper._FINDDOC_RESULT[] pRow;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AADMSEATRGUIDEF
    {
        public uint ulFlags;
        public int lEnvironmentId;
        public int lTableId;
        public int lColumnId;
        public int lGuiId;
        public int lPageNo;
        public int lTabOrder;
        public string lpctstrLabel;
        public string lpctstrLabelFont;
        public int lLabelFontH;
        public int lLabelX;
        public int lLabelY;
        public int lLabelWidth;
        public int lLabelHeight;
        public int lEditX;
        public int lEditY;
        public int lEditWidth;
        public int lEditHeight;
        public string lpctstrPrompt;
        public int lGuiFlags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AaDocItem
    {
        public int lProjectId;
        public int lDocumentId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AaDocumentsParam
    {
        public uint uiMask;
        public int iCount;
        public PWWrapper.AaDocItem lpDocuments;
        public int iParam1;
        public uint uiFlags;
        public int iParam2;
        public string sComment;
        private IntPtr hProcessedDocuments;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AAEATTRDEF
    {
        public int iControlType;
        public int iEditFontHeight;
        public int iFieldFlags;
        public int iDefaultValueType;
        public int iFieldLength;
        public int iFieldAccess;
        public int iValueListType;
        public string sEditFont;
        public string sDefaultValue;
        public string sFieldFormat;
        public string sValueListSource;
        public string sExtra1;
        public string sExtra2;
        public string sExtra3;
        public string sExtra4;
        public string sExtra5;
    }

    public enum AAODSHIERARCHY
    {
        AAODS_HIERARCHY_ID = 1
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct AAODSPickList
    {
        public int lPicklistType;
        public int lPicklistClassId;
        public int lPicklistCodeAttrId;
        public int lPicklistValueAttrId;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x401)]
        public string sSelect;
        public bool bForceToList;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x401)]
        public string sFileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x401)]
        public string sFunction;
        public int lUpdateOnEditSiblingsFlags;
    }

    public enum AAODSPickListType
    {
        None,
        Class,
        Select
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AaProjItem
    {
        public uint ulFlags;
        public int lProjectId;
        public int lEnvironmentId;
        public int lParentId;
        public int lStorageId;
        public int lManagerId;
        public int lTypeId;
        public int lWorkflowId;
        public string lptstrName;
        public string lptstrDesc;
        public int lManagerType;
        public int lWorkspaceProfileId;
        public Guid guidVault;
        public int lComponentClassId;
        public int lComponentInstanceId;
        public uint projFlagMask;
        public uint projFlags;
    }

    [Flags]
    public enum AaProjItemFlags : uint
    {
        AADMSPROJF_PROJECTID = 1,
        AADMSPROJF_ENVID = 2,
        AADMSPROJF_PARENTID = 4,
        AADMSPROJF_STORAGEID = 8,
        AADMSPROJF_MANAGERID = 0x10,
        AADMSPROJF_TYPEID = 0x20,
        AADMSPROJF_WORKFLOW = 0x40,
        AADMSPROJF_NAME = 0x80,
        AADMSPROJF_DESC = 0x100,
        AADMSPROJF_MGRTYPE = 0x400,
        AADMSPROJF_WSPACEPROFID = 0x800,
        AADMSPROJF_GUID = 0x1000,
        AADMSPROJF_COMPONENT_CLASSID = 0x2000,
        AADMSPROJF_PROJFLAGS = 0x4000,
        AADMSPROJF_COMPONENT_INSTANCEID = 0x8000,
        AADMSPROJF_REQUIREDONCREATE = 0x88,
        AADMSPROJF_ALL = 0xffff
    }

    [Flags]
    public enum AccessControlSelectionFlags : uint
    {
        IgnoreParents = 1,
        IgnoreEnvironment = 2,
        IgnoreDefault = 4,
        ExactMatch = 8,
        IgnoreUserSettings = 0x10,
        AllWorkflowStates = 0x20,
        IgnoreObjAcce = 0x10000
    }

    public enum AccessDocumentRights
    {
        SECURITYOBJECT = 2,
        DOCFULLCONTROL = 3,
        DOCCHANGEPERMISSIONS = 4,
        DOCDELETE = 5,
        DOCREAD = 7,
        DOCWRITE = 6,
        DOCFILEREAD = 8,
        DOCFILEWRITE = 9,
        DOCNOACCESS = 10
    }

    public enum AccessFolderRights
    {
        SECURITYOBJECT = 1,
        FULLCONTROL = 2,
        CHANGEPERMISSIONS = 3,
        CREATESUBFOLDER = 4,
        DELETE = 5,
        READ = 6,
        WRITE = 7,
        NOACCESS = 8,
        FOLDER_ACCESS = 1,
        DOCUMENT_ACCESS = 2
    }

    [Flags]
    public enum AccessMaskFlags : uint
    {
        None = 0,
        Control = 1,
        Write = 2,
        Read = 4,
        FileWrite = 8,
        FileRead = 0x10,
        Create = 0x20,
        Delete = 0x40,
        Full = 0xffff
    }

    public enum AccessMasks
    {
        None = 0,
        Control = 1,
        Write = 2,
        Read = 4,
        FWrite = 8,
        FRead = 0x10,
        Create = 0x20,
        Delete = 0x40,
        Free = 0x80,
        ChangeWorkflowState = 0x100,
        Full = 0xffff
    }

    public enum AccessObjectProperty
    {
        ObjType = 1,
        ObjID1 = 2,
        ObjID2 = 3,
        Workflow = 4,
        State = 5,
        MemberType = 6,
        MemberId = 7,
        AccessMask = 8,
        ObjGUID = 9
    }

    public enum AccessObjectType
    {
        UserIgnoresAccessCtrl,
        EnvironmentProject,
        Project,
        EnvironmentDocument,
        Document,
        ODSComponent
    }

    public enum AccessObjectTypes
    {
        AADMSAOTYPE_ENV_PROJ = 1,
        AADMSAOTYPE_PROJECT = 2,
        AADMSAOTYPE_ENV_DOC = 3,
        AADMSAOTYPE_DOCUMENT = 4,
        AADMSAOTYPE_ODS_COMPONENT = 5
    }

    public enum AccessUserProperty
    {
        UserID = 1,
        AccessMask = 2
    }

    public enum ApplActionProperty
    {
        APPLACTION_PROP_APPLICATION_ID = 1,
        APPLACTION_PROP_USER_ID = 2,
        APPLACTION_PROP_ACTION_TYPE = 3,
        APPLACTION_PROP_FLAGS = 4,
        APPLACTION_PROP_PROGRAM_CLASS = 5,
        APPLACTION_PROP_EXECUTABLE_PATH = 6,
        APPLACTION_PROP_PROGRAM_NAME = 7,
        APPLACTION_PROP_ARGUMENTS = 8,
        APPLACTION_PROP_IS_DEFAULT = 9
    }

    public enum ApplicationActionTypesIndices
    {
        DMS_APPLACTION_OPEN,
        DMS_APPLACTION_VIEW,
        DMS_APPLACTION_REDLINE,
        DMS_APPLACTION_PRINT,
        DMS_APPLACTION_SCANREFS
    }

    public enum ApplicationProperty
    {
        ID = 1,
        Name = 2,
        ViewerId = 3
    }

    [Flags]
    public enum AttributeCopyFlags : uint
    {
        None = 0,
        IgnoreEnvironmentCopyOptions = 1,
        CopyCodeFields = 2,
        SkipCopyInSameEnvironment = 4
    }

    public enum AttributeDefaultValueTypes
    {
        None,
        Fixed,
        Select,
        SystemVariable,
        Function
    }

    public enum AttributeDefinitionProperty
    {
        EnvironmentID = 1,
        TableID = 2,
        ColumnID = 3,
        ControlType = 4,
        EditFontHeight = 5,
        FieldFlags = 6,
        DefaultValueType = 7,
        FieldLength = 8,
        FieldAccess = 9,
        ValueListType = 10,
        EditFont = 11,
        DefaultValue = 12,
        FieldFormat = 13,
        ValueListSource = 14,
        Extra1 = 15,
        Extra2 = 0x10,
        Extra3 = 0x11,
        Extra4 = 0x12,
        Extra5 = 0x13
    }

    public enum AttributeLinkageTypes
    {
        AADMS_EALNK_DOCUMENT = 1,
        AADMS_EALNK_ENVIRONMENT = 2
    }

    public enum AttributeParameterFlags
    {
        Unique = 1,
        Required = 2,
        EditableIfFinal = 0x10,
        MultiSelect = 0x40,
        LimitToList = 0x80,
        CopyClearNewSheet = 0x1000,
        CopyClearInEnvironment = 0x2000,
        CopyClearOutEnvironment = 0x4000,
        CopyClearOutDatabase = 0x8000
    }

    public enum AuditTrailActions
    {
        AADMSAT_ACT_DOC_FIRST = 0x3e8,
        AADMSAT_ACT_DOC_UNKNOWN = 0x3e8,
        AADMSAT_ACT_DOC_CREATE = 0x3e9,
        AADMSAT_ACT_DOC_MODIFY = 0x3ea,
        AADMSAT_ACT_DOC_ATTR = 0x3eb,
        AADMSAT_ACT_DOC_FILE_ADD = 0x3ec,
        AADMSAT_ACT_DOC_FILE_REM = 0x3ed,
        AADMSAT_ACT_DOC_FILE_REP = 0x3ee,
        AADMSAT_ACT_DOC_CIN = 0x3ef,
        AADMSAT_ACT_DOC_VIEW = 0x3f0,
        AADMSAT_ACT_DOC_CHOUT = 0x3f1,
        AADMSAT_ACT_DOC_CPOUT = 0x3f2,
        AADMSAT_ACT_DOC_GOUT = 0x3f3,
        AADMSAT_ACT_DOC_STATE = 0x3f4,
        AADMSAT_ACT_DOC_FINAL_S = 0x3f5,
        AADMSAT_ACT_DOC_FINAL_R = 0x3f6,
        AADMSAT_ACT_DOC_VERSION = 0x3f7,
        AADMSAT_ACT_DOC_MOVE = 0x3f8,
        AADMSAT_ACT_DOC_COPY = 0x3f9,
        AADMSAT_ACT_DOC_SECUR = 0x3fa,
        AADMSAT_ACT_DOC_REDLINE = 0x3fb,
        AADMSAT_ACT_DOC_DELETE = 0x3fc,
        AADMSAT_ACT_DOC_EXPORT = 0x3fd,
        AADMSAT_ACT_DOC_FREE = 0x3fe,
        AADMSAT_ACT_DOC_EXTRACT = 0x3ff,
        AADMSAT_ACT_DOC_DISTRIBUTE = 0x400,
        AADMSAT_ACT_DOC_LAST = 0x400,
        AADMSAT_ACT_USER_LOGIN = -669,
        AADMSAT_ACT_USER_LOGOUT = -670,
        AADMSAT_ACT_USER_CONNCOUNT = -671
    }

    public enum AuditTrailTypes
    {
        AADMSAT_TYPE_FIRST = 1,
        AADMSAT_TYPE_VAULT = 1,
        AADMSAT_TYPE_DOCUMENT = 2,
        AADMSAT_TYPE_DOCUMENT_SET = 3,
        AADMSAT_TYPE_WORKFLOW = 4,
        AADMSAT_TYPE_STATE = 5,
        AADMSAT_TYPE_USER = 6,
        AADMSAT_TYPE_LAST = 6,
        AADMSAT_TYPE_DS_REPORT = -55,
        AADMSAT_TYPE_USER_LILO = -56
    }

    public enum CodeDefinitionType
    {
        All = -1,
        PartOfCode = 1,
        DocumentCodePlaceHolder = 2,
        AdditionalDocumentCode = 3,
        RevisionPlaceHolder = 4
    }

    public enum ColumnProperty
    {
        ColumnID = 1,
        TableID = 2,
        SQLType = 3,
        Precision = 4,
        Scale = 5,
        Type = 6,
        Length = 7,
        Unique = 8,
        Name = 9,
        Desc = 10,
        Format = 11
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CompactPropertyReference
    {
        public Guid guidPropertySet;
        public string sPropertyName;
        public uint uiPropertyID;
        public uint uiResultType;
    }

    [Flags]
    public enum CreateVersionsFromSourceFlags : uint
    {
        AARULEO_ADD_SOURCE_ATTRS = 1,
        AARULEO_REMOVE_TARGET_ATTRS = 2,
        AARULEO_APPLY_SOURCE_NAME = 4,
        AARULEO_APPLY_SOURCE_FNAME = 8,
        AARULEO_NO_UPDATE = 0x10,
        AARULEO_NO_WIZARDS = 0x20,
        AARULEO_RECREATE_RELATIONS = 0x40,
        AARULEO_SKIP_SAME_TABLE_ATTRS = 0x80,
        AARULEO_INCLUDE_VERSIONS = 0x100
    }

    [Flags]
    public enum CriteriaFlags : uint
    {
        GT_RESTRICTION = 0,
        GT_JOIN = 1,
        GT_ROWSET_ID = 2,
        GT_SUBQUERY = 3,
        GT_FREE_TEXT = 4,
        GT_UNION = 5,
        GT_QRY_SPLIT = 6,
        GT_FIELD_MASK = 7,
        VT_SINGLE_VALUE = 0,
        VT_NO_VALUE = 8,
        VT_VALUE_ARRAY = 0x10,
        VT_PROPERTY_ID = 0x18,
        VT_VALUE_BLOB = 0x1000,
        VT_FIELD_MASK = 0xf018,
        UT_REGULAR = 0,
        UT_UI_HINT_ONLY = 0x20,
        UT_OVERLAY = 0x60,
        UT_SUBQUERY_TAG = 160,
        UT_SUBGROUP_TAG = 0xc0,
        UT_FIELD_MASK = 0xfe0
    }

    [Flags]
    public enum CriteriaGroupType : uint
    {
        GT_RESTRICTION = 0,
        GT_JOIN = 1,
        GT_ROWSET_ID = 2,
        GT_SUBQUERY = 3,
        GT_FREE_TEXT = 4,
        GT_UNION = 5,
        GT_QRY_SPLIT = 6,
        GT_FIELD_MASK = 7
    }

    [Flags]
    public enum CriteriaUsageType : uint
    {
        UT_REGULAR = 0,
        UT_UI_HINT_ONLY = 0x20,
        UT_OVERLAY = 0x60,
        UT_SUBQUERY_TAG = 160,
        UT_SUBGROUP_TAG = 0xc0,
        UT_FIELD_MASK = 0xfe0
    }

    [Flags]
    public enum CriteriaValueType : uint
    {
        VT_SINGLE_VALUE = 0,
        VT_NO_VALUE = 8,
        VT_VALUE_ARRAY = 0x10,
        VT_PROPERTY_ID = 0x18,
        VT_VALUE_BLOB = 0x1000,
        VT_FIELD_MASK = 0xf018
    }

    public enum CriterionDataType
    {
        AADMS_ATTRFORM_DATATYPE_STRING = 1,
        AADMS_ATTRFORM_DATATYPE_INT = 2,
        AADMS_ATTRFORM_DATATYPE_UINT = 3,
        AADMS_ATTRFORM_DATATYPE_FLOAT = 4,
        AADMS_ATTRFORM_DATATYPE_DATE_TO_DAY = 5,
        AADMS_ATTRFORM_DATATYPE_DATE_TO_SEC = 6,
        AADMS_ATTRFORM_DATATYPE_STRING_AS_DATE_TO_DAY = 7,
        AADMS_ATTRFORM_DATATYPE_STRING_AS_DATE_TO_SEC = 8,
        AADMS_ATTRFORM_DATATYPE_GUID = 9,
        AADMS_ATTRFORM_DATATYPE_BIGINT = 10,
        AADMS_ATTRFORM_DATATYPE_UBIGINT = 11,
        AADMS_ATTRFORM_DATATYPE_TIMESPAN = 12
    }

    public enum CustomFolderItemIds
    {
        DSCI_UWSPFLDR_PRIVATE = 1,
        DSCI_UWSPFLDR_GLOBAL = 2,
        DSCI_UWSPFLDR_OTHERS = 3
    }

    public enum CustomHierarchMemberItemType
    {
        AADMSUWITYPE_PROJECT = 1,
        AADMSUWITYPE_DOCUMENT = 2
    }

    public enum CustomHierarchyProperties
    {
        UWSP_PROP_USER = 1,
        UWSP_PROP_CUSTHRCHY = 2,
        UWSP_PROP_PARENT = 3,
        UWSP_PROP_FLAGS = 4,
        UWSP_PROP_HASSUBITEMS = 5,
        UWSP_PROP_NAME = 6,
        UWSP_PROP_DESC = 7
    }

    public enum DatasourceGenericSettings
    {
        AADMS_GEN = 100,
        AADMS_GEN_CREATE_ON_FIRST_STATE = 0x66,
        AADMS_GEN_CAN_CHANGE_VERSION_STATE = 0x67,
        AADMS_GEN_USE_RECYCLE_BIN = 0x68,
        AADMS_GEN_CREATE_DOC_ON_STATE = 0x69,
        AADMS_GEN_GDR = 0x6a,
        AADMS_GEN_COMMON_ENVIRONMENT = 0x6b,
        AADMS_GEN_VERSION_FLAGS = 0x6c,
        AADMS_GEN_FIRST_VERSION_NO = 0x6d,
        AADMS_GEN_DOC_STATE_CHANGE_FLAGS = 110,
        AADMS_GEN_NOTIFY_ABOUT_EVENT_SUCCESS = 0x6f,
        AADMS_GEN_AUDT_DOC_TRACE = 0x70,
        AADMS_GEN_AUDT_VLT_TRACE = 0x71,
        AADMS_GEN_AUDT_SET_TRACE = 0x72,
        AADMS_GEN_AUDT_TRUNCATE = 0x73,
        AADMS_GEN_AUDT_TRUNCATE_PARAM = 0x74,
        AADMS_GEN_AUDT_TRUNCATE_TIME_UNITS = 0x75,
        AADMS_GEN_AUDT_TRUNCATE_USE_TABLE = 0x76,
        AADMS_GEN_AUDT_TRUNCATE_TABLE_NAME = 0x77,
        AADMS_GEN_FORCE_CASE_INSENS_FIND_DOCS = 120,
        AADMS_GEN_CAN_MOVE_VERSIONS = 0x79,
        AADMS_GEN_ENG_COMP_ENABLED = 0x7a,
        AADMS_GEN_SAVED_SEARCH_FOLDER = 0x7b,
        AADMS_GEN_IMAGES_FOLDER = 0x7c,
        AADMS_GEN_MRU_ITEMS_TO_SHOW = 0x7d,
        AADMS_GEN_MRU_TRUNCATE_VALUE = 0x7e,
        AADMS_GEN_MRU_TRUNCATE_UNITS = 0x7f,
        AADMS_GEN_DATASOURCE_GUID = 0x80,
        AADMS_GEN_DEFAULT_RIGHTS_ADD_EVERYONE = 0x81,
        AADMS_GEN_DEFAULT_RIGHTS_CLASS_MASK = 130,
        AADMS_GEN_DEFAULT_RIGHTS_ATTRIBUTE_MASK = 0x83,
        AADMS_GEN_DEFAULT_RIGHTS_METHOD_MASK = 0x84,
        AADMS_GEN_SPATIAL_NEW_FOLDER_INHERIT = 0x8a,
        AADMS_GEN_SPATIAL_NEW_DOC_INHERIT = 0x8b,
        AADMS_GEN_WEB_DEFAULT_SERVER = 140,
        AADMS_GEN_USE_UNION_BASED_FIND_DOCS = 0x8d,
        AADMS_GEN_SHAREPOINT_DEFAULT_SERVER = 0x8e,
        AADMS_GEN_PUBLISHER_DEFAULT_SERVER = 0x8f,
        AADMS_GEN_TEMPLATES_FOLDER = 0x90,
        AADMS_GEN_SHAREABLE_DOCUMENTS = 0x91
    }

    public enum DatasourceStatisticsProperty
    {
        STAT_PROP_LAST_UPDATED = 0,
        STAT_PROP_USER_COUNT = 1,
        STAT_PROP_GROUP_COUNT = 2,
        STAT_PROP_MIN_USERS_PER_GROUP = 3,
        STAT_PROP_MAX_USERS_PER_GROUP = 4,
        STAT_PROP_AVG_USERS_PER_GROUP = 5,
        STAT_PROP_POPULATED_FOLDER_COUNT = 6,
        STAT_PROP_MIN_DOCS_PER_FOLDER = 7,
        STAT_PROP_MAX_DOCS_PER_FOLDER = 8,
        STAT_PROP_AVG_DOCS_PER_FOLDER = 9,
        STAT_PROP_ITEMS_WITH_AUDIT_RECS = 10,
        STAT_PROP_MIN_AUDIT_RECS_PER_FOLDER = 11,
        STAT_PROP_MAX_AUDIT_RECS_PER_FOLDER = 12,
        STAT_PROP_AVG_AUDIT_RECS_PER_FOLDER = 13,
        STAT_PROP_MIN_REFERENCE_ATTACHMENTS = 14,
        STAT_PROP_MAX_REFERENCE_ATTACHMENTS = 15,
        STAT_PROP_AVG_REFERENCE_ATTACHMENTS = 0x10,
        STAT_PROP_MAX_FOLDER_DEPTH = 0x11,
        STAT_PROP_MIN_FOLDER_DEPTH = 0x12,
        STAT_PROP_AVG_FOLDER_DEPTH = 0x13,
        STAT_PROP_STORAGE_COUNT = 20,
        STAT_PROP_FOLDER_COUNT = 0x15,
        STAT_PROP_PROJECT_COUNT = 0x16,
        STAT_PROP_RICHPROJECT_COUNT = 0x17,
        STAT_PROP_DOC_COUNT = 0x18,
        STAT_PROP_AUDIT_COUNT = 0x19,
        STAT_PROP_ENVIRONMENT_COUNT = 0x1a,
        STAT_PROP_VIEW_COUNT = 0x1b,
        STAT_PROP_PROPERTY_COUNT = 0x1c,
        STAT_PROP_WORKFLOW_COUNT = 0x1d,
        STAT_PROP_THUMB_COUNT = 30,
        STAT_PROP_QUERY_COUNT = 0x1f,
        STAT_PROP_ACCESSCONTROL_COUNT = 0x20,
        STAT_PROP_MASTERFILE_COUNT = 0x21,
        STAT_PROP_DEPARTMENT_COUNT = 0x22,
        STAT_PROP_STATE_COUNT = 0x23,
        STAT_PROP_ODS_CLASS_COUNT = 0x24,
        STAT_PROP_MAX_DOC_FILE_SIZE = 0x25,
        STAT_PROP_MIN_DOC_FILE_SIZE = 0x26,
        STAT_PROP_AVG_DOC_FILE_SIZE = 0x27,
        STAT_PROP_DOCS_PROCESSING = 40,
        STAT_PROP_DOCS_PROCESSED = 0x29,
        STAT_PROP_DOCS_WITH_THUMB = 0x2a,
        STAT_PROP_DOCS_WITHOUT_THUMB = 0x2b,
        STAT_PROP_DOCS_NOT_PROCESSED = 0x2c,
        STAT_PROP_EMPTY_FOLDER_COUNT = 0x2d,
        STAT_PROP_ATTRIBUTE_COUNT = 0x2e,
        STAT_PROP_MIN_ATTRIBUTES_PER_ENV = 0x2f,
        STAT_PROP_MAX_ATTRIBUTES_PER_ENV = 0x30,
        STAT_PROP_AVG_ATTRIBUTES_PER_ENV = 0x31,
        STAT_PROP_CHKL_REC_COUNT = 50,
        STAT_PROP_ORPHANED_THUMB_COUNT = 0x33,
        STAT_PROP_DOCS_WITH_FPROPS_COUNT = 0x34,
        STAT_PROP_AFP_FTR_DOCS_PROCESSED = 0x35,
        STAT_PROP_AFP_FTR_DOCS_UNPROCESSED = 0x36,
        STAT_PROP_AFP_FTR_DOCS_PROCESSING = 0x37,
        STAT_PROP_AFP_THUMB_DOCS_PROCESSED = 0x38,
        STAT_PROP_AFP_THUMB_DOCS_UNPROCESSED = 0x39,
        STAT_PROP_AFP_THUMB_DOCS_PROCESSING = 0x3a,
        STAT_PROP_AFP_FPROP_DOCS_PROCESSED = 0x3b,
        STAT_PROP_AFP_FPROP_DOCS_UNPROCESSED = 60,
        STAT_PROP_AFP_FPROP_DOCS_PROCESSING = 0x3d,
        STAT_PROP_LAST = 0x3d
    }

    public enum DataSourceType
    {
        Unknown = 0,
        RIS = 1,
        ODBC = 2,
        Informix = 3,
        Ingres = 4,
        Oracle = 5,
        SqlAnywhere = 6,
        SqlServer = 7,
        Sybase = 8,
        DB2 = 9,
        Optinet = 10,
        Solid = 11,
        ODBC_Informix = 12,
        ODBC_Ingres = 0x10,
        ODBC_Oracle = 20,
        ODBC_SqlAnywhere = 0x18,
        ODBC_SqlServer = 0x1c,
        ODBC_Sybase = 0x20,
        ODBC_DB2 = 0x24,
        ODBC_Solid = 0x2c
    }

    public enum DepartmentProperty
    {
        ID = 1,
        Name = 2,
        Desc = 3,
        DisplayName = 4
    }

    public enum DialogBoxCommandId
    {
        IDOK = 1,
        IDCANCEL = 2,
        DABORT = 3,
        IDRETRY = 4,
        IDIGNORE = 5,
        IDYES = 6,
        IDNO = 7,
        IDCLOSE = 8,
        IDHELP = 9
    }

    public enum DmsDataBufferStringPropertyEnum
    {
        PROJ_PROP_NAME = 12,
        PROJ_PROP_DESC = 13,
        PROJ_PROP_CODE = 14,
        PROJ_PROP_VERSION = 15,
        PROJ_PROP_CREATE_TIME = 0x10,
        PROJ_PROP_UPDATE_TIME = 0x11,
        DOC_PROP_NAME = 20,
        DOC_PROP_FILENAME = 0x15
    }

    public enum DocListUpdateTypeMasks : uint
    {
        AADLUISF_NOMASTERCHECK = 2,
        AADLUISF_NOSETCHECK = 1
    }

    public enum DocumentCodeDefinitionFlag : uint
    {
        AllowEmpty = 2
    }

    public enum DocumentCodeDefinitionProperty
    {
        EnvironmentID = 1,
        TableID = 2,
        ColumnID = 3,
        Type = 4,
        SerialType = 5,
        Flags = 6,
        OrderNumber = 7,
        ConnectString = 8
    }

    public enum DocumentCodeSerialType
    {
        None,
        Number,
        UsedWith
    }

    [Flags]
    public enum DocumentCopyFlags : uint
    {
        AADMS_DOCCOPY_CAN_OVERWRITE = 1,
        AADMS_DOCCOPY_ATTRS = 2,
        AADMS_DOCCOPY_NO_ATTRS = 4,
        AADMS_DOCCOPY_NO_SETITEM = 8,
        AADMS_DOCCOPY_MOVE = 0x10,
        AADMS_DOCCOPY_NOFILE = 0x20,
        AADMS_DOCCOPY_NO_HOOKS = 0x10000,
        AADMS_DOCCOPY_LOG_MOVE = 0x20000,
        AADMS_DOCCOPY_INCLUDE_VERSIONS = 0x40000,
        AADMS_DOCCOPY_COPY_ACCESS = 0x80000,
        AADMS_DOCCOPY_MOVE_NO_VERSIONS = 0x100000,
        AADMS_DOCCOPY_COPY_CONFBLOCKS = 0x200000,
        AADMS_DOCCOPY_COPYVERSIONSTR = 0x400000
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DocumentCreateParam
    {
        public uint ulMask;
        public int lProjectId;
        public int lDocumentId;
        public int lFileType;
        public int lItemType;
        public int lApplicationId;
        public int lDepartmentId;
        public string lpctstrFileName;
        public string lpctstrName;
        public StringBuilder lptstrWorkingFile;
        public int lBufferSize;
        public uint ulFlags;
        public int lWorkspaceProfileId;
        public bool bLeaveOut;
        public int lAttributeId;
        public Guid guidProject;
        public Guid guidDocument;
    }

    [Flags]
    public enum DocumentCreationFlag : uint
    {
        Default = 0,
        NoAttributeRecord = 1,
        CreateAttributeRecord = 2,
        NoAuditTrail = 4
    }

    public enum DocumentDeleteMasks
    {
        None = 0,
        NoSetChild = 1,
        NoSetParent = 2,
        MoveAction = 4,
        IncludeVersions = 8
    }

    public enum DocumentListDefinitions : uint
    {
        AADLSTF_ATTACH_TO_ATTR_SHEET = 1,
        AADLSTF_VIRTUAL_LIST = 2,
        AADLSTF_CHECKBOXES = 4,
        AADLSTF_ISTYPEWINDOW = 8,
        AADLSTF_PUT_NAME_FIRST = 0x10,
        AADLSTF_ENABLE_BACK_FWD_BROWSING = 0x80
    }

    public enum DocumentOperations
    {
        AAOPER_DOC_FIRST = 0x3e8,
        AAOPER_DOC_CREATE = 0x3e8,
        AAOPER_DOC_CREATE_LEAVE_OUT = 0x3e9,
        AAOPER_DOC_COPY = 0x3ea,
        AAOPER_DOC_MOVE = 0x3eb,
        AAOPER_DOC_DELETE = 0x3ec,
        AAOPER_DOC_MODIFY = 0x3ed,
        AAOPER_DOC_CHECKOUT = 0x3ee,
        AAOPER_DOC_COPYOUT = 0x3ef,
        AAOPER_DOC_EXPORT = 0x3f0,
        AAOPER_DOC_CHECKIN = 0x3f1,
        AAOPER_DOC_CHECKIN_LEAVE_COPY = 0x3f2,
        AAOPER_DOC_CREATE_LINK_DATA = 0x406,
        AAOPER_DOC_UPDATE_LINK_DATA = 0x407,
        AAOPER_DOC_DELETE_LINK_DATA = 0x408
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DocumentProperties_stc
    {
        public uint uiPropertyCount;
        public PWWrapper.QueryResultFlags eResultFlags;
        public PWWrapper.CompactPropertyReference[] sProperty;
    }

    public enum DocumentProperty
    {
        ID = 1,
        VersionNumber = 2,
        ProposalNumber = 3,
        CreatorID = 4,
        UpdaterID = 5,
        UserID = 6,
        Size = 7,
        FileType = 8,
        ItemType = 9,
        StorageID = 10,
        SetID = 11,
        SetType = 12,
        WorkFlowID = 13,
        StateID = 14,
        ApplicationID = 15,
        DepartmentID = 0x10,
        OriginalNumber = 0x12,
        IsOutToMe = 0x13,
        Name = 20,
        FileName = 0x15,
        Desc = 0x16,
        Version = 0x17,
        CreateTime = 0x18,
        UpdateTime = 0x19,
        DMSStatus = 0x1a,
        DMSDate = 0x1b,
        Node = 0x1c,
        ProjectID = 0x1d,
        Access = 30,
        IsLogicalSetMaster = 0x1f,
        IsRedlineMaster = 0x20,
        IsRefMaster = 0x21,
        HasFinalStatus = 0x23,
        Manager = 0x24,
        FileUpdater = 0x25,
        LastRtLocker = 0x26,
        ItemFlags = 0x27,
        FileUpdateTime = 40,
        LastRtLockTime = 0x29,
        Is3DFile = 0x2a,
        Is2DFile = 0x2b,
        MgrType = 0x2c,
        IsUrl = 0x2d,
        UrlName = 0x2e,
        DocGuid = 0x2f,
        ProjGuid = 0x30,
        OrigGuid = 0x31,
        WSpaceProfID = 50
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct DocumentRequestColumns
    {
        [FieldOffset(0)]
        public PWWrapper.DocumentProperties_stc properties;
        [FieldOffset(0)]
        public byte[] padding;
        [FieldOffset(0)]
        public PWWrapper.CompactPropertyReference[] columns;
    }

    public enum DocumentType
    {
        Normal = 10,
        History = 11,
        Set = 12,
        Redline = 13,
        ModelerBRP = 14,
        Abstract = 15,
        Unknown = 0
    }

    public enum DocumentTypes
    {
        AADMS_DOCUMENT_FILE_COMMAND = 15,
        AADMS_DOCUMENT_FILE_REPLACE = 0,
        AADMS_DOCUMENT_FILE_RENAME = 1,
        AADMS_DOCUMENT_FILE_ADD = 2,
        AADMS_DOCUMENT_FILE_KEEP_ITYPE = 0x10
    }

    public delegate int DoumentHookFunction(int hookId, int hookType, ref PWWrapper.AaDocumentsParam aParam1, int aParam2, ref int pResult);

    public enum DscItemTypes
    {
        DSCITYPE_NONE = 0,
        DSCITYPE_DEFAULT_ROOT = 1,
        DSCITYPE_DATASOURCE_ROOT = 2,
        DSCITYPE_DATASOURCE = 3,
        DSCITYPE_VAULTROOT = 4,
        DSCITYPE_DMSVAULT = 5,
        DSCITYPE_DMSDOCUMENT = 6,
        DSCITYPE_MSGFLDRROOT = 11,
        DSCITYPE_MSGFOLDER = 12,
        DSCITYPE_MSGSPECIALFOLDER = 13,
        DSCITYPE_UWSPROOT = 14,
        DSCITYPE_UWSPACE = 15,
        DSCITYPE_UWSPACE_FOLDER = 0x10,
        DSCITYPE_UWSPACE_USER = 0x11,
        DSCITYPE_DMSDOCUMENTSET = 0x12,
        DSCITYPE_HTMLPG = 0x13,
        DSCITYPE_SEARCH_RESULTS = 20,
        DSCITYPE_SAVED_QUERY_ROOT = 0x15,
        DSCITYPE_SAVED_QUERY_TYPE = 0x16,
        DSCITYPE_SAVED_QUERY_ITEM = 0x17,
        DSCITYPE_COMPONENT_MAIN = 0x18,
        DSCITYPE_COMPONENT = 0x19,
        DSCITYPE_LINKSET_ROOT = 0x1a,
        DSCITYPE_ALL = 0xfffffff
    }

    public enum EnvAttrGuiProps
    {
        ENVIRONMENTID = 1,
        TABLEID = 2,
        COLUMNID = 3,
        GUIID = 4,
        PAGENO = 5,
        TABORDER = 6,
        LABELFONT_HEIGHT = 7,
        LABEL_X = 8,
        LABEL_Y = 9,
        LABEL_WIDTH = 10,
        LABEL_HEIGHT = 11,
        EDIT_X = 12,
        EDIT_Y = 13,
        EDIT_WIDTH = 14,
        EDIT_HEIGHT = 15,
        GUIFLAGS = 0x10,
        LABEL = 0x11,
        LABELFONT = 0x12,
        PROMPT = 0x13
    }

    public enum EnvAttributeGuiFlags : uint
    {
        ENVIRONMENTID = 1,
        TABLEID = 2,
        COLUMNID = 4,
        GUIID = 8,
        PAGENO = 0x10,
        TABORDER = 0x20,
        LABEL = 0x40,
        LABELFONT = 0x80,
        LABELFONT_HEIGHT = 0x100,
        LABEL_X = 0x200,
        LABEL_Y = 0x400,
        LABEL_WIDTH = 0x800,
        LABEL_HEIGHT = 0x1000,
        EDIT_X = 0x2000,
        EDIT_Y = 0x4000,
        EDIT_WIDTH = 0x8000,
        EDIT_HEIGHT = 0x10000,
        PROMPT = 0x20000,
        GUIFLAGS = 0x40000,
        REQUIRED = 15
    }

    public enum EnvironmentInfo
    {
        ENV_PROP_ID = 1,
        ENV_PROP_TABLEID = 2,
        ENV_PROP_FLAGS = 3,
        ENV_PROP_ATTRNO = 4,
        ENV_PROP_NAME = 5,
        ENV_PROP_DESC = 6,
        ENV_PROP_VIEWID = 7
    }

    public enum EnvironmentProperty
    {
        ID = 1,
        TableID = 2,
        Flags = 3,
        AttrNo = 4,
        Name = 5,
        Desc = 6,
        ViewID = 7
    }

    public enum EnvTriggerProperty
    {
        EnvironmentID = 1,
        TableID = 2,
        ColumnID = 3,
        TriggeredColumnID = 4,
        OrderNumber = 5,
        ValueType = 6,
        Value = 7
    }

    [Flags]
    public enum FetchDocumentFlags : uint
    {
        CheckOut = 0,
        Export = 1,
        CopyOut = 2,
        Refresh = 4,
        Lock = 8,
        UseUpToDateCopy = 0x10,
        AcceptCheckouts = 0x20,
        CopyOutMasters = 0x40,
        AsSetMembers = 0x1000,
        ExportReferences = 0x2000,
        ChangeSetId = 0x4000,
        UseVaultDirs = 0x8000,
        IgnoreMasters = 0x10000,
        GiveOut = 0x20000,
        MarkAsView = 0x40000,
        View = 0x80000,
        NoAuditTrail = 0x80000,
        AddToMRU = 0x100000,
        IgnoreExport = 0x400000,
        DO_NOT_CHANGE_SET_ID_FOR_CHECKED_OUT_REFERENCES = 0x200000,
        SHARED_CHECKOUT = 0x1000000,
        MASTER_AS_SET = 0x10000000,
        IGNORE_REDLINE_REL = 0x20000000,
        NESTED_REFERENCES = 0x40000000,
        REDLINED_REFERENCES = 0x80000000,
        SEND_TO_FOLDER = 0x20100,
        SHARED_EXPORT = 0x1000001
    }

    [Flags]
    public enum FindDscItemFlags : uint
    {
        AAFINDDSCITEM_VISIBLE = 1,
        AAFINDDSCITEM_ITEMTYPE = 2,
        AAFINDDSCITEM_ITEMID = 4,
        AAFINDDSCITEM_ITEMTEXT = 8,
        AAFINDDSCITEM_EXPAND = 0x10,
        AAFINDDSCITEM_ITEMDATA = 0x20,
        AAFINDDSCITEM_ALLVISIBLE = 0x1000,
        AAFINDDSCITEM_AFFECTSPARENT = 0x2000,
        AAFINDDSCITEM_KEEPSELECTION = 0x4000
    }

    public delegate int GenericHookFunction(int hookId, int hookType, [In, Out] IntPtr ppDocsParam, int aParam2, ref int pResult);

    public enum GroupProperty
    {
        ID = 1,
        Name = 2,
        Desc = 3,
        Type = 4,
        SecProvider = 5
    }

    public enum GuiProperty
    {
        Id = 1,
        Name = 2,
        Desc = 3
    }

    public enum HookActions
    {
        AAHOOK_SUCCESS,
        AAHOOK_ERROR,
        AAHOOK_CALL_NEXT_IN_CHAIN,
        AAHOOK_CALL_DEFAULT
    }

    public delegate int HookFunction(int hookId, int hookType, int aParam1, int aParam2, ref int pResult);

    public enum HookIdentifiers
    {
        AADMSHOOK_FIRST = 0x3ea,
        AAHOOK_CREATE_DOCUMENT = 0x4b2,
        AAHOOK_CHECKOUT_DOCUMENT = 0x4b6,
        AAHOOK_CHECKIN_DOCUMENT = 0x4ba,
        AAHOOK_CHANGE_DOC_STATE = 0x4c0,
        AAHOOK_UPDATE_LINK_DATA = 0x4c2
    }

    public enum HookTypes
    {
        AAPREHOOK = 1,
        AAACTIONHOOK = 2,
        AAPOSTHOOK = 3,
        AAPOSTHOOK_FAIL = 4
    }

    public enum LinkDataProperty
    {
        TableID = 1,
        ColumnID = 2,
        ColumnType = 3,
        ColumnLength = 4,
        ColumnName = 5,
        ColumnFormat = 6,
        ColumnDescription = 7,
        ColumnNativeType = 8
    }

    [Flags]
    public enum LinkDataSelectFlags : uint
    {
        Creator = 1,
        CreateTime = 2,
        Updater = 4,
        UpdateTime = 8,
        SysColsOnly = 0x10,
        DocGuid = 0x20
    }

    public enum LinkDataSpecialColsIds
    {
        AADMSLINKDATACOL_PROJECTNO = -2,
        AADMSLINKDATACOL_DOCUMENTNO = -3,
        AADMSLINKDATACOL_UNIQUEVALUE = -4,
        AADMSLINKDATACOL_CREATORNO = -5,
        AADMSLINKDATACOL_CREATETIME = -6,
        AADMSLINKDATACOL_UPDATERNO = -7,
        AADMSLINKDATACOL_UPDATETIME = -8,
        AADMSLINKDATACOL_DOCGUID = -9
    }

    public enum LinkProperty
    {
        VaultID = 1,
        DocumentID = 2,
        TableID = 3,
        ColumnID = 4,
        ColumnValue = 5,
        DocGuid = 6
    }

    public enum ManagerTypeProperty
    {
        None,
        User,
        Group,
        UserList,
        AllUsers
    }

    public enum ManagerTypes
    {
        User = 1,
        Group = 2,
        UserList = 3,
        AllUsers = 4
    }

    public enum MenuCommandIds : uint
    {
        AAMENU_PROJECT_FIRST = 0x7562,
        IDMP_CREATE = 0x7563,
        IDMP_MODIFY = 0x7564,
        IDMP_DELETE = 0x7565,
        IDMP_PURGE = 0x7566,
        IDMP_WORKFL = 0x7567,
        IDMP_REFRESH = 0x7568,
        IDMP_PROPERTY = 0x7569,
        IDMP_COPYOUT = 0x756a,
        IDMP_PROACCESS = 0x756b,
        IDMP_COPY = 0x756c,
        IDMP_PASTE = 0x756d,
        IDMP_UFLDRREMOVE = 0x7574,
        IDMP_PROPERTYREAD = 0x7576,
        IDMP_EXPORT_TO = 0x7577,
        IDMP_COPYOUT_TO = 0x7578,
        IDMP_CLEAN_AUDITTRAIL = 0x7579,
        IDMP_RENAME = 0x757a,
        IDMP_EXPORT = 0x757b,
        IDMP_FIND_DOCS = 0x757c,
        IDMP_UPGRADE = 0x757d,
        IDMP_DOWNGRADE = 0x757e,
        IDMP_SCAN_REFERENCES = 0x757f,
        IDMP_CREATE_RICHPROJECT = 0x7580,
        AAMENU_DOC_FIRST = 0x7724,
        IDMD_OPEN = 0x7725,
        IDMD_OPEN_WITH = 0x7726,
        IDMD_PRINT = 0x7727,
        IDMD_QUICKVIEW = 0x7728,
        IDMD_NEW = 0x772a,
        IDMD_MODIFY = 0x772b,
        IDMD_SAVE_AS = 0x772c,
        IDMD_DELETE = 0x772d,
        IDMD_CHECKIN = 0x772f,
        IDMD_CHECKOUT = 0x7730,
        IDMD_COPYOUT = 0x7731,
        IDMD_PURGECOPY = 0x7732,
        IDMD_FREE = 0x7733,
        IDMD_SETMODIFY = 0x7735,
        IDMD_SETCREATE = 0x7736,
        IDMD_REMOVE = 0x7737,
        IDMD_VERSION = 0x7739,
        IDMD_STATE = 0x773a,
        IDMD_PROPERTIES = 0x773d,
        IDMD_EXPORT = 0x773e,
        IDMD_REDLINE = 0x773f,
        IDMD_REFRESH_COPY = 0x7740,
        IDMD_UPDATE_SERVER = 0x7741,
        IDMD_SEND_MAIL = 0x7742,
        IDMD_SEND_NOTICE = 0x7743,
        IDMD_COPY_OUT_TO = 0x7744,
        IDMD_SET_FINAL = 0x7745,
        IDMD_REMOVE_FINAL = 0x7746,
        IDMD_OPEN_READONLY = 0x7747,
        IDMD_UFLDRREMOVE = 0x7748,
        IDMD_CHANGE_PREV_STATE = 0x774b,
        IDMD_CHANGE_NEXT_STATE = 0x774c,
        IDMD_MODIFY_ATTR = 0x774d,
        IDMD_COPY_LIST = 0x774e,
        IDMD_PRINT_LIST = 0x774f,
        IDMD_COPYLIST_TABSEPARATED = 0x7751,
        IDMD_COPYLIST_SPACESEPARATED = 0x7752,
        IDMD_IMPORT = 0x7753,
        IDMD_GENERATE_CODE = 0x7754,
        IDMD_DELETE_SHEET = 0x7755,
        IDMD_CREATE_MULTIPLE = 0x7756,
        IDMD_OPENVAULT = 0x7757,
        IDMD_COPY = 0x7758,
        IDMD_MOVE = 0x7759,
        IDMD_MEMBERIN = 0x775a,
        IDMD_MASTER_LINKS = 0x775b,
        IDMD_RELEASE_MASTER = 0x775c,
        IDMD_RELEASE_REF = 0x775d,
        IDMD_COPY_ATTR_DATA = 0x775e,
        IDMD_PASTE_ATTR_DATA = 0x775f,
        IDMD_ADD_SHEET = 0x7760,
        IDMD_CLEAN_AUDITTRAIL = 0x7761,
        IDMD_RENAME = 0x7762,
        IDMD_CODE_RESERVATION = 0x7763,
        IDMD_SEND_MAIL_AS_LINK = 0x7764,
        IDMD_SHARED_CHECKOUT = 0x7765,
        IDMD_IMPORT_FROM = 0x7766,
        IDMD_ADD_COMMENT = 0x7767,
        IDMD_SCAN_REFERENCES = 0x7768,
        IDMD_SHOW_MARKUPS = 0x7769,
        AAMENU_ITEM_TOOLS_FIRST = 0x7882,
        IDMT_NOTICES = 0x7882,
        IDMT_SETTING_QUERY_DLG = 0x7883,
        IDMT_QUERY_DLG = 0x7884,
        IDMT_ICON_ASSOCIATION = 0x7885,
        IDMT_PROGRAM_ASSOCIATION = 0x7886,
        IDMT_EXTENSION_ASSOCIATION = 0x7887,
        IDMT_SETTING_LAZY_REFRESH = 0x7888,
        IDMT_SETTING_ALL_TABLES = 0x7889,
        IDMT_SETTING_SET_OPEN = 0x788a,
        IDMT_USER_SETTINGS = 0x788b,
        IDMT_SHOW_CHECKIN_ONEXIT = 0x788c,
        IDMT_SETTING_UPDATE_NEVER = 0x788d,
        IDMT_SETTING_UPDATE_AFTER = 0x788e,
        IDMT_SETTING_UPDATE_ONEACH = 0x788f,
        IDMT_CHECKIN_DLG = 0x7890,
        IDMT_SET_INTERFACE = 0x7891,
        IDMT_SCAN_REFERENCES = 0x7892,
        IDMT_SHOW_DLG_ON_ERROR = 0x7893,
        IDMT_WIZARD_MANAGER = 0x7895,
        IDMT_CODE_RESERVATION = 0x7896,
        IDMT_USE_LASTVAULT = 0x7897,
        IDMT_USE_LASTPAGE = 0x7898,
        IDMT_CUSTOMIZE = 0x7899,
        IDMT_NETWORK_CONFIG = 0x789a,
        IDMT_USER_MANAGEMENT = 0x789b
    }

    public enum MenuCommandsStateFlags : uint
    {
        AAMF_SEL_MORE_THAN_ONE = 0x100
    }

    [Flags]
    public enum MenuItemStateFlag : uint
    {
        Show = 0,
        Hidden = 1,
        GrayedOut = 2,
        Checked = 4,
        ForceShow = 8,
        Popup = 0x1000,
        Separator = 0x2000,
        Undefined = 0xffff,
        StateMask = 0xfff
    }

    public enum MenuItemStateFlags
    {
        AAMS_GRAYED = 2
    }

    [Flags]
    public enum NewVersionCreationFlags : uint
    {
        None = 0,
        CopyAttrs = 1,
        KeepRelations = 2
    }

    public enum ObjectTypeForLinkData
    {
        None,
        DocumentByProject,
        Document,
        DocumentByWorkspace,
        DocumentBySet,
        DocumentByAttrRec
    }

    public enum ObjectTypes
    {
        UserIgnoresAccessControl,
        EnvProject,
        Project,
        EnvDoc,
        Document,
        Components
    }

    public enum ODSAttributeDataType
    {
        Int16 = 1,
        Long32 = 2,
        Float32 = 3,
        Double64 = 4,
        String = 5,
        Timestamp = 6,
        Raw = 7,
        LongRaw = 8,
        DateTime = 9
    }

    public enum ODSAttributeProperty
    {
        ID = 1,
        Name = 2,
        Desc = 3,
        Visibility = 4,
        DataType = 5,
        DataLength = 6,
        Control = 7,
        InstanceColumn = 8,
        Label = 9,
        FunctionId = 12,
        MirrorId = 13,
        LinkId = 14,
        LinkAttrId = 15,
        Direction = 0x10,
        UIType = 0x20
    }

    public enum ODSAttributeTypes
    {
        AAODS_ATTRTYPE_DATABASE = 1,
        AAODS_ATTRTYPE_USERDATA = 2,
        AAODS_ATTRTYPE_CONSTANT = 3,
        AAODS_ATTRTYPE_LINKAGE = 4,
        AAODS_ATTRTYPE_LAST = 4
    }

    public enum ODSClassProperty
    {
        ID = 1,
        Name = 2,
        Desc = 3,
        SystemClass = 4,
        KeyId = 5,
        ClassIdAttr = 6,
        TableName = 7,
        SequenceName = 8,
        CatalogName = 9,
        CatalogKeyId = 10,
        ModTime = 11,
        IsVersion = 12,
        CurrentId = 13,
        FutureId = 14,
        HistoryId = 15,
        ClassType = 20,
        Label = 0x15
    }

    public enum ODSClassTypes : uint
    {
        AAODS_CLASS_NORMAL = 1,
        AAODS_CLASS_LINK = 2,
        AAODS_CLASS_SYSTEM = 4,
        AAODS_CLASS_FUTURE = 0x10,
        AAODS_CLASS_HISTORY = 0x20
    }

    public enum ODSNativeIdType
    {
        Undefined,
        None,
        DgnElementId,
        DgnModelId,
        DgnLevelId,
        XGLPath,
        XPath,
        JSpaceId,
        SheetName,
        DgnCustomLStyleId,
        SheetView,
        DgnCellId,
        DgnSavedViewId,
        JSpaceIdLink
    }

    [Flags]
    public enum ProjectCopyDeleteAndExportFlags : uint
    {
        ExcludeParent = 1,
        NoDocuments = 2,
        NoSets = 4,
        NoRecursion = 8,
        Attributes = 0x10,
        NoProjects = 0x20,
        TakeOwnership = 0x40,
        AllowCopyAll = 0x80,
        NoCheckedOut = 0x100,
        SetReferences = 0x200,
        OwnCheckOuts = 0x400,
        NoActiveVersion = 0x800,
        DelManagedWorkspaceVars = 0x1000,
        CopyWorkflow = 0x1000,
        CopyAccess = 0x2000,
        CopyManager = 0x4000,
        CopyStorage = 0x8000,
        CopyEnvironment = 0x10000,
        CopyVersions = 0x20000,
        Components = 0x40000,
        CopySavedSearch = 0x80000,
        CopyResources = 0x100000,
        CopyConfigurationBlocks = 0x200000,
        CopyContents = 0x400000,
        CopyWorkspaceProfile = 0x800000,
        ExportEmptyProjects = 0x100000,
        ExportRootProject = 0x200000,
        ExportUsingProjectDescriptions = 0x400000,
        ExportRefsToMaster = 0x800000,
        ExportGiveOut = 0x1000000,
        ExportOuterRefs = 0x2000000,
        ExportRewriteRefs = 0x4000000,
        ExportShared = 0x8000000,
        ForCopy = 0x10000000,
        NonDMSExport = 0x20000000
    }

    public enum ProjectNumericPropertyEnum
    {
        PROJ_PROP_ID = 1,
        PROJ_PROP_VERSIONNO = 2,
        PROJ_PROP_MANAGERID = 3,
        PROJ_PROP_STORAGEID = 4,
        PROJ_PROP_CREATORID = 5,
        PROJ_PROP_UPDATERID = 6,
        PROJ_PROP_WORKFLOWID = 7,
        PROJ_PROP_STATEID = 8,
        PROJ_PROP_TYPE = 9,
        PROJ_PROP_ARCHIVEID = 10,
        PROJ_PROP_ISPARENT = 11,
        PROJ_PROP_ENVIRONMENTID = 0x15,
        PROJ_PROP_PARENTID = 0x16,
        PROJ_PROP_MGRTYPE = 0x17
    }

    public enum ProjectProperty
    {
        ID = 1,
        VersionNo = 2,
        ManagerID = 3,
        StorageID = 4,
        CreatorID = 5,
        UpdaterID = 6,
        WorkflowID = 7,
        StateID = 8,
        Type = 9,
        ArchiveID = 10,
        IsParent = 11,
        Name = 12,
        Desc = 13,
        Code = 14,
        Version = 15,
        CreateTime = 0x10,
        UpdateTime = 0x11,
        Config = 0x12,
        Table = 0x13,
        EnvironmentID = 0x15,
        ParentID = 0x16,
        MgrType = 0x17,
        Access = 0x18,
        ProjGuid = 0x19,
        PprjGuid = 0x1a,
        WSpaceProfID = 0x1b,
        ComponentClassId = 0x1c,
        Flags = 30,
        ComponentInstanceId = 0x1f
    }

    public enum ProjectResourceTypes
    {
        Application = 1,
        Department = 2,
        Environment = 3,
        StorageArea = 4,
        View = 5,
        Workflow = 6,
        WorkspaceProfile = 7
    }

    public enum ProjectTypes
    {
        AADMS_PROJECT_TYPE_NORMAL = 0,
        AADMS_PROJECT_TYPE_RICH = 2
    }

    public class ProjectWiseApplication
    {
        public int ID;
        public string Name;
        public int ViewerId;

        public ProjectWiseApplication(int iID, string sName)
        {
            this.ID = iID;
            this.Name = sName;
        }

        public ProjectWiseApplication(int iID, string sName, int iViewerId)
        {
            this.ID = iID;
            this.Name = sName;
            this.ViewerId = iViewerId;
        }
    }

    public class ProjectWiseGroup
    {
        public int ID;
        public string Name;
        public string Descripion;
        public string GroupType;
        public string SecurityProvider;

        public ProjectWiseGroup(int iID, string sName)
        {
            this.ID = iID;
            this.Name = sName;
        }

        public ProjectWiseGroup(int iID, string sName, string sDescription, string sSecurityProvider, string sType)
        {
            this.ID = iID;
            this.Name = sName;
            this.Descripion = sDescription;
            this.SecurityProvider = sSecurityProvider;
            this.GroupType = sType;
        }
    }

    public class ProjectWiseUser
    {
        public int ID;
        public string Name;
        public string Description;
        public string SecurityProvider;
        public string EMail;
        public string UserType;
        public bool Disabled;

        public ProjectWiseUser(int iID, string sName)
        {
            this.ID = iID;
            this.Name = sName;
        }

        public ProjectWiseUser(int iID, string sName, string sDescription, string sSecProvider, string sEmail, string sUserType, bool bDisabled)
        {
            this.ID = iID;
            this.Name = sName;
            this.Description = sDescription;
            this.SecurityProvider = sSecProvider;
            this.EMail = sEmail;
            this.UserType = sUserType;
            this.Disabled = bDisabled;
        }
    }

    public class ProjectWiseUserList
    {
        public int ID;
        public string Name;
        public string Descripion;
        public int ListType;
        public int Owner;

        public ProjectWiseUserList(int iID, string sName)
        {
            this.ID = iID;
            this.Name = sName;
        }

        public ProjectWiseUserList(int iID, string sName, string sDescription, int iOwner, int iType)
        {
            this.ID = iID;
            this.Name = sName;
            this.Descripion = sDescription;
            this.Owner = iOwner;
            this.ListType = iType;
        }
    }

    public class PWColumn
    {
        public string Name { get; set; }

        public int ColumnId { get; set; }

        public int TableId { get; set; }

        public string TypeName { get; set; }

        public int Length { get; set; }
    }

    public enum QueryProperty
    {
        QRY_DOC_PROP_NONE = 0,
        QRY_DOC_PROP_ENVIRONMENT_ID = 1,
        QRY_DOC_PROP_PROJ_ID = 2,
        QRY_DOC_PROP_PROJ_NAME = 3,
        QRY_DOC_PROP_PROJ_DESC = 4,
        QRY_DOC_PROP_FILENAME = 5,
        QRY_DOC_PROP_NAME = 6,
        QRY_DOC_PROP_DESC = 7,
        QRY_DOC_PROP_VERSION = 8,
        QRY_DOC_PROP_VERSIONSEQ = 9,
        QRY_DOC_PROP_CREATORID = 10,
        QRY_DOC_PROP_UPDATERID = 11,
        QRY_DOC_PROP_DMSSTATUS = 12,
        QRY_DOC_PROP_LASTUSERID = 13,
        QRY_DOC_PROP_FILETYPE = 14,
        QRY_DOC_PROP_ITEMTYPE = 15,
        QRY_DOC_PROP_STORAGEID = 0x10,
        QRY_DOC_PROP_WORKFLOWID = 0x11,
        QRY_DOC_PROP_STATEID = 0x12,
        QRY_DOC_PROP_APPLICATIONID = 0x13,
        QRY_DOC_PROP_DEPARTMENTID = 20,
        QRY_DOC_PROP_INCSUBVAULTS = 0x15,
        QRY_DOC_PROP_FINAL_STATUS = 0x16,
        QRY_DOC_PROP_FINAL_USER = 0x17,
        QRY_DOC_PROP_FINAL_DATE = 0x18,
        QRY_DOC_PROP_LOCATIONID = 0x19,
        QRY_DOC_PROP_FILE_REVISION = 0x1a,
        QRY_DOC_PROP_OVERLAPS = 0x1b,
        QRY_DOC_PROP_MIMETYPE = 0x1c,
        QRY_DOC_PROP_ID = 0x65,
        QRY_DOC_PROP_PROPOSALNO = 0x66,
        QRY_DOC_PROP_SIZE = 0x68,
        QRY_DOC_PROP_SETID = 0x69,
        QRY_DOC_PROP_SETTYPE = 0x6a,
        QRY_DOC_PROP_ORIGINALNO = 0x6b,
        QRY_DOC_PROP_IS_OUT_TO_ME = 0x6c,
        QRY_DOC_PROP_CREATE_TIME = 0x6d,
        QRY_DOC_PROP_UPDATE_TIME = 110,
        QRY_DOC_PROP_DMSDATE = 0x6f,
        QRY_DOC_PROP_NODE = 0x70,
        QRY_DOC_PROP_ACCESS = 0x71,
        QRY_DOC_PROP_MANAGERID = 0x72,
        QRY_DOC_PROP_FILE_UPDATERID = 0x73,
        QRY_DOC_PROP_LAST_RT_LOCKERID = 0x74,
        QRY_DOC_PROP_ITEM_FLAGS = 0x75,
        QRY_DOC_PROP_FILE_UPDATE_TIME = 0x76,
        QRY_DOC_PROP_LAST_RT_LOCK_TIME = 0x77,
        QRY_DOC_PROP_MGRTYPE = 120,
        QRY_DOC_PROP_DOCGUID = 0x79,
        QRY_DOC_PROP_PROJGUID = 0x7a,
        QRY_DOC_PROP_ORIGGUID = 0x7b,
        QRY_DOC_PROP_PROJ_VERSIONNO = 0x7c,
        QRY_DOC_PROP_PROJ_MANAGERID = 0x7d,
        QRY_DOC_PROP_PROJ_STORAGEID = 0x7e,
        QRY_DOC_PROP_PROJ_CREATORID = 0x7f,
        QRY_DOC_PROP_PROJ_UPDATERID = 0x80,
        QRY_DOC_PROP_PROJ_WORKFLOWID = 0x81,
        QRY_DOC_PROP_PROJ_STATEID = 130,
        QRY_DOC_PROP_PROJ_TYPE = 0x83,
        QRY_DOC_PROP_PROJ_ARCHIVEID = 0x84,
        QRY_DOC_PROP_PROJ_ISPARENT = 0x85,
        QRY_DOC_PROP_PROJ_CODE = 0x86,
        QRY_DOC_PROP_PROJ_VERSION = 0x87,
        QRY_DOC_PROP_PROJ_CREATE_TIME = 0x88,
        QRY_DOC_PROP_PROJ_UPDATE_TIME = 0x89,
        QRY_DOC_PROP_PROJ_CONFIG = 0x8a,
        QRY_DOC_PROP_PROJ_PARENTID = 140,
        QRY_DOC_PROP_PROJ_MGRTYPE = 0x8d,
        QRY_DOC_PROP_PROJ_ACCESS = 0x8e,
        QRY_DOC_PROP_PROJ_PROJGUID = 0x8f,
        QRY_DOC_PROP_PROJ_PPRJGUID = 0x90,
        QRY_PROP_ACCUMULATED_TEXTS = 200,
        QRY_PROP_DATASOURCE_GUID = 0xc9,
        QRY_PROP_VIEW_ID = 250,
        QRY_DOC_PROP_CHECKOUT_USERID = 0x12d,
        QRY_DOC_PROP_CHECKOUT_NODE = 0x12e,
        QRY_DOC_PROP_CHECKOUT_COUTTIME = 0x12f,
        QRY_FTR_PROP_SEARCH_TEXT = 1,
        QRY_FTR_PROP_SCOPE_ID = 2
    }

    [Flags]
    public enum QueryResultFlags : uint
    {
        Unfiltered = 0,
        NoDocVersions = 1,
        VersionsBySetting = 2,
        VersionFieldMask = 3
    }

    public enum ReferenceInformationProperty
    {
        ElementIDUint64 = 1,
        MasterGUID = 2,
        MasterModelID = 3,
        ReferenceGUID = 4,
        ReferenceModelID = 5,
        NestDepth = 6,
        ReferenceType = 7,
        Flags = 8
    }

    [Flags]
    public enum ReferenceListFlags : uint
    {
        FromReferenceInfo = 1,
        FromSetInfo = 2,
        AllowSelfReferences = 4
    }

    public enum RestrictionRelation
    {
        DMS_RELATION_NONE = 0,
        DMS_RELATION_EQUAL = 1,
        DMS_RELATION_NOTEQUAL = 2,
        DMS_RELATION_LESSTHAN = 3,
        DMS_RELATION_GREATERTHAN = 4,
        DMS_RELATION_GREATEROREQUAL = 5,
        DMS_RELATION_LESSOREQUAL = 6,
        DMS_RELATION_BETWEEN = 7,
        DMS_RELATION_ISNULL = 8,
        DMS_RELATION_ISNOTNULL = 9,
        DMS_RELATION_ISLIKE = 10,
        DMS_RELATION_IN = 11,
        DMS_RELATION_NOTIN = 12,
        DMS_RELATION_INNERJOIN = 13,
        DMS_RELATION_LEFTOUTERJOIN = 14,
        DMS_RELATION_RIGHTOUTERJOIN = 15,
        DMS_RELATION_ISNOTLIKE = 0x10,
        DMS_RELATION_NOTBETWEEN = 0x11,
        DMS_RELATION_NODE_OR_SUBNODE = 0x12,
        DMS_RELATION_DERIVED_TYPE = 0x13,
        DMS_RELATION_EXPRESSION = 0x400,
        DMS_RELATION_INCL_PHRASE = 0x1388,
        DMS_RELATION_INCL_ANYWORD = 0x1389,
        DMS_RELATION_INCL_ALLWORDS = 0x138a,
        DMS_RELATION_INCL_NONEOFWORDS = 0x138b,
        DMS_RELATION_SUBQUERY_COLUMN = 0x1771,
        DMS_RELATION_UNION_ALL = 0x1772,
        DMS_RELATION_SUBORGROUP = 0x1b5b,
        DMS_RELATION_SUBANDGROUP = 0x1b5c
    }

    public enum SetProperty
    {
        ID = 1,
        MemberId = 2,
        Type = 3,
        ParentProjectId = 4,
        ParentItemId = 5,
        ChildProjectId = 6,
        ChildItemId = 7,
        Relation = 8,
        Transfer = 9,
        SetProjectID = 10,
        SetItemId = 11,
        SDocGuid = 12,
        PDocGuid = 13,
        CDocGuid = 14
    }

    public enum SetRelationType
    {
        Sibling = 2,
        Redline = 3,
        Reference = 4
    }

    public enum SetType
    {
        Unknown = 0,
        Flat = 2,
        Logical = 3
    }

    [Flags]
    public enum SetTypeMasks : uint
    {
        Flat = 0x10000,
        Logical = 0x20000,
        Redline = 0x80000,
        Ref = 0x100000,
        Satellite = 0x200000,
        All = 0xff0000
    }

    public enum SQLSelectDBColumnTypes
    {
        DateTime = 0x11,
        OracleNumber = 2,
        SQLGuid = 0x15,
        SQLInteger = 3,
        SQLBoolean = 0x16,
        Char = 9,
        SQLBigInt = 0x17,
        VarChar = 10,
        VarWChar = 13,
        WChar = 12,
        SQLReal = 6,
        SQLSmallInt = 4,
        SQLTinyInt = 0x1a
    }

    public enum SqlSelectProperties
    {
        SQLSELECT_COLUMN_TYPE = 1,
        SQLSELECT_COLUMN_NATIVE_TYPE = 2,
        SQLSELECT_COLUMN_LENGTH = 3,
        SQLSELECT_COLUMN_NAME = 4
    }

    public enum SQLSelectPWTypes
    {
        Long = 0x18,
        String = 12,
        Double = 7,
        Guid = 0x15,
        Short = 4,
        Integer = 3,
        SQLReal = 6
    }

    public enum StateProperty
    {
        ID = 1,
        Name = 2,
        Desc = 3
    }

    public enum StorageProperty
    {
        ID = 1,
        Name = 2,
        Desc = 3,
        Node = 4,
        Path = 5,
        Protocol = 6,
        DisplayName = 7
    }

    public enum TableProperty
    {
        Id = 1,
        Type = 2,
        Name = 3,
        Desc = 4
    }

    public enum TypeMask
    {
        Flat = 0x10000,
        Logical = 0x20000,
        Redline = 0x80000,
        Ref = 0x100000,
        All = 0x1f0000
    }

    public enum UserListMemberProperty
    {
        ListID = 1,
        MemberType = 2,
        MemberID = 3
    }

    public enum UserListProperty
    {
        ID = 1,
        Name = 2,
        Description = 3,
        Type = 4,
        Owner = 5
    }

    public enum UserProperties
    {
        Name,
        Desc,
        Password,
        Email,
        Type,
        SecProvider
    }

    public enum UserProperty
    {
        ID = 1,
        Name = 2,
        Desc = 3,
        Password = 4,
        Email = 5,
        Type = 6,
        SecProvider = 7,
        Flags = 8
    }

    public enum UserType
    {
        DMSUser,
        NTUser
    }

    public enum ValueListProperty
    {
        EnvironmentID = 1,
        TableID = 2,
        ColumnID = 3,
        ValueID = 4,
        Value = 5,
        Description = 6
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VaultDescriptor
    {
        public uint Flags;
        public int VaultID;
        public int EnvironmentID;
        public int ParentID;
        public int StorageID;
        public int ManagerID;
        public int TypeID;
        public int WorkflowID;
        public string Name;
        public string Description;
        public string Configuration;
        public int ManagerType;
        public int WorkspaceProfileId;
        public Guid GuidVault;
    }

    public enum VaultDescriptorFlags : uint
    {
        VaultID = 1,
        EnvironmentID = 2,
        ParentID = 4,
        StorageID = 8,
        ManagerID = 0x10,
        TypeID = 0x20,
        Workflow = 0x40,
        Name = 0x80,
        Description = 0x100,
        Configuration = 0x200,
        ManagerType = 0x400,
        WSpaceProfID = 0x800
    }

    public enum VaultType
    {
        Normal,
        Workspace
    }

    public enum WebLinkActions
    {
        None,
        Open,
        OpenReadOnly,
        View,
        Markup
    }

    public enum WorkflowProperty
    {
        ID = 1,
        Type = 2,
        Name = 3,
        Desc = 4
    }

    public enum WorkflowStateProperty
    {
        WorkflowID = 1,
        StateID = 2,
        PreviousState = 3,
        NextState = 4
    }

    public enum WorkflowTypes
    {
        AADMS_WORKFLOW_PROJECT = 1,
        AADMS_WORKFLOW_DOCUMENT = 2,
        AADMS_WORKFLOW_BOTH = 3
    }
}
