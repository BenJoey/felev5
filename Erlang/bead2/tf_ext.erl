-module(tf_ext).
-export([run/2]).

run(F, L) ->
  Dispatcher = spawn(fun() -> dispatcher(L) end),
  Collector = spawn(fun() -> collector([]) end),
  [ spawn(fun() -> worker({Dispatcher, Collector, F}) end) || 
    _ <- lists:seq(1, 2*erlang:system_info(logical_processors))].

dispatcher([]) ->
  receive
    {newlist, List} ->
      dispatcher(List)
  end;
dispatcher([H | T]) ->
  receive
    {ready, Worker} ->
      Worker ! {data, H},
      dispatcher(T)
  end.

collector(Acc) ->
  receive
    {result, Res} ->
      collector([Res | Acc]);
    {subresult, Pid} ->
      Pid ! Acc,
      collector(Acc)
  end.

worker({Dispatcher, Collector, F} = State) ->
  Dispatcher ! {ready, self()},
  receive
    {data, Data} ->
      Collector ! {result, F(Data)},
      worker(State)
  after
    5000 ->
      Dispatcher ! {exit, self()},
      exit(normal)
  end.
