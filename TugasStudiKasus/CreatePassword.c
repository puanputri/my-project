#include <stdio.h>
#include <string.h>

int main() {
    char password[50];
    const char correct_password[] = "password123";
    
    printf("Masukkan password: ");
    scanf("%s", password);
    
    // Membandingkan password menggunakan strcmp
    if (strcmp(password, correct_password) == 0) {
        printf("Password yang dimasukkan benar\n");
    } else {
        printf("Password yang dimasukkan salah\n");
    }
}