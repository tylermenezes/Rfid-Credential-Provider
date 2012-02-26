#include <string>

void decrypt(char* toDecrypt, char* password){
	for(int i = 0; i < strlen(toDecrypt); i++){
		toDecrypt[i] = (char)(((int)toDecrypt[i]) - 1);
		toDecrypt[i] = toDecrypt[i] ^ password[i % strlen(password)];
	}
}