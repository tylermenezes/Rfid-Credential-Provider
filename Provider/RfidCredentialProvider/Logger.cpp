#include "common.h"
#include <stdio.h>
#include <time.h>
#include <iostream>
#include <string>
#include "Logger.h"

void Log(const char *toWrite){
	FILE * pFile;

	time_t     now;
	struct tm  *ts;
	char       buf[80];
 
	now = time(NULL);
 
	ts = localtime(&now);
	strftime(buf, sizeof(buf), "%a %Y-%m-%d %H:%M:%S", ts);

	pFile = fopen ("C:\\RfidCredentialProvider.log.txt","a");
	fprintf(pFile, "%s\t%s\n", buf, toWrite);
	fclose(pFile);
}