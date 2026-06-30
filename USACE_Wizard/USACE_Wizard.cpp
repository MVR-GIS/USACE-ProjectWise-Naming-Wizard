#include "stdafx.h"
#include "USACE_Wizard.h"
#include <string>
#include "TCHAR.H" // for _tcscpy_s
//GTH Include windows for debugging
#include <iostream>
#include <fstream>
using namespace std;

//DEFINE_OWN_GUIDS(); // needed to define actionOpen

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

HICON g_hDocCreationIcon = 0;

#define USACE_DRW							L"USACE Document Renaming Wizard"

/////////////////////////////////////////////////////////////////////////////
// CUSACE_WizardApp

BEGIN_MESSAGE_MAP(CUSACE_WizardApp, CWinApp)
	//{{AFX_MSG_MAP(CUSACE_WizardApp)
	// NOTE - the ClassWizard will add and remove mapping macros here.
	//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// CUSACE_WizardApp construction

/****************************************************************************
*
* FUNC:  CUSACE_WizardApp::CUSACE_WizardApp
*
* DESC:  Constructor
*
* MISC:
*
* RETN:
*
****************************************************************************/
CUSACE_WizardApp::CUSACE_WizardApp()
{
}

/****************************************************************************
*
* FUNC:  CUSACE_WizardApp::~CUSACE_WizardApp
*
* DESC:  Destructor
*
* MISC:
*
* RETN:
*
****************************************************************************/
CUSACE_WizardApp::~CUSACE_WizardApp()
{	
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CUSACE_WizardApp object

CUSACE_WizardApp theApp;

/****************************************************************************
*
* FUNC:  DocCreationCallback
*
* DESC:  Create new application document wizard's callback function
*
* MISC:  -
*
* RETN:  AAWIZCR_OK - Execution succeded
*        AAWIZCR_NO_SUPPORT - message not supported
*        AAWIZCR_DEFAULT - default implementation should be called
*
****************************************************************************/

VOID WriteLog(LONG ID, string msg)
{
	//GTH
	try
	{
		std::string filename = R"(c:\drv\dcw.log)";
		ofstream MyFile(filename, std::ios::app);

		// Write to the file
		MyFile << ID;
		MyFile << " - ";
		MyFile << msg;
		MyFile << '\n';

		// Close the file
		MyFile.close();
	}
	catch (exception ex)
	{
		//Do nothing
	}
	//End GTH
}

LRESULT AAAPI DocCreationCallback
	(
	LONG lMessageId,        // i    MessageId to execute
	ULONG_PTR ulParam1,         // i    Message specific data
	ULONG_PTR ulParam2,          // i    Message specific data
	AAPARAM aaParam
	)
{
	try
	{
		AFX_MANAGE_STATE(AfxGetStaticModuleState());


		WriteLog(ulParam1, "Param 1");
		WriteLog(ulParam2, "Param 2");
		WriteLog(lMessageId, "Prior to Switch statement");

		switch (lMessageId)
		{

		case AAWIZCM_SUPPORTS_MSG:
			WriteLog((LONG)ulParam1, "Inside AAWIZCM_SUPPORTS_MSG");
			switch ((LONG)ulParam1)
			{
			case AAWIZCM_SUPPORTS_MSG:
			case AAWIZCM_GETNAME:
			case AAWIZCM_GETICON:
			case AAWIZCM_DOC_NEW:
			case AAWIZCM_DOC_NEW_MULTIPLE:
			case AAWIZCM_DOC_IMPORT_MULTIPLE:
			case AAWIZCM_DOC_GETNAME:
			case AAWIZCM_DOC_SAVEAS2:
			{
				TRACE(L"Supports Msg, %d, %d\n", lMessageId, ulParam1);
				return TRUE;
			}

			case AAWIZCM_DOC_DROP:
			case AAWIZCM_DOC_PASTE:
			case AAWIZCM_DOC_COPY:
			case AAWIZCM_DOC_DROP_MULTIPLE:
			{
				TRACE(L"Supports Msg (Copy/Paste), %d, %d\n", lMessageId, ulParam1);

				if (ulParam1 && (AAWIZCM_DOC_DROP_MULTIPLE != lMessageId))
				{
					// LPAAWIZDOCSOURCE lpobSrcDescriptor = (LPAAWIZDOCSOURCE)ulParam1;
					LPAAWIZDOCSOURCE lpobSrcDescriptor = (LPAAWIZDOCSOURCE)(*(ULONG*)ulParam2);

					if (lpobSrcDescriptor->bCutOp)
					{
						return FALSE;
					}
				}

				return TRUE;
			}

			case AAWIZCM_DOC_IMPORT:
			case AAWIZCM_CREATE:
			case AAWIZCM_DESTROYING:
			case AAWIZCM_DESTROY:
			case AAWIZCM_DOC_GETCOUNT:
			case AAWIZCM_DOC_GETMENUITEM:
			case AAWIZCM_ABOUT:

			default:
				return FALSE;
			}

		case AAWIZCM_GETNAME:
			TRACE(L"Get Name, %d, %d\n", lMessageId, ulParam1);
			wcsncpy_s((LPWSTR)ulParam1, ulParam2, _T("USACE Document Wizard"), ulParam2);
			return AAWIZCR_OK;
			break;
			//GTH 6-25-25 This is the source of the menu item
		case AAWIZCM_DOC_GETNAME:
			TRACE(L"Doc Get Name, %d, %d\n", lMessageId, ulParam1);
			wcsncpy_s((LPWSTR)ulParam1, ulParam2, _T("USACE Document Wizard..."), ulParam2);
			return AAWIZCR_OK;
			break;

		case AAWIZCM_GETICON:
			TRACE(L"Get Icon, %d, %d\n", lMessageId, ulParam1);
			*((HICON*)ulParam1) = LoadIcon(AfxGetInstanceHandle(), MAKEINTRESOURCE(IDI_DOC_CREATION));
			((LPSIZE)ulParam2)->cx = 32;
			((LPSIZE)ulParam2)->cy = 32;
			return AAWIZCR_OK;
			break;

		case AAWIZCM_DOC_NEW_MULTIPLE:
		case AAWIZCM_DOC_NEW:
		{
			TRACE(L"Doc New, %d, %d\n", lMessageId, ulParam1);
			LONG lDestProjId = (LONG)ulParam1;
			LONG lDocCount = (LONG)ulParam2;

			if (lDocCount == 0)//because right click --> document -- > new sends a 0 value
			{
				lDocCount = 1;
			}

			for (int i = 0; i < lDocCount; i++)
			{
				if (CallGui(lDestProjId, _T("")) == 0)
				{
					return AAWIZCR_CANCEL;
				}
			}
			return AAWIZCR_OK;
		}
		break;

		case AAWIZCM_DOC_SAVEAS2:
		{
			TRACE(L"Doc Save As, %d, %d\n", lMessageId, ulParam1);
			LPAADOCFILEDLG2_PARAM params = (LPAADOCFILEDLG2_PARAM)ulParam1;
			return (WizardSaveDocument(params)) ? AAWIZCR_OK : AAWIZCR_CANCEL;
		}
		break;

		case AAWIZCM_DOC_DROP:
		case AAWIZCM_DOC_PASTE:
		case AAWIZCM_DOC_COPY:
		{
			TRACE(L"Doc Drop, Copy, Paste, %d, %d\n", lMessageId, ulParam1);

			// LPAAWIZDOCSOURCE lpobSrcDescriptor = (LPAAWIZDOCSOURCE)ulParam1;
			// LPAAWIZDOCSOURCE lpobSrcDescriptor = (LPAAWIZDOCSOURCE)ulParam2;
			LPAAWIZDOCSOURCE lpobSrcDescriptor = (LPAAWIZDOCSOURCE)(LPLONG)ulParam1;

			if (!lpobSrcDescriptor)
				return AAWIZCR_ERROR;

			HDSOURCE hDSTarget = (lpobSrcDescriptor->hDSTarget)
				? lpobSrcDescriptor->hDSTarget
				: aaApi_GetActiveDatasource();
			HDSOURCE hDSSource = (lpobSrcDescriptor->hDSSource)
				? lpobSrcDescriptor->hDSSource
				: aaApi_GetActiveDatasource();

			if ((hDSSource != hDSTarget) || lpobSrcDescriptor->bCutOp)
				return AAWIZCR_DEFAULT;

			// return AAWIZCR_DEFAULT;

			if (lpobSrcDescriptor->lTargetVault > 0)
			{
				if (1 == aaApi_SelectDocument(lpobSrcDescriptor->lSourceVaultId, lpobSrcDescriptor->lSourceDocId))
				{
					wchar_t wcTempPath[256] = { 0 };

					GetTempPath(256, wcTempPath);

					wchar_t wcFetchedFile[512] = { 0 };

					if (aaApi_FetchDocumentFromServer(AADMS_DOCFETCH_GIVE_OUT,
						lpobSrcDescriptor->lSourceVaultId, lpobSrcDescriptor->lSourceDocId, wcTempPath, wcFetchedFile, 512))
					{
						if (CallGui(lpobSrcDescriptor->lTargetVault, wcFetchedFile) == 0)
						{
							return AAWIZCR_CANCEL;
						}

						return AAWIZCR_CANCEL;
					}
				}
			}

			return AAWIZCR_DEFAULT; // AAWIZCR_OK; (produces error)
		}
		break;

		case AAWIZCM_DOC_IMPORT_MULTIPLE:
		{
			TRACE(L"Import multiple, %d, %d\n", lMessageId, ulParam1);
			switch (WizardImportMultiple((LPAAWIZDOCMULTIPLEFILES)ulParam1, (LPLONG)ulParam2))
			{
			case 3:
				return AAWIZCR_DEFAULT;
			case 1:
				return AAWIZCR_OK;
			default:
				return AAWIZCR_CANCEL;
			}
		}

		case AAWIZCM_DOC_DROP_MULTIPLE:
		{
			TRACE(L"Drop multiple, %d, %d\n", lMessageId, ulParam1);
			LPAAWIZDOCMULTIPLESRC lpobSrcDescriptor = (LPAAWIZDOCMULTIPLESRC)ulParam1;

			if (!lpobSrcDescriptor)
				return AAWIZCR_ERROR;

			HDSOURCE hDSTarget = (lpobSrcDescriptor->hDSTarget)
				? lpobSrcDescriptor->hDSTarget
				: aaApi_GetActiveDatasource();
			HDSOURCE hDSSource = (lpobSrcDescriptor->hDSSource)
				? lpobSrcDescriptor->hDSSource
				: aaApi_GetActiveDatasource();

			if ((hDSSource != hDSTarget) || (lpobSrcDescriptor->ulFlags & AADROPDOCF_MOVE))
				return AAWIZCR_DEFAULT;

			/*if ((WPSDCW_SETTING_PASTE_DROP & wpsWDcw_GetSettings()) == 0)
			   return AAWIZCR_DEFAULT;*/

			return (WizardPasteMultipleDocuments(lpobSrcDescriptor, (LPLONG)ulParam2)) ? AAWIZCR_OK : AAWIZCR_CANCEL;
		}

		case AAWIZCM_CREATE:
		case AAWIZCM_DESTROYING:
		case AAWIZCM_DESTROY:
		case AAWIZCM_DOC_GETCOUNT:
		case AAWIZCM_DOC_GETMENUITEM:
			break;
		}
		return aaApi_WizardDefaultCallbackDC(lMessageId, ulParam1, ulParam2);
	}
	catch (exception ex)
	{
		WriteLog(666, "Unhandled Exception");
	}
} //GTH Ends here

BOOL WizardPasteMultipleDocuments
(
   LPAAWIZDOCMULTIPLESRC lpobSrcDescriptor,
   LPLONG lplCreatedDocIds
)
{
   AFX_MANAGE_STATE(AfxGetStaticModuleState());
   ASSERT(lpobSrcDescriptor);
   LONG lDocCount = lpobSrcDescriptor->lArrayLen;
   LONG lLoop = 0;
   std::auto_ptr<long> arrProjectIds = auto_ptr<long>(NULL);
   std::auto_ptr<long> arrDocumentIds = auto_ptr<long>(NULL);

   if(lDocCount > 0)
   {
       arrProjectIds = auto_ptr<long>(new long[lDocCount]);
       arrDocumentIds = auto_ptr<long>(new long[lDocCount]);
   }

   return false;
   //Larry Bernauer <
   long pID;

   SAFEARRAYBOUND Bound;
   Bound.lLbound = 0;
   Bound.cElements = (int)lDocCount;

   SAFEARRAY *psa = SafeArrayCreate(VT_INT, 1, &Bound);
   long HUGEP *pdFreq;
   HRESULT hr = SafeArrayAccessData(psa, (void HUGEP* FAR*)&pdFreq);
   if (SUCCEEDED(hr))
   {
	   if (lDocCount > 0)
	   {
		   //Larry Bernauer >

		   for (lLoop = 0; lLoop < lDocCount; lLoop++)
		   {
			   //Comment out Larry Bernauer *(arrProjectIds.get() + lLoop) = lpobSrcDescriptor->arrSourceDocs[lLoop].lProjectId;
			   //Comment out Larry Bernauer *(arrDocumentIds.get() + lLoop) = lpobSrcDescriptor->arrSourceDocs[lLoop].lDocumentId;

			   //Larry Bernauer <
			   pID = lpobSrcDescriptor->arrSourceDocs[lLoop].lProjectId;

			   *pdFreq++ = lpobSrcDescriptor->arrSourceDocs[lLoop].lDocumentId;
			   //Larry Bernauer >
		   }

		   //Larry Bernauer <
		   SafeArrayUnaccessData(psa);

		   CallDRWGuiMultiTest(pID, psa);

		   SafeArrayDestroy(psa);
	   }
   }
   //Larry Bernauer >

   /*CDCWScope opScope(lDocCount,
                     (LPINT)arrProjectIds.get(),
                     (LPINT)arrDocumentIds.get(),
                     lpobSrcDescriptor->lTargetVault);


   
   if (IDOK == CDCWWizard::ProcessWithCreation(&opScope, wpsWDcw_StoreOnlyDocumentId, lplCreatedDocIds))
   {
      return TRUE;
   }*/

   return FALSE;
}

BOOL WizardPasteDocument
(
   LPAAWIZDOCSOURCE lpobSrcDescriptor,
   LPLONG lplCreatedDocId
)
{
   AFX_MANAGE_STATE(AfxGetStaticModuleState());
   ASSERT(lpobSrcDescriptor && !lpobSrcDescriptor->bCutOp);

   return FALSE;
}

BOOL WizardSaveDocument
(
   LPAADOCFILEDLG2_PARAM lpobDocDescriptor
)
{
   AFX_MANAGE_STATE(AfxGetStaticModuleState());
   LONG lParentProjId = (lpobDocDescriptor->lplProjectId) ? *(lpobDocDescriptor->lplProjectId) : 0;
   AADOC_ITEM aaDoc = { lParentProjId, 0 };
   INT iRet = IDOK;
   TCHAR   tchTempFilePath[_MAX_PATH] = _T("");
   LPCTSTR lpctstrSeed = lpobDocDescriptor->lpctstrSeedFile;
   CString strFormat = lpobDocDescriptor->lpctstrFormat;

   if(lParentProjId == 0)
	   lParentProjId = aaApi_SelectProjectDlg(aaApi_GetMainFrameWindow(), _T(""), 0);
   
   // Clean format string, receiving only the extension
   if (!strFormat.IsEmpty())
   {
       INT iExtensionStart = strFormat.Find('.');
       INT iExtensionEnd = 0;
       if (iExtensionStart >=0)
       {
           iExtensionEnd = strFormat.Find('/', iExtensionStart);

           if (iExtensionEnd == -1)
           {
               // No description - only extension
               strFormat = strFormat.Mid(iExtensionStart+1);
           }
           else
           {
               // Description was proposed
               strFormat = strFormat.Mid(iExtensionStart+1, iExtensionEnd-1);
           }

           if (strFormat.FindOneOf(_T("./\\:*?\"<>|")) >= 0)
           {
               /* more than one extension provided (two or more dots ".") */
               /*   or illegal characters in extension */
               strFormat.Empty();
           }
       }
       else
       {
           strFormat.Empty();
       }
   }

   if (!lpctstrSeed)
   {
      TCHAR szPath[MAX_PATH] = {NULL};
      aaApi_GetUserStringSetting (AADMS_PAR_WRK_WORKING_DIRECTORY, szPath, MAX_PATH); 

      GetTempFileName(szPath, _T("DCW"), 0, tchTempFilePath);
      lpctstrSeed = tchTempFilePath;

      if (!strFormat.IsEmpty())
      { // Change file's extension to proposed if it exists
         TCHAR tchDrive[_MAX_DRIVE];
         TCHAR tchDir[_MAX_DIR];
         TCHAR tchFName[_MAX_FNAME];
         TCHAR tchExt[_MAX_EXT];
         
         CString strOldTempFileName(tchTempFilePath);
         _tsplitpath(tchTempFilePath, tchDrive, tchDir, tchFName, tchExt);
         _tmakepath(tchTempFilePath, tchDrive, tchDir, tchFName, strFormat);
         MoveFile(strOldTempFileName, tchTempFilePath);
      }
   }

   CStringArray saFiles;

   try
   {
      saFiles.Add(lpctstrSeed);
   }
   catch (CMemoryException* e)
   {
      e->Delete();

      if (tchTempFilePath[0])
      {
         DeleteFile(tchTempFilePath);
      }

      return FALSE;
   }

   for(int i = 0; i < saFiles.GetSize(); i++)
   {
	   int iDocumentId = 0;
	   iDocumentId = CallGui(lParentProjId, saFiles[i]);

	   if (lpobDocDescriptor->lplProjectId)
	   {
		   *lpobDocDescriptor->lplProjectId = lParentProjId;
	   }

	   if (lpobDocDescriptor->lplDocumentId)
	   {
		   *lpobDocDescriptor->lplDocumentId = iDocumentId;
	   }

	   if (tchTempFilePath[0])
	   {
		   DeleteFile(tchTempFilePath);
	   }

	   if(iDocumentId == 0)
		   return FALSE;
   }

   return IDOK;
}

//CStringArray& defaultSourceFile()
//{
//	CStringArray& retVal = CStringArray();
//	retVal.SetSize(1);
//	retVal[0] = "Empty";
//	return retVal;
//}


int CallGui(LONG lDestProjId = 0, CString sSourceFile = _T(""))
{
	typedef int (__stdcall *LPSHOWGUIFUNC)(int, LPTSTR);
	HINSTANCE hDLL;
	
	// changed to not refer to static path - DAB 2013-07-24
	// hDLL = LoadLibrary(L"C:\\Program Files (x86)\\Bentley\\ProjectWise\\bin\\USACE_Wizard_GUI.dll");
	hDLL = LoadLibrary(L"USACE_Wizard_GUI.dll");

	LONG iNewDocId = 0;

	// added test for found library - DAB 2013-07-24
	if (hDLL != NULL)
	{
		LPSHOWGUIFUNC lpfnDllShowGui = (LPSHOWGUIFUNC)GetProcAddress(hDLL, "ShowGUI");

		if(!lpfnDllShowGui)
		{
			printf("Exported function 'ShowGUI' not found in USACE_Wizard_GUI.dll.\n");
		}
		else
		{
			LPTSTR lptSourceFile = sSourceFile.GetBuffer(0);
			sSourceFile.ReleaseBuffer();
			iNewDocId = lpfnDllShowGui(lDestProjId, lptSourceFile);
		}
		

		FreeLibrary(hDLL);
	}
	else
	{
		TRACE(L"Library not found\n");
	}

	return iNewDocId;
}

LONG WizardImportMultiple(
	LPAAWIZDOCMULTIPLEFILES lpobImportDescriptor,
	LPLONG lplCreatedDocIds
	)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	TCHAR tchBuffer[_MAX_PATH] = _T("");
	UINT uiCount = 0;
	UINT uiLoop  = 0;
	UINT uiFiles = 0;
	CStringArray saFiles;

	uiCount = DragQueryFile(lpobImportDescriptor->hDropInfo, 0xFFFFFFFF, NULL, 0);

	if (!uiCount)
	{
		return 2;
	}

	try
	{
		saFiles.SetSize(uiCount);
	}
	catch (CMemoryException* e)
	{
		e->Delete();
		return 2;
	}

	//New to handle mulit files at once Larry Bernauer 12/2/18
	TCHAR **fileNames = NULL;
	fileNames = (TCHAR**)malloc(uiCount * sizeof(TCHAR*));
	//New to handle mulit files at once Larry Bernauer 12/2/18

	for (uiLoop = uiFiles = 0; uiLoop < uiCount; uiLoop++)
	{
		*tchBuffer = 0;

		if (0 >= DragQueryFile(lpobImportDescriptor->hDropInfo,
			uiLoop,
			tchBuffer,
			ARRAY_LENGTH(tchBuffer)))
		{
			continue;
		}

		if (aaApi_IsDirectory(tchBuffer))
		{
			return 3;
		}

		saFiles[uiFiles++] = tchBuffer;
		//New to handle mulit files at once Larry Bernauer 12/2/18
		TCHAR * name = new TCHAR[sizeof(tchBuffer)];
		_tcsncpy(name, tchBuffer, sizeof(tchBuffer));
		fileNames[uiLoop] = name;
		//New to handle mulit files at once Larry Bernauer 12/2/18
	}

	//int iDocTestID = CallTest(lpobImportDescriptor->lParentId, saFiles);
	//CString filesImport[saFiles.GetSize()]
	//Drag and drop

	
	//New to handle mulit files at once Larry Bernauer 12/2/18
	SAFEARRAY *pSA = CreateSafeStringArray(uiCount, fileNames);

	CallMultiGUI(uiCount, lpobImportDescriptor->lParentId, pSA);
	delete[] fileNames;
	fileNames = NULL;
	SafeArrayDestroy(pSA);
	pSA = NULL;
	return 1;
	//New to handle mulit files at once Larry Bernauer 12/2/18

	//Original
	//for(int iFileCounter = 0; iFileCounter < saFiles.GetSize(); iFileCounter++)
	//{
	//	int iDocId = CallGui(lpobImportDescriptor->lParentId, saFiles[iFileCounter]);
	//}

	//return 1;
	//Original
}

