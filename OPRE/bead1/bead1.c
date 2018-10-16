#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#define NAME_FORMAT "%50s"
#define EMAIL_FORMAT "[a-zA-Z0-9]@[a-zA-Z].[a-zA-Z]"
#define SAVE_LOC "data.txt"

struct order{
    char name[50];
    char email[50];
    char phone[11];
    time_t time;
    int request;
    int position;
};

typedef struct order order_t;

struct model{
    int lenght;
    order_t* full_log;
};

typedef struct model model_t;

void read_order(order_t* ord) {
    printf("Nev (max 50 karakter): ");
    scanf(NAME_FORMAT, ord->name);
    printf("Email (max 50 karakter): ");
    scanf(EMAIL_FORMAT, ord->email);
    printf("Telefonszam (11 szamjegy): ");
    scanf("%11s", ord->phone);
    printf("Igeny: ");
    scanf("%20s", &(ord->request));
    ord->time = time(NULL);
}

int main()
{
	int quit = 0;
    while(!quit){
        printf("----Fenyes Nap Kft----\n\n");
        printf("Elerheto funkciok:\n");
        printf("1: Uj rendeles rogzitese\n");
        printf("2: Korabbi rendeles modositasa\n");
        printf("3: Teljes listazas\n");
        printf("4: Listazas szurve\n");
        printf("0: Kilepes\n\n");
        int selected;
        model_t Model;
        order_t Current;
        scanf("%d", &selected);
        switch(selected){
            case 0:
                quit=1;
                break;
            case 1:
                printf("----Uj rendeles----\n\n");
                read_order(&Current);
                //insert save order function here
                break;
            case 3:
                list_model();
        }
    }
}