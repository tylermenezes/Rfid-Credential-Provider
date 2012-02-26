#pragma once
#include "common.h"
#include <windows.h>
#include <strsafe.h>
#include <string>
#include <vector>
#include <cstring>

#pragma warning(push)
#pragma warning(disable : 4995)
#include <shlwapi.h>
#pragma warning(pop)

HRESULT FieldDescriptorCoAllocCopy(
    const CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR& rcpfd,
    CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR** ppcpfd
    );

HRESULT FieldDescriptorCopy(
    const CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR& rcpfd,
    CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR* pcpfd
    );

HRESULT UnicodeStringInitWithString(
    PWSTR pwz, 
    UNICODE_STRING* pus
    );

HRESULT KerbInteractiveLogonPack(
    const KERB_INTERACTIVE_LOGON& rkil,
    BYTE** prgb,
    DWORD* pcb
    );

HRESULT RetrieveNegotiateAuthPackage(
    ULONG * pulAuthPackage
    );

std::wstring s2ws(const std::string& s);
std::string ws2s(const std::wstring& si);
wchar_t* wcs2cs(std::wstring& s);
char* s2cs(std::string& s);
unsigned char* s2ucs(std::string& s);