static SAFEARRAY *CreateSafeStringArray(long nElements, TCHAR *elements[])
{
	SAFEARRAYBOUND saBound[1];

	saBound[0].cElements = nElements;
	saBound[0].lLbound = 0;

	SAFEARRAY *pSA = SafeArrayCreate(VT_BSTR, 1, saBound);

	if (pSA == NULL)
	{
		return NULL;
	}

	for (int ix = 0; ix < nElements; ix++)
	{
		BSTR pData = SysAllocString(elements[ix]);

		long rgIndicies[1];

		rgIndicies[0] = saBound[0].lLbound + ix;

		HRESULT hr = SafeArrayPutElement(pSA, rgIndicies, pData);

		_tprintf(TEXT("%d"), hr);
	}

	return pSA;
}

int CallMultiGUI(long lCount = 0, long lProjectID = 0, SAFEARRAY* saFiles = NULL)
{
	typedef int(__stdcall *LPSHOWGUIFUNC)(long, long, SAFEARRAY*);
	HINSTANCE hDLL;

	// changed to not refer to static path - DAB 2013-07-24
	// hDLL = LoadLibrary(L"C:\\Program Files (x86)\\Bentley\\ProjectWise\\bin\\USACE_Wizard_GUI.dll");
	hDLL = LoadLibrary(L"USACE_Wizard_GUI.dll");

	LONG iNewDocId = 0;

	// added test for found library - DAB 2013-07-24
	if (hDLL != NULL)
	{
		LPSHOWGUIFUNC lpfnDllShowGui = (LPSHOWGUIFUNC)GetProcAddress(hDLL, "MultiGUI");

		if (!lpfnDllShowGui)
		{
			printf("Exported function 'ShowGUI' not found in USACE_Wizard_GUI.dll.\n");
		}
		else
		{
			//LPTSTR alptFiles[lCount] = saFiles
			//LPTSTR lptSourceFile = sSourceFile.GetBuffer(0);
			//sSourceFile.ReleaseBuffer();
			iNewDocId = lpfnDllShowGui(lCount, lProjectID, saFiles);

			//TEST
			//LPTESTFUNC lpfnDllTest = (LPTESTFUNC)GetProcAddress(hDLL, "ShowGUI2");
			//lpfnDllTest(lDestProjId, lptSourceFile);
		}


		FreeLibrary(hDLL);
	}
	else
	{
		TRACE(L"Library not found\n");
	}

	return iNewDocId;
}

