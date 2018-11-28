-module(rec_fib).
-export([fib/2]).

fib(N, K) -> fib(N, K, 1).

fib(0, _, _) -> 1;

fib(1, _, _) -> 1;

fib(N, K, P) when P >= K ->
  fib(N-2, K, P) + fib(N-1, K, P);

fib(N, K, P) when P < K ->
  Pid = self(),
  spawn(fun() -> Pid ! fib(N-2, K, P+2) end),
  spawn(fun() -> Pid ! fib(N-1, K, P+2) end),
  Res = [receive
           Num -> Num
         end || _ <- [1,2]],
  lists:sum(Res).
