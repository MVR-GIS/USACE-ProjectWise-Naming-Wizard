#if !defined(AFX_USACE_Wizard_H__7ECE7F63_EF96_4A6D_81D7_217C2F08E9D5__INCLUDED_)
#define AFX_USACE_Wizard_H__7ECE7F63_EF96_4A6D_81D7_217C2F08E9D5__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"           // main symbols
#include <aadmsapi.h>
#include <aawinapi.h>
#include <aawindef.h>
#include <aawddef.h>
#include <aawindms.h>
#include <string>
#include <memory>

using namespace std;

/////////////////////////////////////////////////////////////////////////////
// CUSACE_WizardApp
// See USACE_Wizard.cpp for the implementation of this class
//

class CUSACE_WizardApp : public CWinApp
{
public:
	CUSACE_WizardApp();
	virtual ~CUSACE_WizardApp();

	void InitMenu(AAPROC_EXECCMD lpCallback);

	LONG ExecuteCommand(LPAACMDPARAM lpCommand);

	BOOL InitAdvancedMenu(AAPROC_EXECCMD lpCallback);

	DECLARE_MESSAGE_MAP()
protected:
};

int CallGui(LONG lDestProjId, CString csSourceFile);
LONG WizardImportMultiple(
	LPAAWIZDOCMULTIPLEFILES lpobImportDescriptor,
	LPLONG lplCreatedDocIds
	);
static SAFEARRAY * CreateSafeStringArray(long nElements, TCHAR * elements[]);
int CallMultiGUI(long lCount, long lProjectID, SAFEARRAY * saFiles);
int CallDRWGuiMultiTest(LONG iProjectID, SAFEARRAY * saFiles);
//int CallDRWGui(int iProjectID, int iDocumentID);
BOOL WizardSaveDocument
	(
	LPAADOCFILEDLG2_PARAM lpobDocDescriptor
	);
//BOOL WizardGetIcon
//	(
//	HICON* lphIcon,
//	LPSIZE lpSize
//	);
//BOOL WizardGetName
//	(
//	LPTSTR lpctstrName,
//	LONG   lMaxNameLen
//	);
BOOL WizardPasteDocument
	(
	LPAAWIZDOCSOURCE lpobSrcDescriptor,
	LPLONG lplCreatedDocId
	);
LRESULT AAAPI DocCreationCallback(LONG lMessageId, ULONG_PTR ulParam1, ULONG_PTR ulParam2, AAPARAM aaParam);
BOOL WizardPasteMultipleDocuments
	(
	LPAAWIZDOCMULTIPLESRC lpobSrcDescriptor,
	LPLONG lplCreatedDocIds
	);
/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_USACE_Wizard_H__7ECE7F63_EF96_4A6D_81D7_217C2F08E9D5__INCLUDED_)