/****************************************************************************
*
* FUNC:  ExecCmd
*
* DESC:  Callback function that executes custom menu commands
*
* MISC:  Wraper for CMyMenuApp::ExecuteCommand() method
*
* RETN:  0    OK;
*        AAAPI_EXECCMD_DO_DEFAULT (-1)  Do default action (if any)
*        >0   AAAPI or custom error code
*
****************************************************************************/
LONG AAAPIHOOK ExecCmd
(
	LPAACMDPARAM lpCommand
)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return theApp.ExecuteCommand(lpCommand);
}

/****************************************************************************
*
* FUNC:  CustomInitialize
*
* DESC:  Registers custom wizard types, wizards; adds menu commands
*
* MISC:  -
*
* RETN:  IDOK
*
****************************************************************************/
UINT gWizardID = -1;
extern "C" LONG WINAPI CustomInitialize
	(
	ULONG   ulMask,     // i Application Mask
	LPVOID  lpReserved  // i Reserved (must be NULL)
	)
{
	static BOOL s_bLoaded = FALSE;

	if (s_bLoaded)
		return IDOK;

	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	theApp.InitMenu(ExecCmd);

	AAWIZARD aaWiz;

	g_hDocCreationIcon = theApp.LoadIcon(IDI_DOC_CREATION);

	aaWiz.ulMask = AAWIZM_CATEGORYID | AAWIZM_CALLBACK | AAWIZM_CATEGORYID;
	aaWiz.fnCallback = DocCreationCallback;
	aaWiz.ulCategory = AAAPI_WIZTYPE_DOCUMENT;
	ULONG ulWizId = aaApi_RegisterWizard(&aaWiz);

	s_bLoaded = TRUE;
	return IDOK;
}

