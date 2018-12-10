-module(chat_server).
-include("chat.hrl").
-record(data, {users = #{}, log = [], max}). %% Ez valojaban 4 elemu tuple

%server interface
-export([start/0, stop/0, loop/1]).

start() ->
    global:register_name(chat, spawn(fun init/0)).

stop() ->
    global:send(chat, stop).

init() ->
    process_flag(trap_exit, true),
    InitState = #data{max = 12},
    chat_server:loop(InitState).

loop(D = #data{users=State, log=Log}) ->
    receive
        {'EXIT', Pid, _Reason} -> %% In case of runtime error
            loop(D#data{users = maps:remove(Pid, State)});
        {logout, Pid} ->
            loop(D#data{users = maps:remove(Pid, State)});
        {login, Pid, Nick} ->
            case lists:member(Nick, maps:values(State)) of
                true ->
                    Pid ! deny,
                    loop(D);
                false ->
                    link(Pid),
                    Pid ! ok,
                    loop(D#data{users = State#{Pid => Nick}})
            end;
        {msg, Pid, Msg} ->
            case State of
                #{Pid := Nick} ->
                    NewMsg = Nick ++ ": " ++ Msg,
                    maps:fold(fun(K, _V, _A) ->
                                      K ! {?texttag, NewMsg}
                              end, ok, State),
                    NewD = D#data{log = [{timer:now(), NewMsg} | Log]};
                _ ->
                    io:format("Client did not logged in! ~p~n", [Pid]),
                    NewD = D
            end,
            loop(NewD);
        dump ->
            io:format("Server state: ~p~n", [State]),
            chat_server:loop(D);
        stop ->
            io:format("Server state: ~p.~nServer Terminated", [D])
    end.
