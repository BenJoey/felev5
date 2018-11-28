-module(pr6_notfold).
-export([run2/1]).

run2(N) ->
  %%List of processes
  ProcList = [spawn(fun() -> worker() end) || _<-lists:seq(1,N)],
  sendAround(ProcList, self()),

  %%Now every process has the id to which it needs to send the msg
  %%So we send it to first and wait for it in the main process
  lists:nth(1, ProcList) ! ok,
  receive
    ok -> "end"
  end.

%%This helper function sends each process the id of the next process
%%The last in the list gets the id of the main process
sendAround([], _) -> ok;

sendAround([H|T], MainPid) ->
  case T of
    [] -> H ! MainPid;
    _ -> H ! lists:nth(1, T)
  end,
  sendAround(T, MainPid).

worker()->
  receive
    Pid -> ok
  end,
  receive
    Msg -> Pid ! Msg
  end.
