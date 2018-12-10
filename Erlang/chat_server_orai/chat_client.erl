-module(chat_client).

-export([start/1]).
-include("chat.hrl").

start(Nick) when is_list(Nick) ->
    case ?Chat:login(Nick) of
        logged_in ->
            Loop = self(),
            spawn(fun() -> io(Loop) end),
            loop();
        deny -> "Username is already taken";
        A -> A
    end;

start(Nick) ->
    "Nick name has to be string".

io(Loop) ->
    case string:strip(io:get_line("->"), right, $\n) of
        "#quit" ->
            Loop ! stop;
        String ->
            Loop ! {send, String},
            io(Loop)
    end.

loop() ->
    receive
        stop ->
            ?Chat:logout(),
            "Client stopped";
        {send, String} ->
            ?Chat:send(String),
            loop();
        {?texttag, Msg} ->
            io:format("~p~n", [Msg]),
            loop()
    end.
