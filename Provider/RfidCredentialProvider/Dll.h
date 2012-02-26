#pragma once

extern HINSTANCE g_hinst;
#define HINST_THISDLL g_hinst

void DllAddRef();
void DllRelease();
