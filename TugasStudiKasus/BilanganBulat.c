#include <stdio.h>

int main() {
    int bilangan;
    
    printf("Masukkan bilangan bula: ");
    scanf("%d", &bilangan);
    
    printf("Bilangan sebelumnya: %d\n", bilangan - 1);
    printf("Bilangan setelahnya: %d\n", bilangan + 1);
}