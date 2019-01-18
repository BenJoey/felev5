-module(tf_ext).
-export([run/2]).

run(F, L) ->
    ProcNum = 2*erlang:system_info(logical_processors),
    Dispatcher = spawn(fun() -> dispatcher(L, ProcNum) end),
    Collector = spawn(fun() -> collector([], ProcNum) end),
    [ spawn(fun() -> worker({Dispatcher, Collector, F}) end) || 
      _ <- lists:seq(1, ProcNum)].

dispatcher([], 0) ->  %dispatcher stops if all the workers stopped and there is no data to process
    io:format("Dispatcher stopped~n");
dispatcher([], PN) ->
    receive
        {newlist, List} ->
            dispatcher(List, PN);
        {ready, Worker} ->   %%if the dispatcher gets a ready msg while there are no data to process
            Worker ! stop,   %%it tells the Worker that he can stop
            dispatcher([], PN-1)
    end;
dispatcher([H | T], PN) ->
    receive
        {ready, Worker} ->
            Worker ! {data, H},
            dispatcher(T, PN)
    end.

collector(Acc, 0) ->
    io:format("Collector stopped~nHere are the results: ~p~n", [lists:reverse(Acc)]);
collector(Acc, PN) ->
    receive
        {result, Res} ->
            collector([Res | Acc], PN);
        {subresult, Pid} ->
            Pid ! Acc,
            collector(Acc, PN);
        worker_stopped ->
            collector(Acc, PN-1)
    end.

worker({Dispatcher, Collector, F} = State) ->
    Dispatcher ! {ready, self()},
    receive
        {data, Data} ->
            Collector ! {result, F(Data)},
            worker(State);
        stop ->
            Collector ! worker_stopped,  %% worker also notifies the collector that he stopped
            io:format("~p Pid stopped~n", [self()])
    end.
