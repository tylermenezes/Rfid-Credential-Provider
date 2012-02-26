//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) 2006 Microsoft Corporation. All rights reserved.
//
// Helper functions for copying parameters and packaging the buffer
// for GetSerialization.

#include "helpers.h"
#include "intsafe.h"
#include <string>
#include <vector>
#include <cstring>

#define cchOUTPUT 			1024
#define szOUTPUT_TAG		"Sample"

// 
// Copies the field descriptor pointed to by rcpfd into a buffer allocated 
// using CoTaskMemAlloc. Returns that buffer in ppcpfd.
// 
HRESULT FieldDescriptorCoAllocCopy(
	const CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR& rcpfd,
	CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR** ppcpfd
	)
{
	HRESULT hr;
	DWORD cbStruct = sizeof(CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR);

	CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR* pcpfd = 
		(CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR*)CoTaskMemAlloc(cbStruct);

	if (pcpfd)
	{
		pcpfd->dwFieldID = rcpfd.dwFieldID;
		pcpfd->cpft = rcpfd.cpft;
		
		if (rcpfd.pszLabel)
		{
			hr = SHStrDupW(rcpfd.pszLabel, &pcpfd->pszLabel);
		}
		else
		{
			pcpfd->pszLabel = NULL;
			hr = S_OK;
		}
	}
	else
	{
		hr = E_OUTOFMEMORY;
	}
	if (SUCCEEDED(hr))
	{
		*ppcpfd = pcpfd;
	}
	else
	{
		CoTaskMemFree(pcpfd);  
		*ppcpfd = NULL;
	}


	return hr;
}

//
// Coppies rcpfd into the buffer pointed to by pcpfd. The caller is responsible for
// allocating pcpfd. This function uses CoTaskMemAlloc to allocate memory for 
// pcpfd->pszLabel.
//
HRESULT FieldDescriptorCopy(
	const CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR& rcpfd,
	CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR* pcpfd
	)
{
	HRESULT hr;
	CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR cpfd;

	cpfd.dwFieldID = rcpfd.dwFieldID;
	cpfd.cpft = rcpfd.cpft;

	if (rcpfd.pszLabel)
	{
		hr = SHStrDupW(rcpfd.pszLabel, &cpfd.pszLabel);
	}
	else
	{
		cpfd.pszLabel = NULL;
		hr = S_OK;
	}

	if (SUCCEEDED(hr))
	{
		*pcpfd = cpfd;
	}

	return hr;
}

//
// This function copies the length of pwz and the pointer pwz into the UNICODE_STRING structure
// This function is intended for serializing a credential in GetSerialization only.
// Note that this function just makes a copy of the string pointer. It DOES NOT ALLOCATE storage!
// Be very, very sure that this is what you want, because it probably isn't outside of the
// exact GetSerialization call where the sample uses it.
//
HRESULT UnicodeStringInitWithString(
	PWSTR pwz, 
	UNICODE_STRING* pus
	)
{
	HRESULT hr;
	if (pwz)
	{
		USHORT usCharCount;
		hr = SizeTToUShort(wcslen(pwz), &usCharCount);
		if (SUCCEEDED(hr))
		{
			USHORT usSize;
			hr = SizeTToUShort(sizeof(WCHAR), &usSize);
			if (SUCCEEDED(hr))
			{
				hr = UShortMult(usCharCount, usSize, &(pus->Length)); // Explicitly NOT including NULL terminator
				if (SUCCEEDED(hr))
				{
					pus->MaximumLength = pus->Length;
					pus->Buffer = pwz;
					hr = S_OK;
				}
				else
				{
					hr = HRESULT_FROM_WIN32(ERROR_ARITHMETIC_OVERFLOW);
				}
			}
		}
	}
	else
	{
		hr = E_INVALIDARG;
	}
	return hr;
}

//
// The following function is intended to be used ONLY with the Kerb*Pack functions.  It does
// no bounds-checking because its callers have precise requirements and are written to respect 
// its limitations.
// You can read more about the UNICODE_STRING type at:
// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/secauthn/security/unicode_string.asp
//
static void _UnicodeStringPackedUnicodeStringCopy(
	const UNICODE_STRING& rus,
	PWSTR pwzBuffer,
	UNICODE_STRING* pus
	)
{
	pus->Length = rus.Length;
	pus->MaximumLength = rus.Length;
	pus->Buffer = pwzBuffer;

	CopyMemory(pus->Buffer, rus.Buffer, pus->Length);
}

//
// WinLogon and LSA consume "packed" KERB_INTERACTIVE_LOGONs.  In these, the PWSTR members of each
// UNICODE_STRING are not actually pointers but byte offsets into the overall buffer represented
// by the packed KERB_INTERACTIVE_LOGON.  For example:
// 
// kil.LogonDomainName.Length = 14                             -> Length is in bytes, not characters
// kil.LogonDomainName.Buffer = sizeof(KERB_INTERACTIVE_LOGON) -> LogonDomainName begins immediately
//                                                                after the KERB_... struct in the buffer
// kil.UserName.Length = 10
// kil.UserName.Buffer = sizeof(KERB_INTERACTIVE_LOGON) + 14   -> UNICODE_STRINGS are NOT null-terminated
//
// kil.Password.Length = 16
// kil.Password.Buffer = sizeof(KERB_INTERACTIVE_LOGON) + 14 + 10
// 
// THere's more information on this at:
// http://msdn.microsoft.com/msdnmag/issues/05/06/SecurityBriefs/#void
//