extern "C" int WINAPI DocCmd_DisableMenuItemState
(
UINT uiCmdId,
ULONG ulState
)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return AAMS_GRAYED;
}

extern "C" int WINAPI DocCmd_DisableMenuItem
(
unsigned int count,
long* pProjects,
long* pDocuments
)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return 0;
}

/****************************************************************************
*
* FUNC:  CMyMenuApp::InitMenu
*
* DESC:  Creates custom menus
*
* MISC:  -
*
* RETN:  -
*
****************************************************************************/
void CUSACE_WizardApp::InitMenu
(
	AAPROC_EXECCMD lpCallback
)
{
	InitAdvancedMenu(lpCallback);
}

/****************************************************************************
*
* FUNC:  CMyMenuApp::ExecuteCommand
*
* DESC:  Callback funtion to execute custom menu commands
*
* MISC:  -
*
* RETN:  0    OK;
*        AAAPI_EXECCMD_DO_DEFAULT (-1)  Do default action (if any)
*        >0   AAAPI or custom error code
*
****************************************************************************/
LONG CUSACE_WizardApp::ExecuteCommand
(
	LPAACMDPARAM lpCommand
)
{
	LONG lItems;
	HDOCCMDPARAM  hDocParam;
	LPAADOC_ITEM lpDocItem;

	if (!lpCommand)
	{
		return AAERR_PARAMETER;
	}

	if (lpCommand->lCommandType != AAMENU_DOCUMENT ||
		lpCommand->ushParamType != AACMDPT_DOCUMENT)
	{
		return AAAPI_EXECCMD_DO_DEFAULT;
	}

	hDocParam = *((HDOCCMDPARAM*)lpCommand->lpParam);

	lItems = aaApi_GetDocCmdParamCount(AADOCCMDPT_DOCUMENT1, hDocParam);
	lpDocItem = (LPAADOC_ITEM)aaApi_GetDocCmdParamElements(AADOCCMDPT_DOCUMENT1, hDocParam);

	if (!lpDocItem)
	{
		return AAAPI_EXECCMD_DO_DEFAULT;
	}

	//
	//TCHAR *fileNames = NULL;
	//fileNames = (TCHAR*)malloc((int)lItems * sizeof(TCHAR));
	
	
	long pID;

	

	//VARIANT vRet;
	SAFEARRAYBOUND Bound;
	Bound.lLbound = 0;
	Bound.cElements = (int)lItems;

	SAFEARRAY *psa = SafeArrayCreate(VT_INT, 1, &Bound);
	long HUGEP *pdFreq;
	HRESULT hr = SafeArrayAccessData(psa, (void HUGEP* FAR*)&pdFreq);
	//

	if (SUCCEEDED(hr))
	{
		if (lItems > 0)
		{
			for (LONG i = 0; i < (int)lItems; i++)
			{
				pID = lpDocItem[i].lProjectId;

				*pdFreq++ = lpDocItem[i].lDocumentId;
				//long dID = lpDocItem[i].lDocumentId;

				//fileNames[i] = lpDocItem[i].lDocumentId;

				//CallDRWGui(lpDocItem[i].lProjectId, lpDocItem[i].lDocumentId);
			}

			SafeArrayUnaccessData(psa);

			CallDRWGuiMultiTest(pID, psa);

			SafeArrayDestroy(psa);
		}
		//else if (lItems = 1)
		//{
		//	pID = lpDocItem[0].lProjectId;

		//	*pdFreq++ = lpDocItem[0].lDocumentId;
		//	SafeArrayUnaccessData(psa);

		//	CallDRWGuiMultiTest(pID, psa);
		//	//CallDRWGui(lpDocItem[0].lProjectId, lpDocItem[0].lDocumentId);
		//}
	}


	if (lItems != 1)
	{
		return AAAPI_EXECCMD_DO_DEFAULT;
	}

	return 0;
}

