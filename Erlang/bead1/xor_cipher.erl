%%%=============================================================================
%%% XOR_Cipher code and decode module
%%%=============================================================================

-module(xor_cipher).
-export([toBitString/1,
         charToBitString/1,
         bitStringToChar/1,
         xOr/2,
         encrypt/2,
         decrypt/2,
         isCycledIn/2,
         getKey/2,
         decodeMessage/2,
         test/0
        ]).

%%%=============================================================================
%%% Types
%%%=============================================================================

-type bitString() :: list(0|1).

%%%=============================================================================
%%% API
%%%=============================================================================

%% 1. Converting a decimal number to a bitstring (2 pont)
-spec toBitString(N :: number()) -> bitString().
toBitString(N) -> toBitString(N, []).

%% 2. Converting a character to a bitstring (2 pont)
-spec charToBitString(A :: char()) -> bitString() | {error, atom()}.
charToBitString(A) -> addExtraZeros(toBitString(A), 8).

%% 3. Converting a bitstring to a character (2 pont)
-spec bitStringToChar(BitStr :: bitString()) -> char().
bitStringToChar(BitStr) ->
    {Result, _} = lists:foldl(fun(Curr, {Sofar, Index}) -> 
                                      NewVal = case Curr of
                                                   1 -> Sofar + math:pow(2, Index);
                                                   0 -> Sofar
                                               end,
                                      {NewVal, Index + 1}
                              end, {0, 0}, BitStr),
    trunc(Result).

%% 4. xor operation on bitstrings (2 pont)
-spec xOr(A :: bitString(), B :: bitString()) -> bitString().
xOr(A, B) -> recXOR(A, B, []).

%% 5. Encryption (3 pont)
-spec encrypt(Text :: string(), Key :: string()) -> string().
encrypt(Text, Key) ->
    LongKey = getLongKey(Key, length(Text), [], 1),
    {TextBitStr, KeyBitStr} = {lists:map(fun(L) -> charToBitString(L) end, Text),
                               lists:map(fun(L) -> charToBitString(L) end, LongKey)},
    ResultXOR = [xOr(lists:nth(Ind, TextBitStr), lists:nth(Ind, KeyBitStr)) || Ind<-lists:seq(1, length(Text))],
    lists:map(fun(L) -> bitStringToChar(L) end, ResultXOR).

%% 6. Decryption (1 pont)
-spec decrypt(Cipher :: string(), Key :: string()) -> string().
decrypt(Cipher, Key) -> encrypt(Cipher, Key).

%% 7. Repeated text (2 pont)
-spec isCycledIn(A :: string(), B :: string()) -> true | false.
isCycledIn(A, B) -> cycSearch(A, B, 1).

%% 8. Determining the Key (4 pont)
-spec getKey(Text :: string(), Cipher :: string()) -> string() | 'no_key'.
getKey(Text, Cipher) -> findKey(decrypt(Cipher, Text), 1).

%% 9. Decoding messages (4 + 3 pont)
-spec decodeMessage(Cipher :: string(), TextPart :: string()) -> string().
decodeMessage(Cipher, TextPart) ->
    case searchKey(Cipher, TextPart, 1) of
        'no_solution' -> 'cannot_be_decoded';
        Key ->
            io:format("~p~n", [Key]),
            decrypt(Cipher, Key)
    end.

test() ->
    getKey(encrypt("ave O", "TUT"), "ave O"),
    decodeMessage(encrypt("hurtelnu hztrre kktrenor nununu", "GTA"), "kktrenor").

%%%=============================================================================
%%% Internal functions
%%%=============================================================================

%% Most of the internal functions are the recursive functions
%% Functions in the API calls starts the recursion

-spec toBitString(N :: number(), Acc :: bitString()) -> bitString().
toBitString(0, Acc) -> lists:reverse(Acc);
toBitString(N, Acc) -> toBitString(N div 2, [N rem 2 | Acc]).

%% Adds zeros to the end of the bitString until it reaches the required length
-spec addExtraZeros(Acc :: bitString(), ReqLen :: number()) -> bitString() | {error, atom()}.
addExtraZeros(Acc, ReqLen) when length(Acc) > ReqLen ->
    {error, invalid_character};
addExtraZeros(Acc, ReqLen) when length(Acc) == ReqLen -> Acc;
addExtraZeros(Acc, ReqLen) -> addExtraZeros(Acc ++ [0], ReqLen).

-spec recXOR(A :: bitString(), B :: bitString(), Acc :: bitString()) -> bitString().
recXOR(A, [], Acc) -> lists:merge(lists:reverse(Acc), A);
recXOR([], B, Acc) -> lists:merge(lists:reverse(Acc), B);
recXOR([HeadA | RestA], [HeadB | RestB], Acc) ->
    Current = case HeadA of
                  0 -> HeadB;
                  1 -> (HeadB + 1) rem 2
              end,
    recXOR(RestA, RestB, [Current | Acc]).

%% Repeats the input word until it reaches the required length
-spec getLongKey(OriginalKey :: string(), ReqLen :: number(), Acc :: string(), Index :: number()) -> string().
getLongKey(_, ReqLen, Acc, _) when length(Acc) == ReqLen -> lists:reverse(Acc);
getLongKey(OriginalKey, ReqLen, Acc, Index) ->
    getLongKey(OriginalKey, ReqLen, [lists:nth(Index, OriginalKey) | Acc], (Index rem length(OriginalKey)) + 1).

-spec cycSearch(ToFind :: string(), ToSearch :: string(), Index :: number()) -> true | false.
cycSearch(ToFind, ToSearch, Index) when Index > length(ToSearch) ->
    not (Index =< length(ToFind));  %% If the ToSearch word is shorter than the chars to find, then it's false,
                                    %% otherwise if it did not quit earlier then it's true

cycSearch(ToFind, ToSearch, Index) ->
    FindInd = case (Index rem length(ToFind)) of
                  0 -> length(ToFind);
                  Val -> Val
              end,
    case lists:nth(Index, ToSearch) == lists:nth(FindInd, ToFind) of
        true ->
            cycSearch(ToFind, ToSearch, Index + 1);
        false -> false
    end.

-spec findKey(LongKey :: string(), Index :: number()) -> string() | 'no_key'.
findKey(LongKey, Index) when Index > (length(LongKey) -1) ->
    'no_key';

findKey(LongKey, Index) ->
    case isCycledIn(lists:sublist(LongKey, Index), LongKey) of
        true -> 
            io:format("~p~n", [LongKey]),
            lists:sublist(LongKey, Index);
        false -> findKey(LongKey, Index + 1)
    end.

-spec searchKey(Cipher :: string(), TextPart :: string(), Index :: number()) -> string() | 'no_solution'.
searchKey(Cipher, TextPart, Index) when ((Index + length(TextPart) - 1) > length(Cipher)) ->
    'no_solution';

searchKey(Cipher, TextPart, Index) ->
    case getKey(TextPart, lists:sublist(Cipher, Index, length(TextPart))) of
        'no_key' -> searchKey(Cipher, TextPart, Index + 1);
        Key -> Key
    end.