HRESULT KerbInteractiveLogonPack(
	const KERB_INTERACTIVE_LOGON& rkil,
	BYTE** prgb,
	DWORD* pcb
	)
{
	HRESULT hr;

	// alloc space for struct plus extra for the three strings
	DWORD cb = sizeof(rkil) +
		rkil.LogonDomainName.Length +
		rkil.UserName.Length +
		rkil.Password.Length;

	KERB_INTERACTIVE_LOGON* pkil = (KERB_INTERACTIVE_LOGON*)CoTaskMemAlloc(cb);
	
	if (pkil)
	{
		pkil->MessageType = rkil.MessageType;

		//
		// point pbBuffer at the beginning of the extra space
		//
		BYTE* pbBuffer = (BYTE*)pkil + sizeof(KERB_INTERACTIVE_LOGON);

		//
		// copy each string,
		// fix up appropriate buffer pointer to be offset,
		// advance buffer pointer over copied characters in extra space
		//
		_UnicodeStringPackedUnicodeStringCopy(rkil.LogonDomainName, (PWSTR)pbBuffer, &pkil->LogonDomainName);
		pkil->LogonDomainName.Buffer = (PWSTR)(pbBuffer - (BYTE*)pkil);
		pbBuffer += pkil->LogonDomainName.Length;

		_UnicodeStringPackedUnicodeStringCopy(rkil.UserName, (PWSTR)pbBuffer, &pkil->UserName);
		pkil->UserName.Buffer = (PWSTR)(pbBuffer - (BYTE*)pkil);
		pbBuffer += pkil->UserName.Length;

		_UnicodeStringPackedUnicodeStringCopy(rkil.Password, (PWSTR)pbBuffer, &pkil->Password);
		pkil->Password.Buffer = (PWSTR)(pbBuffer - (BYTE*)pkil);

		*prgb = (BYTE*)pkil;
		*pcb = cb;

		hr = S_OK;
	}
	else
	{
		hr = E_OUTOFMEMORY;
	}

	return hr;
}

// 
// This function packs the string pszSourceString in pszDestinationString
// for use with LSA functions including LsaLookupAuthenticationPackage.
//
HRESULT LsaInitString(PSTRING pszDestinationString, PCSTR pszSourceString)
{
	USHORT usLength;
	HRESULT hr = SizeTToUShort(strlen(pszSourceString), &usLength);
	if (SUCCEEDED(hr))
	{
		pszDestinationString->Buffer = (PCHAR)pszSourceString;
		pszDestinationString->Length = usLength;
		pszDestinationString->MaximumLength = pszDestinationString->Length+1;
		hr = S_OK;
	}
	return hr;
}

//
// Retrieves the 'negotiate' AuthPackage from the LSA. In this case, Kerberos
// For more information on auth packages see this msdn page:
// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/secauthn/security/msv1_0_lm20_logon.asp
//
HRESULT RetrieveNegotiateAuthPackage(ULONG * pulAuthPackage)
{
	HRESULT hr;
	HANDLE hLsa;

	NTSTATUS status = LsaConnectUntrusted(&hLsa);
	if (SUCCEEDED(HRESULT_FROM_NT(status)))
	{
		
		ULONG ulAuthPackage;
		LSA_STRING lsaszKerberosName;
		LsaInitString(&lsaszKerberosName, NEGOSSP_NAME_A);

		status = LsaLookupAuthenticationPackage(hLsa, &lsaszKerberosName, &ulAuthPackage);
		if (SUCCEEDED(HRESULT_FROM_NT(status)))
		{
			*pulAuthPackage = ulAuthPackage;
			hr = S_OK;
		}
		else
		{
			hr = HRESULT_FROM_NT(status);
		}
		LsaDeregisterLogonProcess(hLsa);
	}
	else
	{
		hr= HRESULT_FROM_NT(status);
	}

	return hr;
}

std::wstring s2ws(const std::string& s)
{
	int len;
	int slength = (int)s.length() + 1;
	len = MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, 0, 0); 
	wchar_t* buf = new wchar_t[len];
	MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, buf, len);
	std::wstring r(buf);
	delete[] buf;
	return r;
}

std::string ws2s(const std::wstring& si){
	std::wstring ws(si);
	std::string s;
	s.assign(ws.begin(), ws.end()); 
	return s;
}

wchar_t* wcs2cs(std::wstring& s){
	std::vector<wchar_t> v(s.length() + 1);
	std::wcscpy(&v[0], s.c_str());
	wchar_t* w = &v[0];
	s = w;

	wchar_t* w2 = new wchar_t[wcslen(w) + 1];

	for(int i = 0; i < wcslen(w); i++){
		w2[i] = w[i];
	}
	w2[wcslen(w)] = L'\0';

	return w2;
}

char* s2cs(std::string& s){
	std::vector<char> v(s.length() + 1);
	std::strcpy(&v[0], s.c_str());
	char* w = &v[0];
	s = w;

	char* w2 = new char[strlen(w) + 1];

	for(int i = 0; i < strlen(w); i++){
		w2[i] = w[i];
	}
	w2[strlen(w)] = L'\0';

	return w2;
}

unsigned char* s2ucs(std::string& s){
	unsigned char* cs = (unsigned char*)s2cs(s);
	return cs;
}