int CallDRWGuiMultiTest(long iProjectID = 0, SAFEARRAY* saFiles = NULL)
{
	typedef int(__stdcall *LPSHOWGUIFUNC)(long, SAFEARRAY*);
	//typedef int (__stdcall *LPTESTFUNC)(int, LPTSTR);
	HINSTANCE hDLL;

	// changed to not refer to static path - DAB 2013-07-24
	// hDLL = LoadLibrary(L"C:\\Program Files (x86)\\Bentley\\ProjectWise\\bin\\USACE_Wizard_GUI.dll");
	hDLL = LoadLibrary(L"USACE_Wizard_GUI.dll");

	//LONG iNewDocId = 0;

	// added test for found library - DAB 2013-07-24
	if (hDLL != NULL)
	{
		LPSHOWGUIFUNC lpfnDllShowGui = (LPSHOWGUIFUNC)GetProcAddress(hDLL, "InitDRWTest");

		if (!lpfnDllShowGui)
		{
			printf("Exported function 'InitDRW' not found in USACE_Wizard_GUI.dll.\n");
		}
		else
		{
			//LPTSTR lptSourceFile = sSourceFile.GetBuffer(0);
			//sSourceFile.ReleaseBuffer();
			//iNewDocId = 
				lpfnDllShowGui(iProjectID, saFiles);

			//TEST
			//LPTESTFUNC lpfnDllTest = (LPTESTFUNC)GetProcAddress(hDLL, "ShowGUI2");
			//lpfnDllTest(lDestProjId, lptSourceFile);
		}


		FreeLibrary(hDLL);
	}
	else
	{
		TRACE(L"Library not found\n");
	}

	return -1;
}

