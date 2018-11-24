#include <stdio.h>
#define __USE_XOPEN
#include <time.h>
#include <unistd.h>
#include <wait.h>
#include <string.h>
#include <stdlib.h>
#include <sys/types.h>
#include <signal.h>

#define SAVE_LOC "data.txt"

typedef struct Order{
  char name[50];
  char email[50];
  char phone[12];
  char state[15];
  int request;
  int position;
  time_t time;
} order_t;

typedef struct Model{
  int length;
  order_t* full_log;
} model_t;

int compare_ord(const void *s1, const void *s2){
  order_t *o1 = (order_t *)s1;
  order_t *o2 = (order_t *)s2;
  return (o1->position>o2->position);
}

void Order_to_File(FILE* stream, const order_t* ord){
  char buf[20];
  strftime(buf, 20, "%F;%T", localtime(&(ord->time)));
  fprintf(
      stream, "%d;%s;%s;%s;%s;%d;%s\n", ord->position, ord->name,
      ord->email, ord->phone, ord->state, ord->request, buf
      );
}

void save_data(const model_t* toSave){
  FILE* file = fopen(SAVE_LOC, "w");
  if(file == NULL) {
    return;
  } else {
    fprintf(file, "%d\n", toSave->length + (new_Ord == NULL ? 0 : 1));
    int i;
    for(i = 0; i < toSave->length; ++i) {
      Order_to_File(file, &(toSave->full_log[i]));
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
          file, "%d;%[^;];%[^;];%[^;];%[^;];%d;%s\n", &(curr->position), (char*)&(curr->name),
          (char*)&(curr->email), (char*)&(curr->phone), (char*)&(curr->state), &(curr->request), (char*)timebuf
          );
      strptime((char*)timebuf, "%F;%T", &tm);
      curr->time = mktime(&tm);
    }
  }
  fclose(file);
  qsort(toFill->full_log, toFill->length, sizeof(order_t), compare_ord);
}

int getSameReqOrder(model_t* FullModel, int OrdInd){
  if(strcmp(FullModel->full_log[OrdInd].state, "NotStarted")!=0) return -1;
  int ReqToFind = FullModel->full_log[OrdInd].request;
  int i;
  for(i=0;i<FullModel->length;++i){
    if(i != OrdInd && FullModel->full_log[i].request == ReqToFind && strcmp(FullModel->full_log[i].state,"NotStarted")==0) return i;
  }
  return -1;
}

void handler(int signalnum){
  printf("Signal No. :%d", signalnum);
}

int main(){
  model_t Model;
  return 0;
}
