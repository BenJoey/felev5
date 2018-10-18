#include <stdio.h>
#define _XOPEN_SOURCE
#include <time.h>
#include <string.h>
#include <stdlib.h>

#define NAME_FORMAT "%50s"
#define SEPAR ";"
#define SAVE_LOC "data.txt"

struct Order{
    char name[50];
    char email[50];
    char phone[12];
    int request;
    int position;
    time_t time;
};

typedef struct Order order_t;

struct Model{
    int length;
    order_t* full_log;
};

typedef struct Model model_t;

void Order_to_File(FILE* stream, const order_t* ord){
    char buf[20];
    strftime(buf, 20, "%F %T", localtime(&(ord->time)));
    fprintf(
        stream,
        "%d;%s;%s;%s;%d;%s\n",
        ord->position,
        ord->name,
        ord->email,
        ord->phone,
        ord->request,
        buf
    );
}

int is_Pos_Set(const model_t* _model, int OrdNum){
    int i;
    for(i=0;i<_model->length;++i){
        if(_model->full_log[i].position == OrdNum)return 1;
    }
    return 0;
}

void Save_With_Del_Order(const model_t* toSave, int OrdNum){
    FILE* file = fopen(SAVE_LOC, "w");
    fprintf(file, "%d\n", toSave->length-1);
    int i;
    for(i = 0; i < toSave->length; ++i) {
        if(OrdNum != toSave->full_log[i].position)
            Order_to_File(file, &(toSave->full_log[i]));
    }
    fclose(file);
}

int Get_Next_ID(const model_t* _model){
    int i;
    for(i=0;i<_model->length;++i){
        int j, quit=1;
        for(j=0;j<_model->length;++j)
            if((i+1) == _model->full_log[j].position)quit=0;
        if(quit)return (i+1);
    }
    return (_model->length+1);
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
            new_Ord->position=Get_Next_ID(toSave);
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
            struct tm tm;
            char* timebuf[20];
            fscanf(
                file,
                "%d;%[^;];%[^;];%[^;];%d;%s\n",
                &(curr->position),
                (char*)&(curr->name),
                (char*)&(curr->email),
                (char*)&(curr->phone),
                &(curr->request),
                (char*)timebuf
            );
            strptime((char*)timebuf, "%F %T", &tm);
            curr->time = mktime(&tm);
        }
    }
    fclose(file);
}

void print_order(const order_t* ord){
    char time[20];
    strftime(time, 20, "%F %T", localtime(&(ord->time)));
    printf("\nSorszam: %d   ", ord->position);
    printf("Megrendelo neve: %s   Email-cime: %s   Telefonszama: %s   Igenye: %d   Megrendeles ideje: %s\n",
            ord->name, ord->email, ord->phone, ord->request, time);
}

void list_by_filter(const model_t* model, const char* param, const char type){
    int i;
    for(i=0;i<model->length;++i){
        char req[20];
        sprintf(req, "%ld", model->full_log[i].request);
        switch(type){
            case '1':
                if(strcmp(param, model->full_log[i].name)==0)
                    print_order(&(model->full_log[i]));
                break;
            case '2':
                if(strcmp(param, req)==0)
                    print_order(&(model->full_log[i]));
                break;
        }
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
    scanf("%11s", &(ord->phone));
    printf("Igeny: ");
    scanf("%20d", &(ord->request));
    ord->time = time(NULL);
}

int Mod_Order(model_t* FullModel, int OrdNum){
    int Index=-1;
    int i;
    for(i=0;i<FullModel->length;++i){
        if(FullModel->full_log[i].position == OrdNum) Index = i;
    }
    if(Index==-1)return 0;
    printf("\nModositani kivant megrendeles jelenlegi adatai:");
    print_order(&(FullModel->full_log[Index]));
    printf("Uj adatok:\n");
    read_order(&(FullModel->full_log[Index]));
    return 1;
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
                    int OrdNum;
                    scanf("%d", &OrdNum);
                    switch(choice[0]){
                        case '1':
                            if(is_Pos_Set(&Model, OrdNum)){
                                Save_With_Del_Order(&Model, OrdNum);
                            }else{
                                printf("Nincs ilyen sorszamu megrendeles\n");
                            }
                            break;
                        case '2':
                            if(Mod_Order(&Model, OrdNum)){
                                save_data(&Model, NULL);
                                printf("Megrendeles sikeresen modositva\n");
                            }else{
                                printf("Nincs ilyen sorszamu megrendeles\n");
                            }
                            break;
                    }
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
                    list_by_filter(&Model, param, choice[0]);
                }
                wait_enter();
                break;
            default:
                printf("Hibas parancs");
                wait_enter();
                break;
        }
        free(Model.full_log);
    }
    return 0;
}