//int CallDRWGui(int iProjectID = 0, int iDocumentID = 0)
//{
//	typedef int(__stdcall *LPSHOWGUIFUNC)(int, int);
//	//typedef int (__stdcall *LPTESTFUNC)(int, LPTSTR);
//	HINSTANCE hDLL;
//
//	// changed to not refer to static path - DAB 2013-07-24
//	// hDLL = LoadLibrary(L"C:\\Program Files (x86)\\Bentley\\ProjectWise\\bin\\USACE_Wizard_GUI.dll");
//	hDLL = LoadLibrary(L"USACE_Wizard_GUI.dll");
//
//	LONG iNewDocId = 0;
//
//	// added test for found library - DAB 2013-07-24
//	if (hDLL != NULL)
//	{
//		LPSHOWGUIFUNC lpfnDllShowGui = (LPSHOWGUIFUNC)GetProcAddress(hDLL, "InitDRW");
//
//		if (!lpfnDllShowGui)
//		{
//			printf("Exported function 'InitDRW' not found in USACE_Wizard_GUI.dll.\n");
//		}
//		else
//		{
//			//LPTSTR lptSourceFile = sSourceFile.GetBuffer(0);
//			//sSourceFile.ReleaseBuffer();
//			iNewDocId = lpfnDllShowGui(iProjectID, iDocumentID);
//
//			//TEST
//			//LPTESTFUNC lpfnDllTest = (LPTESTFUNC)GetProcAddress(hDLL, "ShowGUI2");
//			//lpfnDllTest(lDestProjId, lptSourceFile);
//		}
//
//
//		FreeLibrary(hDLL);
//	}
//	else
//	{
//		TRACE(L"Library not found\n");
//	}
//
//	return -1;
//}


