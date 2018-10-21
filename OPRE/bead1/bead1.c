#define _XOPEN_SOURCE 700
#include <time.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>

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

int compare_ord(const void *s1, const void *s2){
    order_t *o1 = (order_t *)s1;
    order_t *o2 = (order_t *)s2;
    return (o1->position>o2->position);
}

void Order_to_File(FILE* stream, const order_t* ord){
    char buf[20];
    strftime(buf, 20, "%F;%T", localtime(&(ord->time)));
    fprintf(
        stream, "%d;%s;%s;%s;%d;%s\n", ord->position, ord->name,
        ord->email, ord->phone, ord->request, buf
    );
}

int Get_Offer(const int request){
    if(request % 250 == 0)return request/250;
    return request/250+1;
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
    for(i=1;i<=_model->length;++i){
        if(is_Pos_Set(_model, i) == 0)return i;
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
                file, "%d;%[^;];%[^;];%[^;];%d;%s\n", &(curr->position), (char*)&(curr->name),
                (char*)&(curr->email), (char*)&(curr->phone), &(curr->request), (char*)timebuf
            );
            strptime((char*)timebuf, "%F;%T", &tm);
            curr->time = mktime(&tm);
        }
    }
    fclose(file);
    qsort(toFill->full_log, toFill->length, sizeof(order_t), compare_ord);
}

void print_order(const order_t* ord){
    char _time[20];
    strftime(_time, 20, "%F;%T", localtime(&(ord->time)));
    printf("\nSorszam: %d   ", ord->position);
    printf("Megrendelo neve: %s   Email-cime: %s   Telefonszama: %s   Igenye: %d\nAjanlott panelek szama: %d     Megrendeles ideje: %s\n",
            ord->name, ord->email, ord->phone, ord->request, Get_Offer(ord->request), _time);
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
    printf("\nNyomjon [Enter]-t a folytatashoz.\n");
    while(getchar()!='\n');
    getchar();
}

void read_order(order_t* ord) {
    printf("Nev (max 50 karakter, szokoz nelkul): ");
    scanf("%50s", &(ord->name));
    printf("Email (max 50 karakter): ");
    scanf("%50s", &(ord->email));
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
        printf("----Fenyes Nap Kft----\n\nElerheto funkciok:\n");
        printf("1: Uj rendeles rogzitese\n2: Korabbi rendeles modositasa\n3: Teljes listazas\n");
        printf("4: Listazas szurve\n5: Ajanlat kerese\n0: Kilepes\n\n");
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
                printf("----Modositas tipusa----\n1. Torles\n2. Adatok/Igeny szerkesztese\n");
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
                printf("----Mi szerint kivan listazni?----\n1. Nev szerint\n2. Igeny szerint\n");
                scanf("%s", &choice);
                if(choice[0]=='1'||choice[0]=='2'){
                    printf("Keresesi parameter: ");
                    char param[50];
                    scanf("%s", &param);
                    list_by_filter(&Model, param, choice[0]);
                }
                wait_enter();
                break;
            case '5':
                printf("----Ajanlott panelek szamanak lekerdezese----\n");
                printf("Igyene: ");
                int req;
                scanf("%d", &req);
                printf("\nMaganak %d darab panel lenne az optimális.\n", Get_Offer(req));
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