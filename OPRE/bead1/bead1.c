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
    char phone[12];
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

void Order_to_File(FILE* stream, const order_t* ord){
    fprintf(
        stream,
        "%d;%s;%s;%s;%d\n",
        ord->position,
        ord->name,
        ord->email,
        ord->phone,
        ord->request
    );
}

void save_data(const model_t* toSave, order_t* new_Ord){
    FILE* file = fopen(SAVE_LOC, "w");
    if(file == NULL) {
        return;
    } else {
        fprintf(file, "%d\n", toSave->length + (new_Ord == NULL ? 0 : 1));
        int i;
        for(i = 0; i < toSave->length; ++i) {
            Order_to_File(file, &(toSave->full_log[i]));
        }
        if(new_Ord!=NULL){
            new_Ord->position=toSave->length+1;
            Order_to_File(file, new_Ord);
        }
        fclose(file);
    }
}

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
                "%d;%[^;];%[^;];%[^;];%d\n",
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

void print_order(const order_t* ord){
    printf("\nSorszam: %d   ", ord->position);
    printf("Megrendelo neve: %s   Email-cime: %s   Telefonszama: %s   Igenye: %d", ord->name, ord->email, ord->phone, ord->request);
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

void read_order(order_t* ord) {
    printf("Nev (max 50 karakter): ");
    scanf(NAME_FORMAT, &(ord->name));
    printf("Email (max 50 karakter): ");
    scanf(NAME_FORMAT, &(ord->email));
    printf("Telefonszam (11 szamjegy): ");
    scanf("%11d", &(ord->phone));
    printf("Igeny: ");
    scanf("%20d", &(ord->request));
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
        char selected[1];
        char choice[1];
        model_t Model;
        load_data(&Model);
        order_t Current;
        scanf("%s", &selected);
        switch(selected[0]){
            case '0':
                quit=1;
                break;
            case '1':
                printf("----Uj rendeles----\n\n");
                read_order(&Current);
                save_data(&Model, &Current);
                wait_enter();
                break;
            case '2':
                printf("----Modositas tipusa----\n");
                printf("1. Torles\n");
                printf("2. Adatok/Igeny szerkesztese\n");
                scanf("%s", &choice);
                if(choice[0]=='1'||choice[0]=='2'){
                    printf("Modositani kivant megrendeles sorszama: ");
                }
                wait_enter();
                break;
            case '3':
                printf("----Megrendelesek teljes listaja:----\n");
                int i;
                for(i=0;i<Model.length;++i){
                    print_order(&(Model.full_log[i]));
                }
                wait_enter();
                break;
            case '4':
                printf("----Mi szerint kivan listazni?----\n");
                printf("1. Nev szerint\n");
                printf("2. Igeny szerint\n");
                scanf("%s", &choice);
                if(choice[0]=='1'||choice[0]=='2'){
                    printf("Keresesi parameter: ");
                    char param[50];
                    scanf("%s", &param);
                    list_by_filter(&Model, param, (int)choice[0]);
                    //printf("%d, test   %s   tests\n\n", choice, param);
                }
                wait_enter();
                break;
            default:
                printf("Hibas parancs");
                wait_enter();
                break;
        }
    }
}