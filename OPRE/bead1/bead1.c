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

void print_order(const order_t* ord){
    printf("Sorszam: %d", ord->position);
    printf("Megrendelo neve: %s   Email-cime: %s   Telefonszama: %s   Igenye: %s", ord->name, ord->email, ord->phone, ord->request);
}

void list_by_filter(const model_t* model, const char* param, const int type){
    int i;
    for(i=0;i<model->lenght;++i){
        char* toFilter;
        switch(type){
            case 1:
                toFilter = model->full_log[i].name;
                break;
            case 2:
                toFilter = (char*)model->full_log[i].request;
                break;
        }
        if(param == toFilter)
            print_order(&(model->full_log[i]));
    }
}

void wait_enter(){
    printf("\nNyomj [Enter]-t a folytatashoz.\n");
    while(getchar()!='\n');
    getchar();
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
        int choice;
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
                wait_enter();
                break;
            case 2:
                printf("Modositas tipusa\n");
                printf("1. Torles\n");
                printf("2. Adatok/Igeny szerkesztese\n");
                if(scanf("%d", &choice)){
                    printf("Modositani kivant megrendeles sorszama: ");
                }
                wait_enter();
                break;
            case 3:
                printf("Megrendelesek teljes listaja:\n\n");
                int i;
                for(i=0;i<Model.lenght;++i){
                    print_order(&(Model.full_log[i]));
                }
                wait_enter();
                break;
            case 4:
                printf("Mi szerint kivan listazni?\n");
                printf("1. Nev szerint\n");
                printf("2. Igeny szerint\n");
                if(scanf("%d", &choice && (choice==1||choice==2))){
                    printf("Keresesi parameter: ");
                    char param[50];
                    scanf("%s", &param);
                    list_by_filter(&Model, param, choice);
                    //printf("%d, test   %s   tests\n\n", choice, param);
                }
                wait_enter();
                break;
            default:
                break;
        }
    }
}