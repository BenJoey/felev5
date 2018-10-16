#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#define NAME_FORMAT "%50s"
#define SEPAR ";"
//#define EMAIL_FORMAT "[a-zA-Z0-9]@[a-zA-Z].[a-zA-Z]"
#define SAVE_LOC "data.txt"

struct Order{
    char name[50];
    char email[50];
    char phone[11];
    time_t time;
    int request;
    int position;
};

typedef struct Order order_t;

struct Model{
    int length;
    order_t* full_log;
};

typedef struct Model model_t;

void load_data(model_t* toFill){
    FILE* file = fopen(SAVE_LOC, "r");
    if(file == NULL) {
        file = fopen(SAVE_LOC, "w");
        fprintf(file, "0\n");
        toFill->length = 0;
        toFill->full_log = NULL;
    }else{
        int length;
        fscanf(file, "%d", &length);
        toFill->length = length;
        toFill->full_log = (order_t*)(malloc(toFill->length * sizeof(order_t)));
        int i;
        for(i=0;i<length;++i){
            order_t* curr = &(toFill->full_log[i]);
            fscanf(
                file,
                "%d;%[^;];%[^;];%[^;];%d",
                (int)&(curr->position),
                (char*)&(curr->name),
                (char*)&(curr->email),
                (char*)&(curr->phone),
                (int)&(curr->request)
            );
        }
    }
    fclose(file);
}

int read_order(order_t* ord) {
    printf("Nev (max 50 karakter): ");
    if(scanf(NAME_FORMAT, ord->name)<0)return 0;
    printf("Email (max 50 karakter): ");
    if(scanf(NAME_FORMAT, ord->email)<0)return 0;
    printf("Telefonszam (11 szamjegy): ");
    if(scanf("%s", &(ord->phone))<0){return 0;}
    printf("Igeny: ");
    if(scanf("%s", ord->request)<0)return 0;
    ord->time = time(NULL);
    return 1;
}

void print_order(const order_t* ord){
    printf("Sorszam: %d", ord->position);
    printf("Megrendelo neve: %s   Email-cime: %s   Telefonszama: %s   Igenye: %s", ord->name, ord->email, ord->phone, ord->request);
}

void list_by_filter(const model_t* model, const char* param, const int type){
    int i;
    for(i=0;i<model->length;++i){
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
                if(read_order(&Current) == 1){
                    printf("%s", Current.name);
                }
                //insert save order function here
                wait_enter();
                break;
            case 2:
                printf("Modositas tipusa\n");
                printf("1. Torles\n");
                printf("2. Adatok/Igeny szerkesztese\n");
                if(scanf("%d", &choice)>0){
                    printf("Modositani kivant megrendeles sorszama: ");
                }
                wait_enter();
                break;
            case 3:
                printf("Megrendelesek teljes listaja:\n\n");
                int i;
                for(i=0;i<Model.length;++i){
                    print_order(&(Model.full_log[i]));
                }
                wait_enter();
                break;
            case 4:
                printf("Mi szerint kivan listazni?\n");
                printf("1. Nev szerint\n");
                printf("2. Igeny szerint\n");
                if(scanf("%d", &choice)>0 && (choice==1||choice==2)){
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