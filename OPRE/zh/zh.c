#include <stdlib.h>
#include <stdio.h>
#include <signal.h>
#include <sys/types.h>
#include <unistd.h>
#include <string.h>
#include <sys/time.h>

void handler(int signumber) {}

int main()
{
  signal(SIGTERM, handler);

  char names[5][100];
  strcpy(names[0], "Bela");
  strcpy(names[1], "Jozsi");
  strcpy(names[2], "Feri");
  strcpy(names[3], "Karcsi");
  strcpy(names[4], "Elemer");
  srand(time(NULL)); //the starting value of random number generation

  int pipefd1[2], pipefd2[2];
  char pl1[100], pl2[100];
  if (pipe(pipefd1) == -1 || pipe(pipefd2) == -1)
  {
    perror("Hiba a pipe nyitaskor!");
    exit(EXIT_FAILURE);
  }

  pid_t player1=fork();
  if(player1<0)
  {
    perror("Fork hiba");
    exit(EXIT_FAILURE);
  }
  else if(player1>0)
  {
    pid_t player2=fork();

    if(player2<0)
    {
      perror("Fork hiba");
      exit(EXIT_FAILURE);
    }
    else if (player2>0)
    {
      pause();
      printf("Players are ready\n");
      pause();
      printf("Players are ready\n");

      sleep(1);
      //printf("ide jo");
      close(pipefd2[1]);
      read(pipefd2[0],pl2,sizeof(pl2)); // reading max 100 chars
      printf("Player2 name: %s\n",pl2);

      close(pipefd1[1]);
      read(pipefd1[0],pl1,sizeof(pl2)); // reading max 100 chars
      printf("Player1 name: %s\n",pl1);

      int r=rand()%5; //number between 0-4

      printf("A jatekovezeto leallt\n");
    }
    else
    {
      sleep(2);
      kill(getppid(),SIGTERM);

      int r=rand()%5; //number between 0-4
      close(pipefd2[0]);
      write(pipefd2[1], names[r],strlen(names[r]));
      close(pipefd2[1]);
      fflush(NULL);
    }
  }
  else
  {
    sleep(4);
    kill(getppid(),SIGTERM);

    int r=rand()%5; //number between 0-4
    r = rand()%5;
    close(pipefd1[0]);
    write(pipefd1[1], names[r], strlen(names[r]));
    close(pipefd1[1]);
    fflush(NULL);
  }
  return 0;
}
