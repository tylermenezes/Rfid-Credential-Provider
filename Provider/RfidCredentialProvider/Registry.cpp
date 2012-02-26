#include <iostream>
#include "common.h"
#include "Registry.h"
#include "helpers.h"

LONG GetDWORDRegKey(HKEY hKey, const std::wstring &strValueName, DWORD &nValue, DWORD nDefaultValue)
{
        nValue = nDefaultValue;
        DWORD dwBufferSize(sizeof(DWORD));
        DWORD nResult(0);
        LONG nError = ::RegQueryValueExW(hKey,
                strValueName.c_str(),
                0,
                NULL,
                reinterpret_cast<LPBYTE>(&nResult),
                &dwBufferSize);
        if (ERROR_SUCCESS == nError)
        {
                nValue = nResult;
        }
        return nError;
}


LONG GetBoolRegKey(HKEY hKey, const std::wstring &strValueName, bool &bValue, bool bDefaultValue)
{
        DWORD nDefValue((bDefaultValue) ? 1 : 0);
        DWORD nResult(nDefValue);
        LONG nError = GetDWORDRegKey(hKey, strValueName.c_str(), nResult, nDefValue);
        if (ERROR_SUCCESS == nError)
        {
                bValue = (nResult != 0) ? true : false;
        }
        return nError;
}


LONG GetStringRegKey(HKEY hKey, const std::wstring &strValueName, std::wstring &strValue, const std::wstring &strDefaultValue)
{
        strValue = strDefaultValue;
        WCHAR szBuffer[512];
        DWORD dwBufferSize = sizeof(szBuffer);
        ULONG nError;
        nError = RegQueryValueExW(hKey, strValueName.c_str(), 0, NULL, (LPBYTE)szBuffer, &dwBufferSize);
        if (ERROR_SUCCESS == nError)
        {
                strValue = szBuffer;
        }
        return nError;
}

std::string GetCharRegKey(HKEY hKey, const std::wstring &strValueName)
{
        char szBuffer[512];
		memset(&szBuffer, 0x00, 512);
        DWORD dwBufferSize = sizeof(szBuffer);
        ULONG nError;
        nError = RegQueryValueExW(hKey, strValueName.c_str(), 0, NULL, (LPBYTE)szBuffer, &dwBufferSize);
        
		std::string s(szBuffer);

		return s;
}
