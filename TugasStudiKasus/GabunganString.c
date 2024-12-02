#include <stdio.h>
#include <string.h>

int main() {
    char str1[50], str2[50], gabungan[100];
    
    printf("Masukkan string pertama: ");
    scanf("%s", str1);
    
    printf("Masukkan string kedua: ");
    scanf("%s", str2);
    
    // Menggabungkan string menggunakan strcat
    strcpy(gabungan, str1);
    strcat(gabungan, str2);
    
    printf("String gabungan: %s\n", gabungan);
}