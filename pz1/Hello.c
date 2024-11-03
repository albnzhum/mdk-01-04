#include <locale.h>
#include <stdio.h>
#include <stdlib.h>

int main(void)
{
    setlocale(LC_ALL, "Russian"); // установка русской локали
    int size;
    printf("Введите размер массива: ");
    fflush(stdout); // очистка выходного буфера
    scanf_s("%d", &size);
    int* array = malloc(size * sizeof(*array)); // выделение блока памяти для массива

    int i = 0;
    printf("Введите элементы массива: ");
    fflush(stdout);

    while (i < size)
    {
        scanf_s("%d", &array[i]); // ввод элементов массива
        i++;
    }
    // сортировка
    for (int i = 1; i < size; i++)
    {
        const int key = array[i];
        int j = i - 1;

        while (j >= 0 && array[j] > key)
        {
            array[j + 1] = array[j];
            j--;
        }

        array[j + 1] = key;
    }

    for (int i = 0; i < size; i++)
    {
        printf("%d ", array[i]);
        fflush(stdout);
    }

    free(array); // освобождение памяти
    return 0;
}
