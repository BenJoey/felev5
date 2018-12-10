-module(chat_iface).

-export([login/1, send/1, logout/0]).

logout() ->
    global:send(chat, {logout, self()}).

send(Msg) ->
    global:send(chat, {msg, self(), Msg}).

login(Nick) when is_list(Nick) ->
    global:send(chat, {login, self(), Nick}),
    receive
        ok -> logged_in;
        deny -> deny
    after
        5000 -> "no connection to server"
    end.
