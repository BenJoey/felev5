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

typedef struct {
  char name[50];
  char email[50];
  char phone[12];
  char state[15];
  int request;
  int position;
  time_t time;
} order_t;

typedef struct {
  int length;
  order_t* full_log;
} model_t;

typedef struct {
  int NumOfOrd;
  int Orders[2];
} child_input_t;

typedef struct {
  int NumOfOrd;
  int Orders[2];
  char msg[15];
} child_output_t;

typedef struct {
  int first;
  int second;
} IntTuple;

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
    fprintf(file, "%d\n", toSave->length);
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

IntTuple FindSameReqOrders(model_t* FullModel){
  IntTuple ret;
  ret.first = -1; ret.second = -1;
  int i;
  for(i=0;i<FullModel->length;++i){
    order_t* curr = &(FullModel->full_log[i]);
    int j;
    for(j=0;j<FullModel->length;++j){
      if(i!=j && curr->request == FullModel->full_log[j].request && strcmp(FullModel->full_log[j].state,"NotStarted")==0 &&
          strcmp(curr->state,"NotStarted")==0){
        ret.first = i; ret.second = j;
        return ret;
      }
    }
  }
  return ret;
}

int isWeekOldOrder(model_t* _model){
  double weekInSecs = 60*60*24*7;
  int i = 0;
  for(i=0;i<_model->length;++i)
    if(strcmp(_model->full_log[i].state,"NotStarted")==0 && difftime(time(NULL), _model->full_log[i].time) > weekInSecs) return i;
  return -1;
}

void handler(int signalnum){
  printf("Signal No. :%d\n", signalnum);
}

int main(){
  int quit = 0;
  model_t Model;
  while(!quit){
    load_data(&Model);
    int i;
    int pipefd[2];
    pid_t pid;

    if(pipe(pipefd) == -1){
      perror("Hiba pipe nyitasakor");
      exit(EXIT_FAILURE);
    }

    signal(SIGUSR1, handler);
    pid=fork();

    if(pid == -1){
      perror("Fork hiba");
      exit(EXIT_FAILURE);
    }
    if(pid == 0){ //child process
      kill(getppid(), SIGUSR1);
      child_input_t input;
      read(pipefd[0], &input, sizeof(child_input_t));
      printf("Child: Munka erkezett\n");
      child_output_t output;
      output.NumOfOrd = input.NumOfOrd;
      int i;
      for(i=0;i<2;++i)
        output.Orders[i] = input.Orders[i];
      strncpy(output.msg, "InProgress", 15);
      close(pipefd[0]);
      write(pipefd[1], &output, sizeof(child_output_t));
      printf("Child: Munka elkezdve\n");
      sleep(3);
      strncpy(output.msg, "Done", 15);
      write(pipefd[1], &output, sizeof(child_output_t));
      printf("Child: Munka befejezve\n");
      kill(getppid(), SIGUSR1);
      exit(0);
    } else{ //parent process
      child_input_t toSend;
      int weekold = isWeekOldOrder(&Model);
      IntTuple temp = FindSameReqOrders(&Model);
      if(weekold != -1){
        toSend.NumOfOrd = 1;
        toSend.Orders[0] = weekold;
        toSend.Orders[1] = -1;
      }
      else if(temp.first != -1){
        toSend.NumOfOrd = 2;
        toSend.Orders[0] = temp.first;
        toSend.Orders[1] = temp.second;
      }
      if(weekold != -1 || temp.first != -1){
        printf("Parent: Munka elkuldve gyereknek\n");
        write(pipefd[1], &toSend, sizeof(child_input_t));
        close(pipefd[1]);
        pause();
        child_output_t res;
        read(pipefd[0], &res, sizeof(child_output_t));
        printf("Parent: Gyerek dolgozik rajta\n");
        for(i=0;i<res.NumOfOrd;++i){
          order_t* curr = &(Model.full_log[res.Orders[i]]);
          strncpy(curr->state, res.msg, 15);
        }
        save_data(&Model);
        pause();
        read(pipefd[0], &res, sizeof(child_output_t));
        for(i=0;i<res.NumOfOrd;++i){
          order_t* curr = &(Model.full_log[res.Orders[i]]);
          strncpy(curr->state, res.msg, 15);
        }
        printf("Parent: Gyerek befejezte a munkat\n");
        save_data(&Model);
        close(pipefd[0]);
      }
      else quit = 1;
    }
    int count = 0;
    for(i=0;i<Model.length;++i){
      order_t* curr = &(Model.full_log[i]);
      if(strcmp(curr->state, "Done") != 0) count++;
    }
    if(count == 0) quit = 1;
    free(Model.full_log);
    kill(pid, SIGTERM);
  }
  return 0;
}
