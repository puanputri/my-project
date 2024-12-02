#include <iostream>
using namespace std;

void menu() {
    cout << "Pilih konversi berikut:" << endl;
    cout << "A. Celcius ke Fahrenheit" << endl;
    cout << "B. Fahrenheit ke Celcius" << endl;
    cout << "C. Celcius ke Reamur" << endl;
    cout << "D. Reamur ke Celcius" << endl;
    cout << "E. Fahrenheit ke Reamur" << endl;
    cout << "F. Reamur ke Fahrenheit" << endl;
    cout << "Masukkan pilihan Anda (A-F): ";
}

int main() {
    char pilihan;
    double suhu, hasil;

    menu();
    cin >> pilihan;

    cout << "Masukkan suhu yang akan dikonversi: ";
    cin >> suhu;

    switch (pilihan) {
        case 'A': // Celcius ke Fahrenheit
        case 'a':
            hasil = (suhu * 9/5) + 32;
            cout << "Hasil: " << hasil << " Fahrenheit" << endl;
            break;
        case 'B': // Fahrenheit ke Celcius
        case 'b':
            hasil = (suhu - 32) * 5/9;
            cout << "Hasil: " << hasil << " Celcius" << endl;
            break;
        case 'C': // Celcius ke Reamur
        case 'c':
            hasil = suhu * 4/5;
            cout << "Hasil: " << hasil << " Reamur" << endl;
            break;
        case 'D': // Reamur ke Celcius
        case 'd':
            hasil = suhu * 5/4;
            cout << "Hasil: " << hasil << " Celcius" << endl;
            break;
        case 'E': // Fahrenheit ke Reamur
        case 'e':
            hasil = (suhu - 32) * 4/9;
            cout << "Hasil: " << hasil << " Reamur" << endl;
            break;
        case 'F': // Reamur ke Fahrenheit
        case 'f':
            hasil = (suhu * 9/4) + 32;
            cout << "Hasil: " << hasil << " Fahrenheit" << endl;
            break;
        default:
            cout << "Pilihan tidak valid!" << endl;
    }

    return 0;
}