/****************************************************************************
*
* FUNC:  CMyMenuApp::InitAdvancedMenu
*
* DESC:  Creates Advanced popup menu
*
* MISC:  -
*
* RETN:  TRUE, FALSE - Eroor
*
****************************************************************************/
BOOL CUSACE_WizardApp::InitAdvancedMenu
(
	AAPROC_EXECCMD lpCallback
)
{
	ULONG hAdvancedMenu;
	AADMSMENUITEM aaMenuItem;

	ULONG rMI = aaApi_GetCommandHandle(AAMENU_DOCUMENT, IDMD_RENAME);

	aaMenuItem.ulMask = AADMSMIF_FLAGS | AADMSMIF_NAME | AADMSMIF_CMDTYPE | AADMSMIF_PROMPT |
		AADMSMIF_CMDID | AADMSMIF_REQUIRED | AADMSMIF_CALLBACK |
		AADMSMIF_REQUIREDS;
	aaMenuItem.uiFlags = AAMIF_COMMAND;
	aaMenuItem.uiCmdType = AAMENU_DOCUMENT;
	aaMenuItem.lptstrPrompt = USACE_DRW;
	aaMenuItem.lptstrName = USACE_DRW;
	aaMenuItem.uiCommandId = 0;
	aaMenuItem.ulRequiredMask = AAMF_SEL_NONFINAL;
	aaMenuItem.ulRequiredSMask = AAMSF_ANY_DOCUMENT;
	aaMenuItem.fpExecute = lpCallback;
	aaMenuItem.ushParamType = AACMDPT_DOCUMENT;
	hAdvancedMenu = aaApi_AddCustomMenuItem(0, rMI, &aaMenuItem);

	if (!hAdvancedMenu)
	{
		return FALSE;
	}

	return TRUE;
}