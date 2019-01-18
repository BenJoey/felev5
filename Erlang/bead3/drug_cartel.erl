-module(drug_cartel).
-export([warehouse/1, bad_guy/1, fbi/0]).

warehouse(Password) ->
    register(guard, spawn(fun() -> guard([], Password) end)).

bad_guy(MyPassword) ->
    spawn(fun() -> mafioso(MyPassword) end).

fbi() ->
    spawn(fun() -> agent() end).

guard(BadGuysIn, PW) ->
    receive
        {'knock_knock', Pid} ->
            Pid ! 'pw_q',       %% atom for asking password
            guard(BadGuysIn, PW);
        {password, RecvPW, Pid} ->
            {Msg, NewList} = case RecvPW of
                                 PW ->
                                     io:format("Bad Guy ~p entered the warehouse.~n", [Pid]),
                                     {'come_in', [Pid | BadGuysIn]};
                                 _ ->
                                     {'go_away', BadGuysIn}
                             end,
            Pid ! Msg,
            guard(NewList, PW);
        {fbi, FbiPid} ->
            io:format("I'm arrested~n"),
            FbiPid ! BadGuysIn,
            timer:sleep(rand:uniform(2000)),
            lists:map(fun(L) -> L ! run end, BadGuysIn)
    end.

mafioso(Pw) ->
    Pid = self(),
    guard ! {'knock_knock', Pid},
    receive
        'pw_q' ->
            guard ! {password, Pw, Pid}
    after
        3000 ->
            io:format("I'm running away ~p~n", [Pid]),
            exit(normal)
    end,
    receive
        'go_away' ->
            io:format("I'm running away ~p~n", [Pid]),
            exit(normal);
        'come_in' ->
            io:format("I'm in the warehouse ~p~n", [Pid])
    end,
    receive
        run ->
            io:format("I'm running away ~p, the fbi is here.~n", [Pid]);
        {arrest, FbiPid} ->
            io:format("I surrender~n"),
            FbiPid ! surrender
    end.

agent() ->
    Pid = self(),
    guard ! {'knock_knock', Pid},
    receive
        'pw_q' ->
            guard ! {fbi, Pid}
    end,
    receive
        BadGuys ->
            timer:sleep(rand:uniform(2000))
    end,
    lists:map(fun(L) -> L ! {arrest, self()},
                        receive
                            surrender ->
                                io:format("I arrested ~p~n", [L])
                        after
                            3000 ->
                                io:format("~p ran away ~n", [L])
                        end
              end, BadGuys).
